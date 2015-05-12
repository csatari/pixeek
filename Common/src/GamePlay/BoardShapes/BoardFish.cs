using Pixeek.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.BoardShapes
{
    public class BoardFish : IBoardShapes
    {
        public int[][] GetField(Difficulty difficulty)
        {
            int[][] board = null;
            /*if (difficulty == Difficulty.EASY)
            {*/
                //16 - x,7 - y 
                board = new int[7][];
                board[0] = new int[16] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 };
                board[1] = new int[16] { 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 };
                board[2] = new int[16] { 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 };
                board[3] = new int[16] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[4] = new int[16] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 };
                board[5] = new int[16] { 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 };
                board[6] = new int[16] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 };
            //}

            return board;
        }

        public int GetFieldCount(Difficulty difficulty)
        {
            return 79;
        }

        public string ToString()
        {
            return "fish";
        }
    }
}
