using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Input
{
    public class GamePadController : InputSystemController
    {
        ControllerPort port;
        GamePadState currentState;
        GamePadState lastState;

        public ControllerPort Port => port;
        public bool Connected => currentState.IsConnected;
        public int PacketNumber => currentState.PacketNumber;
        public GamePadThumbSticks ThumbSticks => currentState.ThumbSticks;

        public GamePadController(ControllerPort port)
        {
            this.port = port;
        }

        public override void Update()
        {
            lastState = currentState;
            currentState = GamePad.GetState((int)port);
        }

        public bool IsButtonDown(Buttons button, bool triggered)
        {
            if (triggered)
                return currentState.IsButtonDown(button) && lastState.IsButtonUp(button);
            else
                return currentState.IsButtonDown(button);
        }
        public bool IsButtonDown(Buttons button)
        {
            return IsButtonDown(button, true);
        }
        public bool IsButtonDown(string buttonName)
        {
            return IsButtonDown((Buttons)Enum.Parse(typeof(Buttons), buttonName));
        }
    }
}
