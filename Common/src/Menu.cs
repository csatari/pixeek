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
            }

            private Rectangle area;
            private Texture2D texture;
        }

        public class MenuButtonElement : MenuSpriteElement
        {
            public MenuButtonElement(string textureName, Rectangle area) : base(textureName, area)
            {
            }

            public delegate void ClickHandler();

            override public void Update(GameTime gameTime)
            {
            }

            override public bool OnClick(Point area)
            {
                return false;
            }

            private Rectangle hitArea;
            private ClickHandler clickHandler;
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