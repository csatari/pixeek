using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pixeek.GameDrawables;
using Pixeek.ImageLoader;
using Pixeek.Saving;
using Pixeek.SoundVibration;
using System.Collections;
using System.Collections.Generic;
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

            board = levelManager.newGame(GameMode.NORMAL, Difficulty.EASY,imageDatabase.getAllPictures());
            BoardDrawable _boardDrawable = new BoardDrawable(board);
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
        }

        public void Draw(GameTime gameTime)
        {
            UpperMenu.Instance.setTimerText(gameTime.TotalGameTime.ToString("mm\\:ss"));
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
                    Menu.CreateMainMenu();
                    //System.Random random = new System.Random();
                    //BoardDrawable.Instance.board.getField(0, 0).ImageProperty = GameModel.Instance.imageDatabase.getAllPictures()[random.Next(GameModel.Instance.imageDatabase.getAllPictures().Count)];
                });
                exitButton.AddChild(new Pixeek.Menu.MenuSpriteElement("GUI/button_bg", exitRect, "MENU"));
                root.AddChild(exitButton);

            }

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

            public BoardDrawable(Board board)
            {
                this.board = board;
                _instance = this;
            }

            public static BoardDrawable Instance
            {
                get { return _instance; }
                set { _instance = value; }
            }

            public void Update(GameTime gameTime)
            {
            }

            public void Draw()
            {
                Point pos = new Point();
                pos.X = 0;
                pos.Y = GameManager.Height / 8;

                int fieldWidth = GameManager.Width / board.X;
                int fieldHeight = (6 * GameManager.Height / 8) / board.Y;

                //ne legyenek széthúzott mezõk, ezért a kisebb méretet alkalmazom a másik méretnél
                if(fieldWidth > fieldHeight) {
                    fieldWidth = fieldHeight;
                }
                else {
                    fieldHeight = fieldWidth;
                }
                //kirajzolás
                for (int i = 0; i < board.Y; i++)
                {
                    for (int j = 0; j < board.X; j++)
                    {
                        GameManager.Instance.spriteBatch.Draw(
                            board.getField(i, j).ImageProperty.ImageTexture,
                            new Rectangle(pos.X, pos.Y, fieldWidth, fieldHeight),
                            Color.White);

                        pos.X += GameManager.Width/board.X;
                    }
                    pos.X = 0;
                    pos.Y += (6*GameManager.Height/8) / board.Y;
                }
            }
        }
    }
}