using HentaiGame.Script.Structure;
using HentaiGame.Script.Structure.JsonObjects;
using HentaiGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HentaiGame.Scenes
{
    public class Tile
    {
        public Rectangle sourceRect;
        public Rectangle destination;

        public string imagePath;
        public Tileset tileset;

        public Tile(Tileset tileset, Rectangle sourceRect, Rectangle destination, string imagePath)
        {
            this.sourceRect = sourceRect;
            this.destination = destination;
            this.imagePath = imagePath;
            this.tileset = tileset;

        }
    }

    public class SplashScreen : GameScreen
    {
        private Tilemap map;
        private List<Tile> tiles;
        private Player player;

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            tiles = new List<Tile>();
            map = JsonConvert.DeserializeObject<Tilemap>(File.ReadAllText("TestMap.tmj"));

            string basePath = System.AppDomain.CurrentDomain.BaseDirectory + "//";

            var playertex = Texture2D.FromFile(Engine.batch.GraphicsDevice,basePath + "Tiles\\Sprite Pack - New\\16x16\\Base\\Base Character PNG\\Base Idle.png");
            player = new Player(playertex, new Vector2(0, 0));

            foreach (var layer in map.layers)
            {
                var iterator = layer.chunks.First().data;
                for (int i = 0; i < iterator.Count; i++)
                {
                    int tileId = iterator[i];
                    Tileset tileset = map.tilesets.Find(x => x.isTileInSet(tileId));
                    if (tileset == null)
                    {
                        Console.WriteLine("Couldnt find tileset for id " + tileId);
                        continue;
                    }

                    tileId = tileId - tileset.firstgid;

                    Point tileCordinates = new Point(0, 0);
                    if (tileId > tileset.columns)
                    {
                        int y = tileId / tileset.columns;
                        tileCordinates.Y = y;

                        tileId -= y * tileset.columns;
                    }

                    tileCordinates.X = tileId;
                    tileCordinates.X = tileCordinates.X * 16;
                    tileCordinates.Y = tileCordinates.Y * 16;

                    string path = tileset.image;

                    path = basePath + path;

                    Rectangle destination = new Rectangle(0, 0, 16, 16);
                    if(i >= layer.width)
                        destination.Y = i / layer.width;

                    destination.X = i - (destination.Y * layer.width);

                    destination.X = destination.X * 16;
                    destination.Y = destination.Y * 16;

                    Rectangle sourceRectangle = new Rectangle(tileCordinates, new Point(16, 16));
                    tiles.Add(new Tile(tileset, sourceRectangle, destination, path));
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            player.Update();
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            foreach (var tile in tiles)
            {
                Vector2 newPos = new Vector2()
                {
                    X = (int)tile.destination.X,
                    Y = (int)tile.destination.Y
                };

                var tex = tile.tileset.GetTexture(batch.GraphicsDevice, tile.imagePath);
                batch.Draw(tex, tile.destination, tile.sourceRect, Color.White);
            }

            player.Draw(batch);
        }
    }
}
