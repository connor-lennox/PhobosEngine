using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhobosEngine.Serialization;

namespace PhobosEngine
{
    public class SpriteAnimator : Component
    {
        private Texture2D spritesheet;
        public Texture2D Spritesheet {
            get => spritesheet;
            set {
                spritesheet = value;
                if(renderer != null)
                {
                    renderer.sprite = spritesheet;
                }
            }
        }
        public List<SpriteAnimationFrame> AnimationFrames {get; private set;} = new List<SpriteAnimationFrame>();

        private float frameTimer = 0f;
        private int currentFrameIndex = 0;

        public SpriteAnimationFrame CurrentFrame => AnimationFrames[currentFrameIndex];

        private SpriteRenderer renderer;

        public override void Init()
        {
            renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = Spritesheet;
        }

        public override void Update()
        {
            frameTimer += Time.DeltaTime;

            // Move to the next animation frame, subtract "consumed" time from timer
            if(frameTimer >= CurrentFrame.frameTime)
            {
                currentFrameIndex = (currentFrameIndex + 1) % AnimationFrames.Count;
                renderer.sourceRect = CurrentFrame.sourceRectangle;
                frameTimer -= CurrentFrame.frameTime;
            }
        }

        public override void Serialize(Utf8JsonWriter writer)
        {
            base.Serialize(writer);

            ResourceReference spriteRef = ResourceReference.FromTexture2D(spritesheet);
            if(spriteRef.isValid)
            {
                writer.WriteSerializable("spritesheetRef", spriteRef);
            }

            writer.WriteSerializableArray("animationFrames", AnimationFrames.ToArray());
        }

        public override void Deserialize(JsonElement json)
        {
            base.Deserialize(json);

            if(json.TryGetProperty("spritesheetRef", out JsonElement spriteElem))
            {
                spritesheet = ResourceDatabase.LoadTexture(spriteElem.GetSerializable<ResourceReference>());
            }

            AnimationFrames = new List<SpriteAnimationFrame>(json.GetProperty("animationFrames").GetSerializableArray<SpriteAnimationFrame>());
        }

    }

    public class SpriteAnimationFrame : ISerializable
    {
        public float frameTime;
        public Rectangle sourceRectangle;

        public void Serialize(Utf8JsonWriter writer)
        {
            writer.WriteNumber("frameTime", frameTime);
            writer.WriteRectangle("sourceRect", sourceRectangle);
        }

        public void Deserialize(JsonElement json)
        {
            frameTime = json.GetProperty("frameTime").GetSingle();
            sourceRectangle = json.GetProperty("sourceRect").GetRectangle();
        }
    }
}