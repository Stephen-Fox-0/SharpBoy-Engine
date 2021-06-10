using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Graphics
{
    public class Sprite
    {
        protected Texture2D _texture;

        /// <summary>
        /// Gets or sets the opacity of the sprite.
        /// </summary>
        public float Opacity { get; set; }

        /// <summary>
        /// Gets or sets the origin of the sprite.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the sprite.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the scale of the sprite.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Gets or sets the position of the sprite.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Gets or sets the volocity of the sprite.
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// Gets the rectangle of the sprite.
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle( (int)(Position.X - Origin.X), (int)(Position.Y - Origin.Y), (int)(_texture.Width * Scale), (int)(_texture.Height * Scale) );
            }
        }


        Screen.System.SceneSystem sceneManager;

        /// <summary>
        /// Gets or sets a value indercating this sprite was removed.
        /// </summary>
        public bool IsRemoved { get; set; }

        public Sprite( Texture2D texture, Screen.System.SceneSystem manager )
        {
            _texture = texture;

            Opacity = 1f;

            Origin = new Vector2( _texture.Width / 2, _texture.Height / 2 );
            this.sceneManager = manager;
        }

        public void Update( GameTime gameTime )
        {
            Position += Velocity;

            if (Rectangle.Top > sceneManager.GraphicsDeviceManager.Bounds.Resulotion.Size.Height)
                IsRemoved = true;
        }

        public void Draw( GameTime gameTime, SpriteBatch spriteBatch )
        {
            spriteBatch.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend );
            spriteBatch.Draw( _texture, Position, null, Color.White * Opacity, Rotation, Origin, Scale, SpriteEffects.None, 0 );
            spriteBatch.End();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}