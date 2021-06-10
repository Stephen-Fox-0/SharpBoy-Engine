using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Graphics
{
    public class TextureCollection : List<TextureData>
    {
        public TextureData this[string name]
        {
            get
            {
                TextureData data = new TextureData();
                ForEach(_ =>
                {
                    if (_.TextureName == name)
                        data = _;
                });
                return data;
            }
            set
            {
                for (int i = 0; i < Count; i++)
                    if (this[i].TextureName == name)
                        this[i] = value;
            }
        }

        public void Add(string path, string name)
        {
            Add(new TextureData() { TexturePath = path, TextureName = name });
        }

        public void Remove(string name)
        {
            Remove(this[name]);
        }

        public void For(Action<TextureData, int> action)
        {
            for (int i = 0; i < Count; i++)
                action?.Invoke(this[i], i);
        }
    }
}
