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

        public SerializedInfo Serialize()
        {
            SerializedInfo info = new SerializedInfo();
            info.Write("mUInt", mUInt);
            info.Write("mInt", mInt);
            info.Write("mFloat", mFloat);
            info.Write("mDouble", mDouble);
            info.Write("mBool", mBool);
            info.Write("mString", mString);
            return info;
        }

        public void Deserialize(SerializedInfo info)
        {
            mUInt = info.ReadUInt("mUInt");
            mInt = info.ReadInt("mInt");
            mFloat = info.ReadFloat("mFloat");
            mDouble = info.ReadDouble("mDouble");
            mBool = info.ReadBool("mBool");
            mString = info.ReadString("mString");
        }
    }
}