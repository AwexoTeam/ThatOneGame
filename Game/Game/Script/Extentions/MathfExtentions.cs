using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class MathfExtentions
{
    public static float Lerp(this float a, float b, float f)
    { return (float)(a * (1.0 - f) + b * f); }

    public static Vector2 Lerp(this Vector2 a, Vector2 b, float f)
    {
        float lerpedX = a.X.Lerp(b.X, f);
        float lerpedY = a.Y.Lerp(b.Y, f);

        return new Vector2(lerpedX, lerpedY);
    } 
}