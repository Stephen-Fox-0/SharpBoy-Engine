using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Graphics
{
    public struct SceneBounds
    {
        SceneResulotion resulotion;
        int x, y;

        /// <summary>
        /// Gets the resulotion of this bounds.
        /// </summary>
        public SceneResulotion Resulotion => resulotion;

        /// <summary>
        /// Gets X.
        /// </summary>
        public int X => x;

        /// <summary>
        /// Gets Y.
        /// </summary>
        public int Y => y;

        public SceneBounds(SceneResulotion resulotion)
        {
            this.x = resulotion.Size.Width - resulotion.AspectRatio.Width;
            this.y = resulotion.Size.Height - resulotion.AspectRatio.Height;
            this.resulotion = resulotion;
        }
    }
}
