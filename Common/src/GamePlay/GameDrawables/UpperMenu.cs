using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixeek.Game;
using System.Collections.Generic;
using System.Timers;

namespace Pixeek.GameDrawables
{
    /// <summary>
    /// Ez az oszt�ly felel�s a fels� men� kirajzol�s��rt
    /// </summary>
    public class UpperMenu
    {
        private static UpperMenu _instance = null;
        public TimeDrawable timerBackground;
        public ScoreDrawable scoreDrawable;
        //public RectangleOverlay centerBackground;
        public Pixeek.Menus.Elements.MenuElement root;

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
        /// Lek�rdezi az �sszes kirajzolt komponenst egy list�ban
        /// </summary>
        /// <returns></returns>
        public List<DrawableGameComponent> getAllComponents()
        {
            List<DrawableGameComponent> list = new List<DrawableGameComponent>();

            list.Add(timerBackground);
            list.Add(scoreDrawable);

            return list;
        }

        public void Draw(Scoring scoring)
        {
            timerBackground = new TimeDrawable(GameManager.Instance);
            scoreDrawable = new ScoreDrawable(GameManager.Instance);
            scoreDrawable.Scoring = scoring;
            //centerBackground = new RectangleOverlay(new Rectangle(GameManager.Width / 7, 0, 5*GameManager.Width / 7, GameManager.Height / 8), new Color(227,227,227), 100, GameManager.Instance);

            GameManager.Instance.Components.Add(timerBackground);
            GameManager.Instance.Components.Add(scoreDrawable);

            root = new Menus.Elements.MenuElement();

            Rectangle exitRect = new Rectangle(6 * GameManager.Width / 7, 0, GameManager.Width / 7, GameManager.Height / 8);
            Pixeek.Menus.Elements.MenuButtonElement exitButton = new Pixeek.Menus.Elements.MenuButtonElement(exitRect, delegate()
            {
               
                ExitHandler();
                //Menu.CreateMainMenu();
                //System.Random random = new System.Random();
                //BoardDrawable.Instance.board.getField(0, 0).ImageProperty = GameModel.Instance.imageDatabase.getAllPictures()[random.Next(GameModel.Instance.imageDatabase.getAllPictures().Count)];
            });
            exitButton.AddChild(new Pixeek.Menus.Elements.MenuSpriteElement("GUI/button_bg", exitRect, "MENU"));
            root.AddChild(exitButton);

        }

        //Be�ll�tja az id�z�t� sz�veg�t
        public void setTimerText(string txt)
        {
            timerBackground.TimerText = txt;
        }
        public string getTimerText()
        {
            return timerBackground.TimerText;
        }
    }
}