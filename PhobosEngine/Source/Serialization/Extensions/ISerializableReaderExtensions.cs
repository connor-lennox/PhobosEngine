using System.Collections.Generic;

namespace PhobosEngine.Serialization
{
    public static class ISerializableReaderExtensions
    {
        public static T[] ReadSerializableArray<T>(this ISerializationReader self) where T : ISerializable, new()
        {
            int count = self.ReadInt();
            T[] arr = new T[count];

            for(int i = 0; i < count; i++)
            {
                T elem = new T();
                self.ReadSerializableInto(elem);
                arr[i] = elem;
            }

            return arr;
        }

        public static void ReadSerializableInto(this ISerializationReader self, ISerializable serializable)
        {
            serializable.Deserialize(self);
        }
    }
}