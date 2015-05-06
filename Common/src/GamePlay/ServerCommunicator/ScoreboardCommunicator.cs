using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Pixeek.Game;
using Pixeek.ServerCommunicator.Objects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace Pixeek.ServerCommunicator
{
    /// <summary>
    /// Singleton object for communicating with the server about the scoreboard
    /// </summary>
    public class ScoreboardCommunicator : ServerCommunicator
    {
        private static ScoreboardCommunicator instance;

        //singleton Instance
        public static ScoreboardCommunicator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScoreboardCommunicator();
                }
                return instance;
            }
        }

        public delegate void Top10Score(ScoreboardResponse scr);
        public delegate void ScoreSent();

        public void getTop10Scores(GameMode gameMode, Difficulty difficulty, Top10Score ts)
        {
            if (gameMode == GameMode.ENDLESS || gameMode == GameMode.FIGHT || gameMode == GameMode.TIMER)
            {
                throw new InvalidParameterException("There is no scoreboard for endless, fight or timer mode");
            }
            string gmStr = gameMode.ToString().ToLower();
            string diffStr = difficulty.ToString().ToLower();

            sendGetCommand("/scoreboard/" + gmStr + "/" + diffStr,
                delegate(String s)
                {
                    ts(JsonConvert.DeserializeObject<ScoreboardResponse>(s));
                });
        }

        public void sendScore(GameMode gameMode, Difficulty difficulty, ScoreboardRequest scoreboard, ScoreSent scoreSent)
        {
            if (gameMode == GameMode.ENDLESS || gameMode == GameMode.FIGHT || gameMode == GameMode.TIMER)
            {
                throw new InvalidParameterException("There is no scoreboard for endless, fight or timer mode");
            }
            string gmStr = gameMode.ToString().ToLower();
            string diffStr = difficulty.ToString().ToLower();
            sendPutCommand("/register-score/" + gmStr + "/" + diffStr,scoreboard,
                delegate(String s)
                {
                    scoreSent();
                });

        }
    }
}