using System.Collections.Generic;
namespace Pixeek.Game
{
    public class LevelManager
    {
        public LevelManager()
        {

        }
        public Board newGame(GameMode gameMode, Difficulty difficulty, List<Image> imageList)
        {
            Board board = new Board(imageList);
            board.createBoard(difficulty);

            return board;
        }
    }
}