
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HentaiGame.Script.Structure
{
    public class Player : Sprite
    {
        private float speed = 50;
        private Vector2 minPos, maxPos;
        private int tileSize = 64;
        public Player(Texture2D texture, Vector2 _position) : base(texture, _position)
        {
            tileWidth = tileSize;
            tileHeight = tileSize;

            origin = new Vector2(tileWidth / 2, tileHeight / 2);
            Console.WriteLine(origin);
        }

        public void Update()
        {
            var state = Keyboard.GetState();

            var direction = Vector2.Zero;
            if (state.IsKeyDown(Keys.W)) direction.Y--;
            if (state.IsKeyDown(Keys.A)) direction.X--;
            if (state.IsKeyDown(Keys.S)) direction.Y++;
            if (state.IsKeyDown(Keys.D)) direction.X++;

            if (direction != Vector2.Zero)
                direction.Normalize();

            position += direction * speed * Engine.deltaTime;
            position = Vector2.Round(position);

            var dx = (Engine.RESOLUTION_WIDTH / 2) - position.X - origin.X;
            var dy = (Engine.RESOLUTION_HEIGHT / 2) - position.Y - origin.Y;

            Engine.transform = Matrix.CreateTranslation(dx, dy, 1);
        }
    }
}
