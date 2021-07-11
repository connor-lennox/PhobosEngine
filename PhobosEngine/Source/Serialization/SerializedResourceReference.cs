using System;
using System.Reflection;
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

        public void Serialize(ISerializationWriter writer)
        {
            writer.Write(pathToResource);
        }

        public void Deserialize(ISerializationReader reader)
        {
            pathToResource = reader.ReadString();
        }

    }
}