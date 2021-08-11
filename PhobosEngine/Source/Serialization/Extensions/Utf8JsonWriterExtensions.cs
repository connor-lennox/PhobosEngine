using System.Text.Json;
using Microsoft.Xna.Framework;

namespace PhobosEngine.Serialization
{
    public static class Utf8JsonWriterExtensions
    {
        public static void WriteSerializable(this Utf8JsonWriter self, string key, ISerializable serializable)
        {
            self.WriteStartObject(key);
            serializable.Serialize(self);
            self.WriteEndObject();
        }

        public static void WriteSerializableValue(this Utf8JsonWriter self, ISerializable serializable)
        {
            // Keyless version of WriteSerializable, used for array internals
            self.WriteStartObject();
            serializable.Serialize(self);
            self.WriteEndObject();
        }

        public static void WriteSerializableArray(this Utf8JsonWriter self, string key, ISerializable[] serializables)
        {
            self.WriteStartArray(key);
            foreach(ISerializable serializable in serializables)
            {
                self.WriteSerializableValue(serializable);
            }
            self.WriteEndArray();
        }

        public static void WriteVector2(this Utf8JsonWriter self, string key, Vector2 vector)
        {
            self.WriteStartArray(key);
            self.WriteNumberValue(vector.X);
            self.WriteNumberValue(vector.Y);
            self.WriteEndArray();
        }

        public static void WriteVector2Value(this Utf8JsonWriter self, Vector2 vector)
        {
            // Keyless variant of WriteVector2, used for array internals
            self.WriteStartArray();
            self.WriteNumberValue(vector.X);
            self.WriteNumberValue(vector.Y);
            self.WriteEndArray();
        }

        public static void WriteColor(this Utf8JsonWriter self, string key, Color color)
        {
            self.WriteNumber(key, color.PackedValue);
        }
    }
}