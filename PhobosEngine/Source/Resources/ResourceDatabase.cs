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

        private static Dictionary<Texture2D, string> texturePaths = new Dictionary<Texture2D, string>();
        private static Dictionary<SoundEffect, string> soundEffectPaths = new Dictionary<SoundEffect, string>();

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
                Texture2D result = Texture2D.FromFile(graphicsDevice, Path.Combine(ResourcesRoot, refPath));
                textureCache[refPath] = result;
                texturePaths[result] = refPath;
            }

            return textureCache[refPath];
        }

        public static SoundEffect LoadSoundEffect(ResourceReference reference)
        {
            string refPath = reference.resourcePath;
            if(!soundEffectCache.ContainsKey(refPath))
            {
                SoundEffect result = SoundEffect.FromFile(Path.Combine(ResourcesRoot, refPath));
                soundEffectCache[refPath] = result;
                soundEffectPaths[result] = refPath;
            }

            return soundEffectCache[refPath];
        }

        public static bool TryGetTexturePath(Texture2D texture, out string path)
        {
            return texturePaths.TryGetValue(texture, out path);
        }

        public static bool TryGetSoundEffectPath(SoundEffect soundEffect, out string path)
        {
            return soundEffectPaths.TryGetValue(soundEffect, out path);
        }

        public static void ClearCaches()
        {
            textureCache.Clear();
            soundEffectCache.Clear();
        }
    }
}