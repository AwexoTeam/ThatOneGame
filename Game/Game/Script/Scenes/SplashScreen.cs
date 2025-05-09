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
using System.Linq;
using System.Timers;

namespace ThatOneGame.Scenes
{
    public class SplashScreen : GameScreen
    {
        public static Map map;
        private Player player;
        private int playerLayer;


        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory + "//";

            //Get first avaialable map.
            string mapFile = Map.GetFirstMap(basePath);
            map = new Map("Test", mapFile);
            map.InitializeMap();

            //Get Player texture
            player = new Player(new Vector2(0,0));
            player.Init();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            player.Update();
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);

            bool playerHasBeenDrawn = false;
            foreach (var tile in map.tiles)
            {
                tile.Draw(batch);
                if (playerLayer == tile.order)
                {
                    playerHasBeenDrawn = true;
                    player.Draw(batch);
                }
            }

            if (!playerHasBeenDrawn)
                player.Draw(batch);

            if (!Player.debugMode)
                return;

            foreach (var tile in map.tiles)
            {
                var rects = Tile.GetCollisionRects(tile);
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

    }
}
