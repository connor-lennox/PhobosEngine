namespace PhobosEngine.Serialization
{
    public interface ISerializationReader
    {
        string ReadString();
        uint ReadUInt();
        int ReadInt();
        float ReadFloat();
        double ReadDouble();
        bool ReadBool();
    }
}