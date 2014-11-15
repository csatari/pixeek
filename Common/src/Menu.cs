using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.Diagnostics;
/*
namespace Pixeek
{
    public class Prototype : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        Dictionary<string, Texture2D> sprites = new Dictionary<string, Texture2D>();
        List<string> names = new List<string>();
        Dictionary<string, string> nameData = new Dictionary<string, string>();

        Dictionary<Point, string> fields = new Dictionary<Point, string>();
        System.Random random = new System.Random();

        const int dWidth = 128;
        const int dHeight = 128;
        const int fruitCount = 16;

        string nextToFind = null;

        ButtonState lastButtonState = ButtonState.Released;

        public Prototype()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("spriteFont1");

            System.IO.Stream imgStream = TitleContainer.OpenStream("Content/images.txt");
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

                int perRow = GraphicsDevice.Viewport.Width / dWidth;
                Point p = new Point(i % perRow, i / perRow);
                p.X *= dWidth;
                p.Y *= dHeight;

                fields.Add(p, image);
            }
        }

        protected void LoadImage(string fname, string name)
        {
            names.Add(fname);
            sprites[fname] = Content.Load<Texture2D>(fname);
            nameData[fname] = name;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                 Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
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
                    }
                }
            }

            lastButtonState = Mouse.GetState().LeftButton;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //spriteBatch.Draw(sprites["apple1"], new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            foreach (KeyValuePair<Point, string> kvp in fields)
            {
                spriteBatch.Draw(sprites[kvp.Value], new Rectangle(kvp.Key.X, kvp.Key.Y, dWidth, dHeight), Color.White);
            }

            if (nextToFind != null)
            {
                spriteBatch.DrawString(font, nameData[nextToFind], new Vector2(16, 16),
                    Color.White, 0, new Vector2(10, 100 - GraphicsDevice.Viewport.Height), 1, SpriteEffects.None, 0);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
*/