using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixeek.BoardShapes;
using Pixeek.Game;
using Pixeek.Transformation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Pixeek
{
    class GameTesting
    {
        static void assert(bool expression)
        {
            if (!expression)
            {
                throw new System.Exception("Testing assertion failure");
            }
        }

        public static void RunAllTests()
        {
            assert(true);
            RunGameTests();
        }

        static void RunGameTests()
        {
            System.TimeSpan simple = new System.TimeSpan(1000);
            // check the diamond shaped board
            checkDiamondShape(Game.Difficulty.EASY);
            checkDiamondShape(Game.Difficulty.NORMAL);
            checkDiamondShape(Game.Difficulty.HARD);

            checkFishShape(Game.Difficulty.EASY);
            checkFishShape(Game.Difficulty.NORMAL);
            checkFishShape(Game.Difficulty.HARD);

            //checking if scoring adds the right points and combos
            Scoring scoring = new Scoring(2500);
            assert(scoring.Combo == 1 && scoring.Score == 0);
            for (int i = 0; i < 15; i++)
            {
                scoring.addPoint(1);
            }
            assert(scoring.Combo == 32 && scoring.Score == 351);
            scoring.addPoint(0);
            assert(scoring.Combo == 32 && scoring.Score == 351);
            scoring.addPoint(-1);
            assert(scoring.Combo == 32 && scoring.Score == 351);
            Thread.Sleep(2600);
            assert(scoring.Combo == 16 && scoring.Score == 351);

            //creating random boards
            LevelManager levelManager = new LevelManager();
            List<Image> imageList = new List<Image>();
            //creates a new empty texture
            Texture2D t2d = new Texture2D(GameManager.Instance.GraphicsDevice, 100, 100);
            for (int i = 0; i < 15; i++)
            {
                imageList.Add(new Image() { ImageTexture = t2d, Name = "random" + i });
            }
            //checking if a testboard after starting a new game, has a board, and the images to find can be found on the board
            Board testBoard = levelManager.newGame(GameMode.NORMAL, Difficulty.EASY, null, imageList);
            assert(testBoard.X == 5 && testBoard.Y == 5);
            assert(levelManager.ImagesToFind.ToFind.Count > 0);
            for (int i = 0; i < levelManager.ImagesToFind.ToFind.Count; i++)
            {
                assert(levelManager.ImagesToFind.ToFind[i].ImageTexture == t2d);
                assert(levelManager.ImagesToFind.ToFind[i].Name.Substring(0, 6).Equals("random"));
            }
            testBoard = levelManager.newGame(GameMode.TIME, Difficulty.NORMAL, null, imageList);
            assert(testBoard.X == 9 && testBoard.Y == 9);
            for (int i = 0; i < levelManager.ImagesToFind.ToFind.Count; i++)
            {
                assert(levelManager.ImagesToFind.ToFind[i].Name.Substring(0, 6).Equals("random"));
            }
            assert(levelManager.ImagesToFind.ToFind.Count > 0);

            testBoard = levelManager.newGame(GameMode.ENDLESS, Difficulty.HARD, null, imageList);
            assert(testBoard.X == 16 && testBoard.Y == 16);
            assert(levelManager.ImagesToFind.ToFind.Count > 0);
            for (int i = 0; i < levelManager.ImagesToFind.ToFind.Count; i++)
            {
                assert(levelManager.ImagesToFind.ToFind[i].Name.Substring(0, 6).Equals("random"));
            }

            //creates a testfield, which cannot be found on any randomly created board
            Field testField = new Field(
                new Image() { ImageTexture = t2d, Name = "nemtalalhato" } ,
                0, 0, 0, false, new Transformator(Difficulty.HARD, 1));
            //the testfield cannot be clicked
            assert(levelManager.tryClickedField(testField) == false);
            //try to click one random image, which is in the tofind list 
            assert(levelManager.tryClickedField(
                testBoard.AllFields.Find(field => field.ImageProperty.Name == levelManager.ImagesToFind.ToFind[0].Name)
                ));
            //adding and removing fields from the board
            testBoard.AddToAllFields(testField);
            assert(testBoard.AllFields.Contains(testField));
            testBoard.AllFields.Remove(testField);
            assert(testBoard.AllFields.Contains(testField) == false);

            //the board has a square shape, and the size of the hard difficulty
            int squareSide = (int)Math.Sqrt((double)testBoard.AllFields.Count);
            assert(squareSide == 16);

            //creates a new diamond shaped board, and checks some fields if they are on the correct place
            testBoard = levelManager.newGame(GameMode.NORMAL, Difficulty.EASY, new BoardDiamond(), imageList);
            assert(testBoard.getField(0, 0) == null);
            assert(testBoard.getField(3, 0) != null);
            assert(testBoard.getField(3, 0).ColumnIndex == 3 && testBoard.getField(3, 0).RowIndex == 0);

            //checks if changeField function works properly
            Field oldField = testBoard.getField(3, 0);
            string oldstr = oldField.ImageProperty.Name;
            testBoard.changeField(oldField, Difficulty.EASY,
                delegate()
                {
                    levelManager.ImagesToFind.addNewImageToFind();
                });
            //can cause exception in odd cases if the random new image is the same as it was
            assert(oldstr != testBoard.getField(3, 0).ImageProperty.Name);

            //checks the ImagesToFind if it doesnt contain an image, which cannot be found on the board, and checkes one, which should be on it
            ImagesToFind imagesToFind = levelManager.ImagesToFind;
            assert(imagesToFind.tryToFindField(testField) == false);
            assert(imagesToFind.tryToFindField(testBoard.AllFields.Find(field => field.ImageProperty.Name == levelManager.ImagesToFind.ToFind[0].Name)));

            //adds a new image to find, and checks if the images to find count has increased
            int oldCount = imagesToFind.ToFind.Count;
            imagesToFind.addNewImageToFind();
            assert(oldCount + 1 == imagesToFind.ToFind.Count);

            assert(imagesToFind.ToString().Contains(imagesToFind.ToFind[0].Name));

            //creates some new images to find, and checks if it is created properly
            imagesToFind = new ImagesToFind();
            assert(imagesToFind.ToString() == "");

            imagesToFind = ImagesToFind.createNewImagesToFind(GameMode.NORMAL, Difficulty.EASY, testBoard);

            assert(imagesToFind.ToFind.Count > 0);
        }

        static void checkDiamondShape(Game.Difficulty diff) {
            IBoardShapes boardShape = new BoardDiamond();
            int[][] fields = boardShape.getField(diff);
            int notnull = 0;
            for (int y = 0; y < fields.GetLength(0); y++)
            {
                for (int x = 0; x < fields[y].GetLength(0); x++)
                {
                    if (fields[y][x] == 1)
                    {
                        notnull++;
                    }
                }
            }
            assert((fields.Length * fields[0].Length) + 1 == notnull * 2);
            assert(fields[0][(fields.Length/2)] == 1 && fields[fields.Length-1][(fields.Length/2)] == 1
                && fields[(fields.Length / 2)][0] == 1 && fields[(fields.Length / 2)][fields.Length - 1] == 1);
        }

        static void checkFishShape(Game.Difficulty diff)
        {
            IBoardShapes boardShape = new BoardFish();
            int[][] fields = boardShape.getField(diff);
            assert(fields[0].Equals(fields[1]) == false);
        }
    }
}