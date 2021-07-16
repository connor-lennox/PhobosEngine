using System;
using System.Collections.Generic;

using PhobosEngine.Serialization;

namespace PhobosEngine
{
    public class GameEntity : ISerializable
    {
        private static uint _nextId = 0;

        public bool Active {get; set;} = true;

        public Transform Transform { get; private set; }
        private List<Component> components = new List<Component>();

        public uint Id { get; private set; }

        public GameEntity()
        {
            Transform = new Transform();
            Id = _nextId++;     
        }

        public void Update()
        {
            foreach(var item in components)
            {
                if(item.Active) item.Update();
            }
        }

        public T AddComponent<T>(T comp) where T : Component
        {
            if(comp.Entity != null)
            {
                throw new InvalidOperationException("Components that are already owned cannot be added to an Entity.");
            }
            components.Add(comp);
            comp.Entity = this;

            return comp;
        }

        public T GetComponent<T>() where T : Component
        {
            foreach(Component comp in components)
            {
                if(comp is T)
                {
                    return comp as T;
                }
            }
            return null;
        }

        public T[] GetComponents<T>() where T : Component
        {
            List<T> comps = new List<T>();
            foreach(Component comp in components)
            {
                if(comp is T)
                {
                    comps.Add(comp as T);
                }
            }
            return comps.ToArray();
        }

        public bool RemoveComponent<T>() where T: Component
        {
            Component comp = GetComponent<T>();
            if(comp != null)
            {
                RemoveComponent(comp);
                return true;
            }
            return false;
        }

        public void RemoveComponent(Component comp)
        {
            if(comp.Entity != this)
            {
                throw new InvalidOperationException("A Component cannot be removed from an Entity that is not its owner.");
            }
            comp.Entity = null;
            components.Remove(comp);
        }

        public bool HasComponent<T>() where T : Component
        {
            foreach(Component comp in components)
            {
                if(comp is T)
                {
                    return true;
                }
            }
            return false;
        }

        public void ClearComponents()
        {
            components.Clear();
        }

        public void Serialize(ISerializationWriter writer)
        {
            Transform.Serialize(writer);
            writer.Write(components.Count);
            foreach(Component c in components)
            {
                c.Serialize(writer);
            }
        }

        public void Deserialize(ISerializationReader reader)
        {
            Transform.Deserialize(reader);
            ClearComponents();
            int numComponents = reader.ReadInt();
            for(int i = 0; i < numComponents; i++)
            {
                components.Add(Component.DeserializeInstance(reader));
            }
        }
    }
}