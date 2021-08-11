using System;
using System.IO;
using System.Text.Json;
using NUnit.Framework;

using Microsoft.Xna.Framework;

using PhobosEngine.Serialization;

namespace PhobosEngine.Tests.Serialization
{
    [TestFixture]
    public class SerializationTests
    {
        // private MemoryStream stream;

        // [SetUp]
        // public void SetUp()
        // {
        //     stream = new MemoryStream();
        // }

        // private void WriteTestValues(ISerializationWriter writer)
        // {
        //     writer.Write(TestValues.testString);
        //     writer.Write(TestValues.testUInt);
        //     writer.Write(TestValues.testInt);
        //     writer.Write(TestValues.testFloat);
        //     writer.Write(TestValues.testDouble);
        //     writer.Write(TestValues.testBool);
        // }

        // private void ReadTestValues(ISerializationReader reader)
        // {
        //     Assert.AreEqual(reader.ReadString(), TestValues.testString);
        //     Assert.AreEqual(reader.ReadUInt(), TestValues.testUInt);
        //     Assert.AreEqual(reader.ReadInt(), TestValues.testInt);
        //     Assert.AreEqual(reader.ReadFloat(), TestValues.testFloat);
        //     Assert.AreEqual(reader.ReadDouble(), TestValues.testDouble);
        //     Assert.AreEqual(reader.ReadBool(), TestValues.testBool);
        // }

        // [Test]
        // public void SimpleTypes_TextSerializer_WriteAndRead()
        // {
        //     TextSerializationWriter writer = new TextSerializationWriter(stream);
        //     WriteTestValues(writer);
        //     writer.Flush();

        //     stream.Position = 0;
        //     TextSerializationReader reader = new TextSerializationReader(stream);
        //     ReadTestValues(reader);
        // }

        // [Test]
        // public void DefaultGameEntity_TextSerializer_SerializesCorrectly()
        // {
        //     GameEntity oldEntity = new GameEntity();
        //     Vector2 pos = new Vector2(4, 5);
        //     oldEntity.Transform.Position = pos;

        //     TextSerializationWriter writer = new TextSerializationWriter(stream);
        //     writer.Write(oldEntity);
        //     writer.Flush();

        //     stream.Position = 0;
        //     TextSerializationReader reader = new TextSerializationReader(stream);
        //     GameEntity newEntity = new GameEntity();
        //     newEntity.Deserialize(reader);

        //     Assert.AreEqual(newEntity.Transform.Position, pos);
        // }

        private TestValueHolder ConstructTestHolder()
        {
            TestValueHolder holder = new TestValueHolder();
            holder.mUInt = TestValues.testUInt;
            holder.mInt = TestValues.testInt;
            holder.mFloat = TestValues.testFloat;
            holder.mDouble = TestValues.testDouble;
            holder.mBool = TestValues.testBool;
            holder.mString = TestValues.testString;
            return holder;
        }

        private void CheckTestHolder(TestValueHolder holder)
        {
            Assert.AreEqual(holder.mString, TestValues.testString);
            Assert.AreEqual(holder.mUInt, TestValues.testUInt);
            Assert.AreEqual(holder.mInt, TestValues.testInt);
            Assert.AreEqual(holder.mFloat, TestValues.testFloat);
            Assert.AreEqual(holder.mDouble, TestValues.testDouble);
            Assert.AreEqual(holder.mBool, TestValues.testBool);
        }

        [Test]
        public void SimpleTypes_SerializedInfo_ContainsCorrectValues()
        {
            TestValueHolder holder = ConstructTestHolder();

            SerializedInfo info = holder.Serialize();

            TestValueHolder holderCopy = new TestValueHolder();
            holderCopy.Deserialize(info);

            CheckTestHolder(holderCopy);
        }

        [Test]
        public void DefaultGameEntity_SerializedInfo_ContainsCorrectValues()
        {
            GameEntity oldEntity = new GameEntity();
            Vector2 pos = new Vector2(4, 5);
            oldEntity.Transform.Position = pos;

            SerializedInfo info = oldEntity.Serialize();

            GameEntity newEntity = new GameEntity();
            newEntity.Deserialize(info);

            Assert.AreEqual(newEntity.Transform.Position, pos);
        }

        [Test]
        public void SimpleTypes_SerializedInfo_WritesToJSON()
        {
            TestValueHolder holder = ConstructTestHolder();
            SerializedInfo holderInfo = holder.Serialize();

            MemoryStream stream = new MemoryStream();
            Utf8JsonWriter writer = new Utf8JsonWriter(stream);
            JsonSerializerOptions options = new JsonSerializerOptions {
                WriteIndented = true,
                Converters = {new SerializedInfoJsonConverter()}
            };
            JsonSerializer.Serialize<SerializedInfo>(writer, holderInfo, options);
            writer.Flush();

            stream.Position = 0;
            Utf8JsonReader reader = new Utf8JsonReader(stream.ToArray());
            SerializedInfo readInfo = JsonSerializer.Deserialize<SerializedInfo>(ref reader, options);

            TestValueHolder holderCopy = new TestValueHolder();
            holderCopy.Deserialize(readInfo);

            CheckTestHolder(holderCopy);
        }

        [Test]
        public void DefautGameEntity_SerializedInfo_WritesToJSON()
        {
            GameEntity oldEntity = new GameEntity();
            Vector2 pos = new Vector2(4, 5);
            oldEntity.Transform.Position = pos;

            SerializedInfo info = oldEntity.Serialize();

            MemoryStream stream = new MemoryStream();
            Utf8JsonWriter writer = new Utf8JsonWriter(stream);
            JsonSerializerOptions options = new JsonSerializerOptions {
                WriteIndented = true,
                Converters = {new SerializedInfoJsonConverter()}
            };
            JsonSerializer.Serialize<SerializedInfo>(writer, info, options);
            writer.Flush();

            stream.Position = 0;
            Console.Write(System.Text.Encoding.UTF8.GetString(stream.ToArray()));

            stream.Position = 0;
            Utf8JsonReader reader = new Utf8JsonReader(stream.ToArray());
            SerializedInfo readInfo = JsonSerializer.Deserialize<SerializedInfo>(ref reader, options);


            GameEntity newEntity = new GameEntity();
            newEntity.Deserialize(readInfo);

            Assert.AreEqual(newEntity.Transform.Position, pos);
        }
    }
}