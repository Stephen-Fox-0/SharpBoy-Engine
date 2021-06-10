using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpBoyEngine.Screen.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Fonting
{
    public sealed class FontManager
    {
        List<SpriteFont> spriteFonts;
        FontCollection fonts;

        public SpriteFont this[string fontName]
        {
            get
            {
                for (int i = 0; i < fonts.Count; i++)
                    if (fonts[i].Name == fontName)
                        return spriteFonts[i];

                return null;
            }
        }
        public void Initialize(ContentManager contentManager)
        {
            foreach (var font in fonts)
                spriteFonts.Add(contentManager.Load<SpriteFont>(font.Path));
        }
        public void AddFont(params FontInfo[] font)
        {
            fonts.AddRange(font);
        }
        public void RemoveFont(FontInfo font)
        {
            fonts.Remove(font) ;
        }


        public FontManager()
        {
            fonts = new FontCollection();
            spriteFonts = new List<SpriteFont>();
        }
    }
}
