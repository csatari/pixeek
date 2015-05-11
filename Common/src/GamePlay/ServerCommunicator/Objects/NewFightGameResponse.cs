using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pixeek.ServerCommunicator.Objects
{
    [Serializable]
    public class NewFightGameResponse
    {
        public string opponent { get; set; }

        public class layout
        {
            public int height { get; set; }
            public int width { get; set; }
            public bool[] active_fields { get; set; }
        }

        public List<Board> board { get; set; }

        public class Board
        {
            public int row_index { get; set; }
            public int col_index { get; set; }
            public string image { get; set; }
            public string word { get; set; }
        }

        public List<string> to_find { get; set; }
    }
}