using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Components
{
    public abstract class SceneSystemComponent
    {
        Game game;

        /// <summary>
        /// Gets the game.
        /// </summary>
        public Game Game => game;

        public SceneSystemComponent(Game game) => this.game = game;

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }
        public virtual void LoadContent(ContentManager content) { }
    }
}
