using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using RpgGame.Structure;

namespace RpgGame.Script.Components
{
    public class BoxCollider : Component, IRenderable
    {
        public int width;
        public int height;

        public Vector2 offset;

        public BoxCollider(Rectangle bounds)
        {
            width = bounds.Width;
            height = bounds.Height;

            offset = new Vector2(bounds.X, bounds.Y);
        }

        public BoxCollider(int x, int y,  int width, int height)
        {
            this.width = width;
            this.height = height;

            offset = new Vector2(x, y);
        }

        public void PostDraw(SpriteBatch batch) { }
        public void Draw(SpriteBatch batch)
        {
            if (!Globals.DebugMode)
                return;

            batch.DrawRectangle(GetCollisionRect(), Color.Green);
        }

        public Rectangle GetCollisionRect()
        {
            var x = gameObject.position.X + offset.X;
            var y = gameObject.position.Y + offset.Y;

            return new Rectangle((int)x, (int)y, width, height);
        }

        public bool IsCollidingWith(Tile tile, out Vector2 collisionDir)
        {
            var collisionBox = GetCollisionRect();
            collisionDir = Vector2.Zero;

            var tileCollisions = Tile.GetCollisionRects(tile);
            if (tileCollisions == null)
                return false;

            if (tileCollisions.Count <= 0)
                return false;

            foreach (var collision in tileCollisions)
            {
                //TODO: calc direction
                if (collision.Intersects(collisionBox))
                    collisionDir = new Vector2(collision.X, collision.Y);
                return true;
            }

            return false;
        }

        public bool IsCollidingWith(Rectangle rect, out Vector2 collisionDir)
        {
            var collisionBox = GetCollisionRect();

            //TODO: calc direction
            collisionDir = Vector2.Zero;
            return collisionBox.Intersects(rect);
        }
    }
}