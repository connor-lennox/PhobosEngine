using System;
using System.Collections.Generic;

using PhobosEngine.Serialization;

namespace PhobosEngine
{
    public class GameEntity : ISerializable
    {
        public bool Active {get; set;} = true;

        public Transform Transform { get => GetComponent<Transform>(); }
        private List<Component> components = new List<Component>();

        public GameEntity()
        {
            // All GameEntities get a Transform for free
            AddComponent(new Transform());
        }

        public void Update()
        {
            foreach(var item in components)
            {
                if(item.Active) item.Update();
            }
        }

        public void AddComponent(Component comp)
        {
            if(comp.Entity != null)
            {
                throw new InvalidOperationException("Components that are already owned cannot be added to an Entity.");
            }
            components.Add(comp);
            comp.Entity = this;
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

        public void RemoveComponent(Component comp)
        {
            if(comp.Entity != this)
            {
                throw new InvalidOperationException("A Component cannot be removed from an Entity that is not its owner.");
            }

            if(comp.GetType() == typeof(Transform)) {
                throw new InvalidOperationException("Transforms cannot be removed!");
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
            writer.Write(components.Count);
            foreach(Component c in components)
            {
                c.Serialize(writer);
            }
        }

        public void Deserialize(ISerializationReader reader)
        {
            ClearComponents();
            int numComponents = reader.ReadInt();
            for(int i = 0; i < numComponents; i++)
            {
                components.Add(Component.DeserializeInstance(reader));
            }
        }
    }
}