using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using TiledSharp;


namespace ThatOneGame.Structure.JsonObjects
{
    public class Tile
    {
        public int order;
        public int id;
        public bool isEmpty;
        public Rectangle sourceRect;
        public Rectangle destination;

        public string imagePath;
        public TmxTileset tileset;
        public TmxLayer layer;
        public TmxTilesetTile tile;

        public static Dictionary<string,Texture2D> textures = new Dictionary<string, Texture2D>();


        public Tile(TmxTileset tileset, Rectangle sourceRect, Rectangle destination, string imagePath, bool isEmpty)
        {
            this.sourceRect = sourceRect;
            this.destination = destination;
            this.imagePath = imagePath;
            this.tileset = tileset;
            this.isEmpty = isEmpty;

        }
        public void Draw(SpriteBatch batch)
        {
            if (!textures.ContainsKey(imagePath))
            {
                var texture = Texture2D.FromFile(batch.GraphicsDevice, imagePath);
                textures.Add(imagePath, texture);
            }

            var tileset = textures[imagePath];

            batch.Draw(tileset, destination, sourceRect, Color.White);
        }

        public bool isCollising(Rectangle collision)
        {
            return false;
        }

    }

}
