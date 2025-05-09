using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatOneGame.Structure.JsonObjects;
using TiledSharp;

namespace ThatOneGame.Structure
{
    public class Map
    {

        public string name;
        public string mapPath;
        public TmxMap map;
        public List<Tile> tiles;

        public Map(string name, string mapPath)
        {
            tiles = new List<Tile>();

            this.mapPath = mapPath;
            this.name = name;
            
        }

        public void InitializeMap()
        {
            try
            {
                map = new TmxMap(mapPath);

            }
            catch (System.ArgumentException e)
            {
                Console.WriteLine("Invalid Arugment probably no map files");
                Console.WriteLine(e.Message);
                Console.WriteLine();

            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Couldnt find certain file");
                Console.WriteLine(e.Message);
                Console.WriteLine();

            }

            if (map == null)
                return;

            //Time to fill our tiles collection.
            foreach (var layer in map.Layers)
            {
                for (int i = 0; i < layer.Tiles.Count; i++)
                {
                    IterateMap(i, layer);
                }
            }

            tiles.OrderBy(x => x.order);
        }

        private void IterateMap(int iterator, TmxLayer layer)
        {
            int order = map.Layers.ToList().FindIndex(x => x == layer);
            order--;

            int tileId = layer.Tiles[iterator].Gid;

            int originalId = tileId;

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
            if (tileId > tileset.Columns)
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
            if (desX > map.Width - 1)
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
            _tile.order = order;

            tiles.Add(_tile);
        }

        public static string GetFirstMap(string basePath)
        {
            try
            {
                var files = Directory.GetFiles(basePath + @"..\Tiled\");
                if (files.Length <= 0)
                    return string.Empty;

                var mapFile = Array.Find(files, x => Path.GetExtension(x) == ".tmx");

                return mapFile;
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Dir not found \n" + basePath);
                return string.Empty;
            }
        }
    }
}
