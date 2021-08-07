using Microsoft.Xna.Framework;

namespace PhobosEngine.Serialization
{
    public static class ISerializableWriterMonogameExtensions
    {
        public static void Write(this ISerializationWriter self, Vector2 vec)
        {
            self.Write(vec.X);
            self.Write(vec.Y);
        }

        public static void Write(this ISerializationWriter self, Vector2[] vecs)
        {
            self.Write(vecs.Length);
            for(int i = 0; i < vecs.Length; i++)
            {
                self.Write(vecs[i]);
            }
        }

        public static void Write(this ISerializationWriter self, Color color)
        {
            self.Write(color.PackedValue);
        }
    }
}