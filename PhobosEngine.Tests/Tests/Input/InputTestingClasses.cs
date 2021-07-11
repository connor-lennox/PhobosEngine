using PhobosEngine.Input;

namespace PhobosEngine.Tests.Input
{
    // Testing control signal: just returns the value it's given
    public class TestingControlSignal : ControlSignal
    {
        public float signalValue;

        public override float GetSignal()
        {
            return signalValue;
        }
    }
}