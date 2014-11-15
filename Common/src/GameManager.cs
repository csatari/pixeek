using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.Diagnostics;

namespace Pixeek
{
    public class GameManager : Microsoft.Xna.Framework.Game
    {
        public static GameManager Instance
        {
            get
            {
                return _instance;
            }
        }

        static GameManager _instance = null;

        public interface Scene
        {
            void Initialize();
            void LoadContent();
            void Update(GameTime gameTime);
            void Draw(GameTime gameTime);
        }

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public SpriteFont font;

        Scene currentScene = null;

        public static int Width
        {
            get
            {
                return Instance.GraphicsDevice.Viewport.Width;
            }
        }

        public static int Height
        {
            get
            {
                return Instance.GraphicsDevice.Viewport.Height;
            }
        }

        public GameManager()
        {
            _instance = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        public void SwitchScene(Scene scene)
        {
            currentScene = scene;
            currentScene.Initialize();
            currentScene.LoadContent();
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            Content.RootDirectory = "Content";
            font = Content.Load<SpriteFont>("spriteFont1");
           
            //SwitchScene(new Prototype());
            SwitchScene(new Menu());
       }

        protected override void Update(GameTime gameTime)
        {
            currentScene.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            currentScene.Draw(gameTime);

            spriteBatch.End();
        }
    }
}
