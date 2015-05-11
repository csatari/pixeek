using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixeek.BoardShapes;
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

        //public delegate void CheckInForMultiHandler(CheckInForMultiResponse response);
        public delegate void NewFightGameHandler(NewFightGameResponse response);
        public delegate void NewTimeGameHandler(NewTimeGameResponse response);
        public delegate void WonHandler(GameOverResponse response);

        /*public void checkInForMulti(GameMode gameMode, Difficulty difficulty, CheckInForMultiRequest request, CheckInForMultiHandler newBoardHandler)
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
        }*/


        public void Registration(string name, GameMode gameMode, Difficulty difficulty, IBoardShapes shape)
        {
            if (gameMode == GameMode.ENDLESS || gameMode == GameMode.NORMAL || gameMode == GameMode.TIME)
            {
                throw new InvalidParameterException("There is no multiplayer for normal, time or endless gamemodes");
            }
            string gmStr = gameMode.ToString().ToLower();
            string diffStr = difficulty.ToString().ToLower();

            RegistrationRequest request = new RegistrationRequest()
            {
                difficulty = diffStr,
                layout = shape.ToString(),
                mode = gmStr,
                name = name
            };
            fastJSON.JSON.Parameters.UseExtensions = false;
                
            SendSocket(fastJSON.JSON.ToJSON(request));
        }

        public void SendTimeOut()
        {
            GameOverResponse request = new GameOverResponse()
            {
                purpose = "time_out"
            };
            fastJSON.JSON.Parameters.UseExtensions = false;

            SendSocket(fastJSON.JSON.ToJSON(request));
        }

        public void YouWonListener(WonHandler handler)
        {
            SocketListener(delegate(string response)
            {
                if (handler != null)
                {
                    handler(fastJSON.JSON.ToObject<GameOverResponse>(response));
                }
            });
        }

        public void NewFightGameListener(NewFightGameHandler handler)
        {
            SocketListener(delegate(string response)
            {
                if (handler != null)
                {
                    handler(fastJSON.JSON.ToObject<NewFightGameResponse>(response));
                }
            });
        }

        public void NewTimeGameListener(NewTimeGameHandler handler)
        {
            SocketListener(delegate(string response)
            {
                if (handler != null)
                {
                    handler(fastJSON.JSON.ToObject<NewTimeGameResponse>(response));
                }
            });
        }
    }
}