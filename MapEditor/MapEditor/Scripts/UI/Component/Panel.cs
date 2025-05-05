using MapEditor.Scripts.Definations.Enums;
using MapEditor.Scripts.Definations.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using System;

namespace MapEditor.Scripts.UI.Component
{
    public class Panel : GameObject
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public Color color;
        public AnchorMode mode;

        public Panel(AnchorMode mode, Point point, Point size, Color color)
        {
            x = point.X;
            y = point.Y;

            width = size.X;
            height = size.Y;

            this.mode = mode;
            this.color = color;
        }

        public Panel(AnchorMode mode, int x, int y, int width, int height, Color color)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            this.mode = mode;
            this.color = color;
        }

        protected Point GetActualPosition()
        {
            int screenWidth = Engine.ScreenSize.X;
            int screenHeight = Engine.ScreenSize.Y;

            if (mode == AnchorMode.TopRight)
                return new Point(screenWidth - width - x, y);

            if (mode == AnchorMode.BottomLeft)
                return new Point(x, screenHeight - height - y);

            if(mode == AnchorMode.BottomRight)
                return new Point(screenWidth - width - x, screenHeight - height - y);

            if(mode == AnchorMode.TopMiddle || mode == AnchorMode.BottomMiddle)
            {
                float posX = screenWidth;
                posX = screenWidth / 2;
                posX -= width / 2;
                
                int newY = mode == AnchorMode.BottomMiddle ? screenHeight - height - y : y;

                return new Point((int)posX, newY);
            }

            if(mode == AnchorMode.MiddleLeft || mode == AnchorMode.MiddleRight)
            {
                float posY = screenHeight / 2;
                posY -= height / 2;

                int newX = mode == AnchorMode.MiddleRight ? screenWidth - width - x : x;

                return new Point(newX,(int)posY);
            }

            if(mode == AnchorMode.Middle)
            {
                float posX = screenWidth / 2;
                posX -= width / 2;

                float posY = screenHeight / 2;
                posY -= height / 2;

                return new Point((int)posX - x, (int)posY - y);
            }

            return new Point(0, 0);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.FillRectangle(new Rectangle(GetActualPosition(), new Point(width, height)), color);

        }
    }
}
