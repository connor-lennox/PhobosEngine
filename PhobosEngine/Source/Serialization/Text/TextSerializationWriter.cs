using System.IO;

namespace PhobosEngine.Serialization
{
    public class TextSerializationWriter : StreamWriter, ISerializationWriter
    {
        public TextSerializationWriter(string filename) : base(File.OpenWrite(filename)) {}
        public TextSerializationWriter(Stream stream) : base(stream) {}

        public new void Write(string value)
        {
            WriteLine(value.Length);
            WriteLine(value);
        }

        public new void Write(uint value)
        {
            WriteLine(value);
        }

        public new void Write(int value)
        {
            WriteLine(value);
        }

        public new void Write(float value)
        {
            WriteLine(value);
        }

        public new void Write(double value)
        {
            WriteLine(value);
        }

        public new void Write(bool value)
        {
            WriteLine(value);
        }

        public void Write(ISerializable value)
        {
            value.Serialize(this);
        }
    }
}