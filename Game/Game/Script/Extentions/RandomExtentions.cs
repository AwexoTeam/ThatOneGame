using Microsoft.Xna.Framework;
using System;

namespace HentaiGame.Script.Extentions
{
    public static class RandomExtentions
    {
        public static Vector2 insideUnitCircle(this Random random)
        {
            double R = 1;

            double r = R * Math.Sqrt(random.NextDouble());
            double theta = random.NextDouble() * 2 * Math.PI;

            double x = r * Math.Cos(theta);
            double y = r * Math.Sin(theta);

            return new Vector2((float)x, (float)y);
        }
    }
}
