using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixeek.Game;
using System.Collections.Generic;

namespace Pixeek.ImageLoader
{
    //TODO nem így kéne csinálni, csak átmásoltam a prototype-ból - albert
    public class ImageDatabase
    {

        List<string> names = new List<string>();
        const int dWidth = 128;
        const int dHeight = 128;

        List<Image> images = new List<Image>();

        public List<Image> getAllPictures()
        {
            return images;
        }
        public void LoadContent()
        {
            System.IO.Stream imgStream = TitleContainer.OpenStream(GameManager.Instance.Content.RootDirectory + "/images.txt");
            System.IO.StreamReader reader = new System.IO.StreamReader(imgStream);
            while (true)
            {
                string line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }

                string[] data = line.Split(new char[] { ' ' });

                int lastPos = data[0].LastIndexOf('.');
                string name = data[0].Substring(0, lastPos);

                LoadImage(name, data[1]);
            }
        }
        protected void LoadImage(string fname, string name)
        {
            Image image = new Image()
            {
                Name = name,
                ImageTexture = GameManager.Instance.Content.Load<Texture2D>(fname)
            };
            images.Add(image);
        }
    }
}