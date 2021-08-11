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

        public override SerializedInfo Serialize()
        {
            SerializedInfo info = base.Serialize();
            info.Write("zoom", Zoom);
            info.Write("boundsX", Bounds.X);
            info.Write("boundsY", Bounds.Y);
            info.Write("boundsWidth", Bounds.Width);
            info.Write("boundsHeight", Bounds.Height);
            return info;
        }

        public override void Deserialize(SerializedInfo info)
        {
            base.Deserialize(info);
            Zoom = info.ReadFloat("zoom");
            Bounds = new Rectangle(info.ReadInt("boundsX"), info.ReadInt("boundsY"), info.ReadInt("boundsWidth"), info.ReadInt("boundsHeight"));
            UpdateRenderMatrix();
        }
    }
}