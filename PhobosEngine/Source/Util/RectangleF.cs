using Microsoft.Xna.Framework;

namespace PhobosEngine.Util
{
    public struct RectangleF
    {
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
                   other.Right > Left &&
                   other.Top < Bottom &&
                   other.Bottom < Top;
        }
    }
}