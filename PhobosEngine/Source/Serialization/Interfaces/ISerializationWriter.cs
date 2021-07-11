namespace PhobosEngine.Serialization
{
    public interface ISerializationWriter
    {
        void Write(string value);
        void Write(int value);
        void Write(float value);
        void Write(double value);
        void Write(bool value);
        void Write(ISerializable value);
    }
}