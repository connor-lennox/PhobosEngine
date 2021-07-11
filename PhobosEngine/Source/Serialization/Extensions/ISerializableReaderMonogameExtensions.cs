using Microsoft.Xna.Framework;

namespace PhobosEngine.Serialization
{
    public static class ISerializableReaderMonogameExtensions
    {
        public static Vector2 ReadVector2(this ISerializationReader self)
        {
            return new Vector2(self.ReadFloat(), self.ReadFloat());
        }
    }
}