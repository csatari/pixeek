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

        public GameManager()
        {
            _instance = this;
            graphics = new GraphicsDeviceManager(this);
        }

        void SwitchScene(Scene scene)
        {
            currentScene = scene;
            currentScene.Initialize();
            currentScene.LoadContent();
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;


            spriteBatch = new SpriteBatch(GraphicsDevice);
            Content.RootDirectory = "Content";
            font = Content.Load<SpriteFont>("spriteFont1");
           
            SwitchScene(new Prototype());
       }

        protected override void Update(GameTime gameTime)
        {
            currentScene.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            currentScene.Draw(gameTime);
        }
    }
}
