using Microsoft.Xna.Framework;

namespace PhobosEngine.Serialization
{
    public static class SerializedInfoMonogameExtensions
    {
        // Vector2        
        public static void Write(this SerializedInfo info, string key, Vector2 vector)
        {
            SerializedInfo vectorInfo = new SerializedInfo();
            vectorInfo.Write("x", vector.X);
            vectorInfo.Write("y", vector.Y);
            info.Write(key, vectorInfo);
        }

        public static Vector2 ReadVector2(this SerializedInfo info, string key)
        {
            SerializedInfo vectorInfo = info.ReadSerializedInfo(key);
            return new Vector2(vectorInfo.ReadFloat("x"), vectorInfo.ReadFloat("y"));
        }

        // Vector2[]
        public static void Write(this SerializedInfo info, string key, Vector2[] vectors)
        {
            SerializedInfo[] vectorInfos = new SerializedInfo[vectors.Length];
            for(int i = 0; i < vectors.Length; i++)
            {
                SerializedInfo tempInfo = new SerializedInfo();
                tempInfo.Write("x", vectors[i].X);
                tempInfo.Write("y", vectors[i].Y);
                vectorInfos[i] = tempInfo;
            }
            info.Write(key, vectorInfos);
        }

        public static Vector2[] ReadVector2Array(this SerializedInfo info, string key)
        {
            SerializedInfo[] vectorInfos = info.ReadSerializedInfoArray(key);
            Vector2[] vectors = new Vector2[vectorInfos.Length];
            for(int i = 0; i < vectorInfos.Length; i++)
            {
                SerializedInfo curInfo = vectorInfos[i];
                vectors[i] = new Vector2(curInfo.ReadFloat("x"), curInfo.ReadFloat("y"));
            }
            return vectors;
        }

        public static void Write(this SerializedInfo info, string key, Color color)
        {
            info.Write(key, color.PackedValue);
        }

        public static Color ReadColor(this SerializedInfo info, string key)
        {
            return new Color(info.ReadUInt(key));
        }
    }
}