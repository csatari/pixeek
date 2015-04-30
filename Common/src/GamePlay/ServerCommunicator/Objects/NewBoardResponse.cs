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
    public class NewBoardResponse
    {
        public IList<Board> board { get; set; }

        public class Board
        {
            public string word { get;set; }
            public string image { get; set; }
        }
    }
}