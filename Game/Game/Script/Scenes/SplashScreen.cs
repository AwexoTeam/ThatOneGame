using ThatOneGame.Structure.JsonObjects;
using ThatOneGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using System;
using System.Collections.Generic;
using System.IO;
using TiledSharp;

namespace ThatOneGame.Scenes
{
    public class SplashScreen : GameScreen
    {
        private TmxMap map;
        public static List<Tile> tiles;
        private Player player;
        
        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            tiles = new List<Tile>();
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory + "//";

            //Get first avaialable map.
            string mapFile = GetMap(basePath);

            map = new TmxMap(mapFile);

            //Get Player texture
            LoadPlayer(basePath);
            
            //Time to fill our tiles collection.
            foreach (var layer in map.Layers)
            {
                for (int i = 0; i < layer.Tiles.Count; i++)
                {
                    IterateMap(i, layer);
                }
            }

        }

        private void LoadPlayer(string basePath)
        {

            var playertex = Texture2D.FromFile(Engine.batch.GraphicsDevice, basePath + "Data\\Tilesets\\Tiles\\Sprite Pack - New\\16x16\\Base\\Base Character PNG\\Base Idle.png");
            player = new Player(playertex, new Vector2(0, 0));

        }

        private string GetMap(string basePath)
        {
            Console.WriteLine(basePath+ "//Data");
            var files = Directory.GetFiles(basePath+ "//Data");
            if (files.Length <= 0)
                return string.Empty;

            var mapFile = Array.Find(files, x => Path.GetExtension(x) == ".tmx");

            return mapFile;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            player.Update();

            var state = Keyboard.GetState();
        }

        private void IterateMap(int iterator, TmxLayer layer)
        {

            //Get the tile ID
            int tileId = layer.Tiles[iterator].Gid;
            
            int originalId = tileId;

            //If its empty tile theres no need to do math
            if (tileId == 0)
                return;

            TmxLayerTile tile = layer.Tiles[iterator];
            TmxTileset tileset = null;

            foreach (var ts in map.Tilesets)
            {
                if (tileId < ts.FirstGid)
                    continue;

                if (tileId > ts.FirstGid + ts.TileCount)
                    continue;

                tileset = ts;
            }

            if (tileset == null)
                return;

            int tilesetId = tileId - tileset.FirstGid;
            TmxTilesetTile tilesetTile = null;
            if (tileset.Tiles.ContainsKey(tilesetId))
                tilesetTile = tileset.Tiles[tilesetId];

            //Since we are 0 indexed we gotta remove the first id of the tileset.
            
            int x = tileId - tileset.FirstGid;
            
            //Initialize our rects for drawing
            Rectangle destination = new Rectangle(0, 0, 16, 16);
            Rectangle sourceRect = new Rectangle(0, 0, 16, 16);

            //To find the x and y this is the math
            if(tileId > tileset.Columns)
            {
                int y = (int)(x / tileset.Columns);
                sourceRect.Y = y;

                x -= (int)(y * tileset.Columns);
            }

            //Set the values
            sourceRect.X = x;

            //What we calculated was tile coordinates.
            //So to find the pixel coordinates we multi by the tile width and height
            sourceRect.X *= tileset.TileWidth;
            sourceRect.Y *= tileset.TileHeight;

            //Same math as before really just based on i instead of tile ID
            int desX = iterator;
            if(desX > map.Width - 1)
            {
                int y = desX / map.Width;
                destination.Y = y;

                desX -= y * map.Width;
            }

            destination.X = desX;
            destination.X *= tileset.TileWidth;
            destination.Y *= tileset.TileHeight;

            //Make the tile instance
            Tile _tile = new Tile(tileset, sourceRect, destination, tileset.Image.Source, false);
            _tile.layer = layer;
            _tile.id = originalId;
            _tile.tile = tilesetTile;

            //Add our collisions
            
            tiles.Add(_tile);
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);

            foreach (var tile in tiles)
            {
                player.Draw(batch);
                tile.Draw(batch);
            }

            //Quick logic here to draw the collisions.
            foreach (var tile in tiles)
            {
                var rects = GetCollisionRects(tile);
                if (rects == null)
                    continue;

                if (rects.Count <= 0)
                    continue;

                foreach (var col in rects)
                {
                    batch.DrawRectangle(col, Color.Lime);
                }
            }
        }


        public Rectangle GetCollisionRect(Tile tile, int groupId, int id)
        {
            var data = tile.tile.ObjectGroups[groupId].Objects[id];

            Rectangle myCollision = new Rectangle(0, 0, (int)data.Width, (int)data.Height);
            
            myCollision.X = tile.destination.X;
            myCollision.Y = tile.destination.Y;

            int x = (int)data.X;
            int y = (int)data.Y;

            myCollision.X += x;
            myCollision.Y += y;

            return myCollision;
        }

        public List<Rectangle> GetCollisionRects(Tile tile)
        {
            List<Rectangle> rtn = new List<Rectangle>();
            if (tile.tile == null)
                return null;

            for (int i = 0; i < tile.tile.ObjectGroups.Count; i++)
            {
                for (int j = 0; j < tile.tile.ObjectGroups[i].Objects.Count; j++)
                {
                    rtn.Add(GetCollisionRect(tile, i, j));
                }
            }
            return rtn;
        }
    }
}
