using System;
using Microsoft.Xna.Framework;

using PhobosEngine.Serialization;

namespace PhobosEngine
{
    public class Transform : ISerializable
    {
        public Vector2 Position {get; set;}
        // Rotation is in degrees
        public float Rotation {get; set;}
        public Vector2 Scale {get; set;}

        public Transform() : this(Vector2.Zero) {}
        public Transform(Vector2 position) : this(position, 0) {}
        public Transform(Vector2 position, float rotation) : this(position, rotation, Vector2.One) {}

        public Transform(Vector2 position, float rotation, Vector2 scale)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
        }

        public Vector2 Right { get {
            float rads = PBMath.Deg2Rad(Rotation);
            return new Vector2(MathF.Cos(rads), MathF.Sin(rads));
        }}

        public Vector2 Up { get {
            float rads = PBMath.Deg2Rad(Rotation + 90);
            return new Vector2(MathF.Cos(rads), MathF.Sin(rads));
        }}

        public void PointTowards(Vector2 target)
        {
            Rotation = PBMath.Rad2Deg(MathF.Atan2(target.Y - Position.Y, target.X - Position.X));
        }

        public void Serialize(ISerializationWriter writer)
        {
            writer.Write(Position);
            writer.Write(Rotation);
            writer.Write(Scale);
        }

        public void Deserialize(ISerializationReader reader)
        {
            Position = reader.ReadVector2();
            Rotation = reader.ReadFloat();
            Scale = reader.ReadVector2();
        }
    }
}