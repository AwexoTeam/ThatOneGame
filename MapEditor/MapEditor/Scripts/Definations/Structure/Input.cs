using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Scripts.Definations.Structure
{
    public static class Input
    {
        private static MouseState previousMouseState = new MouseState();

        public static bool isPressed(Keys key)
        {
            KeyboardState state = Keyboard.GetState();
            return state.IsKeyUp(key);
        }

        public static bool isPressed(int number)
        {
            var ms = Mouse.GetState();
            ButtonState currState = number == 0 ? ms.LeftButton : ms.RightButton;
            ButtonState prevState = number == 0 ? previousMouseState.LeftButton : previousMouseState.RightButton;

            previousMouseState = ms;

            return currState == ButtonState.Released && prevState == ButtonState.Pressed;
        }
    }
}
