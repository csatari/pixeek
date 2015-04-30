using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Pixeek.ServerCommunicator.Objects
{
    public class ScoreboardResponse
    {
        public IList<ScoreBoard> scoreboard { get; set; }

        public class ScoreBoard
        {
            public string player { get;set; }
            public int score { get; set; }
            public long timestamp { get; set; }
        }
    }
}