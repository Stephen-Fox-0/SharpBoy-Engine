using Microsoft.Xna.Framework;
using SharpBoyEngine.Screen.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Input
{
    public sealed class InputSystem :IService
    {
        List<InputSystemController> controllers;

        public string Name => "Input System Service";

        public InputSystem()
        {
            this.controllers = new List<InputSystemController>();
        }


        public T GetController<T>() where T: InputSystemController
        {
            foreach (var control in controllers)
                if (control is T)
                    return (T)control;

            return null;
        }
        public void AddController<T>(T controller) where T: InputSystemController
        {
            controllers.Add(controller);
        }

        public bool ContainsType<T>() where T:InputSystemController
        {
            bool flag = false;
            foreach (var item in controllers)
                if (item is T)
                    flag = true;
            return flag;
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < controllers.Count; i++)
            {
                controllers[i].Update();
            }
        }
    }
}
