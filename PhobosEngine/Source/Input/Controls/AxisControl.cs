namespace PhobosEngine.Input
{
    public class AxisControl : Control
    {
        private ControlSignal positiveSignal;
        private ControlSignal negativeSignal;

        private float previousState;
        public float State {get; private set;} = 0.0f;

        public AxisControl(ControlSignal positiveSignal, ControlSignal negativeSignal)
        {
            this.positiveSignal = positiveSignal;
            this.negativeSignal = negativeSignal;
        }

        public override void Update()
        {
            previousState = State;
            State = 0;
            if(positiveSignal != null)
            {
                State += positiveSignal.GetSignal();
            }
            if(negativeSignal != null)
            {
                State -= negativeSignal.GetSignal();
            }
            if(State != previousState)
            {
                OnModified?.Invoke(State);
            }
        }

        public delegate void AxisEventHandler(float state);

        public event AxisEventHandler OnModified;
    }
}