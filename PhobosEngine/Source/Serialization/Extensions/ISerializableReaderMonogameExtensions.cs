using Microsoft.Xna.Framework;

namespace PhobosEngine.Serialization
{
    public static class ISerializableReaderMonogameExtensions
    {
        public static Vector2 ReadVector2(this ISerializationReader self)
        {
            return new Vector2(self.ReadFloat(), self.ReadFloat());
        }

        public static Vector2[] ReadVector2Array(this ISerializationReader self)
        {
            int length = self.ReadInt();
            Vector2[] arr = new Vector2[length];
            for(int i = 0; i < length; i++)
            {
                arr[i] = self.ReadVector2();
            }

            return arr;
        }
    }
}