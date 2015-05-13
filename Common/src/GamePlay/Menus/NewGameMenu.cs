using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Pixeek;
using Pixeek.BoardShapes;
using Pixeek.Game;
using Pixeek.GameDrawables;
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
        public static bool Tutorial { get; set; }

        private NewGameMenu() : base() 
        {
            //Tutorial = false;
        }

        public override void LoadContent() 
        {
            
        }

        static bool music = false;
        static bool vibration = false;
        static String musicText = "MUSIC: OFF";
        static string vibText = "VIBRATION: OFF";
        static MenuSpriteElement musicSpriteElement;
        static Rectangle musicRect;
        static Rectangle vibRect;
        static Rectangle playRect;
        static MenuSpriteElement vibrationSpriteElement;
        private Game.GameModel gameModel;
        static TutorialElement tutorial;
        
        static MainMenuPlainSelector<GameMode> gamemodeSelector;
        static MainMenuPlainSelector<Difficulty> difficultySelector;

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
                musicRect = new Rectangle(GameManager.Width - 500, 1, 151, 71);
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
                vibRect = new Rectangle(GameManager.Width - 345, 1, 190, 71);
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
                difficultySelector = new MainMenuPlainSelector<Difficulty>(Difficulty.NORMAL);
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
                gamemodeSelector = new MainMenuPlainSelector<GameMode>(GameMode.NORMAL);
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
                playRect = new Rectangle(Convert.ToInt32(0.78125 * GameManager.Width), Convert.ToInt32(0.444 * GameManager.Height), Convert.ToInt32(0.114 * GameManager.Width), Convert.ToInt32(0.0583 * GameManager.Height));
                MenuButtonElement playButton = new MenuButtonElement(playRect,
                    delegate()
                    {
                        if (gamemodeSelector.Selected == GameMode.FIGHT || gamemodeSelector.Selected == GameMode.TIMER) return; //nincs megvalósítva a multiplayer :(

                        gameModel = new Game.GameModel(MainMenu.imageDatabase, gamemodeSelector.Selected, difficultySelector.Selected, music, vibration, null, null);
                        NewGame(gamemodeSelector.Selected, difficultySelector.Selected, music, vibration);
                        GameManager.Instance.SwitchScene(gameModel);
                    }
                );
                bg.AddChild(playButton);
                playButton.AddChild(new MenuSpriteElement("GUI/button_play.png", playRect));
            }

            if (NewGameMenu.Tutorial)
            {
                //tutorial
                tutorial = new TutorialElement();

                bg.AddChild(tutorial);

                tutorial.AddRectangle(new Point((GameManager.Width / 2) + 50, GameManager.Height / 3), new Point((GameManager.Width / 2) + 50, GameManager.Height / 3), 
                    "This is the first place you see when you want to play.");
                tutorial.AddRectangle(new Point(gamemodeSelector.BaseX, gamemodeSelector.BaseY), new Point(gamemodeSelector.BaseX+gamemodeSelector.Width, gamemodeSelector.BaseY+gamemodeSelector.GetHeight()), 
                    "Here you can select the gamemode you want");
                tutorial.AddRectangle(new Point(gamemodeSelector.BaseX, gamemodeSelector.BaseY), new Point(gamemodeSelector.BaseX + gamemodeSelector.Width, gamemodeSelector.BaseY + gamemodeSelector.GetHeightOfElement(1)),
                    "In Normal mode you have a fixed number of images \nand you need to find them fast for higher scores");
                tutorial.AddRectangle(new Point(gamemodeSelector.BaseX, gamemodeSelector.BaseY + gamemodeSelector.GetHeightOfElement(1)), new Point(gamemodeSelector.BaseX + gamemodeSelector.Width, gamemodeSelector.BaseY + gamemodeSelector.GetHeightOfElement(2)),
                    "In Endless mode you have infinite number of images for training");
                tutorial.AddRectangle(new Point(gamemodeSelector.BaseX, gamemodeSelector.BaseY + gamemodeSelector.GetHeightOfElement(2)), new Point(gamemodeSelector.BaseX + gamemodeSelector.Width, gamemodeSelector.BaseY + gamemodeSelector.GetHeightOfElement(3)),
                    "In Time mode you have a fixed number of images \nbut only half minutes. Your score will be multiplied\nwith the seconds remaining");
                tutorial.AddRectangle(new Point(gamemodeSelector.BaseX, gamemodeSelector.BaseY), new Point(gamemodeSelector.BaseX + gamemodeSelector.Width, gamemodeSelector.BaseY + gamemodeSelector.GetHeight()),
                    "Please select a gamemode!");

                tutorial.AddRectangle(new Point(difficultySelector.BaseX, difficultySelector.BaseY), new Point(difficultySelector.BaseX + difficultySelector.Width, difficultySelector.BaseY + difficultySelector.GetHeight()),
                    "You can choose from difficulties, we have three here");
                tutorial.AddRectangle(new Point(difficultySelector.BaseX, difficultySelector.BaseY), new Point(difficultySelector.BaseX + difficultySelector.Width, difficultySelector.BaseY + gamemodeSelector.GetHeightOfElement(1)),
                    "In easy mode you get less tasks and pure images");
                tutorial.AddRectangle(new Point(difficultySelector.BaseX, difficultySelector.BaseY + difficultySelector.GetHeightOfElement(1)), new Point(difficultySelector.BaseX + difficultySelector.Width, difficultySelector.BaseY + difficultySelector.GetHeightOfElement(2)),
                    "In normal mode you get average tasks and \nsome of the images are rotated, blurred or colored");
                tutorial.AddRectangle(new Point(difficultySelector.BaseX, difficultySelector.BaseY + difficultySelector.GetHeightOfElement(2)), new Point(difficultySelector.BaseX + difficultySelector.Width, difficultySelector.BaseY + difficultySelector.GetHeightOfElement(3)),
                    "In hard mode you get a lot of tasks and \nall of the images are rotated, blurred or colored");
                tutorial.AddRectangle(new Point(difficultySelector.BaseX, difficultySelector.BaseY), new Point(difficultySelector.BaseX + difficultySelector.Width, difficultySelector.BaseY + difficultySelector.GetHeight()),
                    "Please choose a mode if the selected is not good for you");

                tutorial.AddRectangle(new Point(musicRect.Left, musicRect.Top), new Point(musicRect.Right, musicRect.Bottom),
                    "You can set the music here, if you want");
                tutorial.AddRectangle(new Point(vibRect.Left, vibRect.Top), new Point(vibRect.Right, vibRect.Bottom),
                    "And the vibration too");

                tutorial.AddRectangle(new Point(playRect.Left-10, playRect.Top-10), new Point(playRect.Right+10, playRect.Bottom+10),
                    "If all set, please click on the Play button now");
                tutorial.ShowNextTutorial();
            }

        }

        public void NewGame(GameMode gameMode, Difficulty difficulty, bool music, bool vibration )
        {
            if (gameMode == GameMode.ENDLESS || gameMode == GameMode.NORMAL || gameMode == GameMode.TIME)
            {
                IBoardShapes boardShape = CreateBoardShape();
                int pictureCount = boardShape.GetFieldCount(difficulty);

                LevelManager levelManager = new LevelManager();

                SinglePlayerGameCommunicator.Instance.StartSinglePlayer(difficulty, pictureCount,
                    delegate(ServerCommunicator.Objects.NewBoardResponse nr)
                    {
                        List<Image> imageList = ServerCommunicator.Objects.NewBoardResponse.getImagesFromResponse(GameManager.Instance.GraphicsDevice, nr);
                        Board board = levelManager.NewGame(gameMode, difficulty, boardShape, imageList);
                        gameModel.SetLevelManager(levelManager);
                        gameModel.SetBoard(board);

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
        private IBoardShapes CreateBoardShape()
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
