using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Components.MessageBox
{
    public class MessageBoxSystem : SceneSystemComponent
    {
        public MessageBoxSystem(Game game) : base(game)
        {
        }

        /// <summary>
        /// Show a message on screen.
        /// 
        /// Notice this function only works if the main game is an instance of <see cref="SharpWindow"/>
        /// </summary>
        public void Show( string message, MessageBoxButton button, EventHandler accepted, EventHandler canceled )
        {
            MessageDrawableComponent component = new MessageDrawableComponent( message, button, Game );
            component.Accepted += accepted;
            component.Canceled += canceled;
            Game.Components.Add( component );
        }

        /// <summary>
        /// Show a message on screen.
        /// 
        /// Notice this function only works if the main game is an instance of <see cref="SharpWindow"/>
        /// </summary>
        public void Show( string message, EventHandler accepted, EventHandler canceled )
        {
            Show( message, MessageBoxButton.OkCancel, accepted, canceled );
        }
        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }
    }
}
