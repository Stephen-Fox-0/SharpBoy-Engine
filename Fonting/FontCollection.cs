using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Fonting
{
    public class FontCollection : List<FontInfo>
    {
        public void Add(string fontName, string path)
        {
            FontInfo info = new FontInfo(fontName, path);
            this.Add(info);
        }
        public void Remove(string fontName)
        {
            FontInfo font = new FontInfo();
            foreach (var data in this)
                if (data.Name == fontName)
                    font = data;

            this.Remove(font);
        }

        public FontInfo this[string name]
        {
            get
            {
                FontInfo info = new FontInfo();
                foreach (var font in this)
                    if (font.Name == name)
                        info = font;

                return info;
            }
            set
            {
                for (int i = 0; i < this.Count; i++)
                    if (this[i].Name == name)
                        this[i] = value;
            }
        }
    }
}
