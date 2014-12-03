using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.Diagnostics;

namespace Pixeek
{
    public class Prototype : GameManager.Scene
    {
        Dictionary<string, Texture2D> sprites = new Dictionary<string, Texture2D>();
        List<string> names = new List<string>();
        Dictionary<string, string> nameData = new Dictionary<string, string>();

        Dictionary<Point, string> fields = new Dictionary<Point, string>();
        System.Random random = new System.Random();

        const int dWidth = 128;
        const int dHeight = 128;
        const int fruitCount = 16;

        int score = 0;
        const int maxScore = 5;

        string nextToFind = null;

        ButtonState lastButtonState = ButtonState.Released;

        public Prototype()
        {
            
        }

        public void Initialize()
        {
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

                string[] data = line.Split(new char[]{' '});

                int lastPos = data[0].LastIndexOf('.');
                string name = data[0].Substring(0, lastPos);

                LoadImage(name, data[1]);
            }
            
            for (int i = 0; i < System.Math.Min(names.Count, fruitCount); ++i)
            {
                string image = names[random.Next(names.Count)];

                int perRow = GameManager.Instance.GraphicsDevice.Viewport.Width / dWidth;
                Point p = new Point(i % perRow, i / perRow);
                p.X *= dWidth;
                p.Y *= dHeight;

                fields.Add(p, image);
            }
        }

        protected void LoadImage(string fname, string name)
        {
            names.Add(fname);
            sprites[fname] = GameManager.Instance.Content.Load<Texture2D>(fname);
            nameData[fname] = name;
        }

        public void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                 Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                GameManager.Instance.Exit();
            }

            if (nextToFind == null)
            {
                int find = random.Next(fields.Count);
                nextToFind = new List<string>(fields.Values)[find];
            }

            if (lastButtonState == ButtonState.Pressed &&
                Mouse.GetState().LeftButton == ButtonState.Released)
            {
                Point pos = Mouse.GetState().Position;
                pos.X /= dWidth;
                pos.Y /= dHeight;
                pos.X *= dWidth;
                pos.Y *= dHeight;

                if (fields.ContainsKey(pos))
                {
                    string name = fields[pos];
                    if (nameData[name] == nameData[nextToFind])
                    {
                        nextToFind = null;
                        fields[pos] = names[random.Next(names.Count)];
                        ++score;
                    }
                }
            }

            lastButtonState = Mouse.GetState().LeftButton;

            if (score > maxScore)
            {
                Menu.CreateGameOverMenu(true,score,null);
            }
        }

        public void Draw(GameTime gameTime)
        {
            //spriteBatch.Draw(sprites["apple1"], new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            foreach (KeyValuePair<Point, string> kvp in fields)
            {
                GameManager.Instance.spriteBatch.Draw(sprites[kvp.Value], new Rectangle(kvp.Key.X, kvp.Key.Y, dWidth, dHeight), Color.White);
            }

            if (nextToFind != null)
            {
                GameManager.Instance.spriteBatch.DrawString(GameManager.Instance.font, nameData[nextToFind], new Vector2(16, 16),
                    Color.White, 0, new Vector2(10, 100 - GameManager.Instance.GraphicsDevice.Viewport.Height), 1, SpriteEffects.None, 0);
            }
        }
    }
}
