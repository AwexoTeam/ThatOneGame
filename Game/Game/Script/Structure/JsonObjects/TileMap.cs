using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace ThatOneGame.Structure.JsonObjects
{
    public class Chunk
    {
        public List<int> data { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Property
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool value { get; set; }
    }

    public class Layer
    {
        public List<int> data { get; set; }
        public int height { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int opacity { get; set; }
        public string type { get; set; }
        public bool visible { get; set; }
        public int width { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public List<Property> properties { get; set; }
    }

    public class TilesetData
    {
        public int firstgid { get; set; }
        public string source { get; set; }

        [JsonIgnore]
        public int tilecount { get; set; }

        [JsonIgnore]
        public int columns { get; set; }

        [JsonIgnore]
        public string name;

        public bool isInTileset(int id)
        {
            if (id < firstgid)
                return false;

            if (id >= firstgid + tilecount)
                return false;

            return true;
        }
    }

    public class Tilemap
    {
        public int compressionlevel { get; set; }
        public int height { get; set; }
        public bool infinite { get; set; }
        public List<Layer> layers { get; set; }
        public int nextlayerid { get; set; }
        public int nextobjectid { get; set; }
        public string orientation { get; set; }
        public string renderorder { get; set; }
        public string tiledversion { get; set; }
        public int tileheight { get; set; }
        public List<TilesetData> tilesets { get; set; }
        public int tilewidth { get; set; }
        public string type { get; set; }
        public string version { get; set; }
        public int width { get; set; }
    }

    public class Object
    {
        public double height { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int rotation { get; set; }
        public string type { get; set; }
        public bool visible { get; set; }
        public double width { get; set; }
        public double x { get; set; }
        public double y { get; set; }
    }

    public class Objectgroup
    {
        public string draworder { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public List<Object> objects { get; set; }
        public int opacity { get; set; }
        public string type { get; set; }
        public bool visible { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Tile
    {
        public int id;
        public bool isEmpty;
        public Rectangle sourceRect;
        public Rectangle destination;

        public string imagePath;
        public Tileset tileset;
        public Layer layer;

        public List<Object> collisions;

        public Tile(Tileset tileset, Rectangle sourceRect, Rectangle destination, string imagePath, bool isEmpty)
        {
            this.sourceRect = sourceRect;
            this.destination = destination;
            this.imagePath = imagePath;
            this.tileset = tileset;
            this.isEmpty = isEmpty;

        }
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(tileset.GetTexture(batch.GraphicsDevice, imagePath), destination, sourceRect, Color.White);
        }

        public bool isCollising(Rectangle collision)
        {
            if (collisions.Count <= 0)
                return false;

            for (int i = 0; i < collisions.Count; i++)
            {
                Rectangle myCollision = GetCollisionRect(i);
                return myCollision.Intersects(collision);
            }
            return false;
        }

        public Rectangle GetCollisionRect(int id)
        {
            var obj = collisions[id];

            int x = (int)obj.x;
            int y = (int)obj.y;

            int width = (int)obj.width;
            int height = (int)obj.height;

            Rectangle myCollision = new Rectangle(0,0, 16, 16);

            myCollision.X = this.destination.X;
            myCollision.Y = this.destination.Y;

            return myCollision;
        }

        public List<Rectangle> GetCollisionRects()
        {
            List<Rectangle> rtn = new List<Rectangle>();
            for (int i = 0; i < collisions.Count; i++)
                rtn.Add(GetCollisionRect(i));

            return rtn;
        }
    }


    public class Tileset
    {
        public int columns { get; set; }
        public string image { get; set; }
        public int imageheight { get; set; }
        public int imagewidth { get; set; }
        public int margin { get; set; }
        public int spacing { get; set; }
        public int tilecount { get; set; }
        public string tiledversion { get; set; }
        public int tileheight { get; set; }
        public List<TileData> tiles { get; set; }
        public int tilewidth { get; set; }
        public string type { get; set; }
        public string version { get; set; }

        [JsonIgnore]
        public int firstgid { get; set; }
        [JsonIgnore]
        public string name;

        [JsonIgnore]
        private Texture2D texture { get; set; }

        public Texture2D GetTexture(GraphicsDevice graphic, string path)
        {
            if (texture != null)
                return texture;

            path = path.Replace("..", "");
            path = AppDomain.CurrentDomain.BaseDirectory + "//" + path;
            texture = Texture2D.FromFile(graphic, path);
            return texture;
        }
    }

    public class TileData
    {
        public int id { get; set; }
        public Objectgroup objectgroup { get; set; }
    }
}
