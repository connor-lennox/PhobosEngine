using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhobosEngine.Serialization;

namespace PhobosEngine.Graphics
{
    public class SpriteRenderer : Component
    {
        public Texture2D sprite;
        public Vector2 SpriteOrigin { get {
            return Transform.Position - new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
        }}

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, SpriteOrigin, Color.White);
        }

        public override void Serialize(ISerializationWriter writer)
        {
            base.Serialize(writer);
            // TODO: serialize reference to sprite
        }
    }
}