using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Pixeek.Game;
using System.Collections.Generic;
using System.Timers;

namespace Pixeek.GameDrawables
{
    /// <summary>
    /// Ez az osztály felelõs a Board kirajzolásáért
    /// </summary>
    public class BoardDrawable
    {
        private static BoardDrawable _instance = null;
        public Board board;
        ButtonState lastButtonState = ButtonState.Released;
        private int gap = 5;
        private int fieldWidth;
        private int fieldHeight;
        private int startingXpos;
        Dictionary<Field, Rectangle> fieldPositionDictionary = new Dictionary<Field, Rectangle>();

        public delegate void ClickHandler(Field field);
        private ClickHandler clickHandler;

        public BoardDrawable(Board board, ClickHandler clickHandler)
        {
            this.board = board;
            _instance = this;
            this.clickHandler = clickHandler;
            //az elemek szélessége és magassága függ az ablak méretétõl
            fieldWidth = GameManager.Width / board.X;
            fieldHeight = (6 * GameManager.Height / 8) / board.Y;

            //ne legyenek széthúzott mezõk, ezért a kisebb méretet alkalmazom a másik méretnél
            if (fieldWidth > fieldHeight)
            {
                fieldWidth = fieldHeight;
            }
            else
            {
                fieldHeight = fieldWidth;
            }

            //x tengely kirajzolásához kezdõpont
            startingXpos = (GameManager.Width - (board.X * fieldWidth)) / 2;
        }

        public static BoardDrawable Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        public void PositionClicked(Point pos)
        {
            if (pos != Point.Zero)
            {
                //sorindex és oszlopindex kiszámolása az egérkattintásból
                Field clickedField = null;
                foreach (KeyValuePair<Field, Rectangle> entry in fieldPositionDictionary)
                {
                    if (entry.Value.Contains(pos))
                    {
                        clickedField = entry.Key;
                        break;
                    }
                }
                if (clickedField != null)
                {
                    clickHandler(clickedField);
                }
            }
        }

        public void Draw()
        {
            Point pos = new Point();
            pos.X = startingXpos;
            pos.Y = GameManager.Height / 8;

            //kirajzolás sor és oszlopindex alapján
            for (int i = 0; i < board.Y; i++)
            {
                for (int j = 0; j < board.X; j++)
                {
                    if (board.getField(i, j) != null)
                    {
                        Rectangle rectangle = new Rectangle(pos.X, pos.Y, fieldWidth - gap, fieldHeight - gap);

                        if (board.getField(i, j).Available)
                        {
                            GameManager.Instance.spriteBatch.Draw(
                        board.getField(i, j).ImageProperty.ImageTexture,
                        rectangle,
                        Color.White);
                        }
                        else
                        {
                            GameManager.Instance.spriteBatch.Draw(
                        board.getField(i, j).ImageProperty.ImageTexture,
                        rectangle,
                        Color.Gray);
                        }

                        if (fieldPositionDictionary.ContainsKey(board.getField(i, j)))
                        {
                            fieldPositionDictionary[board.getField(i, j)] = rectangle;
                        }
                        else
                        {
                            fieldPositionDictionary.Add(board.getField(i, j), rectangle);
                        }
                    }

                    pos.X += fieldWidth;
                }
                pos.X = startingXpos;
                pos.Y += ((6 * GameManager.Height / 8) / board.Y);
            }
        }
    }
}