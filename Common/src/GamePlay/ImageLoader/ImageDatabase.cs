using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixeek.Game;
using System.Collections.Generic;
using System.IO;

namespace Pixeek.ImageLoader
{
    public class ImageDatabase
    {
        List<Image> images = new List<Image>();

        public List<Image> getAllPictures()
        {
            return images;
        }
        public void LoadContent()
        {
            System.IO.Stream imgStream = TitleContainer.OpenStream(GameManager.Instance.Content.RootDirectory + "/images.txt");
            System.IO.StreamReader reader = new System.IO.StreamReader(imgStream);
            string line = reader.ReadLine();
            while (line != null)
            {
                string[] data = line.Split(new char[] { ' ' });
                int lastPos = data[0].LastIndexOf('.');
                string name = data[0].Substring(0, lastPos);
                LoadImage(name, data[1]);
                line = reader.ReadLine();
            }
        }

        private void LoadImage(string fname, string name)
        {
            Image image = new Image()
            {
                Name = name,
                ImageTexture = GameManager.Instance.Content.Load<Texture2D>(fname)
            };
            images.Add(image);
        }

        public static Image LoadImageFromBase64String(GraphicsDevice gd, string name, string base64)
        {
            byte[] byteArray = System.Convert.FromBase64String(base64);
            Texture2D image = Texture2D.FromStream(gd, new MemoryStream(byteArray));
            return new Image() { ImageTexture = image, Name = name };
        }
    }
}