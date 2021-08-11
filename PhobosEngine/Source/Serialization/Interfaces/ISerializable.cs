namespace PhobosEngine.Serialization
{
    public interface ISerializable
    {
        SerializedInfo Serialize();
        void Deserialize(SerializedInfo info);
    }
}