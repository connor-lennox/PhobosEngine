using Microsoft.Xna.Framework.Graphics;

namespace PhobosEngine
{
    public abstract class Renderer : Component
    {
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}