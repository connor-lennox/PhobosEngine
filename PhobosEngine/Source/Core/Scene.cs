using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PhobosEngine.Serialization;

namespace PhobosEngine
{
    public class Scene : ISerializable
    {
        private List<GameEntity> entities = new List<GameEntity>();
        public Camera MainCamera {get; set;}

        public Color BackgroundColor {get; set;} = Color.CornflowerBlue;

        public void AddEntity(GameEntity entity)
        {
            entities.Add(entity);
        }

        public void RemoveEntity(GameEntity entity)
        {
            entities.Remove(entity);
        }

        public void Update()
        {
            foreach(GameEntity entity in entities)
            {
                if(entity.Active) entity.Update();
            }
        }

        public void Draw(SpriteBatch batch)
        {
            Matrix cameraMatrix = MainCamera.RenderMatrix;
            batch.Begin(transformMatrix: cameraMatrix, samplerState: SamplerState.PointClamp);
            
            foreach(GameEntity entity in entities)
            {
                if(entity.Active && entity.HasComponent<Renderer>())
                {
                    foreach(Renderer renderer in entity.GetComponents<Renderer>())
                    {
                        // Draw sprite - this will have to do for now
                        // Lighting will obviously make this more complicated
                        renderer.Draw(batch);
                    }
                }
            }

            batch.End();
        }

        public void Serialize(Utf8JsonWriter writer)
        {
            writer.WriteSerializableArray("entities", entities.ToArray());
            if(MainCamera != null && entities.Contains(MainCamera.Entity)) {
                writer.WriteNumber("camIndex", entities.IndexOf(MainCamera.Entity));
            }
        }

        public void Deserialize(JsonElement json)
        {
            entities = new List<GameEntity>();
            foreach(JsonElement elem in json.GetProperty("entities").EnumerateArray()) {
                GameEntity newEnt = new GameEntity();
                newEnt.Deserialize(elem);
                entities.Add(newEnt);
            }

            if(json.TryGetInt32(out int camIndex)) {
                MainCamera = entities[camIndex].GetComponent<Camera>();
            }
        }
    }
}