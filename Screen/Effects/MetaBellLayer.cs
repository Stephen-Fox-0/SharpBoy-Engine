using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Effects
{
    public class MetaBallLayer :Components.SceneSystemComponent
    {

        public static GraphicsDevice Device;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        Texture2D instructions, overlay;
        Texture2D[] metaballTextures;
        Color[] glowColors;
        RenderTarget2D metaballTarget;
        AlphaTestEffect effect;

        List<Metaball> balls = new List<Metaball>();
        bool showOverlay = true;
        int currentColor = 0;

        Random rand;

        public const int MetaballRadius = 158;
        public const float MetaballScale = 1f;
        const int NumMetaballs = 20;


        public MetaBallLayer(Game game) : base(game)
        {
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            foreach (var ball in balls)
                spriteBatch.Draw(ball.Texture, ball.Position, null, Color.White * 0.3f, 0f, Vector2.Zero, MetaballScale, SpriteEffects.None, 0.5f);
            spriteBatch.End();
        }



        public override void LoadContent( ContentManager content )
        {
            metaballTextures = new Texture2D[5];
            metaballTextures[0] = Metaball.GenerateTexture(Game.GraphicsDevice, MetaballRadius, Metaball.CreateTwoColorPicker( Color.Red, Color.Yellow ) );
            metaballTextures[1] = Metaball.GenerateTexture(Game.GraphicsDevice, MetaballRadius, Metaball.CreateTwoColorPicker( Color.Blue, Color.Cyan ) );
            metaballTextures[2] = Metaball.GenerateTexture(Game.GraphicsDevice, MetaballRadius, Metaball.CreateTwoColorPicker( Color.Green, Color.Yellow ) );
            metaballTextures[3] = Metaball.GenerateTexture(Game.GraphicsDevice, MetaballRadius, Metaball.CreateTwoColorPicker( Color.Magenta, Color.White ) );
            metaballTextures[4] = Metaball.GenerateTexture(Game.GraphicsDevice, MetaballRadius, Metaball.CreateTwoColorPicker( Color.Red, Color.Red ) );

            glowColors = new Color[] { Color.Red, Color.Blue, Color.Lime, Color.Magenta, Color.Red };

            // initialize the alpha test effect.
            effect = new AlphaTestEffect(Game.GraphicsDevice );
            var viewport = Game.GraphicsDevice.Viewport;
            effect.Projection = Matrix.CreateTranslation( -0.5f, -0.5f, 0 ) * Matrix.CreateOrthographicOffCenter( 0, viewport.Width, viewport.Height, 0, 0, 1 );
            effect.ReferenceAlpha = 128;
            rand = new Random();
            for (int i = 0; i < NumMetaballs; i++)
            {
                var ball = new Metaball( );
                ball.Position = new Vector2( rand.Next(Game.GraphicsDevice.Viewport.Width ), rand.Next(Game.GraphicsDevice.Viewport.Height ) ) - new Vector2( 400 );
                ball.Velocity = new Vector2( rand.Next( 0, 2 ), rand.Next( 0, 5 ) );
                ball.Texture = metaballTextures[0];
                ball.Glow = glowColors[0];
                
                balls.Add( ball );
            }


        }

        public override void Update( GameTime gameTime )
        {
            foreach (var ball in balls)
                ball.Update();
        }
    }
}
