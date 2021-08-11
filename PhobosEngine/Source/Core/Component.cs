using System;

using PhobosEngine.Serialization;

namespace PhobosEngine
{
    public class Component : ISerializable
    {
        public bool Active {get; set;} = true;

        public GameEntity Entity {get; set;}

        public Transform Transform {get=>Entity.Transform;}

        public virtual void Init() {}

        public virtual void Update() {}

        public virtual void OnParentTransformModified() {}

        public T GetComponent<T>() where T : Component
        {
            return Entity.GetComponent<T>();
        }

        public T[] GetComponents<T>() where T : Component
        {
            return Entity.GetComponents<T>();
        }

        public bool HasComponent<T>() where T : Component
        {
            return Entity.HasComponent<T>();
        }

        public T AddComponent<T>() where T : Component, new()
        {
            return Entity.AddComponent<T>();
        }

        public T AddComponent<T>(T component) where T : Component
        {
            return Entity.AddComponent<T>(component);
        }

        public virtual SerializedInfo Serialize()
        {
            SerializedInfo info = new SerializedInfo();

            // Need to first serialize the real type of this Component
            info.Write("type", GetType().AssemblyQualifiedName);

            // Are we active or not?
            info.Write("active", Active);

            return info;
        }

        public virtual void Deserialize(SerializedInfo info)
        {
            // Whatever is deserializing us has already extracted our type, so that line is gone
            // Just need to grab if we're active or not
            Active = info.ReadBool("active");
        }

        public static Component DeserializeInstance(SerializedInfo info)
        {
            string typeString = info.ReadString("type");
            Type type = Type.GetType(typeString);
            Component component = Activator.CreateInstance(type) as Component;
            component.Deserialize(info);
            return component;
        }
    }
}