using Microsoft.Xna.Framework;

namespace PhobosEngine.Input
{
    public class TwoAxisControl : Control
    {
        private ControlSignal xSignal;
        private ControlSignal ySignal;

        private Vector2 previousState;
        public Vector2 State {get; private set;} = Vector2.Zero;

        public TwoAxisControl(ControlSignal xSignal, ControlSignal ySignal)
        {
            this.xSignal = xSignal;
            this.ySignal = ySignal;
        }

        public override void Update()
        {
            previousState = State;
            State = new Vector2(xSignal.GetSignal(), ySignal.GetSignal());
            if(State != previousState)
            {
                OnModified?.Invoke(State);
            }
        }

        public delegate void TwoAxisEventHandler(Vector2 state);
        
        public event TwoAxisEventHandler OnModified;
    }
}