using Pixeek.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.BoardShapes
{
    public class BoardDiamond : IBoardShapes
    {
        public int[][] getField(Difficulty difficulty)
        {
            int[][] board = null;
            if (difficulty == Difficulty.EASY)
            {
                //7 - x,7 - y 
                board = new int[7][];
                board[0] = new int[7] { 0, 0, 0, 1, 0, 0, 0 };
                board[1] = new int[7] { 0, 0, 1, 1, 1, 0, 0 };
                board[2] = new int[7] { 0, 1, 1, 1, 1, 1, 0 };
                board[3] = new int[7] { 1, 1, 1, 1, 1, 1, 1 };
                board[4] = new int[7] { 0, 1, 1, 1, 1, 1, 0 };
                board[5] = new int[7] { 0, 0, 1, 1, 1, 0, 0 };
                board[6] = new int[7] { 0, 0, 0, 1, 0, 0, 0 };
            }
            else if (difficulty == Difficulty.NORMAL)
            {
                //9 - x,9 - y 
                board = new int[9][];
                board[0] = new int[9] { 0, 0, 0, 0, 1, 0, 0, 0, 0 };
                board[1] = new int[9] { 0, 0, 0, 1, 1, 1, 0, 0, 0 };
                board[2] = new int[9] { 0, 0, 1, 1, 1, 1, 1, 0, 0 };
                board[3] = new int[9] { 0, 1, 1, 1, 1, 1, 1, 1, 0 };
                board[4] = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[5] = new int[9] { 0, 1, 1, 1, 1, 1, 1, 1, 0 };
                board[6] = new int[9] { 0, 0, 1, 1, 1, 1, 1, 0, 0 };
                board[7] = new int[9] { 0, 0, 0, 1, 1, 1, 0, 0, 0 };
                board[8] = new int[9] { 0, 0, 0, 0, 1, 0, 0, 0, 0 };
            }
            else if (difficulty == Difficulty.HARD)
            {
                //15 - x,15 - y 
                board = new int[15][];
                board[0] = new int[15]  { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };
                board[1] = new int[15]  { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
                board[2] = new int[15]  { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 };
                board[3] = new int[15]  { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 };
                board[4] = new int[15]  { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 };
                board[5] = new int[15]  { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 };
                board[6] = new int[15]  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 };
                board[7] = new int[15]  { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                board[8] = new int[15]  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 };
                board[9] = new int[15]  { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 };
                board[10] = new int[15] { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 };
                board[11] = new int[15] { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 };
                board[12] = new int[15] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 };
                board[13] = new int[15] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
                board[14] = new int[15] { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };
            }
            return board;
        }
    }
}
