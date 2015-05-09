using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Pixeek;
using Pixeek.BoardShapes;
using Pixeek.Game;
using Pixeek.ImageLoader;
using Pixeek.Menus.Elements;
using Pixeek.ServerCommunicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.Menus
{
    public class NewGameMenu : Menu, GameManager.Scene
    {
        private static NewGameMenu instance;

        //singleton Instance
        public static NewGameMenu Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NewGameMenu();
                }
                return instance;
            }
        }

        public static bool SinglePlayer { get; set; }

        private NewGameMenu() : base() { }

        public override void LoadContent() 
        {
            
        }

        static bool music = false;
        static bool vibration = false;
        static String musicText = "MUSIC: OFF";
        static string vibText = "VIBRATION: OFF";
        static MenuSpriteElement musicSpriteElement;
        static MenuSpriteElement vibrationSpriteElement;
        private Game.GameModel gameModel;

        static MainMenuPlaintSelector<GameMode> gamemodeSelector;
        static MainMenuPlaintSelector<Difficulty> difficultySelector;

        public override void DrawMenu()
        {
            MenuSpriteElement bg = new MenuSpriteElement("GUI/newgame_menu.jpg", new Rectangle(0, 0, GameManager.Width, GameManager.Height));
            Root.AddChild(bg);

            // exit button
            {
                Rectangle exitRect = new Rectangle(GameManager.Width - 152, 1, 151, 71);
                MenuButtonElement exitButton = new MenuButtonElement(exitRect, delegate()
                {
                    Menu.GoToScene(MainMenu.Instance);
                });
                exitButton.AddChild(new MenuSpriteElement("GUI/button_bg", exitRect, "BACK"));
                bg.AddChild(exitButton);
            }

            // music settings
            {
                Rectangle musicRect = new Rectangle(GameManager.Width - 500, 1, 151, 71);
                MenuButtonElement musicButton = new MenuButtonElement(musicRect, delegate()
                {
                    if (!music)
                    {
                        music = true;
                        musicText = "MUSIC: ON";
                    }
                    else
                    {
                        music = false;
                        musicText = "MUSIC: OFF";
                    }
                    musicSpriteElement.Text = musicText;
                });
                musicSpriteElement = new MenuSpriteElement("GUI/button_bg", musicRect, musicText);
                musicButton.AddChild(musicSpriteElement);
                bg.AddChild(musicButton);
            }

            //vibration settings
            {
                Rectangle vibRect = new Rectangle(GameManager.Width - 345, 1, 190, 71);
                MenuButtonElement vibButton = new MenuButtonElement(vibRect, delegate()
                {
                    if (!vibration)
                    {
                        vibration = true;
                        vibText = "VIBRATION: ON";
                    }
                    else
                    {
                        vibration = false;
                        vibText = "VIBRATION: OFF";
                    }
                    vibrationSpriteElement.Text = vibText;
                });
                vibrationSpriteElement = new MenuSpriteElement("GUI/button_bg", vibRect, vibText);
                vibButton.AddChild(vibrationSpriteElement);
                bg.AddChild(vibButton);
            }

            // difficulty selector
            {
                difficultySelector = new MainMenuPlaintSelector<Difficulty>(Difficulty.NORMAL);
                bg.AddChild(difficultySelector);

                difficultySelector.BaseX = Convert.ToInt32(0.279 * GameManager.Width);
                difficultySelector.BaseY = Convert.ToInt32(0.359 * GameManager.Height);
                difficultySelector.YDiff = Convert.ToInt32(0.085 * GameManager.Height);
                difficultySelector.Width = Convert.ToInt32(0.12 * GameManager.Width);
                difficultySelector.Height = Convert.ToInt32(0.077 * GameManager.Height);


                difficultySelector.AddElement("EASY", Difficulty.EASY);
                difficultySelector.AddElement("NORMAL", Difficulty.NORMAL);
                difficultySelector.AddElement("HARD", Difficulty.HARD);
            }

            //gamemode selector
            {
                gamemodeSelector = new MainMenuPlaintSelector<GameMode>(GameMode.NORMAL);
                bg.AddChild(gamemodeSelector);

                gamemodeSelector.BaseX = Convert.ToInt32(0.077 * GameManager.Width);
                gamemodeSelector.BaseY = Convert.ToInt32(0.359 * GameManager.Height);
                gamemodeSelector.YDiff = Convert.ToInt32(0.085 * GameManager.Height);
                gamemodeSelector.Width = Convert.ToInt32(0.15 * GameManager.Width);
                gamemodeSelector.Height = Convert.ToInt32(0.077 * GameManager.Height);

                if (SinglePlayer)
                {
                    gamemodeSelector.AddElement("NORMAL", GameMode.NORMAL);
                    gamemodeSelector.AddElement("ENDLESS", GameMode.ENDLESS);
                    gamemodeSelector.AddElement("TIME", GameMode.TIME);
                    gamemodeSelector.Selected = GameMode.NORMAL;
                }
                else
                {
                    gamemodeSelector.AddElement("FIGHT", GameMode.FIGHT);
                    gamemodeSelector.AddElement("TIMER", GameMode.TIMER);
                    gamemodeSelector.Selected = GameMode.FIGHT;
                }
            }

            //play button
            {
                Rectangle playRect = new Rectangle(Convert.ToInt32(0.78125 * GameManager.Width), Convert.ToInt32(0.444 * GameManager.Height), Convert.ToInt32(0.114 * GameManager.Width), Convert.ToInt32(0.0583 * GameManager.Height));
                MenuButtonElement playButton = new MenuButtonElement(playRect,
                    delegate()
                    {
                        gameModel = new Game.GameModel(MainMenu.imageDatabase, gamemodeSelector.Selected, difficultySelector.Selected, music, vibration, null, null);
                        newGame(gamemodeSelector.Selected, difficultySelector.Selected, music, vibration);
                        GameManager.Instance.SwitchScene(gameModel);
                    }
                );
                bg.AddChild(playButton);
                playButton.AddChild(new MenuSpriteElement("GUI/button_play.png", playRect));
            }
        }
        public void newGame(GameMode gameMode, Difficulty difficulty, bool music, bool vibration )
        {
            if (gameMode == GameMode.ENDLESS || gameMode == GameMode.NORMAL || gameMode == GameMode.TIME)
            {
                IBoardShapes boardShape = createBoardShape();
                int pictureCount = boardShape.getFieldCount(difficulty);

                LevelManager levelManager = new LevelManager();

                SinglePlayerGameCommunicator.Instance.startSinglePlayer(difficulty, pictureCount,
                    delegate(ServerCommunicator.Objects.NewBoardResponse nr)
                    {
                        List<Image> imageList = ServerCommunicator.Objects.NewBoardResponse.getImagesFromResponse(GameManager.Instance.GraphicsDevice, nr);
                        Board board = levelManager.newGame(gameMode, difficulty, boardShape, imageList);
                        gameModel.setLevelManager(levelManager);
                        gameModel.setBoard(board);

                        GameModel.Loading = false;
                    });
            }
            else
            {
                //TODO multiplayer módok
            }
        }

        /// <summary>
        /// Elkészít egy új tábla alakzatot - TODO szerverről kéne kérnie
        /// </summary>
        /// <returns></returns>
        private IBoardShapes createBoardShape()
        {
            IBoardShapes boardAnimal = new BoardFish();
            Random random = new Random();
            int r = random.Next(3);
            switch (r)
            {
                case 0: { boardAnimal = new BoardDiamond(); break; }
                case 1: { boardAnimal = new BoardFish(); break; }
                default: { boardAnimal = new BoardRectangle(); break; }
            }
            return boardAnimal;
        }
    }
}
