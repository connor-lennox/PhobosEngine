using Microsoft.Xna.Framework.Audio;


namespace PhobosEngine.Audio
{
    public class AudioSource : Component
    {
        // TODO: implement 3d sound

        public SoundEffectInstance sound;

        public void Play()
        {
            sound.Play();
        }

        // TODO: Serialization
    }
}