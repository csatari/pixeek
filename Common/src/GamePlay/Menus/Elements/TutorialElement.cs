using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.Menus.Elements
{
    public class TutorialElement : MenuElement
    {
        /// <summary>
        /// The most left position of the element
        /// </summary>
        public int BaseX { get; set; }
        /// <summary>
        /// The most top position of the element
        /// </summary>
        public int BaseY { get; set; }
        /// <summary>
        /// The space's size between elements
        /// </summary>
        public int YDiff { get; set; }
        /// <summary>
        /// The width of one element
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// The height of one element
        /// </summary>
        public int Height { get; set; }
        private Point TopLeft { get; set; }
        private Point BottomRight { get; set; }

        public delegate void TutorialDismissed();

        private List<Tuple<Point, Point, string, TutorialDismissed>> TutorialElements = new List<Tuple<Point, Point, string, TutorialDismissed>>();

        public TutorialElement()
        {}

        public void AddRectangle(Point topLeft, Point bottomRight, string text, TutorialDismissed td = null)
        {
            TutorialElements.Add(new Tuple<Point, Point, string, TutorialDismissed>(topLeft, bottomRight, text, td));
        }

        public void ShowNextTutorial()
        {
            if (TutorialElements.Count == 0)
            {
                Clear();
            }
            else
            {
                Tuple<Point, Point, string, TutorialDismissed> tut = TutorialElements.First();
                SetRectangle(tut.Item1, tut.Item2, tut.Item3, tut.Item4);
                TutorialElements.RemoveAt(0);
            }
        }

        public void SetRectangle(Point topLeft, Point bottomRight, string text, TutorialDismissed tutorialDismissed)
        {
            Clear();

            String leftText = null;
            String rightText = null;
            String topText = null;
            if (topLeft.X < ((GameManager.Width / 2) - (bottomRight.X - topLeft.X)))
            {
                rightText = text;
            }
            else
            {
                leftText = text;
            }

            if (bottomRight.X == GameManager.Width && topLeft.X == 0)
            {
                leftText = null;
                rightText = null;
                topText = text;
            }

            TopLeft = topLeft;
            BottomRight = bottomRight;

            Rectangle topArea = new Rectangle(0, 0, GameManager.Width, TopLeft.Y);
            TutorialButton tutButton = new TutorialButton(topArea,
                delegate()
                {
                    if (tutorialDismissed != null)
                    {
                        tutorialDismissed();
                    }
                    ShowNextTutorial();
                }
            );
            AddChild(tutButton);
            tutButton.AddChild(new MenuSpriteElement("GUI/tutorial_bg.png", topArea, topText));


            Rectangle bottomArea = new Rectangle(0, BottomRight.Y, GameManager.Width, GameManager.Height - BottomRight.Y);
            tutButton = new TutorialButton(bottomArea,
                delegate()
                {
                    if (tutorialDismissed != null)
                    {
                        tutorialDismissed();
                    }
                    ShowNextTutorial();
                }
            );
            AddChild(tutButton);
            tutButton.AddChild(new MenuSpriteElement("GUI/tutorial_bg.png", bottomArea));

            Rectangle leftArea = new Rectangle(0, TopLeft.Y, TopLeft.X, BottomRight.Y - TopLeft.Y);
            tutButton = new TutorialButton(leftArea,
                delegate()
                {
                    if (tutorialDismissed != null)
                    {
                        tutorialDismissed();
                    }
                    ShowNextTutorial();
                }
            );
            AddChild(tutButton);
            tutButton.AddChild(new MenuSpriteElement("GUI/tutorial_bg.png", leftArea, leftText));

            Rectangle rightArea = new Rectangle(BottomRight.X, TopLeft.Y, GameManager.Width - TopLeft.X, BottomRight.Y - TopLeft.Y);
            tutButton = new TutorialButton(rightArea,
                delegate()
                {
                    if (tutorialDismissed != null)
                    {
                        tutorialDismissed();
                    }
                    ShowNextTutorial();
                }
            );
            AddChild(tutButton);
            tutButton.AddChild(new MenuSpriteElement("GUI/tutorial_bg.png", rightArea, rightText));
        }

        private void Clear()
        {
            if (children != null)
            {
                children.Clear();
            }
        }

    }
}
