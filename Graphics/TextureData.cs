using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TextureData
    {
        public string TexturePath;
        public string TextureName;
    }
}
