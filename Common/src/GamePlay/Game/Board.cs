using System;
using System.Collections.Generic;
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
        /// Kikeresi az oszlopindex �s a sorindex alapj�n a mez�t a t�bl�r�l
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
        /// Felt�lti a Field-ek list�j�t a megadott neh�zs�gen koordin�t�kkal egy�tt.
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
                for (int i = 0; i < X; i++)
                {
                    for (int j = 0; j < Y; j++)
                    {
                        int randomImage = random.Next(imageList.Count);
                        Field field = new Field()
                        {
                            ImageProperty = imageList[randomImage],
                            ColumnIndex = i,
                            RowIndex = j
                        };
                        allFields.Add(field);
                    }
                }
            }
        }


    }
}