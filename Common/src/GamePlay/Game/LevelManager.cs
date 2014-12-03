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
        private Timer time;
        private TimeSpan elapsedTime;
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
        /// <returns>A t�bl�val t�r vissza</returns>
        public Board newGame(GameMode gameMode, Difficulty difficulty, IBoardShapes boardAnimal, List<Image> imageList)
        {
            this.gameMode = gameMode;

            board = new Board(imageList);
            board.createBoard(difficulty,boardAnimal);
            ImagesToFind = Game.ImagesToFind.createNewImagesToFind(gameMode, difficulty, board);
            
            if (gameMode == GameMode.TIME)
            {
                elapsedTime = new TimeSpan(0, 0, 5);
            }
            else
            {
                elapsedTime = TimeSpan.Zero;
            }
            TimeElapsedHandler(elapsedTime);
            time.Start();

            return board;
        }

        /// <summary>
        /// Csak akkor kattint r� egy mez�re, ha az el�rhet�
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool tryClickedField(Field field)
        {
            if (field.Available)
            {
                if (ImagesToFind.tryToFindField(field))
                {
                    if (gameMode != GameMode.ENDLESS)
                    {
                        field.Available = false;
                    }
                    else
                    {
                        board.changeField(field);
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
        public void endGame()
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
                elapsedTime = elapsedTime.Subtract(TimeSpan.FromSeconds(1));
            }
            else
            {
                elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
            }
            if (elapsedTime == TimeSpan.Zero)
            {
                time.Stop();
                time.Dispose();
            }
            TimeElapsedHandler(elapsedTime);
        }

        /// <summary>
        /// Amikor lej�r a sz�ml�l�, akkor h�v�dik meg
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void time_Disposed(object sender, EventArgs e)
        {
            TimeStoppedHandler();
        }

    }
}