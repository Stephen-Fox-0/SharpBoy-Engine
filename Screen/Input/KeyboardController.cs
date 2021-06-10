using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Input
{
    public class KeyboardController : InputSystemController
    {
        KeyboardState currentkeyState;
        KeyboardState lastkeyState;

        public override void Update()
        {
            lastkeyState = currentkeyState;
            currentkeyState = Keyboard.GetState();
        }


        public bool IsKeyDown(Keys key, bool triggered)
        {
            if (triggered)
                return currentkeyState.IsKeyDown(key) && lastkeyState.IsKeyUp(key);
            else
                return currentkeyState.IsKeyDown(key);
        }
        public bool IsKeyDown(Keys key)
        {
            return IsKeyDown(key, true);
        }
        public bool IsKeyDown(string keyName)
        {
            return IsKeyDown((Keys)Enum.Parse(typeof(Keys), keyName));
        }

        public bool IsKeyUp(Keys key, bool triggered)
        {
            if (triggered)
                return currentkeyState.IsKeyUp(key) && lastkeyState.IsKeyDown(key);
            else
                return currentkeyState.IsKeyUp(key);
        }
        public bool IsKeyUp(Keys key)
        {
            return IsKeyUp(key, true);
        }
        public bool IsKeyUp(string keyName)
        {
            return IsKeyUp((Keys)Enum.Parse(typeof(Keys), keyName));
        }
    }
}
