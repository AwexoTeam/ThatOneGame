using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor.Scripts.Definations.Extentions
{
    public static class SpriteBatchExtensions
    {
        public static void DrawTile(this SpriteBatch batch, Texture2D tilemap, Vector2 position, Vector2 tileId, Color color)
        {
            Point tileStart = new Point(0,0);
            Point tileSize = new Point(16,16);

            batch.Draw(tilemap, position, new Rectangle(tileStart, tileSize), color);
        }
    }
}
