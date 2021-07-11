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
    }
}