using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ThatOneGame.Structure
{
    public class SpriteObject : GameObject
    {
        protected int tileSize;
        protected int tileX;
        protected int tileY;

        protected Texture2D texture;
        protected Rectangle destination;
        protected Rectangle sourceRect;

        public SpriteObject() {  }
        public SpriteObject(Vector2 position) : base(position) { }

        public virtual void Draw(SpriteBatch batch)
        {
            PreDraw(batch);
            if (texture == null)
            {
                return;
            }

            batch.Draw(texture, destination, sourceRect, Color.White);
        }

        protected virtual void PreDraw(SpriteBatch batch)
        {
            destination = new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize);
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

        public virtual void UIDraw(SpriteBatch batch)
        {

        }

    }
}
