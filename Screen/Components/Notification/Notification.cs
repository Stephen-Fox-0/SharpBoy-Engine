using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpBoyEngine.Screen.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Components.Notification
{
    public class Notification : DrawableGameComponent
    {
        Anchor anchor;
        Texture2D nTexture;
        Icon icon = Icon.Normal;
        string text;

        int timer = 0;
        int time = 3000;

        float blankX = 0f;
        float width = 0f;
        float height = 0f;
        float blankY = 0f;

        SpriteFont font;

        SpriteBatch spriteBatch;
        Texture2D _blank;

        public bool AudioEnabled { get; set; }
        public float AudioVolume { get; set; }

        /// <summary>
        /// Gets the anchor or position placement of our notification.
        /// </summary>
        public Anchor Anchor => anchor;

        /// <summary>
        /// Gets or sets the transition on time.
        /// </summary>
        public TimeSpan TransitionOnTime
        {
            get => _transitionOnTime;
            set => _transitionOnTime = value;
        }
        private TimeSpan _transitionOnTime = TimeSpan.Zero;

        /// <summary>
        /// Gets or sets the transition of time.
        /// </summary>
        public TimeSpan TransitionOffTime
        {
            get => _transitionOffTime;
            set => _transitionOffTime = value;
        }
        private TimeSpan _transitionOffTime = TimeSpan.Zero;

        // Ranges from zero (fully active, no transition)
        // to one (transitioned fully off to nothing)
        /// <summary>
        /// Gets or sets the transition position
        /// </summary>
        /// <remarks>Ranges from zero (fully active, no transition) to one (transitioned fully off to nothing)</remarks>
        public float TransitionPosition
        {
            get => _transitionPosition;
            set => _transitionPosition = value;
        }
        private float _transitionPosition = 1;

        // Ranges from 1 (fully active, no transition)
        // to 0 (transitioned fully off to nothing)
        /// <summary>
        /// Gets the transitional alpha.
        /// </summary>
        public float TransitionAlpha => 1f - TransitionPosition;

        ResourceContentManager _content;
        Transition state = Transition.TransitionOn;

        bool _isExiting = false;

        /// <summary>
        /// Gets or sets the timescale of to how many seconds to show our notification.
        /// </summary>
        public float TimeScale
        {
            get;
            set;
        }

        /// <summary>
        /// Initialize a new instance of <see cref="Notification"/>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="game"></param>
        /// <param name="icon"></param>
        /// <param name="anchor"></param>
        /// <param name="timeScale"></param>
        /// <param name="timerMiliSeconds"></param>
        public Notification( string text, Game game, Icon icon, Anchor anchor, float timeScale, int timerMiliSeconds = 3000 ) : base( game )
        {
            _content = new Microsoft.Xna.Framework.Content.ResourceContentManager( game.Services, Properties.Resources.ResourceManager );
            this.text = text;
            TransitionOffTime = TimeSpan.FromSeconds( timeScale );
            TransitionOnTime = TimeSpan.FromSeconds( timeScale );
            this.icon = icon;
            this.time = timerMiliSeconds;
            this.anchor = anchor;
        }

        public override void Update( GameTime gameTime )
        {
            base.Update( gameTime );

            try
            {
                timer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timer > time)
                {
                    _isExiting = true;
                    timer = 0;
                }
            }
            catch { }

            if (_isExiting)
            {
                // If the screen is going away to die, it should transition off.
                state = Transition.TransitionOff;

                if (!UpdateTransitionPosition( gameTime, _transitionOffTime, 1 ))
                        ExitScreen();
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                state = UpdateTransitionPosition( gameTime, _transitionOnTime, -1 )
                    ? Transition.TransitionOn
                    : Transition.Active;
            }

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = new SpriteBatch( Game.GraphicsDevice );
            switch (icon)
            {
                case Icon.Normal:
                    nTexture = _content.Load<Texture2D>( "noty" );
                    break;
                case Icon.Warning:
                    nTexture = _content.Load<Texture2D>( "noty_warning" );
                    break;
                case Icon.NetworkWifi:
                    nTexture = _content.Load<Texture2D>( "noty_wifi" );
                    break;
                case Icon.Cloud:
                    nTexture = _content.Load<Texture2D>( "noty_cloud" );
                    break;
                case Icon.Setting:
                    nTexture = _content.Load<Texture2D>( "noty_setting" );
                    break;
                case Icon.Stopped:
                    nTexture = _content.Load<Texture2D>( "noty_pause" );
                    break;
                case Icon.Playing:
                    nTexture = _content.Load<Texture2D>( "noty_play" );
                    break;
                case Icon.Error:
                    nTexture = _content.Load<Texture2D>( "noty_error" );
                    break;
            }

            font = _content.Load<SpriteFont>( "arial" );
            _blank = new Texture2D( GraphicsDevice, 1, 1 );
            _blank.SetData<Color>( new Color[] { Color.White } );

           if(AudioEnabled)
            {
                SoundEffect effect = _content.Load<SoundEffect>("notySFX");
                effect.Play(AudioVolume, 0.0f, 0.0f);
            }

        }

        public bool ShowBorder = false;


      
        public override void Draw( GameTime gameTime )
        {
            base.Draw( gameTime );
            try
            {
                float transitionOffset = (float)Math.Pow( TransitionPosition, 1 );

                var spriteFont = font;

                switch (anchor)
                {
                    case Anchor.TopLeft:
                        blankX = -(GraphicsDevice.Viewport.Width / 2 - spriteFont.MeasureString( text ).X / 2 + 5) * transitionOffset;
                        width = spriteFont.MeasureString( text ).X + nTexture.Width + 15;
                        height = (spriteFont.MeasureString( text ).Y + nTexture.Height) - 5;
                        blankY = 20;
                        break;
                    case Anchor.TopCenter:
                        width = spriteFont.MeasureString( text ).X + nTexture.Width + 15;
                        blankX = (GraphicsDevice.Viewport.Width / 2 - width / 2 + 5);
                        height = (spriteFont.MeasureString( text ).Y + nTexture.Height) - 5;
                        blankY = 20;
                        break;
                    case Anchor.TopRight:
                        width = spriteFont.MeasureString( text ).X + nTexture.Width + 15;
                        blankX = (GraphicsDevice.Viewport.Width - spriteFont.MeasureString( text ).X / 2 + 5) - width - 5;
                        height = (spriteFont.MeasureString( text ).Y + nTexture.Height) - 5;
                        blankY = 20;
                        break;
                    case Anchor.Center:
                        width = spriteFont.MeasureString( text ).X + nTexture.Width + 15;
                        blankX = (GraphicsDevice.Viewport.Width / 2 - width / 2 + 5);
                        height = (spriteFont.MeasureString( text ).Y + nTexture.Height) - 5;
                        blankY = (GraphicsDevice.Viewport.Height / 2 - (int)spriteFont.MeasureString( text ).Y / 2);
                        break;
                    case Anchor.CenterLeft:
                        blankX = -(GraphicsDevice.Viewport.Width / 2 - spriteFont.MeasureString( text ).X / 2 + 5) * (int)transitionOffset;
                        width = spriteFont.MeasureString( text ).X + nTexture.Width + 15;
                        height = (spriteFont.MeasureString( text ).Y + nTexture.Height) - 5;
                        blankY = (GraphicsDevice.Viewport.Height / 2 - (int)spriteFont.MeasureString( text ).Y / 2);
                        break;
                    case Anchor.CenterRight:

                        width = spriteFont.MeasureString( text ).X + nTexture.Width + 15;
                        blankX = (GraphicsDevice.Viewport.Width - spriteFont.MeasureString( text ).X / 2 + 5) - width - 5;
                        height = (spriteFont.MeasureString( text ).Y + nTexture.Height) - 5;
                        blankY = (GraphicsDevice.Viewport.Height / 2 - (int)spriteFont.MeasureString( text ).Y / 2);
                        break;
                    case Anchor.BottomLeft:
                        blankX = -(GraphicsDevice.Viewport.Width / 2 - spriteFont.MeasureString( text ).X / 2 + 5) * transitionOffset;
                        width = spriteFont.MeasureString( text ).X + nTexture.Width + 15;
                        height = (spriteFont.MeasureString( text ).Y + nTexture.Height) - 5;
                        blankY = (GraphicsDevice.Viewport.Height - (int)spriteFont.MeasureString( text ).Y / 2) - (int)height - 10;
                        break;
                    case Anchor.BottomRight:
                        width = spriteFont.MeasureString( text ).X + nTexture.Width + 15;
                        height = (spriteFont.MeasureString( text ).Y + nTexture.Height) - 5;
                        blankX = (GraphicsDevice.Viewport.Width - spriteFont.MeasureString( text ).X / 2 + 5) - width - 5;
                        blankY = ((GraphicsDevice.Viewport.Height - (int)spriteFont.MeasureString( text ).Y / 2) - (int)height - 10);
                        break;
                    case Anchor.BottomCenter:
                        width = spriteFont.MeasureString( text ).X + nTexture.Width + 15;
                        blankX = (GraphicsDevice.Viewport.Width / 2 - width / 2 + 5);
                        height = (spriteFont.MeasureString( text ).Y + nTexture.Height) - 5;
                        blankY = -(GraphicsDevice.Viewport.Height - (int)spriteFont.MeasureString( text ).Y / 2 - (int)height - 10);
                        break;
                }

                spriteBatch.Begin( SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null );
                spriteBatch.DrawTextureShadow( _blank, new Rectangle( 10 + (int)blankX, (int)blankY, (int)width, (int)height + 2 ), Color.White * TransitionAlpha, 0.7f );
                spriteBatch.Draw( nTexture, new Vector2
                    ( blankX + 15,
                     (blankY + height / 2 - spriteFont.MeasureString( text ).Y / 2) - 3 ),
                     Color.White * TransitionAlpha );
                if (ShowBorder)
                {

                    // left
                    spriteBatch.DrawLine( _blank,
                        new Vector2( 10 + blankX, blankY ),
                        new Vector2( 10 + blankX, blankY + height + 2 ),
                        new Color( 51, 152, 219 ) * TransitionAlpha );


                    //right
                    spriteBatch.DrawLine( _blank,
                    new Vector2( 10 + blankX + width, blankY ),
                    new Vector2( 10 + blankX + width, blankY + height + 2 ),
                      new Color( 51, 152, 219 ) * TransitionAlpha );

                    //top
                    spriteBatch.DrawLine( _blank,
                    new Vector2( 10 + blankX, blankY ),
                    new Vector2( 10 + blankX + width, blankY ),
                       new Color( 51, 152, 219 ) * TransitionAlpha );

                    //bottom
                    spriteBatch.DrawLine( _blank,
                    new Vector2( 10 + blankX, blankY + height + 2 ),
                    new Vector2( 10 + blankX + width, blankY + height + 2 ),
                       new Color( 51, 152, 219 ) * TransitionAlpha );
                }
                spriteBatch.End();

                spriteBatch.Begin();
                spriteBatch.DrawString( spriteFont, text, new Vector2( 17 + blankX + nTexture.Width, (blankY + height / 2) - 7 ), Color.Black * TransitionAlpha );
                spriteBatch.End();
            }
            catch { }
        }

        private bool UpdateTransitionPosition( GameTime gameTime, TimeSpan time, int direction )
        {
            float transitionDelta;    // How much should we move by?

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);

            _transitionPosition += transitionDelta * direction;    // Update the transition position

            // Did we reach the end of the transition?
            if (direction < 0 && _transitionPosition <= 0 || direction > 0 && _transitionPosition >= 1)
            {
                _transitionPosition = Microsoft.Xna.Framework.MathHelper.Clamp( _transitionPosition, 0, 1 );
                return false;
            }

            return true;    // Otherwise we are still busy transitioning
        }

        public void ExitScreen()
        {
            Game.Components.Remove( this );
        }
    }
}
