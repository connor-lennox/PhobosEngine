using System;
using Microsoft.Xna.Framework;

using PhobosEngine.Serialization;

namespace PhobosEngine
{
    public class Transform : ISerializable
    {
        public Vector2 position;
        // Rotation is in degrees
        public float rotation;
        public Vector2 scale;

        public Transform() : this(Vector2.Zero) {}
        public Transform(Vector2 position) : this(position, 0) {}
        public Transform(Vector2 position, float rotation) : this(position, rotation, Vector2.One) {}

        public Transform(Vector2 position, float rotation, Vector2 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public Vector2 Right { get {
            float rads = PBMath.Deg2Rad(rotation);
            return new Vector2(MathF.Cos(rads), MathF.Sin(rads));
        }}

        public Vector2 Up { get {
            float rads = PBMath.Deg2Rad(rotation + 90);
            return new Vector2(MathF.Cos(rads), MathF.Sin(rads));
        }}

        public void PointTowards(Vector2 target)
        {
            rotation = PBMath.Rad2Deg(MathF.Atan2(target.Y - position.Y, target.X - position.X));
        }

        public void Serialize(ISerializationWriter writer)
        {
            writer.Write(position);
            writer.Write(rotation);
            writer.Write(scale);
        }

        public void Deserialize(ISerializationReader reader)
        {
            position = reader.ReadVector2();
            rotation = reader.ReadFloat();
            scale = reader.ReadVector2();
        }
    }
}