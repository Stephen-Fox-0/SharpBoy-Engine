using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpBoyEngine.Graphics;
using SharpBoyEngine.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Effects
{
    public abstract class SnowLayorEmitter : Screen.Components.SceneSystemComponent
    {
        private float _generateTimer;

        private float _swayTimer;

        protected Sprite _particlePrefab;

        protected List<Sprite> _particles;

        /// <summary>
        /// How often a particle is produced
        /// </summary>
        public float GenerateSpeed = 0.005f;

        /// <summary>
        /// How often we apply the "GlobalVelociy" to our particles
        /// </summary>
        public float GlobalVelocitySpeed = 1;

        public int MaxParticles = 1000;

        public SnowLayorEmitter( Sprite particle, Game game):base(game)
        {
            _particlePrefab = particle;
            _particles = new List<Sprite>();
        }

        private void AddParticle()
        {
            if (_generateTimer > GenerateSpeed)
            {
                _generateTimer = 0;

                if (_particles.Count < MaxParticles)
                {
                    _particles.Add( GenerateParticle() );
                }
            }
        }

        protected abstract void ApplyGlobalVelocity();

        private void RemovedFinishedParticles()
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                if (_particles[i].IsRemoved)
                {
                    _particles.RemoveAt( i );
                    i--;
                }
            }
        }

        protected abstract Sprite GenerateParticle();


        public override void Update( GameTime gameTime )
        {
            _generateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _swayTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            AddParticle();

            if (_swayTimer > GlobalVelocitySpeed)
            {
                _swayTimer = 0;

                ApplyGlobalVelocity();
            }

            foreach (var particle in _particles)
                particle.Update( gameTime );

            RemovedFinishedParticles();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var particle in _particles)
                particle.Draw( gameTime, spriteBatch );
        }
    }

    public class SnowEmitter : SnowLayorEmitter
    {
        Random rand = new Random();
        public SnowEmitter( Sprite particle, Game game)
          : base( particle, game)
        {

        }

        protected override void ApplyGlobalVelocity()
        {
            var xSway = (float)rand.Next( -2, 2 );

            foreach (var particle in _particles)
                particle.Velocity.X = (xSway * particle.Scale) / 50;
        }

        protected override Sprite GenerateParticle()
        {
            var sprite = _particlePrefab.Clone() as Sprite;

            var xPosition = rand.Next( 0, Game.GraphicsDevice.Viewport.Width);
            var ySpeed = rand.Next( 10, 100 ) / 50f;

            sprite.Position = new Vector2( xPosition, -sprite.Rectangle.Height );
            sprite.Opacity = (float)rand.NextDouble();
            sprite.Rotation = MathHelper.ToRadians( rand.Next( 0, 360 ) );
            sprite.Scale = (float)rand.NextDouble() + rand.Next( 0, 1 );
            sprite.Velocity = new Vector2( 0, ySpeed );

            return sprite;
        }
    }
}
