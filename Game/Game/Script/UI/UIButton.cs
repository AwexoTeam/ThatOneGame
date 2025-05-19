using Microsoft.Xna.Framework;
using System;
using ThatOneGame.Structure;
using ThatOneGame.Structure.UI;

namespace ThatOneGame.Script.UI
{
    public class UIButton : UIElement
    {
        public Action onClick;

        public UIButton(Point _position, Point _size, Color _color, UIAnchor _anchor = UIAnchor.TopLeft, UIStretch _stretch = UIStretch.None):
        base(_position, _size, _color, _anchor, _stretch)
        {
        }

        public virtual bool isClicked()
        {
            if (!rect.IntersectsWithMouse())
                return false;

            if (!Input.IsMouseUp(0))
                return false;

            return true;
        }

        public void  CheckAndExecute()
        {
            if (!isClicked())
                return;

            if (onClick == null)
                return;

            onClick();
        }
    }
}
