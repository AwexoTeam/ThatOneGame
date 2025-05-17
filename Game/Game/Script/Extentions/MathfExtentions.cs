using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatOneGame.Structure;

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

    public static bool Intersects(this Rectangle rect, Vector2 point)
    {
        Point p = new Point((int)point.X, (int)point.Y);
        return Intersects(rect, p);
    }

    public static bool Intersects(this Rectangle rect, Point point)
    {
        if (point.X < rect.X)
            return false;

        if (point.Y < rect.Y)
            return false;

        if (point.X > rect.X + rect.Width)
            return false;

        if (point.Y > rect.Y + rect.Height)
            return false;

        return true;
    }

    public static bool IntersectsWithMouse(this Rectangle rect)
    {
        Vector2 mouse = Input.MousePositionRaw;
        return Intersects(rect, mouse);
    }
}