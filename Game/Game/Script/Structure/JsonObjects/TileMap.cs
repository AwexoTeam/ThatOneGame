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

        public static Rectangle GetCollisionRect(Tile tile, TmxTilesetTile tsTile, int groupId, int id)
        {
            var data = tsTile.ObjectGroups[groupId].Objects[id];

            Rectangle myCollision = new Rectangle(0, 0, (int)data.Width, (int)data.Height);

            myCollision.X = tile.destination.X;
            myCollision.Y = tile.destination.Y;

            int x = (int)data.X;
            int y = (int)data.Y;

            myCollision.X += x;
            myCollision.Y += y;

            return myCollision;
        }

        public static List<Rectangle> GetCollisionRects(Tile tile)
        {
            List<Rectangle> rtn = new List<Rectangle>();
            if (tile.tile == null)
                return null;

            TmxTilesetTile checkTile = tile.tile;
            foreach (var property in tile.tile.Properties)
            {
                if (property.Key.ToLower() != "collision")
                    continue;

                var num = property.Value;
                var collisionTile = tile.tileset.Tiles[int.Parse(num)];
                checkTile = collisionTile;
            }

            for (int i = 0; i < checkTile.ObjectGroups.Count; i++)
            {
                for (int j = 0; j < checkTile.ObjectGroups[i].Objects.Count; j++)
                {
                    rtn.Add(GetCollisionRect(tile, checkTile, i, j));
                }
            }
            return rtn;
        }
    }

}
