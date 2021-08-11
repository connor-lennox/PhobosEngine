using System;
using System.Reflection;
using System.Text.Json;
using Microsoft.Xna.Framework.Content;

namespace PhobosEngine.Serialization
{
    public struct SerializedResourceReference : ISerializable
    {
        string pathToResource;

        public T Load<T>(ContentManager manager)
        {
            return manager.Load<T>(pathToResource);
        }

        public void Serialize(Utf8JsonWriter writer)
        {
            writer.WriteString("resourcePath", pathToResource);
        }

        public void Deserialize(JsonElement json)
        {
            if(json.TryGetProperty("resourcePath", out JsonElement elem))
            {
                pathToResource = json.GetProperty("resourcePath").GetString();
            } else {
                pathToResource = "";
            }
        }

    }
}