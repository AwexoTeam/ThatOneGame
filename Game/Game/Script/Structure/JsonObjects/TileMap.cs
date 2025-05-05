using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HentaiGame.Script.Structure.JsonObjects
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
        public List<Property> properties { get; set; }
        public List<Chunk> chunks { get; set; }
        public int height { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int opacity { get; set; }
        public int startx { get; set; }
        public int starty { get; set; }
        public string type { get; set; }
        public bool visible { get; set; }
        public int width { get; set; }
        public int x { get; set; }
        public int y { get; set; }
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
        public List<Tileset> tilesets { get; set; }
        public int tilewidth { get; set; }
        public string type { get; set; }
        public string version { get; set; }
        public int width { get; set; }
    }

    public class Tileset
    {
        public int columns { get; set; }
        public int firstgid { get; set; }
        public string image { get; set; }
        public int imageheight { get; set; }
        public int imagewidth { get; set; }
        public int margin { get; set; }
        public string name { get; set; }
        public int spacing { get; set; }
        public int tilecount { get; set; }
        public int tileheight { get; set; }
        public int tilewidth { get; set; }

        [JsonIgnore]
        private Texture2D texture { get; set; }

        public bool isTileInSet(int id)
        {
            if (id < firstgid)
                return false;

            if (id > tilecount + firstgid)
                return false;

            return true;
        }

        public Texture2D GetTexture(GraphicsDevice graphic, string path)
        {
            if (texture != null)
                return texture;

            Console.WriteLine("Creating texture from path " + path);
            texture = Texture2D.FromFile(graphic, path);
            return texture;
        }
    }


}
