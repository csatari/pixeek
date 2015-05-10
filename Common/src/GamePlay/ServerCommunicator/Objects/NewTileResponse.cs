using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixeek.Game;
using Pixeek.ImageLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Pixeek.ServerCommunicator.Objects
{
    [Serializable]
    public class NewTileResponse
    {
        public string word { get;set; }
        public string image { get; set; }

        public static Image getImagesFromResponse(GraphicsDevice gd, NewTileResponse response)
        {
            return ImageDatabase.LoadImageFromBase64String(gd, response.word, response.image);
        }
    }
}