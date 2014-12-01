using System;
using System.Collections.Generic;
namespace Pixeek.Game
{
    
    public class Board
    {
        private Random random;
        private List<Image> imageList;
        public List<Field> AllFields
        {
            get;
            set;
        }

        public int X
        {
            get;
            set;
        }
        public int Y
        {
            get;
            set;
        }

        public void AddToAllFields(Field field)
        {
            AllFields.Add(field);
        }

        public Board(List<Image> imageList)
        {
            this.imageList = imageList;
            random = new Random();
            AllFields = new List<Field>();
        }

        /// <summary>
        /// Kikeresi az oszlopindex �s a sorindex alapj�n a mez�t a t�bl�r�l
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public Field getField(int columnIndex, int rowIndex)
        {
            foreach (Field field in AllFields)
            {
                if (field == null) continue;
                if (field.RowIndex == rowIndex && field.ColumnIndex == columnIndex)
                {
                    return field;
                }
            }
            return null;
        }

        /// <summary>
        /// Felt�lti a Field-ek list�j�t a megadott neh�zs�gen koordin�t�kkal egy�tt.
        /// </summary>
        /// <param name="difficulty"></param>
        public void createBoard(Difficulty difficulty)
        {
            X = 0;
            Y = 0;
            if (difficulty == Difficulty.EASY)
            {
                X = 5;
                Y = 5;
            }
            else if (difficulty == Difficulty.NORMAL)
            {
                X = 9;
                Y = 9;
            }
            else if (difficulty == Difficulty.HARD)
            {
                X = 16;
                Y = 16;
            }

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    int randomImage = random.Next(imageList.Count);
                    Field field = new Field()
                    {
                        ImageProperty = imageList[randomImage],
                        ImageNumber = randomImage,
                        ColumnIndex = j,
                        RowIndex = i,
                        Available = true
                    };
                    AllFields.Add(field);
                }
            }
        }

        /// <summary>
        /// Kicser�li a megadott mez�t egy random m�sikra
        /// </summary>
        /// <param name="field"></param>
        public void changeField(Field field)
        {
            int randomImage = random.Next(imageList.Count);
            field.ImageProperty = imageList[randomImage];
            field.ImageNumber = randomImage;
        }


    }
}