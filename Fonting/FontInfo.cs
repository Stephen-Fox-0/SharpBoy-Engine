using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Fonting
{
    public struct FontInfo
    {
        string name;
        string path;

        public string Name => name;
        public string Path => path;

        public FontInfo(string name, string path)
        {
            this.name = name;
            this.path = path;
        }
    }
}
