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
        public void SimpleTypes_Serialize_WritesToJSON()
        {
            TestValueHolder holder = ConstructTestHolder();

            MemoryStream stream = new MemoryStream();
            Utf8JsonWriter writer = new Utf8JsonWriter(stream);
            writer.WriteSerializableValue(holder);
            writer.Flush();

            stream.Position = 0;
            string jsonString = System.Text.Encoding.UTF8.GetString(stream.ToArray());

            JsonElement root = JsonDocument.Parse(jsonString).RootElement;

            TestValueHolder holderCopy = new TestValueHolder();
            holderCopy.Deserialize(root);

            CheckTestHolder(holderCopy);
        }

        [Test]
        public void DefautGameEntity_Serialize_WritesToJSON()
        {
            GameEntity oldEntity = new GameEntity();
            Vector2 pos = new Vector2(4, 5);
            oldEntity.Transform.Position = pos;

            MemoryStream stream = new MemoryStream();
            Utf8JsonWriter writer = new Utf8JsonWriter(stream);
            
            writer.WriteSerializableValue(oldEntity);
            writer.Flush();

            stream.Position = 0;
            string jsonString = System.Text.Encoding.UTF8.GetString(stream.ToArray());

            JsonElement root = JsonDocument.Parse(jsonString).RootElement;

            GameEntity newEntity = new GameEntity();
            newEntity.Deserialize(root);

            Assert.AreEqual(newEntity.Transform.Position, pos);
        }

        internal class DummyComponent : Component
        {
            public int dummyInt;

            public override void Serialize(Utf8JsonWriter writer)
            {
                base.Serialize(writer);
                writer.WriteNumber("dummy", dummyInt);
            }

            public override void Deserialize(JsonElement json)
            {
                base.Deserialize(json);
                dummyInt = json.GetProperty("dummy").GetInt32();
            }
        }

        [Test]
        public void EntityWithComponent_Serialize_MaintainsComponentValue()
        {
            GameEntity oldEntity = new GameEntity();
            DummyComponent comp = new DummyComponent();
            comp.dummyInt = 777;
            oldEntity.AddComponent<DummyComponent>(comp);

            MemoryStream stream = new MemoryStream();
            Utf8JsonWriter writer = new Utf8JsonWriter(stream);
            
            writer.WriteSerializableValue(oldEntity);
            writer.Flush();

            stream.Position = 0;
            string jsonString = System.Text.Encoding.UTF8.GetString(stream.ToArray());

            JsonElement root = JsonDocument.Parse(jsonString).RootElement;

            GameEntity newEntity = new GameEntity();
            newEntity.Deserialize(root);

            DummyComponent recovered;

            Assert.IsTrue(newEntity.TryGetComponent<DummyComponent>(out recovered));
            Assert.AreEqual(777, recovered.dummyInt);
        }
    }
}