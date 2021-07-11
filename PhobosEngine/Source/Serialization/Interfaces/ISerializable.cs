namespace PhobosEngine.Serialization
{
    public interface ISerializable
    {
        void Serialize(ISerializationWriter writer);
        void Deserialize(ISerializationReader reader);
    }
}