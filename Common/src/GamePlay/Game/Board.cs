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

        public void AddToAllFields(Field field)
        {

            allFields.Add(field);
        }

        public Board(List<Image> imageList)
        {
            this.imageList = imageList;
            random = new Random();
            allFields = new List<Field>();
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
                        RowIndex = i
                    };
                    allFields.Add(field);
                }
            }

        }


    }
}