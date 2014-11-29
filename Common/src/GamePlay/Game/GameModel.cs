using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pixeek.GameDrawables;
using Pixeek.ImageLoader;
using Pixeek.Saving;
using Pixeek.SoundVibration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace Pixeek.Game
{
    public class GameModel : GameManager.Scene
    {
        private SoundAndVibration soundAndVibration;
        private Timer timeManager;
        private static GameModel jatekModell = null;
        private ImageDatabase imageDatabase;
        private Save savingManager;
        private LevelManager levelManager;
        private Scoring scoring;
        public Board board;

        public GameModel(ImageDatabase imageDatabase)
        {
            this.imageDatabase = imageDatabase;
        }

        public static GameModel Instance
        {
            get 
            {
                if (jatekModell == null)
                {
                    ImageDatabase imageDatabase = new ImageDatabase();
                    imageDatabase.LoadContent();
                    jatekModell = new GameModel(imageDatabase);
                }
                return jatekModell;
            }
            set 
            {
                jatekModell = value;
            }
        }

        public void Initialize()
        {
            CreateUpperMenu();
            //új játék indítása
            levelManager = new LevelManager();
            levelManager.TimeElapsedHandler = delegate(TimeSpan elapsedTime)
            {
                UpperMenu.Instance.setTimerText(elapsedTime.ToString("mm\\:ss"));
            };
            levelManager.TimeStoppedHandler = delegate()
            {
                Menu.CreateMainMenu();
            };
            UpperMenu.Instance.ExitHandler = delegate()
            {
                levelManager.endGame();
                Menu.CreateMainMenu();
            };

            board = levelManager.newGame(GameMode.NORMAL, Difficulty.NORMAL,imageDatabase.getAllPictures());
            BoardDrawable _boardDrawable = new BoardDrawable(board,
                delegate(Field field) {
                    //TODO lekezelni a mezõre kattintást
                });


        }

        public void LoadContent() 
        { 
        }

        ButtonState lastButtonState = ButtonState.Released;

        public void Update(GameTime gameTime)
        {
            if (lastButtonState != Mouse.GetState().LeftButton)
            {
                UpperMenu.Instance.root.OnPress(Mouse.GetState().Position, Mouse.GetState().LeftButton == ButtonState.Pressed);
                lastButtonState = Mouse.GetState().LeftButton;
            }
            else
            {
                UpperMenu.Instance.root.OnHover(Mouse.GetState().Position, Mouse.GetState().LeftButton == ButtonState.Released);
            }

            BoardDrawable.Instance.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            foreach(DrawableGameComponent component in UpperMenu.Instance.getAllComponents())  
            {
                component.Draw(gameTime);
            }
            UpperMenu.Instance.root.Draw(gameTime, Color.White);

            BoardDrawable.Instance.Draw();
        }

        private void CreateUpperMenu()
        {
            UpperMenu.Instance.Draw();
        }

        /// <summary>
        /// Ez az osztály felelõs a felsõ menü kirajzolásáért
        /// </summary>
        private class UpperMenu
        {
            private static UpperMenu _instance = null;
            public TimeDrawable timerBackground;
            public RectangleOverlay centerBackground;
            public Pixeek.Menu.MenuElement root;

            public delegate void Exit();
            public Exit ExitHandler
            {
                get;
                set;
            }

            public UpperMenu()
            {
            }

            public static UpperMenu Instance
            {
                get 
                {
                    if (_instance == null) _instance = new UpperMenu();
                    return _instance;
                }
                set { _instance = value; }
            }

            /// <summary>
            /// Lekérdezi az összes kirajzolt komponenst egy listában
            /// </summary>
            /// <returns></returns>
            public List<DrawableGameComponent> getAllComponents()
            {
                List<DrawableGameComponent> list = new List<DrawableGameComponent>();

                list.Add(timerBackground);
                list.Add(centerBackground);

                return list;
            }

            public void Draw()
            {
                timerBackground = new TimeDrawable(GameManager.Instance);
                centerBackground = new RectangleOverlay(new Rectangle(GameManager.Width / 7, 0, 5*GameManager.Width / 7, GameManager.Height / 8), new Color(227,227,227), 100, GameManager.Instance);

                GameManager.Instance.Components.Add(timerBackground);
                GameManager.Instance.Components.Add(centerBackground);

                root = new Pixeek.Menu.MenuElement();

                Rectangle exitRect = new Rectangle(6*GameManager.Width / 7, 0, GameManager.Width / 7, GameManager.Height / 8);
                Pixeek.Menu.MenuButtonElement exitButton = new Pixeek.Menu.MenuButtonElement(exitRect, delegate()
                {
                    ExitHandler();
                    //Menu.CreateMainMenu();
                    //System.Random random = new System.Random();
                    //BoardDrawable.Instance.board.getField(0, 0).ImageProperty = GameModel.Instance.imageDatabase.getAllPictures()[random.Next(GameModel.Instance.imageDatabase.getAllPictures().Count)];
                });
                exitButton.AddChild(new Pixeek.Menu.MenuSpriteElement("GUI/button_bg", exitRect, "MENU"));
                root.AddChild(exitButton);

            }

            //Beállítja az idõzítõ szövegét
            public void setTimerText(string txt)
            {
                timerBackground.TimerText = txt;
            }
        }
        /// <summary>
        /// Ez az osztály felelõs a Board kirajzolásáért
        /// </summary>
        private class BoardDrawable
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

            public void Update(GameTime gameTime)
            {
                if (lastButtonState == ButtonState.Pressed &&
                Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    //sorindex és oszlopindex kiszámolása az egérkattintásból
                    Point pos = Mouse.GetState().Position;
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
                        //Debug.WriteLine("X: " + clickedField.RowIndex + " Y: "+clickedField.ColumnIndex);
                    }
                    
                }

                lastButtonState = Mouse.GetState().LeftButton;
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
                            Rectangle rectangle = new Rectangle(pos.X, pos.Y, fieldWidth-gap, fieldHeight-gap);
                            
                            GameManager.Instance.spriteBatch.Draw(
                            board.getField(i, j).ImageProperty.ImageTexture,
                            rectangle,
                            Color.White);

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
                    pos.Y += ((6*GameManager.Height/8) / board.Y);
                }
            }
        }
    }
}