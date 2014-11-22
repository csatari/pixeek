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
                        child.Draw(gameTime, Color.Lerp(baseColor, GetColor(), 0.5f));
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

            virtual protected Color GetColor()
            {
                return Color.White;
            }

            protected List<MenuElement> children = null;
        }

        public class MenuSpriteElement : MenuElement
        {
            public MenuSpriteElement(string textureName, Rectangle area, string text = null)
            {
                this.area = area;
                if (!string.IsNullOrEmpty(textureName))
                {
                    this.texture = GameManager.Instance.Content.Load<Texture2D>(textureName);
                }
                this.text = text;
            }

            override public void Draw(GameTime gameTime, Color baseColor)
            {
                if (texture != null)
                {
                    GameManager.Instance.spriteBatch.Draw(texture, area, Color.Lerp(GetColor(), baseColor, 0.5f));
                }                

                if (text != null)
                {
                    Vector2 size = GameManager.Instance.font.MeasureString(text);
                    Vector2 pos = new Vector2(area.Center.X - size.X / 2, area.Center.Y - size.Y / 2);
                    GameManager.Instance.spriteBatch.DrawString(
                        GameManager.Instance.font, text, pos, Color.Lerp(GetColor(), baseColor, 0.5f));
                }

                base.Draw(gameTime, baseColor);
            }

            protected Rectangle area;
            private Texture2D texture = null;
            private string text = null;
        }

        public class MenuButtonElement : MenuElement
        {
            public MenuButtonElement(Rectangle area, ClickHandler clickHandler)
                : base()
            {
                this.area = area;
                this.clickHandler = clickHandler;
            }

            public delegate void ClickHandler();

            override public void Update(GameTime gameTime)
            {
                base.Update(gameTime);
            }

            override public bool OnPress(Point pos, bool down)
            {
                if (base.OnPress(pos, down))
                {
                    buttonDown = true;
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
            private Rectangle area;
        }

        MenuElement root;

        public Menu(MenuElement root)
        {
            this.root = root;
        }

        public void Initialize()
        {
            if (root == null)
            {
                root = new MenuElement();
            }
        }

        public static void CreateMainMenu()
        {
            MenuElement root = new MenuElement();
            MenuSpriteElement bg = new MenuSpriteElement("GUI/menu_bg.jpg", new Rectangle(0, 0, GameManager.Width, GameManager.Height));
            root.AddChild(bg);

            {
                Rectangle exitRect = new Rectangle(1, 1, 151, 71);
                MenuButtonElement exitButton = new MenuButtonElement(exitRect, delegate()
                    {
                        GameManager.Instance.Exit();
                    });
                exitButton.AddChild(new MenuSpriteElement("GUI/button_bg", exitRect, "EXIT"));
                bg.AddChild(exitButton);
            }
            {
                Rectangle playRect = new Rectangle(800, 350, 340, 75);
                MenuButtonElement playButton = new MenuButtonElement(playRect,
                    delegate()
                    {
                        GameManager.Instance.SwitchScene(new Prototype());
                    }
                    );
                bg.AddChild(playButton);
                playButton.AddChild(new MenuSpriteElement("GUI/newgame_button.png", playRect));
            }

            GameManager.Instance.SwitchScene(new Menu(root));
        }

        public static void CreateGameOverMenu()
        {
            MenuElement root = new MenuElement();
            MenuSpriteElement bg = new MenuSpriteElement("GUI/menu_bg.jpg", new Rectangle(0, 0, GameManager.Width, GameManager.Height));
            //root.AddChild(bg);

            root.AddChild(new MenuSpriteElement(null, new Rectangle(400, 200, 400, 50), "YOU WON!"));

            Rectangle exitRect = new Rectangle(400, 300, 400, 50);
            MenuButtonElement exitButton = new MenuButtonElement(exitRect, delegate()
            {
                Menu.CreateMainMenu();
            });
            exitButton.AddChild(new MenuSpriteElement("GUI/button_bg", exitRect, "BACK TO MAIN MENU"));
            root.AddChild(exitButton);

            GameManager.Instance.SwitchScene(new Menu(root));
        }

        public void LoadContent()
        {

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