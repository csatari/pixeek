using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pixeek.ServerCommunicator.Objects
{
    [Serializable]
    public class FightNotifyRequest
    {
        public int row_index { get; set; }
        public int col_index { get; set; }
        public int score { get; set; }
    }
}