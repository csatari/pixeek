using Microsoft.Xna.Framework;
using Pixeek.BoardShapes;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Pixeek.Game
{
    public class LevelManager
    {
        private GameMode gameMode;
        private Difficulty difficulty;
        private Timer time;

        private GameTime startTime;

        public TimeSpan ElapsedTime { get; set; }
        public bool Paused { get; set; }
        private Board board;
        public delegate void TimeElapsed(TimeSpan elapsedTime);
        public delegate void TimeStopped();

        /// <summary>
        /// Esem�ny, ami akkor h�v�dik meg, amikor eltelik egy m�sodperc a j�t�kban. Param�terben �t is adja az eltelt/marad�k id�t
        /// </summary>
        public TimeElapsed TimeElapsedHandler
        {
            get;
            set;
        }
        /// <summary>
        /// Akkor h�v�dik meg, amikor letelik az id�
        /// </summary>
        public TimeStopped TimeStoppedHandler
        {
            get;
            set;
        }
        public ImagesToFind ImagesToFind
        {
            get;
            set;
        }

        public LevelManager()
        {
            time = new Timer(1000);
            time.Elapsed += time_Elapsed;
            time.Disposed += time_Disposed;
        }

        /// <summary>
        /// Elind�t egy �j j�t�kot a megadott param�terekket
        /// </summary>
        /// <param name="gameMode">j�t�km�d</param>
        /// <param name="difficulty">neh�zs�g</param>
        /// <param name="imageList">k�pek list�ja</param>
        /// <param name="boardAnimal">a t�bla alakzata, ha null, akkor szimpla n�gyzet lesz</param>
        /// <returns>A t�bl�val t�r vissza</returns>
        /*public Board newGame(GameMode gameMode, Difficulty difficulty, IBoardShapes boardAnimal, List<Image> imageList)
        {
            this.gameMode = gameMode;

            board = new Board(imageList);
            board.createBoard(difficulty, boardAnimal); //TODO az imageList-b�l v�lasszon k�pet
            //board.createBoard_New(difficulty, boardAnimal);
            ImagesToFind = Game.ImagesToFind.createNewImagesToFind(gameMode, difficulty, board);
            
            if (gameMode == GameMode.TIME)
            {
                elapsedTime = new TimeSpan(0, 0, 30);
            }
            else
            {
                elapsedTime = TimeSpan.Zero;
            }
            if(TimeElapsedHandler!=null) TimeElapsedHandler(elapsedTime);
            time.Start();

            return board;
        }*/

        /// <summary>
        /// Elind�t egy �j j�t�kot a megadott param�terekket
        /// </summary>
        /// <param name="gameMode">j�t�km�d</param>
        /// <param name="difficulty">neh�zs�g</param>
        /// <param name="imageList">k�pek list�ja</param>
        /// <param name="boardAnimal">a t�bla alakzata, ha null, akkor szimpla n�gyzet lesz</param>
        /// <returns>A t�bl�val t�r vissza</returns>
        public Board NewGame(GameMode gameMode, Difficulty difficulty, IBoardShapes boardAnimal, List<Image> imageList)
        {
            this.gameMode = gameMode;
            this.difficulty = difficulty;
            board = new Board(imageList);
            board.CreateBoard(difficulty, boardAnimal);
            ImagesToFind = Game.ImagesToFind.CreateNewImagesToFind(gameMode, difficulty, board);

            if (gameMode == GameMode.TIME)
            {
                ElapsedTime = new TimeSpan(0, 0, 30);
            }
            else
            {
                ElapsedTime = TimeSpan.Zero;
            }
            if (TimeElapsedHandler != null) TimeElapsedHandler(ElapsedTime);
            startTime = GameManager.CurrentGameTime;
            Paused = false;
            time.Start();

            return board;
        }

        /// <summary>
        /// Csak akkor kattint r� egy mez�re, ha az el�rhet�
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool TryClickedField(Field field)
        {
            if (field.Available && !Paused)
            {
                if (ImagesToFind.TryToFindField(field))
                {
                    if (gameMode != GameMode.ENDLESS)
                    {
                        field.Available = false;
                    }
                    else
                    {
                        board.ChangeField(field, difficulty,
                            delegate()
                            {
                                ImagesToFind.AddNewImageToFind();
                            });
                    }
                    return true;
                }
                return false;
            }
            else return false;
        }

        /// <summary>
        /// Befejezi a j�t�kot, azaz a sz�ml�l�t null�zza
        /// </summary>
        public void EndGame()
        {
            time.Disposed -= time_Disposed;
            time.Elapsed -= time_Elapsed;
            time.Stop();
            time.Dispose();
        }

        /// <summary>
        /// Minden egyes m�sodpercben megh�v�dik, ahogy telik az id�.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void time_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (gameMode == GameMode.TIME)
            {
                ElapsedTime = ElapsedTime.Subtract(TimeSpan.FromSeconds(1));
            }
            else
            {
                ElapsedTime = ElapsedTime.Add(TimeSpan.FromSeconds(1));
            }
            if (ElapsedTime == TimeSpan.Zero)
            {
                time.Stop();
                time.Dispose();
            }
            if (TimeElapsedHandler != null) TimeElapsedHandler(ElapsedTime);
        }

        /// <summary>
        /// Amikor lej�r a sz�ml�l�, akkor h�v�dik meg
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void time_Disposed(object sender, EventArgs e)
        {
            if(TimeStoppedHandler!=null) TimeStoppedHandler();
        }


        public void PauseGame()
        {
            Paused = true;
            time.Stop();
        }
        public void ContinueGame()
        {
            Paused = false;
            time.Start();
        }

    }
}