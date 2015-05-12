using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Pixeek;
using Pixeek.BoardShapes;
using Pixeek.Game;
using Pixeek.ImageLoader;
using Pixeek.Menus.Elements;
using Pixeek.ServerCommunicator;
using Pixeek.ServerCommunicator.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.Menus
{
    public class ScoreboardMenu : Menu, GameManager.Scene
    {
        private static ScoreboardMenu instance;

        //singleton Instance
        public static ScoreboardMenu Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScoreboardMenu();
                }
                return instance;
            }
        }

        static MainMenuPlaintSelector<GameMode> gamemodeSelector;
        static MainMenuPlaintSelector<Difficulty> difficultySelector;
        private MenuSpriteElement infoElement;
        private ScoreboardElements scoreboardElements;

        private ScoreboardMenu() : base() { }

        public override void LoadContent() 
        {
            
        }

        public override void DrawMenu()
        {
            MenuSpriteElement bg = new MenuSpriteElement("GUI/scoreboard_bg.jpg", new Rectangle(0, 0, GameManager.Width, GameManager.Height));
            Root.AddChild(bg);

            // exit button
            {
                Rectangle exitRect = new Rectangle(GameManager.Width - 152, 1, 151, 71);
                MenuButtonElement exitButton = new MenuButtonElement(exitRect, delegate()
                {
                    difficultySelector.SelectedChange -= DifficultyGamemodeSelector_SelectedChange;
                    gamemodeSelector.SelectedChange -= DifficultyGamemodeSelector_SelectedChange;
                    Menu.GoToScene(MainMenu.Instance);
                });
                exitButton.AddChild(new MenuSpriteElement("GUI/button_bg", exitRect, "BACK"));
                bg.AddChild(exitButton);
            }
            // difficulty selector
            {
                difficultySelector = new MainMenuPlaintSelector<Difficulty>(Difficulty.NORMAL);
                bg.AddChild(difficultySelector);

                difficultySelector.BaseX = Convert.ToInt32(0.279 * GameManager.Width);
                difficultySelector.BaseY = Convert.ToInt32(0.359 * GameManager.Height);
                difficultySelector.YDiff = Convert.ToInt32(0.085 * GameManager.Height);
                difficultySelector.Width = Convert.ToInt32(0.12 * GameManager.Width);
                difficultySelector.Height = Convert.ToInt32(0.077 * GameManager.Height);

                difficultySelector.AddElement("EASY", Difficulty.EASY);
                difficultySelector.AddElement("NORMAL", Difficulty.NORMAL);
                difficultySelector.AddElement("HARD", Difficulty.HARD);

                difficultySelector.SelectedChange += DifficultyGamemodeSelector_SelectedChange;
            }

            //gamemode selector
            {
                gamemodeSelector = new MainMenuPlaintSelector<GameMode>(GameMode.NORMAL);
                bg.AddChild(gamemodeSelector);

                gamemodeSelector.BaseX = Convert.ToInt32(0.077 * GameManager.Width);
                gamemodeSelector.BaseY = Convert.ToInt32(0.359 * GameManager.Height);
                gamemodeSelector.YDiff = Convert.ToInt32(0.085 * GameManager.Height);
                gamemodeSelector.Width = Convert.ToInt32(0.15 * GameManager.Width);
                gamemodeSelector.Height = Convert.ToInt32(0.077 * GameManager.Height);

                gamemodeSelector.AddElement("NORMAL", GameMode.NORMAL);
                gamemodeSelector.AddElement("TIME", GameMode.TIME);

                gamemodeSelector.SelectedChange += DifficultyGamemodeSelector_SelectedChange;
            }
            //scores
            {
                scoreboardElements = new ScoreboardElements();
                bg.AddChild(scoreboardElements);

                scoreboardElements.BaseX = Convert.ToInt32(0.4 * GameManager.Width);
                scoreboardElements.BaseY = Convert.ToInt32(0.2 * GameManager.Height);
                scoreboardElements.NameX = Convert.ToInt32(0.5 * GameManager.Width);
                scoreboardElements.NameY = Convert.ToInt32(0.2 * GameManager.Height);
                scoreboardElements.ScoreX = Convert.ToInt32(0.75 * GameManager.Width);
                scoreboardElements.ScoreY = Convert.ToInt32(0.2 * GameManager.Height);
                scoreboardElements.YDiff = Convert.ToInt32(0.05 * GameManager.Height);
                scoreboardElements.Width = Convert.ToInt32(0.12 * GameManager.Width);
                scoreboardElements.Height = Convert.ToInt32(0.077 * GameManager.Height);
            }
            //loading
            {
                Rectangle infoRect = new Rectangle(Convert.ToInt32(0.5 * GameManager.Width),
                                                   Convert.ToInt32(0.45 * GameManager.Height),
                                                   Convert.ToInt32(0.3125 * GameManager.Width),
                                                   Convert.ToInt32(0.07 * GameManager.Height));
                infoElement = new MenuSpriteElement(null, infoRect, "");
                Root.AddChild(infoElement);
            }


            difficultySelector.Selected = Difficulty.EASY;
            gamemodeSelector.Selected = GameMode.NORMAL;
            DifficultyGamemodeSelector_SelectedChange(this, EventArgs.Empty);
        }

        private void DifficultyGamemodeSelector_SelectedChange(object sender, EventArgs e)
        {
            SetLoading(true);
            ScoreboardCommunicator.Instance.GetTop10Scores(gamemodeSelector.Selected, difficultySelector.Selected, 
                delegate(ScoreboardResponse response)
                {
                    AddScore(response);
                    SetLoading(false);
                }
            );
        }

        private void AddScore(ScoreboardResponse resp)
        {
            foreach (ScoreboardResponse.ScoreBoard sb in resp.scoreboard)
            {
                scoreboardElements.AddElement(sb.player, sb.score);
            }
        }

        private void SetLoading(bool loading)
        {
            if (loading)
            {
                scoreboardElements.Clear();
                infoElement.Text = "Loading...";
            }
            else
            {
                infoElement.Text = "";
            }
        }
    }
}
