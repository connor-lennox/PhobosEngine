namespace PhobosEngine.Serialization
{
    public interface ISerializationReader
    {
        string ReadString();
        int ReadInt();
        float ReadFloat();
        double ReadDouble();
        bool ReadBool();
    }
}