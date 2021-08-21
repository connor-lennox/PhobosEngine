using System;
using System.Reflection;
using System.Text.Json;
using Microsoft.Xna.Framework.Content;

namespace PhobosEngine.Serialization
{
    public struct ResourceReference : ISerializable
    {
        string resourcePath;

        public void Serialize(Utf8JsonWriter writer)
        {
            writer.WriteString("resourcePath", resourcePath);
        }

        public void Deserialize(JsonElement json)
        {
            if(json.TryGetProperty("resourcePath", out JsonElement elem))
            {
                resourcePath = json.GetProperty("resourcePath").GetString();
            } else {
                resourcePath = "";
            }
        }

    }
}