using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Graphics
{
    public struct SceneAspectSize
    {
        int w;
        int h;

        public int Width => w;
        public int Height => h;

        public SceneAspectSize(int width, int height)
        {
            w = width;
            h = height;
        }

        public SceneAspectSize(SceneSize screenSize)
        {
            int frameWidth;
            int frameHeight;

            if(screenSize.Width < screenSize.Height)
            {
                frameWidth = screenSize.Height;
                frameHeight = screenSize.Height;
            }
            else
            {
                frameWidth = screenSize.Width;
                frameHeight = screenSize.Width;
            }

            w = frameWidth;
            h = frameHeight;
        }
    }
}
