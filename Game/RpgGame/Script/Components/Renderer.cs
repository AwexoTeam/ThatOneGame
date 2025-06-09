using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using RpgGame.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Components
{
    public class Renderer : Component, IRenderable
    {
        public int tileSize { get; protected set; }

        public Texture2D texture;
        public int tileX;
        public int tileY;

        protected Rectangle destination;
        protected Rectangle sourceRect;

        public Renderer(Texture2D texture, int tileSize, int tileX, int tileY)
        {
            this.texture = texture;
            this.tileSize = tileSize;
            this.tileX = tileX;
            this.tileY = tileY;
        }

        public void Draw(SpriteBatch batch)
        {
            SetValues();

            if (texture == null)
            {
                batch.FillRectangle(destination, Color.White);
                return;
            }

            batch.Draw(texture, destination, sourceRect, Color.White);
        }

        public void SetValues()
        {
            destination = new Rectangle((int)gameObject.position.X, (int)gameObject.position.Y, tileSize, tileSize);
            destination.X -= tileSize / 2;
            destination.Y -= tileSize / 2;

            //TODO: no magic numbers (Half a tiles width and height on the map)
            destination.X += 8;
            destination.Y += 8;

            int x = tileX * tileSize;
            int y = tileY * tileSize;

            int width = tileSize;
            int height = tileSize;

            sourceRect = new Rectangle(x, y, width, height);
        }

        public void PostDraw(SpriteBatch batch)
        {

        }
    }
}
