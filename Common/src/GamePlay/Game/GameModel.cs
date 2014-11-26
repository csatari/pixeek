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
        private Board board;

        public GameModel(ImageDatabase imageDatabase)
        {
            this.imageDatabase = imageDatabase;
        }

        public static GameModel getInstance()
        {
            if (jatekModell == null)
            {
                ImageDatabase imageDatabase = new ImageDatabase();
                imageDatabase.LoadContent();
                jatekModell = new GameModel(imageDatabase);
            }
            return jatekModell;
        }

        public void Initialize()
        {
            CreateUpperMenu();
            List<Image> lista = imageDatabase.getAllPictures();
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
        }

        private void CreateUpperMenu()
        {
            UpperMenu.Instance.Draw();
        }


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
                });
                exitButton.AddChild(new Pixeek.Menu.MenuSpriteElement("GUI/button_bg", exitRect, "MENU"));
                root.AddChild(exitButton);

            }

            public void setTimerText(string txt)
            {
                timerBackground.TimerText = txt;
            }
        }
    }
}