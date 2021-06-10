using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Services.Scene
{
    public interface ISceneService
    {
        void Initialize();

        void Update(GameTime gameTime);

        string Name { get; }
    }
}
