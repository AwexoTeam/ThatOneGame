using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using SharpDX.Direct3D9;
using System;
using ThatOneGame.GameCode;
namespace ThatOneGame.Structure.UI
{
    public class UIElement : GameObject
    {
        public UIAnchor anchor;
        public UIStretch stretch;

        public Rectangle rect { get; private set; }
        public Point pos;
        public Point size;
        public Color color;

        public UIElement(Point _position, Point _size, Color _color, UIAnchor _anchor = UIAnchor.TopLeft, UIStretch _stretch = UIStretch.None)
        {
            this.anchor = _anchor;
            this.stretch = _stretch;
            this.color = _color;
            this.pos = _position;
            this.size = _size;

            EventManager.OnWindowSizeChanged += EventManager_OnWindowSizeChanged;
        }

        private void EventManager_OnWindowSizeChanged(EventArgs e)
        {
            Adjust();
        }

        public void Adjust(bool justUpdate = false)
        {
            if (justUpdate)
            {
                rect = new Rectangle(pos, size);
                return;
            }

            Rectangle _rect = new Rectangle(pos, size);

            if (anchor == UIAnchor.TopRight || anchor == UIAnchor.BottomRight || anchor == UIAnchor.MiddleRight)
            {
                _rect.X = (int)Globals.screenSize.X - rect.Width - pos.X;
            }

            if (anchor == UIAnchor.TopMiddle || anchor == UIAnchor.BottomMiddle || anchor == UIAnchor.Middle)
            {
                float halfScreen = Globals.screenSize.X / 2;
                float halfWidth = rect.Width / 2;

                _rect.X = (int)(halfScreen - halfWidth) - pos.X;
            }

            if (anchor == UIAnchor.BottomLeft || anchor == UIAnchor.BottomMiddle || anchor == UIAnchor.BottomRight)
            {
                _rect.Y = (int)(Globals.screenSize.Y - rect.Height) - pos.Y;
            }

            if (anchor == UIAnchor.Middle || anchor == UIAnchor.MiddleLeft || anchor == UIAnchor.MiddleRight)
            {
                float halfScreen = Globals.screenSize.Y / 2;
                float halfHeight = rect.Height / 2;

                _rect.Y = (int)(halfScreen - halfHeight) - pos.Y;
            }

            rect = new Rectangle(_rect.X, _rect.Y, _rect.Width, _rect.Height);
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.FillRectangle(rect, color);
        }
    }
}
