using Microsoft.Xna.Framework;

namespace PhobosEngine
{
    public struct RaycastHit
    {
        public Collider collider;
        public float distance;
        public Vector2 normal;
        public Vector2 point;
    }
}