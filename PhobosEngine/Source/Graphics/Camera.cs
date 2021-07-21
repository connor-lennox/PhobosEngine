using Microsoft.Xna.Framework;
using PhobosEngine.Serialization;

namespace PhobosEngine.Graphics
{
    public class Camera : Component
    {
        public float Zoom {get; set;} = 1f;
        public Rectangle Bounds {get; set;} = new Rectangle(0, 0, 640, 480);

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

        public override void Serialize(ISerializationWriter writer)
        {
            base.Serialize(writer);
            writer.Write(Zoom);
            writer.Write(Bounds.X);
            writer.Write(Bounds.Y);
            writer.Write(Bounds.Width);
            writer.Write(Bounds.Height);
        }

        public override void Deserialize(ISerializationReader reader)
        {
            base.Deserialize(reader);
            Zoom = reader.ReadFloat();
            Bounds = new Rectangle(reader.ReadInt(), reader.ReadInt(), reader.ReadInt(), reader.ReadInt());
            UpdateRenderMatrix();
        }
    }
}