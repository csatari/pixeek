using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Pixeek.Game;
using Pixeek.ImageLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public static List<Image> getImagesFromResponse(GraphicsDevice gd, NewBoardResponse response)
        {
            List<Image> imageList = new List<Image>();
            foreach(Board b in response.board)
            {
                imageList.Add(ImageDatabase.LoadImageFromBase64String(gd, b.word, b.image));
            }
            return imageList;
        }
    }
}