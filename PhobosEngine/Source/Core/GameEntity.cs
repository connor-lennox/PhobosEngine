using System;
using System.Collections.Generic;
using System.Text.Json;

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
            Transform = new Transform(this);
            Id = _nextId++;     
        }

        public void Update()
        {
            foreach(var item in components)
            {
                if(item.Active) item.Update();
            }
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T newComp = new T();
            components.Add(newComp);
            newComp.Entity = this;
            newComp.Init();

            return newComp;
        }

        public T AddComponent<T>(T comp) where T : Component
        {
            if(comp.Entity != null)
            {
                throw new InvalidOperationException("Components that are already owned cannot be added to an Entity.");
            }
            components.Add(comp);
            comp.Entity = this;
            comp.Init();

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

        public bool TryGetComponent<T>(out T component) where T : Component
        {
            component = GetComponent<T>();
            return component != null;
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

        public void TransformModified()
        {
            // Alert components that we changed
            foreach(Component component in components)
            {
                component.OnParentTransformModified();
            }
        }

        public void Serialize(Utf8JsonWriter writer)
        {
            writer.WriteNumber("id", Id);
            writer.WriteSerializable("transform", Transform);
            writer.WriteSerializableArray("components", components.ToArray());

            // TODO: Serialize reference to parent (by ID? how to deserialize?)
        }

        public void Deserialize(JsonElement json)
        {
            Id = json.GetProperty("id").GetUInt32();
            Transform.Deserialize(json.GetProperty("transform"));
            ClearComponents();
            
            foreach(JsonElement compElem in json.GetProperty("components").EnumerateArray())
            {
                components.Add(Component.DeserializeInstance(compElem));
            }
        }
    }
}