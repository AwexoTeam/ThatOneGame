using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using ThatOneGame.GameCode;

namespace ThatOneGame.Structure
{
    public class TiledScreen : GameScreen
    {
        private Player player;
        private string mapPath;

        public TiledScreen()
        {
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            if (Globals.initialArgs == null || Globals.initialArgs.Length <= 0)
                mapPath = Map.GetFirstMap(basePath);
            else
            {
                Debug.LogDebug("Starting up map " + Globals.initialArgs[0]);
                mapPath = Globals.initialArgs[0];
            }
        }

        public TiledScreen(string mapName)
        {
            this.mapPath = mapName;
        }

        public override void Start()
        {
            base.Start();

            map = new Map("Test", mapPath);
            map.InitializeMap();
        }

        public override void AddGameObject(GameObject gameObject)
        {
            if (gameObject is Player)
                player = (Player)gameObject;

            base.AddGameObject(gameObject);
        }

        public override void Draw(SpriteBatch batch)
        {
            
            bool playerHasBeenDrawn = false;
            foreach (var tile in map.tiles)
            {
                tile.Draw(batch);
                if (map.playerLayer == tile.order && !playerHasBeenDrawn)
                {
                    playerHasBeenDrawn = true;
                    player.Draw(batch);
                }
            }

            if (!playerHasBeenDrawn)
                player.Draw(batch);

            DrawDebug(batch);
            base.Draw(batch);
        }

        public void DrawDebug(SpriteBatch batch)
        {
            if (!Globals.DebugMode)
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
