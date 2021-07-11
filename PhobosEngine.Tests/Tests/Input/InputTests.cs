using System;
using System.IO;
using NUnit.Framework;

using Microsoft.Xna.Framework;

using PhobosEngine.Input;

namespace PhobosEngine.Tests.Input
{
    [TestFixture]
    public class InputTests
    {
        private TestingControlSignal signal;


        [SetUp]
        public void SetUp()
        {
            signal = new TestingControlSignal();

            // testAxisControl = new AxisControl(signal);
            // testTwoAxisControl = new TwoAxisControl(signal, signal);

        }

        [Test]
        public void ButtonControl_RegisteredEvent_EventThrown()
        {
            bool eventThrown = false;
            bool buttonState = false;
            ButtonControl testButtonControl = new ButtonControl(signal);
            testButtonControl.OnPressed += () => {
                eventThrown = true;
                buttonState = true;
            };
            testButtonControl.OnReleased += () => {
                eventThrown = true;
                buttonState = false;
            };

            // Make sure nothing happens with the button right after initialization
            eventThrown = false;
            signal.signalValue = 0;
            testButtonControl.Update();
            Assert.IsFalse(eventThrown);
            Assert.IsFalse(buttonState);
            
            // Button is pressed, event is thrown and state is true
            eventThrown = false;
            signal.signalValue = 1;
            testButtonControl.Update();
            Assert.IsTrue(eventThrown);
            Assert.IsTrue(buttonState);

            // Button is held, no event
            eventThrown = false;
            signal.signalValue = 1f;
            testButtonControl.Update();
            Assert.IsFalse(eventThrown);
            Assert.IsTrue(buttonState);
            
            // Button is released, event is thrown and state is false
            eventThrown = false;
            signal.signalValue = 0;
            testButtonControl.Update();
            Assert.IsTrue(eventThrown);
            Assert.IsFalse(buttonState);
        }

        [Test]
        public void AxisControl_RegisteredEvent_EventThrown()
        {
            bool eventThrown = false;
            float axisState = 0;

            AxisControl testAxisControl = new AxisControl(signal);
            testAxisControl.OnModified += (float state) => {
                eventThrown = true;
                axisState = state;
            };

            // Axis should do nothing before non-zero input
            eventThrown = false;
            signal.signalValue = 0;
            testAxisControl.Update();
            Assert.IsFalse(eventThrown);
            Assert.AreEqual(axisState, 0);

            // Move to full
            eventThrown = false;
            signal.signalValue = 1;
            testAxisControl.Update();
            Assert.IsTrue(eventThrown);
            Assert.AreEqual(axisState, 1);

            // Move to half
            eventThrown = false;
            signal.signalValue = 0.5f;
            testAxisControl.Update();
            Assert.IsTrue(eventThrown);
            Assert.AreEqual(axisState, 0.5f);

            // No movement, event is not thrown
            eventThrown = false;
            signal.signalValue = 0.5f;
            testAxisControl.Update();
            Assert.IsFalse(eventThrown);
            Assert.AreEqual(axisState, 0.5f);
        }

                [Test]
        public void TwoAxisControl_RegisteredEvent_EventThrown()
        {
            bool eventThrown = false;
            Vector2 axisState = Vector2.Zero;

            TwoAxisControl testTwoAxisControl = new TwoAxisControl(signal, signal);
            testTwoAxisControl.OnModified += (Vector2 state) => {
                eventThrown = true;
                axisState = state;
            };

            // Axis should do nothing before non-zero input
            eventThrown = false;
            signal.signalValue = 0;
            testTwoAxisControl.Update();
            Assert.IsFalse(eventThrown);
            Assert.AreEqual(axisState, Vector2.Zero);

            // Move to full
            eventThrown = false;
            signal.signalValue = 1;
            testTwoAxisControl.Update();
            Assert.IsTrue(eventThrown);
            Assert.AreEqual(axisState, Vector2.One);

            // Move to half
            eventThrown = false;
            signal.signalValue = 0.5f;
            testTwoAxisControl.Update();
            Assert.IsTrue(eventThrown);
            Assert.AreEqual(axisState, new Vector2(0.5f, 0.5f));

            // No movement, event is not thrown
            eventThrown = false;
            signal.signalValue = 0.5f;
            testTwoAxisControl.Update();
            Assert.IsFalse(eventThrown);
            Assert.AreEqual(axisState, new Vector2(0.5f, 0.5f));
        }
    }
}