using System.Collections.Generic;

namespace PhobosEngine.Serialization
{
    public class SerializedInfo
    {
        // The "store" here is a dict of string (key) to {string, string[], SerializedInfo, SerializedInfo[]} (value)
        public Dictionary<string, object> Store {get; private set;} = new Dictionary<string, object>();

        // Writing primitive values:
        public void Write(string key, string value)
        {
            Store[key] = value;
        }

        public void Write(string key, int value) => Write(key, value.ToString());
        public void Write(string key, uint value) => Write(key, value.ToString());
        public void Write(string key, float value) => Write(key, value.ToString());
        public void Write(string key, double value) => Write(key, value.ToString());
        public void Write(string key, bool value) => Write(key, value.ToString());

        // Reading primitive values:
        public string ReadString(string key)
        {
            return Store[key] as string;
        }

        public int ReadInt(string key) => int.Parse(ReadString(key));
        public uint ReadUInt(string key) => uint.Parse(ReadString(key));
        public float ReadFloat(string key) => float.Parse(ReadString(key));
        public double ReadDouble(string key) => double.Parse(ReadString(key));
        public bool ReadBool(string key) => bool.Parse(ReadString(key));

        // Read/Write arrays
        public void Write(string key, string[] arr)
        {
            Store[key] = arr;
        }

        public string[] ReadStringArray(string key)
        {
            return Store[key] as string[] ?? new string[0];
        }

        // Read/Write recursively
        public void Write(string key, SerializedInfo childInfo)
        {
            Store[key] = childInfo;
        }

        public SerializedInfo ReadSerializedInfo(string key)
        {
            return Store[key] as SerializedInfo;
        }

        public void Write(string key, SerializedInfo[] childInfos)
        {
            Store[key] = childInfos;
        }

        public SerializedInfo[] ReadSerializedInfoArray(string key)
        {
            return Store[key] as SerializedInfo[] ?? new SerializedInfo[0];
        }

        public void WriteEmptyArray(string key)
        {
            Store[key] = new object[0];
        }
    }
}
