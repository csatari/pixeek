using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private static GameModel jatekModell = null;
        private ImageDatabase imageDatabase;
        private Save savingManager;
        private LevelManager levelManager;
        private Scoring scoring;
        private ToFindDrawable toFindDrawable;
        private GameMode gameMode;
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
            scoring = new Scoring();
            CreateUpperMenu();
            toFindDrawable = new ToFindDrawable(GameManager.Instance);
            GameManager.Instance.Components.Add(toFindDrawable);
            //új játék indítása
            levelManager = new LevelManager();
            levelManager.TimeElapsedHandler = delegate(TimeSpan elapsedTime)
            {
                UpperMenu.Instance.setTimerText(elapsedTime.ToString("mm\\:ss"));
            };
            levelManager.TimeStoppedHandler = delegate()
            {
                //TODO letelt a játék, kéne valami gameover
                levelManager.endGame();
                Menu.CreateMainMenu();
            };
            UpperMenu.Instance.ExitHandler = delegate()
            {
                levelManager.endGame();
                Menu.CreateMainMenu();
            };

            gameMode = GameMode.NORMAL;
            board = levelManager.newGame(gameMode, Difficulty.NORMAL, imageDatabase.getAllPictures());

            toFindDrawable.ImagesToFind = levelManager.ImagesToFind;

            levelManager.ImagesToFind.outOfImages = delegate()
            {
                //TODO letelt a játék, kéne valami gameover
                levelManager.endGame();
                Menu.CreateMainMenu();
            };


            #region save and load test
            /*
            List<Image> temp = new List<Image>();
            temp = imageDatabase.getAllPictures();

            savingManager = new Save();
            savingManager.save(board);
            board = savingManager.load(temp);
            */
            #endregion


            BoardDrawable _boardDrawable = new BoardDrawable(board,
                delegate(Field field) {
                    bool success = levelManager.tryClickedField(field);
                    if (success)
                    {
                        scoring.addPoint(1);
                    }
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

            toFindDrawable.Draw(gameTime);

            BoardDrawable.Instance.Draw();
        }

        /// <summary>
        /// Megrajzolja a felsõ menüt
        /// </summary>
        private void CreateUpperMenu()
        {
            UpperMenu.Instance.Draw(scoring);
        }
    }
}