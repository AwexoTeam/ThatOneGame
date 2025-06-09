using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using RpgGame.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TiledSharp;

namespace RpgGame.Structure
{
    public class Map : Component, IRenderable
    {
        public int playerLayer;
        public string name;
        public string mapPath;
        
        public int playerLayerId;
        public List<Tile> tiles;

        public Map(string name, string mapPath)
        {
            this.mapPath = mapPath;
            this.name = name;
        }

        public override void Start()
        {
            TmxMap map = null;
            tiles = new List<Tile>();

            try
            {
                map = new TmxMap(mapPath);

            }
            catch (ArgumentException e)
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

            for (int i = 0; i < map.Layers.Count; i++)
            {
                var layer = map.Layers[i];
                if (layer == null) continue;

                if(layer.Properties.Count > 0)
                {
                    var index = layer.Properties.ToList().FindIndex(x => x.Key == "Player");
                    playerLayerId = i;
                    Debug.LogDebug("Player Layer at: " + playerLayerId);
                }

                if (layer.Tiles.Count <= 0) continue;
                for (int j = 0; j < layer.Tiles.Count; j++)
                {
                    var _tile = layer.Tiles[j];
                    if(_tile.Gid == 0)
                    {
                        //TODO: Handle empty tiles if needed.
                        continue;
                    }

                    TmxTileset _tileset = null;
                    if(!TryGetTileset(map,_tile, ref _tileset))
                    {
                        Debug.LogError("Couldnt find a tileset encompasing " + _tile.Gid);
                        continue;
                    }

                    string imagePath = _tileset.Image.Source;
                    bool isEmpty = false;
                    
                    int tileWidth = _tileset.TileWidth;
                    int tileHeight = _tileset.TileHeight;


                    Rectangle source = new Rectangle(0, 0, tileWidth, tileHeight);

                    int x = _tile.Gid - _tileset.FirstGid;
                    if(x >= _tileset.Columns)
                    {
                        int y = (int)(x / _tileset.Columns);
                        source.Y = y * tileHeight;

                        x -= (int)(y * _tileset.Columns);
                    }

                    source.X = x * tileWidth;

                    Rectangle destination = new Rectangle(0,0, tileWidth, tileHeight);
                    destination.X = _tile.X * tileWidth;
                    destination.Y = _tile.Y * tileHeight;


                    int id = _tile.Gid;
                    id -= _tileset.FirstGid;

                    Tile t = new Tile(_tileset, source, destination, imagePath, isEmpty);

                    if (_tileset.Tiles.ContainsKey(id))
                    {
                        TmxTilesetTile tt = _tileset.Tiles[id];
                        t.tile = tt;
                    }

                    t.order = i;

                    tiles.Add(t);
                }
            }

            tiles.OrderBy(x => x.order);
        }

        public bool TryGetTileset(TmxMap map, TmxLayerTile tile, ref TmxTileset tileset)
        {
            bool rtn = false;

            foreach (var ts in map.Tilesets)
            {
                int fgid = ts.FirstGid;

                if (tile.Gid < fgid) continue;

                int totalgid = fgid + (int)ts.TileCount;
                if (tile.Gid > totalgid) continue;

                tileset = ts;
                rtn = true;
                break;
            }

            return rtn;
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

        public void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Draw(batch);
            }

            DrawDebug(batch);
        }

        public void DrawDebug(SpriteBatch batch)
        {
            if (!Globals.DebugMode)
                return;

            for (int i = 0; i < tiles.Count; i++)
            {
                var t = tiles[i];
                var cols = Tile.GetCollisionRects(t);
                if (cols == null || cols.Count <= 0)
                    continue;

                for (int j = 0; j < cols.Count; j++)
                {
                    var col = cols[j];
                    batch.DrawRectangle(col, Color.Green);
                }
            }
        }

        public void PostDraw(SpriteBatch batch){ return; }
        public void DrawUI(SpriteBatch batch) { return; }
    }
}
