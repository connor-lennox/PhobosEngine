namespace PhobosEngine.Serialization
{
    public static class ISerializableWriterExtensions
    {
        public static void WriteSerializableArray(this ISerializationWriter self, ISerializable[] arr)
        {
            self.Write(arr.Length);
            foreach(ISerializable elem in arr)
            {
                self.Write(elem);
            }
        }
    }
}