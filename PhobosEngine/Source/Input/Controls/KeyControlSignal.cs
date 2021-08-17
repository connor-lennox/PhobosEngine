using Microsoft.Xna.Framework.Input;

namespace PhobosEngine.Input
{
    public class KeyControlSignal : ControlSignal
    {
        private Keys boundKey;

        public KeyControlSignal(Keys key)
        {
            boundKey = key;
        }

        public override float GetSignal()
        {
            // Key signal is "binary": just 1.0 for pressed and 0.0 for unpressed
            return InputManager.GetKey(boundKey) ? 1.0f : 0.0f;
        }
    }
}