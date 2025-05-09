using ThatOneGame.Structure.JsonObjects;
using ThatOneGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Object = ThatOneGame.Structure.JsonObjects.Object;

namespace ThatOneGame.Scenes
{
    public class SplashScreen : GameScreen
    {
        private Tilemap map;
        public static List<Tile> tiles;
        private Player player;
        private List<Tileset> sets;
        private List<TilesetData> setData;
        
        public override void LoadContent(ContentManager content)
        {
            sets = new List<Tileset>();
            setData = new List<TilesetData>();

            base.LoadContent(content);
            tiles = new List<Tile>();
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory + "//";

            //Get first avaialable map.
            string mapFile = GetMap(basePath);
            map = JsonConvert.DeserializeObject<Tilemap>(File.ReadAllText(mapFile));

            //Load data in from the maps to be used
            foreach (var _tileset in map.tilesets)
            {
                //Get path
                string tilesetPath = basePath + _tileset.source.Replace("..", "");
                string json = File.ReadAllText(tilesetPath);

                //Deserialize and set values that is calced rather than read.
                var set = JsonConvert.DeserializeObject<Tileset>(json);
                set.name = Path.GetFileName(tilesetPath);
                _tileset.name = Path.GetFileName(tilesetPath);

                _tileset.columns = set.columns;
                _tileset.tilecount = set.tilecount;
                
                sets.Add(set);
                setData.Add(_tileset);
            }

            //Get Player texture
            LoadPlayer(basePath);
            
            //Time to fill our tiles collection.
            foreach (var layer in map.layers)
            {
                for (int i = 0; i < layer.data.Count; i++)
                {
                    IterateMap(i, layer);
                }
            }

            //Order the tiles based of layer so it draws right.
            tiles.OrderBy(x => x.layer.id);
        }

        private void LoadPlayer(string basePath)
        {

            var playertex = Texture2D.FromFile(Engine.batch.GraphicsDevice, basePath + "Tiles\\Sprite Pack - New\\16x16\\Base\\Base Character PNG\\Base Idle.png");
            player = new Player(playertex, new Vector2(0, 0));

        }

        private string GetMap(string basePath)
        {
            var files = Directory.GetFiles(basePath);
            var mapFile = Array.Find(files, x => Path.GetExtension(x) == ".tmj");

            return mapFile;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            player.Update();

            var state = Keyboard.GetState();
        }

        private void IterateMap(int iterator, Layer layer)
        {

            //Get the tile ID
            int tileId = layer.data[iterator];
            
            int originalId = tileId;

            //If its empty tile theres no need to do math
            if (tileId == 0)
                return;

            //Find the correct tileset and its tilesetData
            var data = setData.Find(x => x.isInTileset(tileId));
            var tileset = sets.Find(x => x.name == data.name);
            var tileRules = tileset.tiles.Where(x => x.id == tileId - 1);

            //Get all the collisions if it has any.
            List<Object> collisions = new List<Object>();
            foreach (var tileRule in tileRules)
            {
                if(tileRule.objectgroup == null) continue;
                if(tileRule.objectgroup.objects ==  null) continue;
                if (tileRule.objectgroup.objects.Count <= 0) continue;

                collisions.AddRange(tileRule.objectgroup.objects);
            }

            //Since we are 0 indexed we gotta remove the first id of the tileset.
            
            int x = tileId - data.firstgid;
            
            //Initialize our rects for drawing
            Rectangle destination = new Rectangle(0, 0, 16, 16);
            Rectangle sourceRect = new Rectangle(0, 0, 16, 16);

            //To find the x and y this is the math
            if(tileId > tileset.columns)
            {
                int y = x / tileset.columns;
                sourceRect.Y = y;

                x -= y * tileset.columns;
            }

            //Set the values
            sourceRect.X = x;

            //What we calculated was tile coordinates.
            //So to find the pixel coordinates we multi by the tile width and height
            sourceRect.X *= tileset.tilewidth;
            sourceRect.Y *= tileset.tileheight;

            //Same math as before really just based on i instead of tile ID
            int desX = iterator;
            if(desX > map.width - 1)
            {
                int y = desX / map.width;
                destination.Y = y;

                desX -= y * map.width;
            }

            destination.X = desX;
            destination.X *= tileset.tilewidth;
            destination.Y *= tileset.tileheight;

            //Make the tile instance
            Tile tile = new Tile(tileset, sourceRect, destination, tileset.image, false);
            tile.layer = layer;
            tile.id = originalId;

            //Add our collisions
            tile.collisions = collisions.ToList();

            tiles.Add(tile);
        }

        private Layer lastLayer = null;
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);

            foreach (var tile in tiles)
            {
                Layer layer = tile.layer;

                player.Draw(batch);
                tile.Draw(batch);
            }

            //Quick logic here to draw the collisions.
            foreach (var tile in tiles)
            {
                var rects = GetCollisionRects(tile);
                if (rects.Count <= 0)
                    continue;

                foreach (var col in rects)
                {
                    batch.DrawRectangle(col, Color.Lime);
                }
            }
        }


        public Rectangle GetCollisionRect(Tile tile, int id)
        {
            //Values converted for easy debugging
            var obj = tile.collisions[id];

            int x = (int)obj.x;
            int y = (int)obj.y;

            int width = (int)obj.width;
            int height = (int)obj.height;

            Rectangle myCollision = new Rectangle(0, 0, width, height);

            //Set it based on where the tile is drawn
            myCollision.X = tile.destination.X;
            myCollision.Y = tile.destination.Y;

            //Add the starting point of the rectangle.
            myCollision.X += x;
            myCollision.Y += y;

            return myCollision;
        }

        public List<Rectangle> GetCollisionRects(Tile tile)
        {
            List<Rectangle> rtn = new List<Rectangle>();
            for (int i = 0; i < tile.collisions.Count; i++)
                rtn.Add(GetCollisionRect(tile,i));

            return rtn;
        }
    }
}
