using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixeek.BoardShapes;
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
        private Difficulty difficulty;
        public Board board;
        public bool music;
        public bool vib;
        Double timeToSave;
       



        public GameModel(ImageDatabase imageDatabase, GameMode mode, Difficulty diff, bool selectedMusic, bool selectedVib)
        {
            this.imageDatabase = imageDatabase;
            gameMode = mode;
            difficulty = diff;
            music = selectedMusic;
            vib = selectedVib;
           

            soundAndVibration = new SoundAndVibrationWindows();   //target platform to be detected -- Gábor
            
        }

        public static GameModel Instance
        {
            get 
            {
                if (jatekModell == null)
                {
                    ImageDatabase imageDatabase = new ImageDatabase();
                    imageDatabase.LoadContent();
                    jatekModell = new GameModel(imageDatabase, GameMode.NORMAL, Difficulty.EASY, false, false); 
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
                timeToSave = elapsedTime.TotalSeconds;
            };
            levelManager.TimeStoppedHandler = delegate()
            {
                Menu.CreateGameOverMenu(false,scoring.Score,null);
                levelManager.endGame();
            };
            UpperMenu.Instance.ExitHandler = delegate()
            {
                levelManager.endGame();
                saveGame(timeToSave, levelManager.ImagesToFind.ToFind, scoring.Score, scoring.Combo);
                Menu.CreateMainMenu();
            };

            //Létrehozunk egy alakzatot, és átadjuk a pályakészítõnek
            IBoardShapes boardAnimal = new BoardFish();
            board = levelManager.newGame(gameMode, difficulty, boardAnimal, imageDatabase.getAllPictures());

            toFindDrawable.ImagesToFind = levelManager.ImagesToFind;
           

            levelManager.ImagesToFind.outOfImages = delegate()
            {
             
                Menu.CreateGameOverMenu(true, scoring.Score, UpperMenu.Instance.getTimerText());
                levelManager.endGame();
            };

          

            BoardDrawable _boardDrawable = new BoardDrawable(board,
                delegate(Field field) {
                    bool success = levelManager.tryClickedField(field);
                    if (success)
                    {
                        if (music)
                        {
                            soundAndVibration.playSound();
                            soundAndVibration.vibrate();
                        }
                        scoring.addPoint(1);
                        
                    }
                    else
                    {
                        if(music)
                          soundAndVibration.playSoundBad();
                    }
                });
          
        }

        public void saveGame(Double timeToSave, List<Image> imagesToFindSave, int score, int combo)
        {
            List<Image> temp = new List<Image>();
            temp = imageDatabase.getAllPictures();

            
            savingManager = new Save();
            savingManager.save(BoardDrawable.Instance.board, timeToSave, imagesToFindSave, score, combo);
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