using System.IO;

namespace PhobosEngine.Serialization
{
    public class TextSerializationReader : StreamReader, ISerializationReader
    {
        public TextSerializationReader(string filename) : base(File.OpenRead(filename)) {}
        public TextSerializationReader(Stream stream) : base(stream) {}

        public bool ReadBool()
        {
            return bool.Parse(ReadLine());
        }

        public double ReadDouble()
        {
            return double.Parse(ReadLine());
        }

        public float ReadFloat()
        {
            return float.Parse(ReadLine());
        }

        public uint ReadUInt()
        {
            return uint.Parse(ReadLine());
        }

        public int ReadInt()
        {
            return int.Parse(ReadLine());
        }

        public string ReadString()
        {
            int length = ReadInt();
            char[] buffer = new char[length];
            ReadBlock(buffer, 0, length);
            // Remove additional newline character
            ReadLine();
            return new string(buffer);
        }
    }
}