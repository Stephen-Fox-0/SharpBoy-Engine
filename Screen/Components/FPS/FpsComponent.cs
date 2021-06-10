using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpBoyEngine.Screen.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Components.FPS
{
    public enum FpsComponentView
    {
        Detailed,
        Simplified,
    }
    public class FpsComponent : SceneSystemComponent
    {
        FpsCounter counter;
        ResourceContentManager content;
        SpriteFont font;
        FpsComponentView view;
        public FpsComponent(Game game, FpsComponentView viewType) :base(game)
        {
            counter = new FpsCounter();
            content = new ResourceContentManager(game.Services, Properties.Resources.ResourceManager);
            view = viewType;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            counter.Update(deltaTime);

            var fps = "";
            switch(view)
            {
                case FpsComponentView.Detailed:
                    fps = string.Format("Average: {0}\nCurrent: {1}\nSeconds: {2}\nFrames: {3}\nSamples: {4}", new object[]
                    {
                        counter.AverageFramesPerSecond.ToString("00.00"),
                        counter.CurrentFramesPerSecond.ToString("00.00"),
                        counter.TotalSeconds.ToString("00.00"),
                        counter.TotalFrames,
                        counter.Samples,
                    });
                    break;
                case FpsComponentView.Simplified:
                    fps = string.Format("{0}", counter.AverageFramesPerSecond.ToString("00.00"));
                    break;
            }

            Color color;

            if (counter.AverageFramesPerSecond >= 60)
                color = Color.White;
            if (counter.AverageFramesPerSecond <= 40)
                color = Color.Yellow;
            if (counter.AverageFramesPerSecond <= 20)
                color = Color.Orange;
            if (counter.AverageFramesPerSecond < 19)
                color = Color.Red;


            spriteBatch.Begin();
            spriteBatch.DrawTextShadow(content.Load<SpriteFont>("fps"), fps, new Vector2(1, 1), Color.White);
            spriteBatch.End();
        }
    }
}
