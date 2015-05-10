using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Pixeek;
using Pixeek.Game;
using Pixeek.ImageLoader;
using Pixeek.Menus.Elements;
using Pixeek.ServerCommunicator;
using Pixeek.ServerCommunicator.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Pixeek.Menus
{
    public class GameOverMenu : Menu, GameManager.Scene
    {
        private static GameOverMenu instance;
        private static MenuSpriteElement infoElement;
        private static bool scoresSent = false;
        public GameMode GameMode { get; set; }
        public Difficulty Difficulty { get; set; }

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

            //Kilépés gomb
            Rectangle exitRect = new Rectangle(Convert.ToInt32(0.3125 * GameManager.Width),
                                               Convert.ToInt32(0.2167 * GameManager.Height),
                                               Convert.ToInt32(0.3125 * GameManager.Width),
                                               Convert.ToInt32(0.07 * GameManager.Height));
            MenuButtonElement exitButton = new MenuButtonElement(exitRect, delegate()
            {
                Menu.GoToScene(MainMenu.Instance);
            });
            exitButton.AddChild(new MenuSpriteElement("GUI/button_bg", exitRect, "BACK TO MAIN MENU"));
            Root.AddChild(exitButton);

            if (Win)
            {
                //Névbeírás
                Rectangle nameRect = new Rectangle(Convert.ToInt32(0.3125 * GameManager.Width),
                                                   Convert.ToInt32(0.3167 * GameManager.Height),
                                                   Convert.ToInt32(0.2125 * GameManager.Width),
                                                   Convert.ToInt32(0.07 * GameManager.Height));

                MenuTextElement menuText = new MenuTextElement(nameRect);
                Root.AddChild(menuText);

                //Küldő gomb
                Rectangle sendButtonArea = new Rectangle(Convert.ToInt32(0.525 * GameManager.Width),
                                                            Convert.ToInt32(0.3167 * GameManager.Height),
                                                            Convert.ToInt32(0.1 * GameManager.Width),
                                                            Convert.ToInt32(0.07 * GameManager.Height));
                MenuButtonElement sendButton = new MenuButtonElement(sendButtonArea, delegate()
                {
                    SendScores(Point, menuText.Text);
                });
                sendButton.AddChild(new MenuSpriteElement("GUI/button_bg", sendButtonArea, "Send"));
                Root.AddChild(sendButton);

                //Információs szöveg
                Rectangle infoRect = new Rectangle(Convert.ToInt32(0.3125 * GameManager.Width),
                                                   Convert.ToInt32(0.4167 * GameManager.Height),
                                                   Convert.ToInt32(0.3125 * GameManager.Width),
                                                   Convert.ToInt32(0.07 * GameManager.Height));
                infoElement = new MenuSpriteElement(null, infoRect, "");
                Root.AddChild(infoElement);
            }
        }

        private void drawText(string text)
        {
            Root.AddChild(new MenuSpriteElement(null, 
                new Rectangle(Convert.ToInt32(0.3125 * GameManager.Width),
                              Convert.ToInt32(0.08 * GameManager.Height),
                              Convert.ToInt32(0.3125 * GameManager.Width),
                              Convert.ToInt32(0.097 * GameManager.Height)), text));
        }

        private void SendScores(int score, string name)
        {
            if (name.Length == 0)
            {
                infoElement.Text = "Please fill in your name!";
                return;
            }
            if (!scoresSent)
            {
                scoresSent = true;
                infoElement.Text = "Sending...";
                if (GameMode == Game.GameMode.TIME)
                {
                    int time = ((int)Time[3]) * 10 + ((int)Time[4]);
                    score *= time;
                }
                ScoreboardCommunicator.Instance.sendScore(GameMode, Difficulty,
                    new ScoreboardRequest() { player = name, score = score },
                    delegate()
                    {
                        infoElement.Text = "Your score has been registered!";
                    });
            }
        }

    }
}
