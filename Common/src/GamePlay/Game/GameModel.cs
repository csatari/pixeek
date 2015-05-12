using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Pixeek.BoardShapes;
using Pixeek.GameDrawables;
using Pixeek.ImageLoader;
using Pixeek.Menus;
using Pixeek.Menus.Elements;
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

        private MenuElement loadingElement = new MenuElement();
        private MenuElement tutorialRoot = new MenuElement();
        private TutorialElement tutorial;

       
        public GameModel(ImageDatabase imageDatabase, GameMode mode, Difficulty diff, bool selectedMusic, bool selectedVib, LevelManager lm, Board board)
        {
            this.imageDatabase = imageDatabase;
            gameMode = mode;
            difficulty = diff;
            music = selectedMusic;
            vib = selectedVib;
            levelManager = lm;
            this.board = board;
            Loading = true;
           
#if WINDOWS
            soundAndVibration = new SoundAndVibrationWindows();
#endif 
#if ANDROID
            soundAndVibration = new SoundAndVibrationAndroid();
#endif
        }

        public static GameModel Instance
        {
            get 
            {
                if (jatekModell == null)
                {
                    ImageDatabase imageDatabase = new ImageDatabase();
                    imageDatabase.LoadContent();
                    jatekModell = new GameModel(imageDatabase, GameMode.NORMAL, Difficulty.EASY, false, false, new LevelManager(), null); 
                }
                return jatekModell;
            }
            set 
            {
                jatekModell = value;
            }
        }

        public static bool Loading { get; set; }

        public void Initialize()
        {
            //csak a sima és a duplaérintést engedélyezzük
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap;

            scoring = new Scoring();
            CreateUpperMenu();
            toFindDrawable = new ToFindDrawable(GameManager.Instance);
            GameManager.Instance.Components.Add(toFindDrawable);
        }
        public void SetLevelManager(LevelManager lm)
        {
            levelManager = lm;
            UpperMenu.Instance.SetTimerText(levelManager.ElapsedTime.ToString("mm\\:ss"));
            levelManager.TimeElapsedHandler = delegate(TimeSpan elapsedTime)
            {
                UpperMenu.Instance.SetTimerText(elapsedTime.ToString("mm\\:ss"));
                timeToSave = elapsedTime.TotalSeconds;
            };
            levelManager.TimeStoppedHandler = delegate()
            {
                EndGame(false, null);
            };
            UpperMenu.Instance.ExitHandler = delegate()
            {
                levelManager.EndGame();
#if WINDOWS
                //saveGame(timeToSave, levelManager.ImagesToFind.ToFind, scoring.Score, scoring.Combo);
#endif
                Menu.GoToScene(MainMenu.Instance);
            };
            toFindDrawable.ImagesToFind = levelManager.ImagesToFind;


            levelManager.ImagesToFind.outOfImages = delegate()
            {
                scoring.AddPoint(1); // az utolsó képre még adunk egy pontot
                PlayMultimedia(true);
                string timeText = null;
                if (gameMode == GameMode.TIME)
                {
                    timeText = UpperMenu.Instance.GetTimerText();
                }
                EndGame(true, timeText);
            };

            if (NewGameMenu.Tutorial)
            {
                LoadTutorial();
                levelManager.PauseGame();
                tutorial.ShowNextTutorial();
            }
        }
        public void SetBoard(Board board)
        {
            this.board = board;
            BoardDrawable _boardDrawable = new BoardDrawable(board,
                delegate(Field field)
                {
                    FieldClicked(field);
                });
        }

        public void SaveGame(Double timeToSave, List<Image> imagesToFindSave, int score, int combo)
        {
            List<Image> temp = new List<Image>();
            temp = imageDatabase.GetAllPictures();

            
            savingManager = new Save();
            savingManager.SaveGame(BoardDrawable.Instance.board, timeToSave, imagesToFindSave, score, combo);
        }

        private void FieldClicked(Field field)
        {
            string wasFirst = levelManager.ImagesToFind.ToFind[0].Name;
            bool success = levelManager.TryClickedField(field);
            if (success)
            {
                scoring.AddPoint(1);
                if (NewGameMenu.Tutorial && field.ImageProperty.Name.Equals(wasFirst))
                {
                    levelManager.PauseGame();
                    tutorial.ShowNextTutorial();
                }
            }
            PlayMultimedia(success);
        }

        private void EndGame(bool win, string time)
        {
            GameOverMenu.Instance.Win = win;
            GameOverMenu.Instance.Point = scoring.Score;
            GameOverMenu.Instance.Time = time;
            GameOverMenu.Instance.Difficulty = difficulty;
            GameOverMenu.Instance.GameMode = gameMode;
            Menu.GoToScene(GameOverMenu.Instance);
            levelManager.EndGame();
        }

        /// <summary>
        /// Lejátszik egy hangot, vagy rezegteti a készüléket a sikerességtõl és a beállításoktól függõen
        /// </summary>
        /// <param name="success"></param>
        private void PlayMultimedia(bool success)
        {
            if (success)
            {
                if (music)
                {
                    soundAndVibration.PlaySound();
                }
                if (vib)
                {
                    soundAndVibration.Vibrate();
                }
            }
            else
            {
                if (music)
                {
                    soundAndVibration.PlaySoundBad();
                }
                if (vib)
                {
                    soundAndVibration.VibrateBad();
                }
            }
        }

        private void LoadTutorial()
        {
            tutorial = new TutorialElement();

            tutorialRoot.AddChild(tutorial);

            tutorial.AddRectangle(new Point(0, GameManager.Height / 8),
                                    new Point(GameManager.Width, 7 * GameManager.Height / 8),
                "The main board with the pictures is shown here");
            Rectangle time = UpperMenu.Instance.timerBackground.backgroundArea;
            tutorial.AddRectangle(new Point(time.Left, time.Top), new Point(time.Right, time.Bottom),
                "The timer is set here. In Time mode, it is counting down, otherwise it is counting up");

            tutorial.AddRectangle(new Point(6 * GameManager.Width / 7, 0), new Point(GameManager.Width, GameManager.Height / 8),
                "You can finish your game with the button here");

            tutorial.AddRectangle(new Point(0, 7 * GameManager.Height / 8), new Point(GameManager.Width, GameManager.Height),
                "The images to find can be found in the footer", delegate()
                {
                    levelManager.ContinueGame();
                });

            tutorial.AddRectangle(new Point(0, GameManager.Height / 8),
                                    new Point(GameManager.Width, 7 * GameManager.Height / 8),
                "Try to find " + levelManager.ImagesToFind.ToFind[0].Name);


            tutorial.AddRectangle(new Point(0, GameManager.Height / 8),
                                    new Point(GameManager.Width, 7 * GameManager.Height / 8),
                "That's it, have fun!", delegate()
                {
                    levelManager.ContinueGame();
                    NewGameMenu.Tutorial = false;
                });
        }
        public void LoadContent() 
        {
            loadingElement.AddChild(new MenuSpriteElement(null,
                new Rectangle(Convert.ToInt32(0.3125 * GameManager.Width),
                              Convert.ToInt32(0.28 * GameManager.Height),
                              Convert.ToInt32(0.3125 * GameManager.Width),
                              Convert.ToInt32(0.097 * GameManager.Height)), "Loading..."));
        }

        #region Kattintás és érintés kezelése

        ButtonState lastButtonState = ButtonState.Released;
        TouchCollection currentTouchState;

        public void Update(GameTime gameTime)
        {
            if (Loading) return;
            Point pos = new Point();
            //érintés lekezelése
            currentTouchState = TouchPanel.GetState();

            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();
                switch (gesture.GestureType)
                {
                    case GestureType.DoubleTap:
                        break;
                    case GestureType.Tap:
                        UpperMenu.Instance.root.OnPress(new Point((int)gesture.Position.X, (int)gesture.Position.Y), false);
                        tutorialRoot.OnPress(new Point((int)gesture.Position.X, (int)gesture.Position.Y), false);
                        pos = new Point((int)gesture.Position.X, (int)gesture.Position.Y);
                        break;
                }
            }

            //egér lekezelése
            if (lastButtonState != Mouse.GetState().LeftButton)
            {
                UpperMenu.Instance.root.OnPress(Mouse.GetState().Position, Mouse.GetState().LeftButton == ButtonState.Pressed);
                tutorialRoot.OnPress(Mouse.GetState().Position, Mouse.GetState().LeftButton == ButtonState.Pressed);
                pos = Mouse.GetState().Position;
                lastButtonState = Mouse.GetState().LeftButton;
            }
            else
            {
                UpperMenu.Instance.root.OnHover(Mouse.GetState().Position, Mouse.GetState().LeftButton == ButtonState.Released);
            }
            BoardDrawable.Instance.PositionClicked(pos);


            //BoardDrawable.Instance.Update(gameTime);
        }

        #endregion

        #region Kirajzolás

        public void Draw(GameTime gameTime)
        {
            if (Loading)
            {
                loadingElement.Draw(gameTime, Color.White);
                return;
            }
            
            foreach(DrawableGameComponent component in UpperMenu.Instance.GetAllComponents())  
            {
                component.Draw(gameTime);
            }
            UpperMenu.Instance.root.Draw(gameTime, Color.White);

            toFindDrawable.Draw(gameTime);

            BoardDrawable.Instance.Draw();

            tutorialRoot.Draw(gameTime, Color.White);

        }

        /// <summary>
        /// Megrajzolja a felsõ menüt
        /// </summary>
        private void CreateUpperMenu()
        {
            UpperMenu.Instance.Draw(scoring);
        }

        #endregion
    }
}