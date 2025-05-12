using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ThatOneGame.Structure;

namespace ThatOneGame.GameCode
{
    public class Enemy : SpriteObject
    {
        string basePath;

        public Enemy() : base()
        {
            tileSize = 16;

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            basePath += "..\\Tiles\\Enemy\\Slimes\\Baby Slime 01.png";

            position = new Vector2(12*16, 12*16);

        }


    }
}
