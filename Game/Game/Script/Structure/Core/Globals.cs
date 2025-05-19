using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatOneGame.Structure
{
    public static class Globals
    {
        public static string[] initialArgs;
        public static int RESOLUTION_WIDTH = 400;
        public static int RESOLUTION_HEIGHT = 225;

        public static bool DebugMode;
        public static float scale;
        public static Vector2 preferedResolution { get { return new Vector2(RESOLUTION_WIDTH, RESOLUTION_HEIGHT); } }
        public static Vector2 screenSize;
        public static SpriteFont font;
        public static Random random;
    }
}
