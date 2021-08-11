using System.Collections.Generic;

namespace PhobosEngine.Serialization
{
    public class SerializedInfo
    {
        // The "store" here is a dict of string (key) to {string, string[], SerializedInfo, SerializedInfo[]} (value)
        private Dictionary<string, object> store = new Dictionary<string, object>();

        // Writing primitive values:
        public void Write(string key, string value)
        {
            store[key] = value;
        }

        public void Write(string key, int value) => Write(key, value.ToString());
        public void Write(string key, uint value) => Write(key, value.ToString());
        public void Write(string key, float value) => Write(key, value.ToString());
        public void Write(string key, double value) => Write(key, value.ToString());
        public void Write(string key, bool value) => Write(key, value.ToString());

        // Reading primitive values:
        public string ReadString(string key)
        {
            return store[key] as string;
        }

        public int ReadInt(string key) => int.Parse(ReadString(key));
        public uint ReadUInt(string key) => uint.Parse(ReadString(key));
        public float ReadFloat(string key) => float.Parse(ReadString(key));
        public double ReadDouble(string key) => double.Parse(ReadString(key));
        public bool ReadBool(string key) => bool.Parse(ReadString(key));

        // Read/Write recursively
        public void Write(string key, SerializedInfo childInfo)
        {
            store[key] = childInfo;
        }

        public SerializedInfo ReadSerializedInfo(string key)
        {
            return store[key] as SerializedInfo;
        }

        public void Write(string key, SerializedInfo[] childInfos)
        {
            store[key] = childInfos;
        }

        public SerializedInfo[] ReadSerializedInfoArray(string key)
        {
            return store[key] as SerializedInfo[];
        }
    }
}
