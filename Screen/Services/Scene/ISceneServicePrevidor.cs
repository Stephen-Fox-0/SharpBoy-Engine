using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Services.Scene
{
    public interface ISceneServicePrevidor
    {
        void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen);
        void Activate(bool instancePreserved);
        void Draw(GameTime gameTime);
    }
}
