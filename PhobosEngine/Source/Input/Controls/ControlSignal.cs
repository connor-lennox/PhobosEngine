namespace PhobosEngine.Input
{
    // General ControlSignal class. All child signals (buttons, joysticks, etc.) provide a float
    public abstract class ControlSignal
    {
        public abstract float GetSignal();
    }
}