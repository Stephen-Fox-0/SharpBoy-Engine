using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Graphics
{
    public struct SceneSize
    {
        int width;
        int height;

        public int Width => width;
        public int Height => height;

        public SceneSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}
