using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Graphics
{
    public sealed class TextureManager : Screen.Components.SceneSystemComponent
    {
        TextureCollection textures;
        List<Texture2D> texture2Ds;
        bool initialized = false;

        public TextureManager(Game game) : base(game)
        {
            texture2Ds = new List<Texture2D>();
            textures = new TextureCollection();
        }

        public void Add(params TextureData[] data)
        {
            foreach (var d in data)
            {
                textures.Add(d);

                if (initialized)
                {
                    texture2Ds.Add(Game.Content.Load<Texture2D>(d.TexturePath));
                }
            }
        }

        public void Remove(params Texture2D[] data)
        {
            for(int i = 0; i < data.Length;i++)
            {
                var item = data[i];

                texture2Ds.Remove(texture2Ds[i]);
                texture2Ds.Remove(item);
            }    
        }

        public Texture2D this[string name]
        {
            get
            {
                Texture2D texture = null;
                textures.For( (d, i) =>
                {
                    if (d.TextureName == name)
                        texture = texture2Ds[i];
                });
                return texture;
            }
        }

        public void LoadAll()
        {
            foreach(var texture in textures)
            {
                texture2Ds.Add(Game.Content.Load<Texture2D>(texture.TexturePath));
            }

            initialized = true;
        }
    }
}
