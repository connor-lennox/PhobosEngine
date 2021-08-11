using System.Text.Json;
using Microsoft.Xna.Framework;
using PhobosEngine.Serialization;

namespace PhobosEngine
{
    public class Camera : Component
    {
        private float zoom = 1f;
        public float Zoom {
            get => zoom;
            set {
                zoom = value;
                UpdateRenderMatrix();
            }
        }
        
        private Rectangle bounds = new Rectangle(0, 0, 640, 480);
        public Rectangle Bounds {
            get => bounds; 
            set {
                bounds = value;
                UpdateRenderMatrix();
            }
        }

        public Matrix RenderMatrix {get; private set;}

        public override void Init()
        {
            UpdateRenderMatrix();
        }

        public override void OnParentTransformModified()
        {
            UpdateRenderMatrix();
        }

        private void UpdateRenderMatrix()
        {
            RenderMatrix = Matrix.CreateTranslation(new Vector3(-Transform.Position, 0)) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
        }

        public override void Serialize(Utf8JsonWriter writer)
        {
            base.Serialize(writer);
            writer.WriteNumber("zoom", Zoom);
            writer.WriteNumber("boundsX", Bounds.X);
            writer.WriteNumber("boundsY", Bounds.Y);
            writer.WriteNumber("boundsWidth", Bounds.Width);
            writer.WriteNumber("boundsHeight", Bounds.Height);
        }

        public override void Deserialize(JsonElement json)
        {
            base.Deserialize(json);
            Zoom = json.GetProperty("zoom").GetSingle();
            Bounds = new Rectangle(json.GetProperty("boundsX").GetInt32(), json.GetProperty("boundsY").GetInt32(), 
                                    json.GetProperty("boundsWidth").GetInt32(), json.GetProperty("boundsHeight").GetInt32());
            UpdateRenderMatrix();
        }
    }
}