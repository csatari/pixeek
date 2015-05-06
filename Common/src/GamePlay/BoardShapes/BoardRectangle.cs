using Pixeek.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.BoardShapes
{
    public class BoardRectangle : IBoardShapes
    {
        public int[][] getField(Difficulty difficulty)
        {
            int[][] board = null;
            if (difficulty == Difficulty.EASY)
            {
                //5 - x,5 - y 
                board = new int[5][];
                board[0] = new int[5] { 1, 1, 1, 1, 1 };
                board[1] = new int[5] { 1, 1, 1, 1, 1 };
                board[2] = new int[5] { 1, 1, 1, 1, 1 };
                board[3] = new int[5] { 1, 1, 1, 1, 1 };
                board[4] = new int[5] { 1, 1, 1, 1, 1 };
            }
            else if (difficulty == Difficulty.NORMAL)
            {
                //9 - x,9 - y 
                board = new int[9][];
                board[0] = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[1] = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[2] = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[3] = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[4] = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[5] = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[6] = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[7] = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[8] = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            }
            else if (difficulty == Difficulty.HARD)
            {
                //16 - x,16 - y 
                board = new int[16][];
                board[0] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[1] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[2] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[3] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[4] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[5] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[6] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[7] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[8] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[9] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[10] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[11] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[12] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[13] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[14] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[15] = new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            }
            return board;
        }

        public int getFieldCount(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.EASY: return 25;
                case Difficulty.NORMAL: return 81;
                case Difficulty.HARD: return 256;
                default: return 0;
            }
        }
    }
}
