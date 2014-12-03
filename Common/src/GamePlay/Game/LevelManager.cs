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
        /// Esemény, ami akkor hívódik meg, amikor eltelik egy másodperc a játékban. Paraméterben át is adja az eltelt/maradék idõt
        /// </summary>
        public TimeElapsed TimeElapsedHandler
        {
            get;
            set;
        }
        /// <summary>
        /// Akkor hívódik meg, amikor letelik az idõ
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
        /// Elindít egy új játékot a megadott paraméterekket
        /// </summary>
        /// <param name="gameMode">játékmód</param>
        /// <param name="difficulty">nehézség</param>
        /// <param name="imageList">képek listája</param>
        /// <returns>A táblával tér vissza</returns>
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
        /// Csak akkor kattint rá egy mezõre, ha az elérhetõ
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
        /// Befejezi a játékot, azaz a számlálót nullázza
        /// </summary>
        public void endGame()
        {
            time.Disposed -= time_Disposed;
            time.Elapsed -= time_Elapsed;
            time.Stop();
            time.Dispose();
        }

        /// <summary>
        /// Minden egyes másodpercben meghívódik, ahogy telik az idõ.
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
        /// Amikor lejár a számláló, akkor hívódik meg
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void time_Disposed(object sender, EventArgs e)
        {
            TimeStoppedHandler();
        }

    }
}