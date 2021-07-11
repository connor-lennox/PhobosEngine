using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace PhobosEngine.Input {
    static class Input {
        private static KeyboardState keyboardState, lastKeyboardState;
        private static MouseState mouseState, lastMouseState;

        public static Vector2 MousePosition { get=>new Vector2(mouseState.X, mouseState.Y); }

        public static void Update() {
            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
        }

        public static bool GetKey(Keys keys) {
            return keyboardState.IsKeyDown(keys);
        }

        public static bool GetKeyDown(Keys key) {
            return keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }

        public static bool GetKeyUp(Keys key) {
            return keyboardState.IsKeyUp(key) && lastKeyboardState.IsKeyDown(key);
        }
    }
}