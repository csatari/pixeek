using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixeek.Game;
using Pixeek.ServerCommunicator.Objects;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Pixeek.ServerCommunicator
{
    /// <summary>
    /// Singleton object for communicating with the server about single player games
    /// </summary>
    public class SinglePlayerGameCommunicator : ServerCommunicator
    {
        private static SinglePlayerGameCommunicator instance;

        //singleton Instance
        public static SinglePlayerGameCommunicator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SinglePlayerGameCommunicator();
                }
                return instance;
            }
        }

        public delegate void NewBoardHandler(NewBoardResponse scr);
        public delegate void NewTileHandler(NewTileResponse scr);

        public void StartSinglePlayer(Difficulty difficulty, int numberOfFields, NewBoardHandler newBoardHandler)
        {
            string diffStr = difficulty.ToString().ToLower();

            SendGetCommand("/new-board/" + diffStr + "/" + numberOfFields,
                delegate(String s)
                {
                    newBoardHandler(fastJSON.JSON.ToObject<NewBoardResponse>(s));
                });
        }

        public void GetNewTile(Difficulty difficulty, NewTileHandler newTileHandler)
        {
            string diffStr = difficulty.ToString().ToLower();

            SendGetCommand("/new-tile/" + diffStr,
                delegate(String s)
                {
                    newTileHandler(fastJSON.JSON.ToObject<NewTileResponse>(s));
                });
        }
    }
}