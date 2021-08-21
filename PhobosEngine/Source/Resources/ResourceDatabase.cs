using System;
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

        public static string ResourcesRoot = "Resources";
        private static string ResourcesPath => Path.Combine(Environment.CurrentDirectory, ResourcesRoot);

        public static void Init(GraphicsDevice device)
        {
            graphicsDevice = device;
        }

        public static Texture2D LoadTexture(string path)
        {
            if(!textureCache.ContainsKey(path))
            {
                Texture2D result = Texture2D.FromFile(graphicsDevice, Path.Combine(ResourcesPath, path));
                textureCache[path] = result;
                texturePaths[result] = path;
            }

            return textureCache[path];
        }

        public static Texture2D LoadTexture(ResourceReference reference)
        {
            return LoadTexture(reference.resourcePath);
        }

        public static SoundEffect LoadSoundEffect(string path)
        {
            if(!soundEffectCache.ContainsKey(path))
            {
                SoundEffect result = SoundEffect.FromFile(Path.Combine(ResourcesPath, path));
                soundEffectCache[path] = result;
                soundEffectPaths[result] = path;
            }

            return soundEffectCache[path];
        }

        public static SoundEffect LoadSoundEffect(ResourceReference reference)
        {
            return LoadSoundEffect(reference.resourcePath);
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