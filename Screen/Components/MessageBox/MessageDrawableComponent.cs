using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpBoyEngine.Screen.Extentions;
using SharpBoyEngine.Screen.Input;
using SharpBoyEngine.Screen.System;
using SharpBoyEngine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Components.MessageBox
{
    public class MessageDrawableComponent : DrawableGameComponent
    {
        internal static bool isOpen = false;
        public event EventHandler Accepted;
        public event EventHandler Canceled;

        internal SpriteFont font;
        internal Texture2D _blank;

        Rectangle buttonBounds;
        Rectangle button2Bounds;
        public Rectangle bounds;

        Input.InputSystem input;

        ResourceContentManager _content;
        internal SpriteBatch spriteBatch;

        internal string message;
        MessageBoxButton button;

        bool button1Selected = false;
        bool button2Selected = false;

        public int HeightOffset = 0;
        public int WidthOffset = 0;

        internal int boundsHeight = 0;
        internal int boundsWidth = 0;

        internal Input.InputSystem Input
        {
            get => input;
        }

        internal bool drawButtons = true;
        public MessageDrawableComponent( string message, MessageBoxButton button, Game game ) : base( game )
        {
            _content = new ResourceContentManager( game.Services, Properties.Resources.ResourceManager );

            this.message = message.SubString(60, "\n");
            this.button = button;

            input = new Input.InputSystem();
            input.AddController<MouseController>(new MouseController());
            isOpen = true;
        }

       
        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = new SpriteBatch( GraphicsDevice );
            _blank = new Texture2D( GraphicsDevice, 1, 1 );
            _blank.SetData<Color>( new Color[] { Color.White } );

            font = _content.Load<SpriteFont>( "arial" );


            var padding = 5;
            var fontSize = font.MeasureString( message );

            var width = GraphicsDevice.Viewport.Width;
            var height = GraphicsDevice.Viewport.Height;

            boundsHeight = (int)(fontSize.Y + 30 + padding) + HeightOffset;
            boundsWidth = (int)(fontSize.X + padding) + WidthOffset;

            bounds = new Rectangle( width / 2 - boundsWidth / 2, height / 2 - boundsHeight / 2, boundsWidth + padding * 2, boundsHeight + padding * 2 );

            Vector2 button1Size = Vector2.Zero;
            Vector2 button2Size = Vector2.Zero;

            switch (button)
            {
                case MessageBoxButton.OkCancel:
                    button1Size = font.MeasureString( "OK" );
                    button2Size = font.MeasureString( "Cancel" );
                    break;
                case MessageBoxButton.YesNo:
                    button1Size = font.MeasureString( "Yes" );
                    button2Size = font.MeasureString( "No" );
                    break;
            }

        }

        bool _isExiting = false;
        public override void Update( GameTime gameTime )
        {
            base.Update( gameTime );

            input.Update(gameTime);


            buttonBounds = new Rectangle( bounds.X + boundsWidth - 50, bounds.Y + boundsHeight - 15, 50, 20 );
            button2Bounds = new Rectangle( bounds.X +boundsWidth - 105, bounds.Y + boundsHeight - 15, 50, 20 );


            // button input

            var mouseBounds = new Rectangle
                (
                (int)input.GetController<MouseController>().Position.X,
                (int)input.GetController<MouseController>().Position.Y,
                5, 5 );

         if(drawButtons)
            {
                if (mouseBounds.Intersects( buttonBounds ))
                {
                    button1Selected = true;

                    if (input.GetController<MouseController>().IsLeftClick())
                        OnAccepted();
                }
                else
                    button1Selected = false;

                if (mouseBounds.Intersects( button2Bounds ))
                {
                    button2Selected = true;

                    if (input.GetController<MouseController>().IsLeftClick())
                        OnCanceled();
                }
                else
                    button2Selected = false;
            }
        }


        public virtual void OnAccepted()
        {
            SceneSystem.Instance.inputFrozen = false;
            Game.Components.Remove( this );


            isOpen = false;
            Accepted?.Invoke( this, EventArgs.Empty );
        }

        public virtual void OnCanceled()
        {
            SceneSystem.Instance.inputFrozen = false;
            Game.Components.Remove( this );
            isOpen = false;
            Canceled?.Invoke( this, EventArgs.Empty );
        }
        public override void Draw( GameTime gameTime )
        {

            spriteBatch.Begin();

            var port = SceneSystem.Instance.GraphicsDevice.Viewport;

            spriteBatch.Draw( _blank, port.Bounds, Color.Black * 0.4f );

            spriteBatch.DrawTextureShadow( _blank, bounds, Color.Black);

            spriteBatch.DrawRectangle( _blank, bounds, Color.White);
            spriteBatch.DrawTextShadow( font, message, new Vector2( bounds.X + 5, bounds.Y + 5 ), Color.White );
            if (drawButtons)
            {
                spriteBatch.DrawTextureShadow( _blank, buttonBounds, button1Selected ? Color.White * 0.6f : Color.White * 0.4f );
                spriteBatch.DrawTextureShadow( _blank, button2Bounds, button2Selected ? Color.White * 0.6f : Color.White * 0.4f );


                string text1 = "OK";
                string text2 = "Cancel";

                switch (button)
                {
                    case MessageBoxButton.OkCancel: text1 = "OK"; text2 = "Cancel"; break;
                    case MessageBoxButton.YesNo: text1 = "Yes"; text2 = "No"; break;
                }

                var font1 = font.MeasureString( text1 );
                var font2 = font.MeasureString( text2 );

                var b1Bounds = new Vector2(
                    (int)(buttonBounds.X + buttonBounds.Width / 2 - font1.X / 2),
                    (int)(buttonBounds.Y + buttonBounds.Height / 2 - font1.Y / 2) );

                var b2Bounds = new Vector2(
                   (int)(button2Bounds.X + button2Bounds.Width / 2 - font2.X / 2),
                   (int)(button2Bounds.Y + button2Bounds.Height / 2 - font2.Y / 2) );

                spriteBatch.DrawTextShadow( font, text1, b1Bounds, Color.White );
                spriteBatch.DrawTextShadow( font, text2, b2Bounds, Color.White );
            }


            spriteBatch.End();
            base.Draw( gameTime );
        }
    }
}
