using System;
using NUnit.Framework;

namespace PhobosEngine.Tests.Core
{
    internal class DummyComponent : Component {}

    [TestFixture]
    public class ComponentEditingTests
    {
        private GameEntity gameEntity;
        private DummyComponent component;

        [SetUp]
        public void Setup()
        {
            gameEntity = new GameEntity();
            component = new DummyComponent();
        }

        [Test]
        public void AddComponent_SetsComponentEntity()
        {
            gameEntity.AddComponent(component);
            Assert.AreEqual(gameEntity, component.Entity);
        }

        [Test]
        public void AddComponent_AlreadyOwned_ThrowsException()
        {
            gameEntity.AddComponent(component);
            Assert.Throws<InvalidOperationException>(
                () => { gameEntity.AddComponent(component); }
            );
        }

        [Test]
        public void RemoveComponent_UnsetsComponentEntity()
        {
            gameEntity.AddComponent(component);
            gameEntity.RemoveComponent(component);
            Assert.Null(component.Entity);
        }

        [Test]
        public void RemoveComponent_NotOwned_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(
                () => { gameEntity.RemoveComponent(component); }
            );
        }

        [Test]
        public void GetComponent_HasComponent_ReturnsComponent()
        {
            gameEntity.AddComponent(component);
            Assert.AreEqual(gameEntity.GetComponent<DummyComponent>(), component);
        }

        [Test]
        public void GetComponent_DoesNotHaveComponent_ReturnsNull()
        {
            Assert.Null(gameEntity.GetComponent<DummyComponent>());
        }

        [Test]
        public void GetComponent_HadComponent_ReturnsNull()
        {
            // Distinct from above: here the Component was once on the Entity in question.
            gameEntity.AddComponent(component);
            gameEntity.RemoveComponent(component);
            Assert.Null(gameEntity.GetComponent<DummyComponent>());
        }

        [Test]
        public void HasComponent_Does_ReturnsTrue()
        {
            gameEntity.AddComponent(component);
            Assert.True(gameEntity.HasComponent<DummyComponent>());
        }

        [Test]
        public void HasComponent_DoesNot_ReturnsFalse()
        {
            Assert.False(gameEntity.HasComponent<DummyComponent>());
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetComponents_ReturnsProperCount(int amount)
        {
            for(int i = 0; i < amount; i++)
            {
                gameEntity.AddComponent(new DummyComponent());
            }
            Assert.AreEqual(gameEntity.GetComponents<DummyComponent>().Length, amount);
        }

        [Test]
        public void RemoveComponent_RemoveTransform_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(
                () => { gameEntity.RemoveComponent(gameEntity.GetComponent<Transform>()); }
            );
        }
    }
}
