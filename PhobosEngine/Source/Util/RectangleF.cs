using System;
using Microsoft.Xna.Framework;

namespace PhobosEngine.Math
{
    public struct RectangleF
    {
        // (X , Y) is the top left corner
        // (X + Width, Y + Height) is the bottom right corner
        public float X;
        public float Y;

        public float Width;
        public float Height;


        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleF(Vector2 location, Vector2 size)
        {
            X = location.X;
            Y = location.Y;
            Width = size.X;
            Height = size.Y;
        }

        public static RectangleF Empty = new RectangleF(0, 0, 0, 0);

        public float Bottom => Y + Height;
        public float Top => Y;
        public float Left => X;
        public float Right => X + Width;

        public Vector2 Location {
            get => new Vector2(X, Y);
            set {
                X = value.X;
                Y = value.Y;
            }
        }

        public Vector2 Size {
            get => new Vector2(Width, Height);
            set {
                Width = value.X;
                Height = value.Y;
            }
        }

        public Vector2 Center => new Vector2(X + (Width / 2), (Y + Height / 2));
    
        public bool Contains(int x, int y)
        {
            return X <= x && x < (X + Width) && Y <= y && y < (Y + Height);
        }

        public bool Contains(float x, float y)
        {
            return X <= x && x < (X + Width) && Y <= y && y < (Y + Height);
        }

        public bool Intersects(RectangleF other)
        {
            return other.Left < Right &&
                   Left < other.Right &&
                   other.Top < Bottom &&
                   Top < other.Bottom;
        }

        public bool LineIntersects(Vector2 start, Vector2 end)
        {
            Vector2 direction = end - start;

            float distance = 0f;
            float maxValue = float.MaxValue;

            float recipdX = 1f / direction.X;
            float checkLow = (Left - start.X ) * recipdX;
            float checkHigh = (Right - start.X) * recipdX;

            if(checkLow > checkHigh)
            {
                float temp = checkLow;
                checkLow = checkHigh;
                checkHigh = temp;
            }

            distance = MathF.Max(checkLow, distance);
            maxValue = MathF.Min(checkHigh, maxValue);

            if(distance > maxValue)
            {
                return false;
            }

            float recipdY = 1f / direction.Y;
            checkLow = (Top - start.Y) * recipdY;
            checkHigh = (Bottom - start.Y) * recipdY;
            if(checkLow > checkHigh)
            {
                float temp = checkLow;
                checkLow = checkHigh;
                checkHigh = temp;
            }

            distance = MathF.Max(checkLow, distance);
            maxValue = MathF.Min(checkHigh, maxValue);

            if(distance > maxValue)
            {
                return false;
            }

            return distance <= 1.0f;
        }
    }
}