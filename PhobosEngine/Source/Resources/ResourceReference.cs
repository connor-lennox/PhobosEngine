using System;
using System.Reflection;
using System.Text.Json;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using FontStashSharp;

using PhobosEngine.Serialization;

namespace PhobosEngine
{
    public struct ResourceReference : ISerializable
    {
        public static ResourceReference invalidReference = new ResourceReference {isValid=false, resourcePath=""};

        public bool isValid;
        public string resourcePath;

        public static ResourceReference FromTexture2D(Texture2D texture)
        {
            string path;
            if(ResourceDatabase.TryGetTexturePath(texture, out path))
            {
                return new ResourceReference(path);
            } else {
                return invalidReference;
            }
        }

        public static ResourceReference FromSoundEffect(SoundEffect soundEffect)
        {
            string path;
            if(ResourceDatabase.TryGetSoundEffectPath(soundEffect, out path))
            {
                return new ResourceReference(path);
            } else {
                return invalidReference;
            }
        }

        public static ResourceReference FromFontSystem(FontSystem fontSystem)
        {
            string path;
            if(ResourceDatabase.TryGetFontSystemPath(fontSystem, out path))
            {
                return new ResourceReference(path);
            } else {
                return invalidReference;
            }
        }

        public ResourceReference(string resourcePath)
        {
            this.resourcePath = resourcePath;
            this.isValid = true;
        }

        public void Serialize(Utf8JsonWriter writer)
        {
            writer.WriteString("resourcePath", resourcePath);
        }

        public void Deserialize(JsonElement json)
        {
            resourcePath = json.GetProperty("resourcePath").GetString();
        }

    }
}