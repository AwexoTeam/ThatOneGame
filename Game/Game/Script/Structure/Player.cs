
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

namespace ThatOneGame.Structure
{
    public class Player : Sprite
    {
        private float speed = 50;
        private Vector2 minPos, maxPos;
        private int tileSize = 64;
        private Vector2 direction;

        private Rectangle hitbox;
        private Rectangle collisionBox;
        private Rectangle tileTarget;

        public Player(Texture2D texture, Vector2 _position) : base(texture, _position)
        {
            tileWidth = tileSize;
            tileHeight = tileSize;

            origin = new Vector2(tileWidth / 2, tileHeight / 2);
            Console.WriteLine(origin);

            hitbox = new Rectangle(0, 0, 16, 16);
            collisionBox = new Rectangle(0, 0, 16, 8);
        }

        public void Update()
        {

            var state = Keyboard.GetState();

            direction = Vector2.Zero;
            if (state.IsKeyDown(Keys.W)) direction.Y--;
            if (state.IsKeyDown(Keys.A)) direction.X--;
            if (state.IsKeyDown(Keys.S)) direction.Y++;
            if (state.IsKeyDown(Keys.D)) direction.X++;

            Vector2 directionNormalized = direction;
            if (direction != Vector2.Zero)
                directionNormalized.Normalize();

            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;

            hitbox.X += (int)origin.X;
            hitbox.Y += (int)origin.Y;

            collisionBox.X = hitbox.X;
            collisionBox.Y = hitbox.Y;

            collisionBox.X += (int)direction.X * 16;
            collisionBox.Y += (int)direction.Y * 16 + 8;

            var tile = SplashScreen.tiles.FindAll(x => x.destination.Intersects(collisionBox));

            if (tile.Count <= 0)
                return;

            tileTarget = tile.First().destination;
            if (tile.Any(x => x.isCollising(collisionBox)))
                return;



            position += directionNormalized * speed * Engine.deltaTime;
            position = Vector2.Round(position);

            var dx = (Engine.RESOLUTION_WIDTH / 2) - position.X - origin.X;
            var dy = (Engine.RESOLUTION_HEIGHT / 2) - position.Y - origin.Y;

            Engine.transform = Matrix.CreateTranslation(dx, dy, 1);

        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            
            batch.DrawRectangle(hitbox, Color.Pink);
            batch.DrawRectangle(collisionBox, Color.Blue);
            batch.DrawRectangle(tileTarget, Color.Red);
        }
    }
}
