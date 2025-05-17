using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatOneGame.Structure;
using ThatOneGame.Structure.UI;

namespace ThatOneGame.GameCode
{
    public class MenuScreen : GameScreen
    {
        List<UIElement> elements;

        public override void Start()
        {
            elements = new List<UIElement>();

            Point pos = new Point(10,10);

            elements.Add(new UIElement(pos, new Point(100, 100), Color.Lime, UIAnchor.TopLeft));
            elements.Add(new UIElement(pos, new Point(100, 100), Color.Green, UIAnchor.TopMiddle));
            elements.Add(new UIElement(pos, new Point(100, 100), Color.DarkGreen, UIAnchor.TopRight));

            elements.Add(new UIElement(pos, new Point(100, 100), Color.Cyan, UIAnchor.MiddleLeft));
            elements.Add(new UIElement(pos, new Point(100, 100), Color.Blue, UIAnchor.Middle));
            elements.Add(new UIElement(pos, new Point(100, 100), Color.DarkBlue, UIAnchor.MiddleRight));

            elements.Add(new UIElement(pos, new Point(100, 100), Color.HotPink, UIAnchor.BottomLeft));
            elements.Add(new UIElement(pos, new Point(100, 100), Color.Red, UIAnchor.BottomMiddle));
            elements.Add(new UIElement(pos, new Point(100, 100), Color.DarkRed, UIAnchor.BottomRight));

            elements.ForEach(x => x.Adjust());

            base.Start();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!elements.Any(x => hasClicked(x)))
                return;

            ScreenManager.instance.LoadScreen(new TestScreen());
        }

        private bool hasClicked(UIElement element)
        {
            return true;
        }

        public override void UIDraw(SpriteBatch batch)
        {
            base.UIDraw(batch);
            elements.ForEach(x => x.Draw(batch));
        }
    }
}
