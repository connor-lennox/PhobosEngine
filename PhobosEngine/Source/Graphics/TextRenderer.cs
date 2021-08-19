using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhobosEngine
{
    public class TextRenderer : Renderer
    {
        public SpriteFont Font {get; set;}
        public string Text {get; set;}
        public Color TextColor {get; set;}

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Transform.Position, TextColor, Transform.Rotation, 
                                        Transform.Position, Transform.Scale, SpriteEffects.None, 0);
        }

        public override void Serialize(Utf8JsonWriter writer)
        {
            base.Serialize(writer);
        }
    }
}