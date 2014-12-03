using System;
using System.Collections.Generic;
using Pixeek.Transformation;
using Pixeek.BoardShapes;

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
        public void createBoard(Difficulty difficulty, IBoardShapes boardAnimal)
        {
            if (boardAnimal != null)
            {
                int[][] boardMap = boardAnimal.getField(difficulty);
                Y = boardMap.GetLength(0);
                X = boardMap[0].GetLength(0);
                for (int y = 0; y < boardMap.GetLength(0); y++)
                {
                    for (int x = 0; x < boardMap[y].GetLength(0); x++)
                    {
                        if (boardMap[y][x] == 1)
                        {
                            addRandomFieldToAllFields(difficulty, y, x);
                        }
                    }
                }
            }
            else
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
                        addRandomFieldToAllFields(difficulty, i, j);
                    }
                }
            }
        }

        /// <summary>
        /// Hozz�ad az �sszes mez�h�z egy random transzform�lt k�ppel rendelkez� mez�t. A mez� sor �s oszlopadata a param�terben meghat�rozott
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        private void addRandomFieldToAllFields(Difficulty difficulty, int column, int row)
        {
            int randomImage = random.Next(imageList.Count);
            Transformator trf;
            switch (random.Next(5))
            {
                case (0): trf = new Rotate(difficulty, random.Next()); break;
                case (1): trf = new Mirror(difficulty, random.Next()); break;
                case (2): trf = new Blur(difficulty, random.Next()); break;
                case (3): trf = new ColorTransformation(difficulty, random.Next()); break;
                default: trf = new Transformator(difficulty, random.Next()); break;
            }
            Field field = new Field(imageList[randomImage], randomImage, column, row, true, trf);
            AllFields.Add(field);
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