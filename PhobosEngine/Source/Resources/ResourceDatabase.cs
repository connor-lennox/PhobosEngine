using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace PhobosEngine
{
    public static class ResourceDatabase
    {
        private static GraphicsDevice graphicsDevice;
        private static Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();
        private static Dictionary<string, SoundEffect> soundEffectCache = new Dictionary<string, SoundEffect>();

        public static string ResourcesRoot = "/Resources";

        public static void Init(GraphicsDevice graphicsDevice)
        {
            ResourceDatabase.graphicsDevice = graphicsDevice;
        }

        public static Texture2D LoadTexture(ResourceReference reference)
        {
            string refPath = reference.resourcePath;
            if(!textureCache.ContainsKey(refPath))
            {
                textureCache[refPath] = Texture2D.FromFile(graphicsDevice, Path.Combine(ResourcesRoot, refPath));
            }

            return textureCache[refPath];
        }

        public static SoundEffect LoadSoundEffect(ResourceReference reference)
        {
            string refPath = reference.resourcePath;
            if(!soundEffectCache.ContainsKey(refPath))
            {
                soundEffectCache[refPath] = SoundEffect.FromFile(Path.Combine(ResourcesRoot, refPath));
            }

            return soundEffectCache[refPath];
        }

        public static void ClearCaches()
        {
            textureCache.Clear();
            soundEffectCache.Clear();
        }
    }
}