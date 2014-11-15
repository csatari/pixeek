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

            virtual public void Draw(GameTime gameTime, Color baseColor)
            {
                if (children != null)
                {
                    foreach (MenuElement child in children)
                    {
                        child.Draw(gameTime, baseColor);
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

            virtual public bool OnPress(Point pos, bool down)
            {
                if (children != null)
                {
                    foreach (MenuElement child in children)
                    {
                        if (child.OnPress(pos, down))
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

            override public void Draw(GameTime gameTime, Color baseColor)
            {
                GameManager.Instance.spriteBatch.Draw(texture, area, GetColor());

                if (children != null)
                {
                    foreach (MenuElement child in children)
                    {
                        child.Draw(gameTime, Color.Lerp(baseColor, GetColor(), 0.5f));
                    }
                }
            }

            virtual protected Color GetColor()
            {
                return Color.White;
            }

            protected Rectangle area;
            private Texture2D texture;
        }

        public class MenuButtonElement : MenuSpriteElement
        {
            public MenuButtonElement(string textureName, Rectangle area, ClickHandler clickHandler, string text = null) :
                base(textureName, area)
            {
                this.text = text;
                this.clickHandler = clickHandler;
            }

            public delegate void ClickHandler();

            override public void Update(GameTime gameTime)
            {
                base.Update(gameTime);
            }

            override public void Draw(GameTime gameTime, Color baseColor)
            {
                base.Draw(gameTime, baseColor);
                if (text != null)
                {
                    Vector2 size = GameManager.Instance.font.MeasureString(text);
                    Vector2 pos = new Vector2(area.Center.X - size.X / 2, area.Center.Y - size.Y / 2);
                    GameManager.Instance.spriteBatch.DrawString(
                        GameManager.Instance.font, text, pos, Color.Lerp(GetColor(), baseColor, 0.5f));
                }
            }

            override public bool OnPress(Point pos, bool down)
            {
                if (base.OnPress(pos, down))
                {
                    buttonDown = false;
                    return true;
                }

                if (area.Contains(pos))
                {
                    if (buttonDown && !down)
                    {
                        clickHandler();
                        buttonDown = false;
                        return true;
                    }
                    else if (down)
                    {
                        buttonDown = true;
                    }
                }
                else
                {
                    buttonDown = false;
                }

                return false;
            }

            protected override Color GetColor()
            {
                return buttonDown ? Color.Black : Color.White;
            }

            bool buttonDown = false;
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

            MenuButtonElement exitButton = new MenuButtonElement("GUI/button_bg", new Rectangle(1, 1, 151, 71),
                delegate()
                {
                    GameManager.Instance.Exit();
                },
                "EXIT");
            bg.AddChild(exitButton);

            MenuButtonElement playButton = new MenuButtonElement("GUI/newgame_button.png", new Rectangle(800, 350, 340, 75),
                delegate()
                {
                    GameManager.Instance.SwitchScene(new Prototype());
                }
                );
            bg.AddChild(playButton);
        }

        ButtonState lastButtonState = ButtonState.Released;

        public void Update(GameTime gameTime)
        {
            if (lastButtonState != Mouse.GetState().LeftButton)
            {
                root.OnPress(Mouse.GetState().Position, Mouse.GetState().LeftButton == ButtonState.Pressed);
                lastButtonState = Mouse.GetState().LeftButton;
            }
            root.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            root.Draw(gameTime, Color.White);
        }
    }
}