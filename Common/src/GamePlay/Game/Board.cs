using System;
using System.Collections.Generic;
using Pixeek.Transformation;
using Pixeek.BoardShapes;
using Pixeek.ServerCommunicator;
using Pixeek.ServerCommunicator.Objects;
using System.Threading;

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
        /// Kikeresi az oszlopindex és a sorindex alapján a mezõt a tábláról
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public Field GetField(int columnIndex, int rowIndex)
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
        /// Feltölti a Field-ek listáját a megadott nehézségen koordinátákkal együtt.
        /// </summary>
        /// <param name="difficulty"></param>
        /*public void createBoard_old(Difficulty difficulty, IBoardShapes boardAnimal)
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
        }*/

        /// <summary>
        /// Feltölti a Field-ek listáját a megadott nehézségen koordinátákkal együtt.
        /// </summary>
        /// <param name="difficulty"></param>
        public void CreateBoard(Difficulty difficulty, IBoardShapes boardAnimal)
        {
            int[][] boardMap = boardAnimal.GetField(difficulty);
            int imageCounter = 0;
            Y = boardMap.GetLength(0);
            X = boardMap[0].GetLength(0);
            for (int y = 0; y < boardMap.GetLength(0); y++)
            {
                for (int x = 0; x < boardMap[y].GetLength(0); x++)
                {
                    if (boardMap[y][x] == 1)
                    {
                        //addRandomFieldToAllFields(difficulty, y, x);
                        Field field = new Field(imageList[imageCounter], imageCounter, y, x, true, null);
                        AllFields.Add(field);
                        imageCounter++;
                    }
                }
            }
            
        }

        /// <summary>
        /// Hozzáad az összes mezõhöz egy random transzformált képpel rendelkezõ mezõt. A mezõ sor és oszlopadata a paraméterben meghatározott
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        private void AddRandomFieldToAllFields(Difficulty difficulty, int column, int row)
        {
            int randomImage = random.Next(imageList.Count);
            Transformator trf;
            switch (random.Next(5))
            {
                case (0): trf = new Rotate(difficulty, random.Next()); break;
                case (1): trf = new Mirror(difficulty, random.Next()); break;
#if WINDOWS
                case (2): trf = new Blur(difficulty, random.Next()); break;
#endif
                case (3): trf = new ColorTransformation(difficulty, random.Next()); break;
                default: trf = new Transformator(difficulty, random.Next()); break;
            }
            Field field = new Field(imageList[randomImage], randomImage, column, row, true, trf);
            AllFields.Add(field);
        }

        public delegate void FieldChangedHandler();
        /// <summary>
        /// Kicseréli a megadott mezõt egy random másikra
        /// </summary>
        /// <param name="field"></param>
        public void ChangeField(Field field, Difficulty difficulty, FieldChangedHandler handler)
        {
            /*int randomImage = random.Next(imageList.Count);
            field.ImageProperty = imageList[randomImage];
            field.ImageNumber = randomImage;*/
            field.Available = false;
            SinglePlayerGameCommunicator.Instance.GetNewTile(difficulty,
                delegate(NewTileResponse response)
                {
                    Thread.Sleep(500);
                    field.ImageProperty = NewTileResponse.getImagesFromResponse(GameManager.Instance.GraphicsDevice, response);
                    field.Available = true;
                    handler();
                }
            );
        }


    }
}