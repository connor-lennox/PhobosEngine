using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhobosEngine.Serialization;

namespace PhobosEngine
{
    public class SpriteRenderer : Component
    {
        public Texture2D sprite;
        public Color tintColor = Color.White;

        private Vector2 EffectiveSpriteSize { get {
            return new Vector2(sprite.Width * Transform.Scale.X, sprite.Height * Transform.Scale.Y);
        }}

        private Vector2 EffectiveSpriteHalfBounds {get {
            return new Vector2(sprite.Width * Transform.Scale.X / 2, sprite.Height * Transform.Scale.Y / 2);
        }}

        // TODO: sprite sorting layers

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, Transform.Position, null, tintColor, Transform.Rotation, EffectiveSpriteHalfBounds, Transform.Scale, SpriteEffects.None, 0);
        }

        public override SerializedInfo Serialize()
        {
            SerializedInfo info = base.Serialize();
            // TODO: serialize reference to sprite
            info.Write("tintColor", tintColor);
            return info;
        }

        public override void Deserialize(SerializedInfo info)
        {
            base.Deserialize(info);
            // TODO: deserialize sprite reference
            tintColor = info.ReadColor("tintColor");
        }
    }
}