using Microsoft.Xna.Framework;

namespace PhobosEngine.Input
{
    public class TwoAxisControl : Control
    {
        private ControlSignal xPositive;
        private ControlSignal xNegative;
        private ControlSignal yPositive;
        private ControlSignal yNegative;

        private Vector2 previousState;
        public Vector2 State {get; private set;} = Vector2.Zero;

        public TwoAxisControl(ControlSignal xPositive, ControlSignal xNegative, ControlSignal yPositive, ControlSignal yNegative)
        {
            this.xPositive = xPositive;
            this.xNegative = xNegative;
            this.yPositive = yPositive;
            this.yNegative = yNegative;
        }

        public override void Update()
        {
            previousState = State;
            float x = 0;
            if(xPositive != null)
            {
                x += xPositive.GetSignal();
            }
            if(xNegative != null)
            {
                x -= xNegative.GetSignal();
            }

            float y = 0;
            if(yPositive != null)
            {
                y += yPositive.GetSignal();
            }
            if(xNegative != null)
            {
                y -= yNegative.GetSignal();
            }

            State = new Vector2(x, y);
            if(State != previousState)
            {
                OnModified?.Invoke(State);
            }
        }

        public delegate void TwoAxisEventHandler(Vector2 state);
        
        public event TwoAxisEventHandler OnModified;
    }
}