using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpBoyEngine.Screen.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Components.Camara
{
    public class _2DCamara : SceneSystemComponent
    {
        public float Zoom { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Bounds { get; protected set; }
        public Rectangle VisibleArea { get; protected set; }
        public Matrix Transform { get; protected set; }
        private float zoom, previousZoom;


        public _2DCamara(Game game) : base(game)
        {
            Bounds = game.GraphicsDevice.Viewport.Bounds;
            Zoom = 1f;
            Position = Vector2.Zero;
        }


        private void UpdateVisibleArea()
        {
            var inverseViewMatrix = Matrix.Invert(Transform);

            var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var tr = Vector2.Transform(new Vector2(Bounds.X, 0), inverseViewMatrix);
            var bl = Vector2.Transform(new Vector2(0, Bounds.Y), inverseViewMatrix);
            var br = Vector2.Transform(new Vector2(Bounds.Width, Bounds.Height), inverseViewMatrix);

            var min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
            var max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
            VisibleArea = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        internal void UpdateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            UpdateVisibleArea();
        }

        public void MoveCamera(Vector2 movePosition)
        {
            Vector2 newPosition = Position + movePosition;
            Position = newPosition;
        }

        public void AdjustZoom(float zoomAmount)
        {
            Zoom += zoomAmount;
            if (Zoom < .35f)
            {
                Zoom = .35f;
            }
            if (Zoom > 2f)
            {
                Zoom = 2f;
            }
        }

        public void UpdateCamera()
        {
            Bounds = Game.GraphicsDevice.Viewport.Bounds;
            UpdateMatrix();

            Vector2 cameraMovement = Vector2.Zero;
            int moveSpeed = 0;

            if (Zoom > .8f)
            {
                moveSpeed = 15;
            }
            else if (Zoom < .8f && Zoom >= .6f)
            {
                moveSpeed = 20;
            }
            else if (Zoom < .6f && Zoom > .35f)
            {
                moveSpeed = 25;
            }
            else if (Zoom <= .35f)
            {
                moveSpeed = 30;
            }
            else
            {
                moveSpeed = 10;
            }
            previousZoom = zoom;
            zoom = Zoom;

            MoveCamera(cameraMovement);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateCamera();
        }

        public void SetZoomToAspect()
        {
            var aspect = ((float)SceneSystem.Instance.GraphicsDeviceManager.Resulotion.AspectRatio.Width / SceneSystem.Instance.GraphicsDeviceManager.Resulotion.AspectRatio.Height);

            Zoom = (SceneSystem.Instance.GraphicsDeviceManager.Resulotion.Size.Width / SceneSystem.Instance.GraphicsDeviceManager.Resulotion.Size.Height) / aspect;
        }

        public void BeginDraw()
        {
            SceneSystem.Instance.SpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, null, null, null, null, null, this.Transform);
        }


        public void BeginDraw(SamplerState state)
        {
            SceneSystem.Instance.SpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, null, state, null, null, null, this.Transform);
        }
        public void EndDraw()
        {
            SceneSystem.Instance.SpriteBatch.End();
        }
    }
}
