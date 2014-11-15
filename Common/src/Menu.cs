using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.Diagnostics;

namespace Pixeek
{
    public class Menu : GameManager.Scene
    {
        public class MenuElement
        {
            public void AddChild(MenuElement child)
            {
                if (children == null)
                {
                    children = new List<MenuElement>();
                }

                children.Add(child);
            }

            virtual public void Draw(GameTime gameTime)
            {
                if (children != null)
                {
                    foreach (MenuElement child in children)
                    {
                        child.Draw(gameTime);
                    }
                }
            }

            virtual public void Update(GameTime gameTime)
            {
                if (children != null)
                {
                    foreach (MenuElement child in children)
                    {
                        child.Update(gameTime);
                    }
                }
            }

            virtual public bool OnClick(Point pos)
            {
                if (children != null)
                {
                    foreach (MenuElement child in children)
                    {
                        if (child.OnClick(pos))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            protected List<MenuElement> children = null;
        }

        public class MenuSpriteElement : MenuElement
        {
            public MenuSpriteElement(string textureName, Rectangle area)
            {
                this.area = area;
                this.texture = GameManager.Instance.Content.Load<Texture2D>(textureName);
            }

            override public void Draw(GameTime gameTime)
            {
                GameManager.Instance.spriteBatch.Draw(texture, area, Color.White);
                base.Draw(gameTime);
            }

            protected Rectangle area;
            private Texture2D texture;
        }

        public class MenuButtonElement : MenuSpriteElement
        {
            public MenuButtonElement(string textureName, Rectangle area, string text = null) :
                base(textureName, area)
            {
                this.text = text;
            }

            public delegate void ClickHandler();

            override public void Update(GameTime gameTime)
            {
                base.Update(gameTime);
            }

            override public void Draw(GameTime gameTime)
            {
                base.Draw(gameTime);
                if (text != null)
                {
                    Vector2 size = GameManager.Instance.font.MeasureString(text);
                    Vector2 pos = new Vector2(area.Center.X - size.X / 2, area.Center.Y - size.Y / 2);
                    GameManager.Instance.spriteBatch.DrawString(GameManager.Instance.font, text, pos, Color.White);
                }
            }

            override public bool OnClick(Point area)
            {
                base.OnClick(area);
                return false;
            }

            private ClickHandler clickHandler;
            private string text = null;
        }

        MenuElement root;

        public void Initialize()
        {
            root = new MenuElement();
        }

        public void LoadContent()
        {
            MenuSpriteElement bg = new MenuSpriteElement("GUI/menu_bg.jpg", new Rectangle(0, 0, GameManager.Width, GameManager.Height));
            root.AddChild(bg);

            MenuButtonElement exitButton = new MenuButtonElement("GUI/button_bg", new Rectangle(1, 1, 151, 71), "EXIT");
            bg.AddChild(exitButton);

            MenuButtonElement playButton = new MenuButtonElement("GUI/newgame_button.png", new Rectangle(800, 350, 340, 75));
            bg.AddChild(playButton);
        }

        public void Update(GameTime gameTime)
        {
            root.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            root.Draw(gameTime);
        }
    }
}