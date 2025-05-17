using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatOneGame.Structure.UI
{
    public class HorizontalLayout : UIElement
    {
        public List<UIElement> children { get; private set; }
        private bool isEven;
        public int padding;

        public HorizontalLayout(Point _position, Point _size, Color _color, UIAnchor _anchor = UIAnchor.TopLeft, UIStretch _stretch = UIStretch.None) :
        base(_position, _size, _color, _anchor, _stretch)
        {
            this.children = new List<UIElement>();
        }

        public void AddRange(params UIElement[] childs)
        {
            children.AddRange(childs);
            AdjustChildren();
        }

        public void AddChild(UIElement child)
        {
            children.Add(child);
            AdjustChildren();
        }
        public void RemoveChild(UIElement child)
        {
            children.Remove(child);
            AdjustChildren();
        }
        public void RemoveAt(int index)
        {
            children.RemoveAt(index);
            AdjustChildren();
        }

        private void AdjustChildren()
        {
            Debug.LogDebug("Adjusting " + children.Count);
            isEven = children.Count % 2 == 0;

            if (children.Count <= 0)
                return;

            Point originLeft = pos;
            Point originRight = pos;

            int currIndex = 0;
            int startIndex = 0;

            bool isLeft = false;

            if (!isEven)
            {
                startIndex = 1;
                AdjustChild(0, pos, false);

                originRight.X -= 210;
            }

            for (int i = startIndex; i < children.Count; i++)
            {
                isLeft = i <= (children.Count - startIndex) / 2;
                Point origin = isLeft ? originLeft : originRight;
                AdjustChild(i, origin, isLeft);
                
            }
        }

        private void AdjustChild(int index, Point origin, bool isLeft)
        {
            UIElement child = children[index];

            child.anchor = anchor;
            child.stretch = stretch;

            child.pos = origin;

            int offset = padding + child.size.X;
            offset *= isLeft ? -1 : 1;
            offset *= index;

            child.pos.X += offset;
            child.Adjust();
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            foreach (UIElement child in children)
                child.Draw(batch);
        }
    }
}
