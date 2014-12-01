using System;
using System.Collections.Generic;
using Pixeek.Transformation;

namespace Pixeek.Game
{
    public class Board
    {
        private Random random;
        private List<Image> imageList;
        private List<Field> allFields;

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

        public Board(List<Image> imageList)
        {
            this.imageList = imageList;
            random = new Random();
        }

        /// <summary>
        /// Kikeresi az oszlopindex és a sorindex alapján a mezõt a tábláról
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public Field getField(int columnIndex, int rowIndex)
        {
            foreach (Field field in allFields)
            {
                if (field.RowIndex == rowIndex && field.ColumnIndex == columnIndex)
                {
                    return field;
                }
            }
            return null;
        }

        /// <summary>
        /// Feltölti a Field-ek listáját a megadott nehézségen koordinátákkal együtt.
        /// </summary>
        /// <param name="difficulty"></param>
        public void createBoard(Difficulty difficulty)
        {
            allFields = new List<Field>();

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
                    Transformator trf;
                    switch (random.Next(5))
                    {
                        case(0): trf = new Rotate(difficulty, random.Next()); break;
                        case(1): trf = new Mirror(difficulty, random.Next()); break;
                        case(2): trf = new Blur(difficulty, random.Next()); break;
                        case(3): trf = new ColorTransformation(difficulty, random.Next()); break;
                        default: trf = new Transformator(difficulty, random.Next()); break;
                    }
                    Field field = new Field(imageList[randomImage], j, i, trf);
                    allFields.Add(field);
                }
            }

        }


    }
}