using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pixeek.ServerCommunicator.Objects
{
    [Serializable]
    public class RegistrationRequest
    {
        public string name { get; set; }
        public string mode { get; set; }
        public string difficulty { get; set; }
        public string layout { get; set; }
    }
}