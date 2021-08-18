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

        private Vector2 centeringOffset = PhobosGame.GameResolution / 2;
        public Vector2 CenteringOffset {
            get => centeringOffset;
            set {
                centeringOffset = value;
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
                    Matrix.CreateTranslation(new Vector3(centeringOffset.X, centeringOffset.Y, 0)) *
                    Matrix.CreateScale(Zoom);
        }

        public override void Serialize(Utf8JsonWriter writer)
        {
            base.Serialize(writer);
            writer.WriteNumber("zoom", Zoom);
            writer.WriteVector2("offset", CenteringOffset);
        }

        public override void Deserialize(JsonElement json)
        {
            base.Deserialize(json);
            Zoom = json.GetProperty("zoom").GetSingle();
            CenteringOffset = json.GetProperty("offset").GetVector2();
            UpdateRenderMatrix();
        }
    }
}