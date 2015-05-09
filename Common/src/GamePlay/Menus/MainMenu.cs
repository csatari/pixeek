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

        private MainMenu() : base() {
            
            /*ServerCommunicator.ScoreboardCommunicator.Instance.sendScore(Game.GameMode.NORMAL, Game.Difficulty.EASY,
                new ServerCommunicator.ScoreboardRequest() { player = "AAA", score = 1 });*/
            ServerCommunicator.ScoreboardCommunicator.Instance.getTop10Scores(Game.GameMode.NORMAL, Game.Difficulty.EASY,
                delegate(ServerCommunicator.Objects.ScoreboardResponse ts)
                {
                    Console.WriteLine(ts);
                });
            ServerCommunicator.SinglePlayerGameCommunicator.Instance.startSinglePlayer(Game.Difficulty.HARD, 20,
                delegate(ServerCommunicator.Objects.NewBoardResponse nr)
                {
                    Console.WriteLine(nr.ToString());
                });
            ServerCommunicator.SinglePlayerGameCommunicator.Instance.getNewTile(Game.Difficulty.HARD,
                delegate(ServerCommunicator.Objects.NewTileResponse nr)
                {
                    Console.WriteLine(nr.ToString());
                });

            ServerCommunicator.MultiPlayerGameCommunicator.Instance.checkInForMulti(Game.GameMode.FIGHT, Game.Difficulty.EASY,
                new ServerCommunicator.Objects.CheckInForMultiRequest()
                {
                    player_alias = "Albert"
                },
                delegate(ServerCommunicator.Objects.CheckInForMultiResponse nr)
                {
                    Console.WriteLine(nr.ToString());
                });

            ServerCommunicator.MultiPlayerGameCommunicator.Instance.checkInForMulti(Game.GameMode.FIGHT, Game.Difficulty.EASY,
                new ServerCommunicator.Objects.CheckInForMultiRequest()
                {
                    player_alias = "Géza"
                },
                delegate(ServerCommunicator.Objects.CheckInForMultiResponse nr)
                {
                    Console.WriteLine(nr.ToString());
                });
        }

        public override void Initialize()
        {
            base.Initialize();
            //preloadImages();
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
                Rectangle playRect = new Rectangle(Convert.ToInt32(0.64 * GameManager.Width), Convert.ToInt32(0.14 * GameManager.Height),
                                                    Convert.ToInt32(0.265 * GameManager.Width), Convert.ToInt32(0.104 * GameManager.Height));
                MenuButtonElement playButton = new MenuButtonElement(playRect,
                    delegate()
                    {
                        NewGameMenu.SinglePlayer = true;
                        Menu.GoToScene(NewGameMenu.Instance);
                    }
                    );
                bg.AddChild(playButton);
                playButton.AddChild(new MenuSpriteElement("GUI/singleplayer_button.png", playRect));
            }
            {
                Rectangle playRect = new Rectangle(Convert.ToInt32(0.635 * GameManager.Width), Convert.ToInt32(0.31 * GameManager.Height),
                                                    Convert.ToInt32(0.26 * GameManager.Width), Convert.ToInt32(0.09 * GameManager.Height));
                MenuButtonElement playButton = new MenuButtonElement(playRect,
                    delegate()
                    {
                        NewGameMenu.SinglePlayer = false;
                        Menu.GoToScene(NewGameMenu.Instance);
                    }
                    );
                bg.AddChild(playButton);
                playButton.AddChild(new MenuSpriteElement("GUI/multiplayer_button.png", playRect));
            }
            {
                Rectangle playRect = new Rectangle(Convert.ToInt32(0.63 * GameManager.Width), Convert.ToInt32(0.44 * GameManager.Height),
                                                    Convert.ToInt32(0.25 * GameManager.Width), Convert.ToInt32(0.08 * GameManager.Height));
                MenuButtonElement playButton = new MenuButtonElement(playRect,
                    delegate()
                    {
                        Menu.GoToScene(ScoreboardMenu.Instance);
                    }
                    );
                bg.AddChild(playButton);
                playButton.AddChild(new MenuSpriteElement("GUI/scoreboard_button.png", playRect));
            }
            {
                Rectangle playRect = new Rectangle(Convert.ToInt32(0.63 * GameManager.Width), Convert.ToInt32(0.59 * GameManager.Height),
                                                    Convert.ToInt32(0.17 * GameManager.Width), Convert.ToInt32(0.08 * GameManager.Height));
                MenuButtonElement playButton = new MenuButtonElement(playRect,
                    delegate()
                    {
                        
                    }
                    );
                bg.AddChild(playButton);
                playButton.AddChild(new MenuSpriteElement("GUI/tutorial_button.png", playRect));
            }
        }

        #region Image cache

        public static ImageDatabase imageDatabase;

        /// <summary>
        /// Előtölti a képeket, hogy ne a new game megnyomására akadjon be, hanem amikor belépünk a játékba
        /// </summary>
        /*public static void preloadImages()
        {
            imageDatabase = new ImageDatabase();
            imageDatabase.LoadContent();
        }*/
        #endregion
    }
}
