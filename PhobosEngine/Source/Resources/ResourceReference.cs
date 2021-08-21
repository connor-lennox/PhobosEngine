using System;
using System.Reflection;
using System.Text.Json;
using Microsoft.Xna.Framework.Content;

using PhobosEngine.Serialization;

namespace PhobosEngine
{
    public struct ResourceReference : ISerializable
    {
        public string resourcePath;

        public void Serialize(Utf8JsonWriter writer)
        {
            writer.WriteString("resourcePath", resourcePath);
        }

        public void Deserialize(JsonElement json)
        {
            resourcePath = json.GetProperty("resourcePath").GetString();
        }

    }
}