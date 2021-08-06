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

        public virtual void Serialize(ISerializationWriter writer)
        {
            // Need to first serialize the real type of this Component
            writer.Write(GetType().AssemblyQualifiedName);

            // Are we active or not?
            writer.Write(Active);
        }

        public virtual void Deserialize(ISerializationReader reader)
        {
            // Whatever is deserializing us has already extracted our type, so that line is gone
            // Just need to grab if we're active or not
            Active = reader.ReadBool();
        }

        public static Component DeserializeInstance(ISerializationReader reader)
        {
            string typeString = reader.ReadString();
            Type type = Type.GetType(typeString);
            Component component = Activator.CreateInstance(type) as Component;
            component.Deserialize(reader);
            return component;
        }
    }
}