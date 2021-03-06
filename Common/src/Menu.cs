using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Pixeek.ImageLoader;
using System;
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
            virtual public bool OnHover(Point pos, bool hover)
            {
                if (children != null)
                {
                    foreach (MenuElement child in children)
                    {
                        if (child.OnHover(pos, hover))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            virtual protected Color GetColor()
            {
                return baseColor;
            }

            public void SetBaseColor(Color color)
            {
                baseColor = color;
            }

            protected List<MenuElement> children = null;

            protected Color baseColor = Color.White;
        }

        public class MenuSpriteElement : MenuElement
        {
            public MenuSpriteElement(string textureName, Rectangle area, String text = null, float scale = 1.0f)
            {
                this.area = area;
                if (!string.IsNullOrEmpty(textureName))
                {
                    this.texture = GameManager.Instance.Content.Load<Texture2D>(textureName);
                }
                this.Text = text;
                this.scale = scale;
            }

            override public void Draw(GameTime gameTime, Color baseColor)
            {
                if (texture != null)
                {
                    GameManager.Instance.spriteBatch.Draw(texture, area, Color.Lerp(GetColor(), baseColor, 0.5f));
                }

                if (Text != null)
                {
                    Vector2 size = GameManager.Instance.font.MeasureString(Text);
                    Vector2 pos = new Vector2(area.Center.X - size.X / 2, area.Center.Y - size.Y / 2);
                    /*GameManager.Instance.spriteBatch.DrawString(
                        GameManager.Instance.font, Text, pos, Color.Lerp(GetColor(), baseColor, 0.5f));*/
                    GameManager.Instance.spriteBatch.DrawString(GameManager.Instance.font, Text, pos, Color.Lerp(GetColor(), baseColor, 0.5f), 0,
                        new Vector2(0, 0), scale, SpriteEffects.None, 0);
                }

                base.Draw(gameTime, baseColor);
            }

            protected Rectangle area;
            private Texture2D texture = null;
            private float scale;
            public String Text
            {
                get;
                set;
            }
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
#if WINDOWS
                if (base.OnPress(pos, down))
                {
                    buttonDown = true;
                    return true;
                }

                if (area.Contains(pos))
                {
                    if (buttonDown && !down)
                    {
                        if (clickHandler != null)
                        {
                            clickHandler();
                        }
                        Debug.WriteLine("megnyomva");
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
#endif
#if ANDROID
            if (area.Contains(pos))
            {
                if (clickHandler != null)
                {
                    clickHandler();
                }
                Debug.WriteLine("megnyomva");
                return true;
            }
            return false;
#endif
            }

            override public bool OnHover(Point pos, bool hover)
            {
                if (base.OnHover(pos, hover))
                {
                    buttonHovered = true;
                    return true;
                }

                if (area.Contains(pos))
                {
                    if (buttonHovered && !hover)
                    {
                        buttonHovered = false;
                        return true;
                    }
                    else if (hover)
                    {
                        buttonHovered = true;
                    }
                }
                else
                {
                    buttonHovered = false;
                }

                return false;
            }

            protected override Color GetColor()
            {
                Color bc = base.GetColor();
                if (buttonDown)
                {
                    if (bc == Color.White)
                    {
                        return Color.Black;
                    }
                    else
                    {
                        return Color.Lerp(Color.Black, bc, 0.5f);
                    }
                }
                if (buttonHovered)
                {
                    if (bc == Color.White)
                    {
                        return Color.LightGray;
                    }
                    else
                    {
                        return Color.Lerp(Color.LightGray, bc, 0.5f);
                    }
                }
                else return bc;
            }

            bool buttonDown = false;
            bool buttonHovered = false;
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
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap;
            if (root == null)
            {
                root = new MenuElement();
            }
            preloadImages();
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
                Rectangle playRect = new Rectangle(Convert.ToInt32(0.65 * GameManager.Width), Convert.ToInt32(0.35 * GameManager.Height),
                                                    Convert.ToInt32(0.265 * GameManager.Width), Convert.ToInt32(0.104 * GameManager.Height));
                MenuButtonElement playButton = new MenuButtonElement(playRect,
                    delegate()
                    {
                        //GameManager.Instance.SwitchScene(new Prototype());
                        //GameManager.Instance.SwitchScene(new Game.GameModel(imageDatabase));
                        Menu.CreateNewGameMenu();
                    }
                    );
                bg.AddChild(playButton);
                playButton.AddChild(new MenuSpriteElement("GUI/newgame_button.png", playRect));
            }

            GameManager.Instance.SwitchScene(new Menu(root));
        }

        public static void CreateGameOverMenu(bool win, int point, string time)
        {
            MenuElement root = new MenuElement();
            MenuSpriteElement bg = new MenuSpriteElement("GUI/menu_bg.jpg", new Rectangle(0, 0, GameManager.Width, GameManager.Height));
            //root.AddChild(bg);

            if (win)
            {
                if (time != null)
                {
                    root.AddChild(new MenuSpriteElement(null, new Rectangle(Convert.ToInt32(0.3125 * GameManager.Width),
                                                                            Convert.ToInt32(0.28 * GameManager.Height),
                                                                            Convert.ToInt32(0.3125 * GameManager.Width),
                                                                            Convert.ToInt32(0.097 * GameManager.Height)), "YOU WON!\nYou gained " + point
                        + " points. Remaining time: " + time));
                }
                else
                {
                    root.AddChild(new MenuSpriteElement(null, new Rectangle(Convert.ToInt32(0.3125 * GameManager.Width),
                                                                            Convert.ToInt32(0.28 * GameManager.Height),
                                                                            Convert.ToInt32(0.3125 * GameManager.Width),
                                                                            Convert.ToInt32(0.097 * GameManager.Height)), "YOU WON!\nYou gained " + point
                        + " points."));
                }
            }
            else
            {
                root.AddChild(new MenuSpriteElement(null, new Rectangle(Convert.ToInt32(0.3125 * GameManager.Width),
                                                                        Convert.ToInt32(0.28 * GameManager.Height),
                                                                        Convert.ToInt32(0.3125 * GameManager.Width),
                                                                        Convert.ToInt32(0.097 * GameManager.Height)), "GAME OVER!\nYou gained " + point
                        + " points."));
            }


            Rectangle exitRect = new Rectangle(Convert.ToInt32(0.3125 * GameManager.Width),
                                                Convert.ToInt32(0.4167 * GameManager.Height),
                                                Convert.ToInt32(0.3125 * GameManager.Width),
                                                Convert.ToInt32(0.07 * GameManager.Height));
            MenuButtonElement exitButton = new MenuButtonElement(exitRect, delegate()
            {
                Menu.CreateMainMenu();
            });
            exitButton.AddChild(new MenuSpriteElement("GUI/button_bg", exitRect, "BACK TO MAIN MENU"));
            root.AddChild(exitButton);

            GameManager.Instance.SwitchScene(new Menu(root));
        }

        static Game.GameMode selectedGameMode = Game.GameMode.NORMAL;
        static Game.Difficulty selectedDifficulty = Game.Difficulty.NORMAL;

        //TODO ezt kell �trakni m�sik oszt�lyba generikusan
        class DifficultySelector : MenuElement
        {
            override public void Update(GameTime gameTime)
            {
                base.Update(gameTime);

                foreach (KeyValuePair<Game.Difficulty, MenuElement> kvp in elements)
                {
                    if (selectedDifficulty == kvp.Key)
                    {
                        kvp.Value.SetBaseColor(Color.Red);
                    }
                    else
                    {
                        kvp.Value.SetBaseColor(Color.White);
                    }
                }
            }

            Dictionary<Game.Difficulty, MenuElement> elements = new Dictionary<Game.Difficulty, MenuElement>();

            public void AddElementForDifficulty(Game.Difficulty diff, MenuElement elem)
            {
                elements[diff] = elem;
                AddChild(elem);
            }
        }

        // TODO unify with difficultyselector as a generic class
        class GameModeSelector : MenuElement
        {
            override public void Update(GameTime gameTime)
            {
                base.Update(gameTime);

                foreach (KeyValuePair<Game.GameMode, MenuElement> kvp in elements)
                {
                    if (selectedGameMode == kvp.Key)
                    {
                        kvp.Value.SetBaseColor(Color.Red);
                    }
                    else
                    {
                        kvp.Value.SetBaseColor(Color.White);
                    }
                }
            }

            Dictionary<Game.GameMode, MenuElement> elements = new Dictionary<Game.GameMode, MenuElement>();

            public void AddElementForDifficulty(Game.GameMode diff, MenuElement elem)
            {
                elements[diff] = elem;
                AddChild(elem);
            }
        }


        static bool music = false;
        static bool vibration = false;
        static String musicText = "MUSIC: OFF";
        static string vibText = "VIBRATION: OFF";
        static MenuSpriteElement musicSpriteElement;
        static MenuSpriteElement vibrationSpriteElement;


        public static void CreateNewGameMenu()
        {
            MenuElement root = new MenuElement();
            MenuSpriteElement bg = new MenuSpriteElement("GUI/newgame_menu.jpg", new Rectangle(0, 0, GameManager.Width, GameManager.Height));
            root.AddChild(bg);

            {
                Rectangle exitRect = new Rectangle(GameManager.Width - 152, 1, 151, 71);
                MenuButtonElement exitButton = new MenuButtonElement(exitRect, delegate()
                {
                    CreateMainMenu();
                });
                exitButton.AddChild(new MenuSpriteElement("GUI/button_bg", exitRect, "BACK"));
                bg.AddChild(exitButton);
            }

            Rectangle musicRect = new Rectangle(GameManager.Width - 500, 1, 151, 71);
            MenuButtonElement musicButton = new MenuButtonElement(musicRect, delegate()
            {
                if (!music)
                {
                    music = true;
                    musicText = "MUSIC: ON";
                }
                else
                {
                    music = false;
                    musicText = "MUSIC: OFF";
                }
                musicSpriteElement.Text = musicText;
            });
            musicSpriteElement = new MenuSpriteElement("GUI/button_bg", musicRect, musicText);
            musicButton.AddChild(musicSpriteElement);
            bg.AddChild(musicButton);

            Rectangle vibRect = new Rectangle(GameManager.Width - 345, 1, 190, 71);
            MenuButtonElement vibButton = new MenuButtonElement(vibRect, delegate()
            {
                if (!vibration)
                {
                    vibration = true;
                    vibText = "VIBRATION: ON";
                }
                else
                {
                    vibration = false;
                    vibText = "VIBRATION: OFF";
                }
                vibrationSpriteElement.Text = vibText;
            });
            vibrationSpriteElement = new MenuSpriteElement("GUI/button_bg", vibRect, vibText);
            vibButton.AddChild(vibrationSpriteElement);
            bg.AddChild(vibButton);



            {
                DifficultySelector selector = new DifficultySelector();
                bg.AddChild(selector);

                int baseX = Convert.ToInt32(0.279 * GameManager.Width);
                int baseY = Convert.ToInt32(0.359 * GameManager.Height);
                int YDiff = Convert.ToInt32(0.085 * GameManager.Height);
                {
                    Rectangle easyRect = new Rectangle(baseX,
                                                        baseY + YDiff * 0,
                                                        Convert.ToInt32(0.078 * GameManager.Width),
                                                        Convert.ToInt32(0.077 * GameManager.Height));
                    MenuButtonElement easyButton = new MenuButtonElement(easyRect, delegate()
                    {
                        selectedDifficulty = Game.Difficulty.EASY;
                    });
                    easyButton.AddChild(new MenuSpriteElement(null, easyRect, "EASY", 1.5f));
                    selector.AddElementForDifficulty(Game.Difficulty.EASY, easyButton);
                }

                {
                    Rectangle easyRect = new Rectangle(baseX, baseY + YDiff * 1, Convert.ToInt32(0.078 * GameManager.Width), Convert.ToInt32(0.077 * GameManager.Height));
                    MenuButtonElement easyButton = new MenuButtonElement(easyRect, delegate()
                    {
                        selectedDifficulty = Game.Difficulty.NORMAL;
                    });
                    easyButton.AddChild(new MenuSpriteElement(null, easyRect, "NORMAL", 1.5f));
                    selector.AddElementForDifficulty(Game.Difficulty.NORMAL, easyButton);
                }

                {
                    Rectangle easyRect = new Rectangle(baseX, baseY + YDiff * 2, Convert.ToInt32(0.078 * GameManager.Width), Convert.ToInt32(0.077 * GameManager.Height));
                    MenuButtonElement easyButton = new MenuButtonElement(easyRect, delegate()
                    {
                        selectedDifficulty = Game.Difficulty.HARD;
                    });
                    easyButton.AddChild(new MenuSpriteElement(null, easyRect, "HARD", 1.5f));
                    selector.AddElementForDifficulty(Game.Difficulty.HARD, easyButton);
                }
            }

            {
                GameModeSelector selector = new GameModeSelector();
                bg.AddChild(selector);

                int baseX = Convert.ToInt32(0.1 * GameManager.Width);
                int baseY = Convert.ToInt32(0.388 * GameManager.Height);
                int YDiff = Convert.ToInt32(0.085 * GameManager.Height);
                {
                    Rectangle easyRect = new Rectangle(baseX, baseY + YDiff * 0, Convert.ToInt32(0.078 * GameManager.Width), Convert.ToInt32(0.077 * GameManager.Height));
                    MenuButtonElement easyButton = new MenuButtonElement(easyRect, delegate()
                    {
                        selectedGameMode = Game.GameMode.NORMAL;
                    });
                    easyButton.AddChild(new MenuSpriteElement(null, easyRect, "NORMAL", 1.5f));
                    selector.AddElementForDifficulty(Game.GameMode.NORMAL, easyButton);
                }

                {
                    Rectangle easyRect = new Rectangle(baseX, baseY + YDiff * 1, Convert.ToInt32(0.078 * GameManager.Width), Convert.ToInt32(0.077 * GameManager.Height));
                    MenuButtonElement easyButton = new MenuButtonElement(easyRect, delegate()
                    {
                        selectedGameMode = Game.GameMode.ENDLESS;
                    });
                    easyButton.AddChild(new MenuSpriteElement(null, easyRect, "ENDLESS", 1.5f));
                    selector.AddElementForDifficulty(Game.GameMode.ENDLESS, easyButton);
                }

                {
                    Rectangle easyRect = new Rectangle(baseX, baseY + YDiff * 2, Convert.ToInt32(0.078 * GameManager.Width), Convert.ToInt32(0.077 * GameManager.Height));
                    MenuButtonElement easyButton = new MenuButtonElement(easyRect, delegate()
                    {
                        selectedGameMode = Game.GameMode.TIME;
                    });
                    easyButton.AddChild(new MenuSpriteElement(null, easyRect, "TIME", 1.5f));
                    selector.AddElementForDifficulty(Game.GameMode.TIME, easyButton);
                }
            }

            {
                Rectangle playRect = new Rectangle(Convert.ToInt32(0.78125 * GameManager.Width), Convert.ToInt32(0.444 * GameManager.Height), Convert.ToInt32(0.114 * GameManager.Width), Convert.ToInt32(0.0583 * GameManager.Height));
                MenuButtonElement playButton = new MenuButtonElement(playRect,
                    delegate()
                    {
                        //GameManager.Instance.SwitchScene(new Prototype());
                        GameManager.Instance.SwitchScene(new Game.GameModel(imageDatabase, selectedGameMode, selectedDifficulty, music, vibration));
                    }
                    );
                bg.AddChild(playButton);
                playButton.AddChild(new MenuSpriteElement("GUI/button_play.png", playRect));
            }

            GameManager.Instance.SwitchScene(new Menu(root));
        }

        public void LoadContent()
        {

        }

        ButtonState lastButtonState = ButtonState.Released;
        TouchCollection currentTouchState;

        public void Update(GameTime gameTime)
        {
            //�rint�s lekezel�se
            currentTouchState = TouchPanel.GetState();

            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();
                switch (gesture.GestureType)
                {
                    case GestureType.DoubleTap:
                        break;
                    case GestureType.Tap:
                        root.OnPress(new Point((int)gesture.Position.X, (int)gesture.Position.Y), false);
                        break;
                }
            }

            //eg�r lekezel�se
            if (lastButtonState != Mouse.GetState().LeftButton)
            {
                root.OnPress(Mouse.GetState().Position, Mouse.GetState().LeftButton == ButtonState.Pressed);
                lastButtonState = Mouse.GetState().LeftButton;
            }
            else
            {
                root.OnHover(Mouse.GetState().Position, Mouse.GetState().LeftButton == ButtonState.Released);
            }
            root.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            root.Draw(gameTime, Color.White);
        }

        #region Image cache

        public static ImageDatabase imageDatabase;

        /// <summary>
        /// El�t�lti a k�peket, hogy ne a new game megnyom�s�ra akadjon be, hanem amikor bel�p�nk a j�t�kba
        /// </summary>
        public static void preloadImages()
        {
            imageDatabase = new ImageDatabase();
            imageDatabase.LoadContent();
        }
        #endregion
    }
}