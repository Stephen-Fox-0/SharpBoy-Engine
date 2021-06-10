using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Input
{
    public class MouseController : InputSystemController
    {
        MouseState currentState;
        MouseState lastState;

        float currentScrollValue;
        float lastScrollValue;

        Vector2 mousePosition;
        Vector2 mousePositionLast;

        public Vector2 Position => mousePosition;
        public Vector2 PositionLast => mousePositionLast;

        public Rectangle CurrentBounds => new Rectangle((int)Position.X, (int)Position.Y, 10, 10);

        public override void Update()
        {
            lastState = currentState;
            lastScrollValue = currentScrollValue;

            currentState = Mouse.GetState();
            currentScrollValue = currentState.ScrollWheelValue;

            mousePosition = new Vector2(currentState.Position.X, currentState.Position.Y);
            mousePositionLast = new Vector2(lastState.Position.X, lastState.Position.Y);
        }

        public bool IsLeftClick(bool triggered)
        {
            if (triggered)
                return currentState.LeftButton == ButtonState.Pressed && lastState.LeftButton == ButtonState.Released;
            else
                return currentState.LeftButton == ButtonState.Pressed;
        }
        public bool IsLeftClick()
        {
            return IsLeftClick(true);
        }

        public bool IsRightClick(bool triggered)
        {
            if (triggered)
                return currentState.RightButton == ButtonState.Pressed && lastState.RightButton == ButtonState.Released;
            else
                return currentState.RightButton == ButtonState.Pressed;
        }
        public bool IsRightClick()
        {
            return IsRightClick(true);
        }

        public bool IsMiddleClick(bool triggered)
        {
            if (triggered)
                return currentState.MiddleButton == ButtonState.Pressed && lastState.MiddleButton == ButtonState.Released;
            else
                return currentState.MiddleButton == ButtonState.Pressed;
        }
        public bool IsMiddleClick()
        {
            return IsMiddleClick(true);
        }

        public bool ScrollUp()
        {
            return currentScrollValue <= lastScrollValue;
        }
        public bool ScrollDown()
        {
            return currentScrollValue >= lastScrollValue;
        }
    }
}
