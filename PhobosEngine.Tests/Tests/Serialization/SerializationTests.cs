using System;
using System.IO;
using NUnit.Framework;

using Microsoft.Xna.Framework;

using PhobosEngine.Serialization;

namespace PhobosEngine.Tests.Serialization
{
    [TestFixture]
    public class SerializationTests
    {
        private MemoryStream stream;

        [SetUp]
        public void SetUp()
        {
            stream = new MemoryStream();
        }

        private void WriteTestValues(ISerializationWriter writer)
        {
            writer.Write(TestValues.testString);
            writer.Write(TestValues.testUInt);
            writer.Write(TestValues.testInt);
            writer.Write(TestValues.testFloat);
            writer.Write(TestValues.testDouble);
            writer.Write(TestValues.testBool);
        }

        private void ReadTestValues(ISerializationReader reader)
        {
            Assert.AreEqual(reader.ReadString(), TestValues.testString);
            Assert.AreEqual(reader.ReadUInt(), TestValues.testUInt);
            Assert.AreEqual(reader.ReadInt(), TestValues.testInt);
            Assert.AreEqual(reader.ReadFloat(), TestValues.testFloat);
            Assert.AreEqual(reader.ReadDouble(), TestValues.testDouble);
            Assert.AreEqual(reader.ReadBool(), TestValues.testBool);
        }

        [Test]
        public void SimpleTypes_TextSerializer_WriteAndRead()
        {
            TextSerializationWriter writer = new TextSerializationWriter(stream);
            WriteTestValues(writer);
            writer.Flush();

            stream.Position = 0;
            TextSerializationReader reader = new TextSerializationReader(stream);
            ReadTestValues(reader);
        }

        [Test]
        public void DefaultGameEntity_TextSerializer_SerializesCorrectly()
        {
            GameEntity oldEntity = new GameEntity();
            Vector2 pos = new Vector2(4, 5);
            oldEntity.Transform.Position = pos;

            TextSerializationWriter writer = new TextSerializationWriter(stream);
            writer.Write(oldEntity);
            writer.Flush();

            stream.Position = 0;
            TextSerializationReader reader = new TextSerializationReader(stream);
            GameEntity newEntity = new GameEntity();
            newEntity.Deserialize(reader);

            Assert.AreEqual(newEntity.Transform.Position, pos);
        }
    }
}