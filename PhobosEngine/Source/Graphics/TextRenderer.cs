using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FontStashSharp;
using PhobosEngine.Serialization;

namespace PhobosEngine
{
    public class TextRenderer : Renderer
    {
        private FontSystem fontSystem;
        public FontSystem FontSystem {
            get => fontSystem; 
            set {
                fontSystem = value;
                GenerateFont();
            }
        }
        private int fontSize = 12;
        public int FontSize {
            get => fontSize; 
            set {
                fontSize = value;
                GenerateFont();
            }
        }

        private SpriteFontBase font;

        public string Text {get; set;}
        public Color TextColor {get; set;} = Color.White;

        private void GenerateFont()
        {
            if(fontSystem != null)
            {
                font = fontSystem.GetFont(fontSize);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(font != null)
            {
                spriteBatch.DrawString(font, Text, Transform.Position, TextColor, Transform.Scale, Transform.Rotation, Transform.Position, 0);
            }
        }

        public override void Serialize(Utf8JsonWriter writer)
        {
            base.Serialize(writer);
            
            ResourceReference fontRef = ResourceReference.FromFontSystem(fontSystem);
            if(fontRef.isValid)
            {
                writer.WriteSerializable("fontRef", fontRef);
            }

            writer.WriteNumber("fontSize", FontSize);
            writer.WriteString("text", Text);
            writer.WriteColor("textColor", TextColor);
        }

        public override void Deserialize(JsonElement json)
        {
            base.Deserialize(json);
            if(json.TryGetProperty("spriteRef", out JsonElement fontElem))
            {
                fontSystem = ResourceDatabase.LoadFontSystem(fontElem.GetSerializable<ResourceReference>());
            }

            if(json.TryGetProperty("fontSize", out JsonElement fontSizeElem))
            {
                fontSize = fontSizeElem.GetInt32();
            }

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

            GenerateFont();
        }
    }
}