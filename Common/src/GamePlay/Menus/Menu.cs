using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Pixeek;
using Pixeek.Menus.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.Menus
{
    public abstract class Menu : GameManager.Scene
    {
        protected MenuElement Root
        {
            get;
            set;
        }

        public Menu() 
        {
            Root = new MenuElement();
        }

        public virtual void Initialize()
        {
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap;
            if (Root == null)
            {
                Root = new MenuElement();
            }
        } 

        public abstract void DrawMenu();

        public abstract void LoadContent();

        ButtonState lastButtonState = ButtonState.Released;
        TouchCollection currentTouchState;

        public void Update(GameTime gameTime)
        {
            //Érintés lekezelése
            currentTouchState = TouchPanel.GetState();

            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();
                switch (gesture.GestureType)
                {
                    case GestureType.DoubleTap:
                        break;
                    case GestureType.Tap:
                        Root.OnPress(new Point((int)gesture.Position.X, (int)gesture.Position.Y), false);
                        break;
                }
            }

            //egér lekezelése
            if (lastButtonState != Mouse.GetState().LeftButton)
            {
                Root.OnPress(Mouse.GetState().Position, Mouse.GetState().LeftButton == ButtonState.Pressed);
                lastButtonState = Mouse.GetState().LeftButton;
            }
            else
            {
                Root.OnHover(Mouse.GetState().Position, Mouse.GetState().LeftButton == ButtonState.Released);
            }
            Root.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            Root.Draw(gameTime, Color.White);
        }

        public static void GoToScene(Menu scene)
        {
            MenuElement root = new MenuElement();
            scene.Root = root;
            scene.DrawMenu();
            GameManager.Instance.SwitchScene(scene);
        }
    }
}
