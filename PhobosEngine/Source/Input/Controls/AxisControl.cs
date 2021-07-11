namespace PhobosEngine.Input
{
    public class AxisControl : Control
    {
        private ControlSignal signal;

        private float previousState;
        public float State {get; private set;} = 0.0f;

        public AxisControl(ControlSignal signal)
        {
            this.signal = signal;
        }

        public override void Update()
        {
            previousState = State;
            State = signal.GetSignal();
            if(State != previousState)
            {
                OnModified?.Invoke(State);
            }
        }

        public delegate void AxisEventHandler(float state);

        public event AxisEventHandler OnModified;
    }
}