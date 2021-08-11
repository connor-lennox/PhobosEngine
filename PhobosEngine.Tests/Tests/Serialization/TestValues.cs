using System.Text.Json;

using PhobosEngine.Serialization;

namespace PhobosEngine.Tests.Serialization
{
    public static class TestValues
    {
        public static uint testUInt = 5102342;
        public static int testInt = 7;
        public static float testFloat = 412315.2351f;
        public static double testDouble = 571892.123;
        public static bool testBool = true;
        public static string testString = "test string";
    }

    public class TestValueHolder : ISerializable
    {
        public uint mUInt;
        public int mInt;
        public float mFloat;
        public double mDouble;
        public bool mBool;
        public string mString;

        public void Serialize(Utf8JsonWriter writer)
        {
            writer.WriteNumber("mUInt", mUInt);
            writer.WriteNumber("mInt", mInt);
            writer.WriteNumber("mFloat", mFloat);
            writer.WriteNumber("mDouble", mDouble);
            writer.WriteBoolean("mBool", mBool);
            writer.WriteString("mString", mString);
        }

        public void Deserialize(JsonElement json)
        {
            mUInt = json.GetProperty("mUInt").GetUInt32();
            mInt = json.GetProperty("mInt").GetInt32();
            mFloat = json.GetProperty("mFloat").GetSingle();
            mDouble = json.GetProperty("mDouble").GetDouble();
            mBool = json.GetProperty("mBool").GetBoolean();
            mString = json.GetProperty("mString").GetString();
        }
    }
}