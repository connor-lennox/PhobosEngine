namespace PhobosEngine.Input
{
    public class ButtonControl : Control
    {
        private ControlSignal signal;

        private bool previousState = false;
        public bool State {get; private set;} = false;
        private const float cutoffValue = 0.5f;

        public ButtonControl(ControlSignal signal)
        {
            this.signal = signal;
        }

        public override void Update()
        {
            previousState = State;
            State = signal.GetSignal() > cutoffValue;

            if(State && !previousState)
            {
                OnPressed?.Invoke();
            } else if(!State && previousState)
            {
                OnReleased?.Invoke();
            }
        }

        public delegate void ButtonEventHandler();

        public event ButtonEventHandler OnPressed;
        public event ButtonEventHandler OnReleased;
    }
}