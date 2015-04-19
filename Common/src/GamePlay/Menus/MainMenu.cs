using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Pixeek;
using Pixeek.ImageLoader;
using Pixeek.Menus.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.Menus
{
    public class MainMenu : Menu, GameManager.Scene
    {
        private static MainMenu instance;

        //singleton Instance
        public static MainMenu Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainMenu();
                }
                return instance;
            }
        }

        private MainMenu() : base() { }

        public override void Initialize()
        {
            base.Initialize();
            preloadImages();
        }

        public override void LoadContent() { }

        public override void DrawMenu()
        {
            
            MenuSpriteElement bg = new MenuSpriteElement("GUI/menu_bg.jpg", new Rectangle(0, 0, GameManager.Width, GameManager.Height));
            Root.AddChild(bg);

            {
                Rectangle exitRect = new Rectangle(1, 1, 151, 71);
                MenuButtonElement exitButton = new MenuButtonElement(exitRect, delegate()
                {
                    GameManager.Instance.Exit();
                });
                exitButton.AddChild(new MenuSpriteElement("GUI/button_bg", exitRect, "EXIT"));
                bg.AddChild(exitButton);
            }
            {
                Rectangle playRect = new Rectangle(Convert.ToInt32(0.65 * GameManager.Width), Convert.ToInt32(0.35 * GameManager.Height),
                                                    Convert.ToInt32(0.265 * GameManager.Width), Convert.ToInt32(0.104 * GameManager.Height));
                MenuButtonElement playButton = new MenuButtonElement(playRect,
                    delegate()
                    {
                        Menu.GoToScene(NewGameMenu.Instance);
                    }
                    );
                bg.AddChild(playButton);
                playButton.AddChild(new MenuSpriteElement("GUI/newgame_button.png", playRect));
            }
        }

        #region Image cache

        public static ImageDatabase imageDatabase;

        /// <summary>
        /// Előtölti a képeket, hogy ne a new game megnyomására akadjon be, hanem amikor belépünk a játékba
        /// </summary>
        public static void preloadImages()
        {
            imageDatabase = new ImageDatabase();
            imageDatabase.LoadContent();
        }
        #endregion
    }
}
