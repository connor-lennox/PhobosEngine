using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhobosEngine.Serialization;

namespace PhobosEngine
{
    public class SpriteRenderer : Renderer
    {
        public Texture2D sprite;
        public Rectangle sourceRect;
        public Color tintColor = Color.White;

        private Vector2 EffectiveSpriteSize { get {
            return new Vector2(sprite.Width * Transform.Scale.X, sprite.Height * Transform.Scale.Y);
        }}

        private Vector2 EffectiveSpriteHalfBounds {get {
            return new Vector2(sprite.Width * Transform.Scale.X / 2, sprite.Height * Transform.Scale.Y / 2);
        }}

        // TODO: sprite sorting layers

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite, Transform.Position, sourceRect, tintColor, Transform.Rotation, EffectiveSpriteHalfBounds, Transform.Scale, SpriteEffects.None, 0);
        }

        public override void Serialize(Utf8JsonWriter writer)
        {
            base.Serialize(writer);
            
            ResourceReference spriteRef = ResourceReference.FromTexture2D(sprite);
            if(spriteRef.isValid)
            {
                writer.WriteSerializable("spriteRef", spriteRef);
            }

            if(sourceRect != null)
            {
                writer.WriteRectangle("sourceRect", sourceRect);
            }
            writer.WriteColor("tintColor", tintColor);
        }

        public override void Deserialize(JsonElement json)
        {
            base.Deserialize(json);
            
            if(json.TryGetProperty("spriteRef", out JsonElement spriteElem))
            {
                sprite = ResourceDatabase.LoadTexture(spriteElem.GetSerializable<ResourceReference>());
            }

            if(json.TryGetProperty("sourceRect", out JsonElement rectElem))
            {
                sourceRect = rectElem.GetRectangle();
            }
            tintColor = json.GetProperty("tintColor").GetColor();
        }
    }
}