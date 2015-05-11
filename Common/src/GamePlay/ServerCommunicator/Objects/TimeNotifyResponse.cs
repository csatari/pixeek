using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pixeek.ServerCommunicator.Objects
{
    [Serializable]
    public class TimeNotifyResponse
    {
        public string purpose { get; set; }

        [Serializable]
        public class new_tile
        {
            public int row_index { get; set; }
            public int col_index { get; set; }
            public string image { get; set; }
            public string word { get; set; }
        }
        public string new_task { get; set; }
    }
}