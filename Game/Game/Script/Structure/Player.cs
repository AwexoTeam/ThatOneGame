
using ThatOneGame.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatOneGame.Structure.JsonObjects;
using SharpDX.Direct3D9;

namespace ThatOneGame.Structure
{
    public enum Direction
    {
        North,
        South,
        West,
        East,

    }

    public class Player
    {
        public int animationTick;
        public Texture2D texture { get; protected set; }
        public Vector2 position;
        
        public int tileX;
        public int tileY;

        public int tileWidth;
        public int tileHeight;

        private float speed = 50;
        private Vector2 minPos, maxPos;
        private int tileSize = 64;
        private Vector2 direction;
        private Direction dir;

        private Rectangle hitbox;
        private Rectangle collisionBox;
        private Rectangle tileTarget;

        private bool hasInit = false;

        public Player(Vector2 _position)
        {
            position = _position;

            tileWidth = tileSize;
            tileHeight = tileSize;

            hitbox = new Rectangle(0, 0, 16, 16);
            collisionBox = new Rectangle(0, 0, 16, 8);
        }

        public void Init()
        {
            var graphics = Engine.batch.GraphicsDevice;
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory + "//";
            basePath += "Tiles\\Sprite Pack - New\\16x16\\Base\\Base Character PNG\\Base Idle.png";

            texture = Texture2D.FromFile(graphics, basePath);
        }

        public void Update()
        {
            var state = Keyboard.GetState();

            direction = Vector2.Zero;
            if (state.IsKeyDown(Keys.W)) direction.Y--;
            if (state.IsKeyDown(Keys.A)) direction.X--;
            if (state.IsKeyDown(Keys.S)) direction.Y++;
            if (state.IsKeyDown(Keys.D)) direction.X++;

            SetDirection();
            //tileX = animationTick;
            //tileY = (int)dir;

            Vector2 directionNormalized = direction;
            if (direction != Vector2.Zero)
                directionNormalized.Normalize();

            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;

            collisionBox.X = hitbox.X;
            collisionBox.Y = hitbox.Y;

            collisionBox.X += (int)direction.X;
            collisionBox.Y += (int)direction.Y + 8;

            var tiles = SplashScreen.map.tiles.FindAll(x => x.destination.Intersects(collisionBox));

            if (tiles.Count <= 0)
                return;

            bool isColling = false;
            foreach (var tile in tiles)
            {
                if (!isCollising(tile))
                    continue;

                isColling = true;
                break;
            }

            if (isColling && hasInit)
                return;

            position += directionNormalized * speed * Engine.deltaTime;
            position = Vector2.Round(position);

            var dx = (Engine.RESOLUTION_WIDTH / 2) - position.X;
            var dy = (Engine.RESOLUTION_HEIGHT / 2) - position.Y;

            Engine.transform = Matrix.CreateTranslation(dx, dy, 1);

            if (!hasInit)
                hasInit = true;
        }

        private void SetDirection()
        {
            if (direction == Vector2.Zero)
                return;

            if (direction.Y < 0)
                dir = Direction.North;

            if (direction.Y > 0)
                dir = Direction.South;

            if (direction.X < 0)
                dir = Direction.West;

            if(direction.X > 0)
                dir = Direction.East;
        }

        public bool isCollising(Tile tile)
        {
            var tileCollisions = Tile.GetCollisionRects(tile);
            if (tileCollisions == null)
                return false;

            if (tileCollisions.Count <= 0)
                return false;

            foreach(var collision in tileCollisions)
            {
                if (collision.Intersects(collisionBox))
                    return true;
            }

            return false;
        }


        public void Draw(SpriteBatch batch)
        {
            Rectangle destination = new Rectangle((int)position.X, (int)position.Y, tileWidth, tileHeight);
            destination.X -= tileWidth / 2;
            destination.Y -= tileHeight / 2;

            Rectangle sourceRect = new Rectangle(tileX * tileWidth, tileY * tileHeight, tileWidth, tileHeight);
            batch.Draw(texture, destination, sourceRect, Color.White);

            batch.DrawRectangle(hitbox, Color.Pink);
            batch.DrawRectangle(collisionBox, Color.Blue);
            batch.DrawRectangle(tileTarget, Color.Red);
        }
    }
}
