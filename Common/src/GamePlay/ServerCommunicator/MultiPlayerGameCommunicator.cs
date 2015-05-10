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
    /// Singleton object for communicating with the server about multi player games
    /// </summary>
    public class MultiPlayerGameCommunicator : ServerCommunicator
    {
        private static MultiPlayerGameCommunicator instance;

        //singleton Instance
        public static MultiPlayerGameCommunicator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MultiPlayerGameCommunicator();
                }
                return instance;
            }
        }

        public delegate void CheckInForMultiHandler(CheckInForMultiResponse response);

        public void checkInForMulti(GameMode gameMode, Difficulty difficulty, CheckInForMultiRequest request, CheckInForMultiHandler newBoardHandler)
        {
            if (gameMode == GameMode.ENDLESS || gameMode == GameMode.NORMAL || gameMode == GameMode.TIME)
            {
                throw new InvalidParameterException("There is no multiplayer for normal, time or endless gamemodes");
            }
            string gmStr = gameMode.ToString().ToLower();
            string diffStr = difficulty.ToString().ToLower();

            sendPostCommand("/check-in-for-multi/" + gmStr + "/" + diffStr,request,
                delegate(String s)
                {
                    newBoardHandler(fastJSON.JSON.ToObject<CheckInForMultiResponse>(s));
                });
        }
    }
}