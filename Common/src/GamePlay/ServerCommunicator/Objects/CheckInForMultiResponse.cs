using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pixeek.ServerCommunicator.Objects
{
    [Serializable]
    public class CheckInForMultiResponse
    {
        public string port { get;set; }
    }
}