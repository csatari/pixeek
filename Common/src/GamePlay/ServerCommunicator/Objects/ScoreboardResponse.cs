using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pixeek.ServerCommunicator.Objects
{
    [Serializable]
    public class ScoreboardResponse
    {
        public List<ScoreBoard> scoreboard { get; set; }

        [Serializable]
        public class ScoreBoard
        {
            public string player { get;set; }
            public int score { get; set; }
            public long timestamp { get; set; }
        }
    }
}