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

        public SerializedInfo Serialize()
        {
            SerializedInfo info = new SerializedInfo();
            info.Write("resourcePath", pathToResource);
            return info;
        }

        public void Deserialize(SerializedInfo info)
        {
            pathToResource = info.ReadString("resourcePath");
        }

    }
}