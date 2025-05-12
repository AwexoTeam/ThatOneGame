using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatOneGame.Structure
{
    public class EntityBase : AnimatedObject
    {
        //Collision Specific
        protected Rectangle hitbox;
        protected Rectangle collisionBox;
        protected Rectangle tileTarget;

        //Character Sprite Specific
        protected string baseName;
        protected Dictionary<EntityState, Texture2D> stateTextures;

        protected Vector2 direction;
        protected Direction dir;
        protected Direction lastDir;

        protected EntityState state;

        public EntityBase()
        {
            collisionBox = new Rectangle(0, 0, 16, 8);
            hitbox = new Rectangle(0, 0, 16, 16);

        }

        public EntityBase(Vector2 position) : base(position)
        {
            collisionBox = new Rectangle(0, 0, 16, 8);
            hitbox = new Rectangle(0, 0, 16, 16);
        }

        public override void Start()
        {
            base.Start();
            
            stateTextures = new Dictionary<EntityState, Texture2D>();

            foreach (var state in Enum.GetValues(typeof(EntityState)).Cast<EntityState>())
            {
                Texture2D _texture = null;
                if (baseName == "NILL")
                {
                    Console.WriteLine(basePath);
                    _texture = Texture2D.FromFile(Engine.batch.GraphicsDevice, $"{basePath}");
                    stateTextures.Add(state, _texture);
                    continue;
                }

                _texture = Texture2D.FromFile(Engine.batch.GraphicsDevice, $"{basePath}//{baseName} {state}.png");
                stateTextures.Add(state, _texture);
            }


        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;

            collisionBox.X = hitbox.X;
            collisionBox.Y = hitbox.Y;

            collisionBox.X += (int)direction.X;
            collisionBox.Y += (int)direction.Y + 8;
        }

        protected override void TickAnimation()
        {
            texture = stateTextures[state];
            base.TickAnimation();
        }

        public bool IsCollidingWith(Tile tile, out Vector2 collisionDir)
        {
            collisionDir = Vector2.Zero;
            var tileCollisions = Tile.GetCollisionRects(tile);
            if (tileCollisions == null)
                return false;

            if (tileCollisions.Count <= 0)
                return false;

            foreach (var collision in tileCollisions)
            {
                if (collision.Intersects(collisionBox))
                    collisionDir = new Vector2(collision.X, collision.Y);
                return true;
            }

            return false;
        }

        protected void SetDirection()
        {
            if (direction == Vector2.Zero)
                return;

            if (direction.Y < 0)
                dir = Direction.North;

            if (direction.Y > 0)
                dir = Direction.South;

            if (direction.X < 0)
                dir = Direction.West;

            if (direction.X > 0)
                dir = Direction.East;

            if (lastDir != dir)
            {
                lastDir = dir;
                animationTick = 0;
            }
        }

    }
}
