using System.Text.Json;

namespace PhobosEngine.Serialization
{
    public interface ISerializable
    {
        void Serialize(Utf8JsonWriter writer);
        void Deserialize(JsonElement json);
    }
}