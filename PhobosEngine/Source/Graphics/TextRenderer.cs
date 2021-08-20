using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhobosEngine.Serialization;

namespace PhobosEngine
{
    public class TextRenderer : Renderer
    {
        public SpriteFont Font {get; set;}
        public string Text {get; set;}
        public Color TextColor {get; set;} = Color.White;

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Transform.Position, TextColor, Transform.Rotation, 
                                        Transform.Position, Transform.Scale, SpriteEffects.None, 0);
        }

        public override void Serialize(Utf8JsonWriter writer)
        {
            base.Serialize(writer);
            // TODO: Write AssetReference for Font
            writer.WriteString("text", Text);
            writer.WriteColor("textColor", TextColor);
        }

        public override void Deserialize(JsonElement json)
        {
            base.Deserialize(json);
            if(json.TryGetProperty("text", out JsonElement textProperty))
            {
                Text = textProperty.GetString();
            } else {
                Text = "";
            }

            if(json.TryGetProperty("textColor", out JsonElement colorProperty))
            {
                TextColor = colorProperty.GetColor();
            } else {
                TextColor = Color.White;
            }
        }
    }
}