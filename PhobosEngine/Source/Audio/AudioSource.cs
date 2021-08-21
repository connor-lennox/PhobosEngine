using System.Text.Json;
using Microsoft.Xna.Framework.Audio;

using PhobosEngine.Serialization;

namespace PhobosEngine.Audio
{
    public class AudioSource : Component
    {
        // TODO: implement 3d sound

        private SoundEffect sound;
        public SoundEffect Sound {
            get => sound;
            set {
                sound = value;
                if(soundInstance != null)
                {
                    soundInstance.Dispose();
                }
                soundInstance = sound.CreateInstance();
            }
        }
        private SoundEffectInstance soundInstance;

        public void Play()
        {
            soundInstance.Play();
        }

        public override void Serialize(Utf8JsonWriter writer)
        {
            base.Serialize(writer);

            ResourceReference soundRef = ResourceReference.FromSoundEffect(sound);
            if(soundRef.isValid)
            {
                writer.WriteSerializable("soundRef", soundRef);
            }
        }

        public override void Deserialize(JsonElement json)
        {
            base.Deserialize(json);

            if(json.TryGetProperty("soundRef", out JsonElement soundElem))
            {
                sound = ResourceDatabase.LoadSoundEffect(soundElem.GetSerializable<ResourceReference>());
            }
        }
    }
}