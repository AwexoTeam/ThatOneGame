using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ThatOneGame.Structure
{
    public class Sprite
    {
        public Texture2D texture { get; private set; }
        public Vector2 position;
        public Vector2 origin;

        public int tileX;
        public int tileY;

        public int tileWidth;
        public int tileHeight;

        public Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;

            origin = new Vector2(tileWidth / 2, tileHeight / 2);
        }

        public virtual void Draw(SpriteBatch batch)
        {

            batch.Draw(texture, position, new Rectangle(tileX * tileWidth, tileY * tileHeight, tileWidth, tileHeight), Color.White);
        }
    }
}
