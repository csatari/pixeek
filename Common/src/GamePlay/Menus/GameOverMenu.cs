using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Pixeek;
using Pixeek.Game;
using Pixeek.ImageLoader;
using Pixeek.Menus.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.Menus
{
    public class GameOverMenu : Menu, GameManager.Scene
    {
        private static GameOverMenu instance;

        //singleton Instance
        public static GameOverMenu Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameOverMenu();
                }
                return instance;
            }
        }

        private GameOverMenu() : base() { }

        public override void LoadContent() { }

        public bool Win { get; set; }
        public int Point { get; set; }
        public string Time { get; set; }


        public override void DrawMenu()
        {
            MenuSpriteElement bg = new MenuSpriteElement("GUI/menu_bg.jpg", new Rectangle(0, 0, GameManager.Width, GameManager.Height));

            if (Win)
            {
                if (Time != null)
                {
                    drawText("YOU WON!\nYou gained " + Point + " points. Remaining time: " + Time);
                }
                else
                {
                    drawText("YOU WON!\nYou gained " + Point + " points.");
                }
            }
            else
            {
                drawText("GAME OVER!\nYou gained " + Point + " points.");
            }


            Rectangle exitRect = new Rectangle(Convert.ToInt32(0.3125 * GameManager.Width),
                                               Convert.ToInt32(0.4167 * GameManager.Height),
                                               Convert.ToInt32(0.3125 * GameManager.Width),
                                               Convert.ToInt32(0.07 * GameManager.Height));
            MenuButtonElement exitButton = new MenuButtonElement(exitRect, delegate()
            {
                Menu.GoToScene(MainMenu.Instance);
            });
            exitButton.AddChild(new MenuSpriteElement("GUI/button_bg", exitRect, "BACK TO MAIN MENU"));
            Root.AddChild(exitButton);
        }

        private void drawText(string text)
        {
            Root.AddChild(new MenuSpriteElement(null, 
                new Rectangle(Convert.ToInt32(0.3125 * GameManager.Width),
                              Convert.ToInt32(0.28 * GameManager.Height),
                              Convert.ToInt32(0.3125 * GameManager.Width),
                              Convert.ToInt32(0.097 * GameManager.Height)), text));
        }

    }
}
