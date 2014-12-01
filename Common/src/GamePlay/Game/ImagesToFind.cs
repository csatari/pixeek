using System;
using System.Collections.Generic;
using System.Linq;

namespace Pixeek.Game
{
    public class ImagesToFind
    {
        public List<Image> ToFind
        {
            get;
            set;
        }
        public Board Board
        {
            private get;
            set;
        }
        public GameMode GameMode {
            private get;
            set;
        }

        public delegate void OutOfImages();
        public OutOfImages outOfImages;

        /// <summary>
        /// Megpróbálja megtalálni a megadott mezõt a keresett képek listájában. 
        /// Ha megtalálta, akkor kiszedi és játékmód szerint újat rak be, vagy úgy hagyja.
        /// </summary>
        /// <param name="clickedField">A kattintott mezõ</param>
        /// <returns>true - ha megtalálta a mezõt, false ha nem</returns>
        public bool tryToFindField(Field clickedField)
        {
            Image foundImage = null;
            foreach (Image image in ToFind)
            {
                if (clickedField.ImageProperty.Name.Equals(image.Name))
                {
                    foundImage = image;
                }
            }
            if (foundImage != null)
            {
                ToFind.Remove(foundImage);

                if (GameMode == Game.GameMode.ENDLESS)
                {
                    addNewImageToFind();
                }

                if (ToFind.Count == 0) outOfImages();

                return true;
            }
            else return false;
        }

        /// <summary>
        /// Hozzáad egy új random képet
        /// </summary>
        public void addNewImageToFind()
        {
            Random random = new Random();
            //kiválaszt megadott számú random mezõt az elkészített táblából
            Field toFindRandomFieldFromBoard = Board.AllFields.OrderBy(x => random.Next()).Take(1).First();
            //a random mezõkbõl kiszedi a képet - ez azért kell, mert egy kép többször is szerepelhet
            ToFind.Add(toFindRandomFieldFromBoard.ImageProperty);
        }

        /// <summary>
        /// Kiírja az összes képet, x1, x2, ... szorzókkal együtt
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            Dictionary<string, int> imageWithMultiplication = new Dictionary<string, int>();

            foreach (Image image in ToFind)
            {
                if (!imageWithMultiplication.ContainsKey(image.Name))
                {
                    imageWithMultiplication.Add(image.Name, 1);
                }
                else
                {
                    imageWithMultiplication[image.Name]++;
                }
                
            }

            int newline = (int)Math.Ceiling(imageWithMultiplication.Count / 2.0);
            int counter = 0;
            string write = "";

            foreach (KeyValuePair<string, int> kvp in imageWithMultiplication)
            {
                if (counter == newline)
                {
                    counter = 0;
                    write += "\r\n";
                }
                if (kvp.Value > 1)
                {
                    write += kvp.Key + " " + kvp.Value + "x, ";
                }
                else
                {
                    write += kvp.Key + ", ";
                }
                counter++;
            }
            if (write.Length > 2)
            {
                write = write.Substring(0, write.Length - 2);
            }
            return write;
        }

        /// <summary>
        /// Elkészíti a megtalálandó képek osztályát az elkészített táblából nehézség szerint
        /// </summary>
        /// <param name="gameMode"></param>
        /// <param name="difficulty"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public static ImagesToFind createNewImagesToFind(GameMode gameMode, Difficulty difficulty, Board board) {

            ImagesToFind imagesToFind = new ImagesToFind();
            imagesToFind.ToFind = new List<Image>();
            imagesToFind.Board = board;
            imagesToFind.GameMode = gameMode;

            //Nehézség szerint kitalálja, hogy hány képet is kelljen keresnie a játékosnak
            Random random = new Random();
            int toFindCount = 0;
            if (difficulty == Difficulty.EASY)
            {
                toFindCount = 5 + random.Next(-2,2);
            }
            else if (difficulty == Difficulty.NORMAL)
            {
                toFindCount = 10 + random.Next(-3, 3);
            }
            else if (difficulty == Difficulty.HARD)
            {
                toFindCount = 20 + random.Next(-5, 5);
            }
            //kiválaszt megadott számú random mezõt az elkészített táblából
            List<Field> toFindRandomFieldsFromBoard = board.AllFields.OrderBy(x => random.Next()).Take(toFindCount).ToList();
            //a random mezõkbõl kiszedi a képet - ez azért kell, mert egy kép többször is szerepelhet
            foreach (Field field in toFindRandomFieldsFromBoard)
            {
                imagesToFind.ToFind.Add(field.ImageProperty);
            }
            return imagesToFind;
        }
    }
}