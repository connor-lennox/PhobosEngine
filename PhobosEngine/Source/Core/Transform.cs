using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Xna.Framework;

using PhobosEngine.Serialization;
using PhobosEngine.Math;

namespace PhobosEngine
{
    public class Transform : ISerializable
    {
        private Vector2 position;
        public Vector2 Position {
            get {
                UpdateTransform();
                return position;
            }
            set => SetPosition(value);
        }

        // Rotation is in radians
        private float rotation;
        public float Rotation {
            get {
                UpdateTransform();
                return rotation;
            }
            set => SetRotation(value);
        }

        public float RotationDegrees {
            get => PBMath.Rad2Deg(Rotation);
            set => Rotation = PBMath.Deg2Rad(value);
        }

        private Vector2 scale;
        public Vector2 Scale {
            get {
                UpdateTransform();
                return scale;
            }
            set => SetScale(value);
        }

        private Vector2 localPosition;
        public Vector2 LocalPosition {
            get {
                UpdateTransform();
                return localPosition;
            }
            set {
                localPosition = value;
                positionDirty = transformDirty = true;
                Entity.TransformModified();
            }
        }

        private float localRotation;
        public float LocalRotation {
            get {
                UpdateTransform();
                return localRotation;
            }
            set {
                localRotation = value;
                rotationDirty = transformDirty = true;
                Entity.TransformModified();
            }
        }

        private Vector2 localScale;
        public Vector2 LocalScale {
            get {
                UpdateTransform();
                return localScale;
            }
            set {
                localScale = value;
                scaleDirty = transformDirty = true;
                Entity.TransformModified();
            }
        }

        // Dirty flags determine when to recalculate transformation matrices
        private bool transformDirty;
        private bool positionDirty;
        private bool rotationDirty;
        private bool scaleDirty;
        private bool inverseWorldDirty;

        // World and Local transformation matrices
        private Matrix2D worldTransform;
        public Matrix2D WorldTransform {
            get {
                UpdateTransform();
                return worldTransform;
            }
        }
        private Matrix2D localTransform;
        public Matrix2D LocalTransform {
            get {
                UpdateTransform();
                return localTransform;
            }
        }
        private Matrix2D inverseWorldTransform;
        public Matrix2D InverseWorldTransform {
            get {
                UpdateTransform();
                return inverseWorldTransform;
            }
        }

        // Matrices representing the local position, rotation, and scale transformations
        private Matrix2D positionMatrix;
        private Matrix2D rotationMatrix;
        private Matrix2D scaleMatrix;

        // Parent Transform
        private Transform parent;
        public Transform Parent {
            get => parent;
            set {
                if(parent != null)
                {
                    parent.children.Remove(this);
                }
                parent = value;
                parent.children.Add(this);
                transformDirty = true;
            }
        }

        // List of children Transforms. These need to be notified when we become dirty
        // TODO: Figure out how to serialize these references
        private List<Transform> children = new List<Transform>();
        public List<Transform> Children {
            get; private set;
        }

        // GameEntity that owns this Transform
        public GameEntity Entity {get; private set;}

        public Transform(GameEntity entity) : this(entity, Vector2.Zero) {}
        public Transform(GameEntity entity, Vector2 position) : this(entity, position, 0) {}
        public Transform(GameEntity entity, Vector2 position, float rotation) : this(entity, position, rotation, Vector2.One) {}

        public Transform(GameEntity entity, Vector2 position, float rotation, Vector2 scale)
        {
            this.Entity = entity;
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;

            positionDirty = rotationDirty = scaleDirty = true;
            SetDirty();
        }

        public Vector2 Right { get {
            float rads = Rotation;
            return new Vector2(MathF.Cos(rads), MathF.Sin(rads));
        }}

        public Vector2 Up { get {
            float rads = Rotation + (MathF.PI / 2);
            return new Vector2(MathF.Cos(rads), MathF.Sin(rads));
        }}

