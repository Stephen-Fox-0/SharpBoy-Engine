using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SharpBoyEngine.Screen
{
    public abstract class SceneObject
    {
        Scene parentScene;

        public Scene ParentScene
        {
            get => parentScene;
        }

        public SceneObject(Scene parentScene) => this.parentScene = parentScene;

        public abstract void LoadContent(ContentManager contentManager);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public virtual void Update(GameTime gameTime) { }
        public virtual void HandleInput(Input.InputSystem inputSystem) { }
    }
}
