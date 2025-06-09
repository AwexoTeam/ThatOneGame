using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Script.Utils.Extentions
{
    public static class RandomExtentions
    {
        public static float NextFloat(this Random rng, float min, float max)
        {
            double num = rng.NextDouble();
            num = num * (max - min) + min;

            return (float)num;
        }
    }
}
