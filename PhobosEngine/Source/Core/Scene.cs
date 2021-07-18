using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PhobosEngine.Graphics;

namespace PhobosEngine
{
    public class Scene
    {
        private List<GameEntity> entities = new List<GameEntity>();
        public Camera MainCamera {get; set;}

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
            batch.Begin(transformMatrix: cameraMatrix);
            
            foreach(GameEntity entity in entities)
            {
                if(entity.Active && entity.HasComponent<SpriteRenderer>())
                {
                    foreach(SpriteRenderer renderer in entity.GetComponents<SpriteRenderer>())
                    {
                        // Draw sprite - this will have to do for now
                        // Lighting will obviously make this more complicated
                        renderer.Draw(batch);
                    }
                }
            }

            batch.End();
        }
    }
}