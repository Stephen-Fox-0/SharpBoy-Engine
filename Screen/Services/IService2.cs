using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Services
{
    /// <summary>
    /// Contains a draw method.
    /// </summary>
    public interface IService2 : IService
    {
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
