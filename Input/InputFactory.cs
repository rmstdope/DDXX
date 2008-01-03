using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Input
{
    internal class InputFactory : IInputFactory
    {
        private KeyboardState keyboardState;
        private GamePadState gamePadState;

        public void Step()
        {
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);
            //System.Diagnostics.Debug.WriteLine("Escape if " + currentState.IsKeyDown(Keys.A));
        }

        public bool KeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public bool GamePadButtonA()
        {
            return gamePadState.Buttons.A == ButtonState.Pressed;
        }

        public bool GamePadButtonB()
        {
            return gamePadState.Buttons.B == ButtonState.Pressed;
        }

        public bool GamePadButtonX()
        {
            return gamePadState.Buttons.X == ButtonState.Pressed;
        }

        public bool GamePadButtonY()
        {
            return gamePadState.Buttons.Y == ButtonState.Pressed;
        }

        public bool GamePadUp()
        {
            return gamePadState.DPad.Up == ButtonState.Pressed;
        }

        public bool GamePadDown()
        {
            return gamePadState.DPad.Down == ButtonState.Pressed;
        }

        public bool GamePadRight()
        {
            return gamePadState.DPad.Right == ButtonState.Pressed;
        }

        public bool GamePadLeft()
        {
            return gamePadState.DPad.Left == ButtonState.Pressed;
        }

    }
}
