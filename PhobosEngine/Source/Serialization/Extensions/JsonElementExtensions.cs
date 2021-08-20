using System;
using System.Text.Json;
using Microsoft.Xna.Framework;

namespace PhobosEngine.Serialization
{
    public static class JsonElementExtensions
    {
        public static Vector2 GetVector2(this JsonElement self)
        {
            if(self.ValueKind != JsonValueKind.Array || self.GetArrayLength() != 2)
            {
                throw new InvalidOperationException("malformed Vector2 serialization");
            }

            var enumerator = self.EnumerateArray();

            Vector2 vector = new Vector2();
            enumerator.MoveNext();
            vector.X = enumerator.Current.GetSingle();
            enumerator.MoveNext();
            vector.Y = enumerator.Current.GetSingle();
            enumerator.Dispose();
            return vector;
        }

        public static Color GetColor(this JsonElement self)
        {
            uint packed;
            if(self.ValueKind != JsonValueKind.Number || !self.TryGetUInt32(out packed))
            {
                throw new InvalidOperationException("malformed Color serialization");
            }
            return new Color(packed);
        }

        public static Rectangle GetRectangle(this JsonElement self)
        {
            int x, y, width, height;
            if(!self.GetProperty("x").TryGetInt32(out x) || !self.GetProperty("y").TryGetInt32(out y) ||
                    self.GetProperty("width").TryGetInt32(out width) || !self.GetProperty("height").TryGetInt32(out height))
            {
                throw new InvalidOperationException("malformed Rectangle serialization");
            }

            return new Rectangle(x, y, width, height);
        }
    }
}