        public Transform SetPosition(Vector2 position)
        {
            if(Parent != null)
            {
                Parent.UpdateInverseWorld();
                PBMath.Transform(ref position, ref Parent.inverseWorldTransform, out localPosition);
            } else {
                localPosition = position;
            }
            positionDirty = true;
            SetDirty();
            // Entity.TransformModified();
            return this;
        }

        public Transform SetRotation(float rotation)
        {
            if(Parent != null)
            {
                localRotation = rotation - Parent.rotation;
            } else {
                localRotation = rotation;
            }
            rotationDirty = true;
            SetDirty();
            // Entity.TransformModified();
            return this;
        }

        public Transform SetScale(Vector2 scale)
        {
            if(Parent != null)
            {
                localScale = scale / Parent.scale;
            } else {
                localScale = scale;
            }
            scaleDirty = true;
            SetDirty();
            // Entity.TransformModified();
            return this;
        }

        private void SetDirty()
        {
            transformDirty = true;
            foreach(Transform child in children)
            {
                child.SetDirty();
            }
            Entity.TransformModified();
        }

        private void UpdateTransform()
        {
            if (transformDirty)
            {
                if(Parent != null)
                {
                    Parent.UpdateTransform();
                }

                if(positionDirty)
                {
                    Matrix2D.CreateTranslation(localPosition, out positionMatrix);
                    positionDirty = false;
                }

                if(rotationDirty)
                {
                    Matrix2D.CreateRotation(localRotation, out rotationMatrix);
                    rotationDirty = false;
                }

                if(scaleDirty)
                {
                    Matrix2D.CreateScale(localScale, out scaleMatrix);
                    scaleDirty = false;
                }

                Matrix2D.Multiply(ref positionMatrix, ref rotationMatrix, out localTransform);
                Matrix2D.Multiply(ref localTransform, ref scaleMatrix, out localTransform);

                if(Parent == null)
                {
                    worldTransform = localTransform;
                    position = localPosition;
                    rotation = localRotation;
                    scale = localScale;
                } else {
                    Matrix2D.Multiply(ref Parent.worldTransform, ref localTransform, out worldTransform);
                    PBMath.Transform(ref localPosition, ref Parent.worldTransform, out position);
                    rotation = localRotation + Parent.rotation;
                    scale = localScale * Parent.scale;
                }

                inverseWorldDirty = true;
            }
        }

        private void UpdateInverseWorld()
        {
            UpdateTransform();
            if(inverseWorldDirty)
            {
                Matrix2D.Invert(ref worldTransform, out inverseWorldTransform);
            }
        }

        public void PointTowards(Vector2 target)
        {
            Rotation = MathF.Atan2(target.Y - Position.Y, target.X - Position.X);
        }

        public void Serialize(Utf8JsonWriter writer)
        {
            writer.WriteVector2("localPosition", LocalPosition);
            writer.WriteNumber("localRotation", LocalRotation);
            writer.WriteVector2("localScale", LocalScale);
        }

        public void Deserialize(JsonElement json)
        {
            LocalPosition = json.GetProperty("localPosition").GetVector2();
            LocalRotation = json.GetProperty("localRotation").GetSingle();
            LocalScale = json.GetProperty("localScale").GetVector2();

            transformDirty = positionDirty = rotationDirty = scaleDirty = true;
        }

        // GameEntity passthrough
        public T GetComponent<T>() where T : Component
        {
            return Entity.GetComponent<T>();
        }

        public T[] GetComponents<T>() where T : Component
        {
            return Entity.GetComponents<T>();
        }

        public bool TryGetComponent<T>(out T component) where T : Component
        {
            return Entity.TryGetComponent<T>(out component);
        }

        public bool HasComponent<T>() where T : Component
        {
            return Entity.HasComponent<T>();
        }
    
        public T AddComponent<T>() where T : Component, new()
        {
            return Entity.AddComponent<T>();
        }

        public T AddComponent<T>(T component) where T : Component
        {
            return Entity.AddComponent<T>(component);
        }
    }
}