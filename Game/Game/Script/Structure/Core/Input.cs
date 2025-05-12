using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ThatOneGame.Structure
{
    public static class Input
    {
        private static KeyboardState currKeyboardState;
        private static MouseState currMouseState;

        private static KeyboardState lastKeyboardState;
        private static MouseState lastMouseState;

        public static int MouseDelta { get => currMouseState.ScrollWheelValue; }
        public static Vector2 MousePosition { get => MousePositionRaw / Globals.scale; }
        public static Vector2 MousePositionRaw { get => new Vector2(currMouseState.X, currMouseState.Y); }

        public static void StartUpdate()
        {
            currKeyboardState = Keyboard.GetState();
            currMouseState = Mouse.GetState();
        }
        public static void EndUpdate()
        {
            lastKeyboardState = currKeyboardState;
            lastMouseState = currMouseState;
        }

        public static bool IsKeyDown(Keys key)
            => currKeyboardState.IsKeyDown(key);

        public static bool IsKeyUp(Keys key)
        {
            bool currState = currKeyboardState.IsKeyDown(key);
            bool lastState = lastKeyboardState.IsKeyDown(key);

            return currState && !lastState;
        }

        public static bool IsMouseDown(int button)
        {
            if (button == 0 && currMouseState.LeftButton == ButtonState.Pressed)
                return true;

            if (button == 1 && currMouseState.RightButton == ButtonState.Pressed)
                return true;

            if (button == 2 && currMouseState.MiddleButton == ButtonState.Pressed)
                return true;

            return false;
        }

        public static bool IsMouseUp(int button)
        {
            var currState = button == 0 ? currMouseState.LeftButton : currMouseState.RightButton;
            currState = button == 2 ? currMouseState.MiddleButton : currState;

            var lastState = button == 0 ? lastMouseState.LeftButton : lastMouseState.RightButton;
            lastState = button == 2 ? lastMouseState.MiddleButton : lastState;

            bool curr = currState == ButtonState.Pressed;
            bool last = lastState == ButtonState.Pressed;

            return curr && !last;
        }

        public static Keys[] GetPressedKeys() => currKeyboardState.GetPressedKeys();
    }
}
