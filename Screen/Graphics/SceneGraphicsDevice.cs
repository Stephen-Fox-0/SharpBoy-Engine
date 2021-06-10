using Microsoft.Xna.Framework;
using SharpBoyEngine.Screen.Components.Camara;
using SharpBoyEngine.Screen.Services;
using SharpBoyEngine.Screen.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Graphics
{
    public sealed class SceneGraphicsDevice :IEnumerable<SceneResulotion>
    {
        GraphicsDeviceManager _graphicsDevice;
        SceneResulotion _currentResulotion;
        SceneResulotionCollection _resulotions;
        SceneBounds bounds;
        public int ResulotionCount => _resulotions.Count;
        public SceneResulotion Resulotion
        {
            get => _currentResulotion;
        }

        public SceneBounds Bounds => bounds;

        public SceneGraphicsDevice(GraphicsDeviceManager deviceManager)
        {
            if (deviceManager == null)
                return;
            this._graphicsDevice = deviceManager;
            this._resulotions = new SceneResulotionCollection();

            // set the graphics device manager profile, to hires.
            this._graphicsDevice.GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile.HiDef;
        }

        public void ApplyResulotion(string name)
        {
            SceneResulotion resulotion = null;

            foreach (var item in _resulotions)
                if (item.Name == name)
                    resulotion = item;

            if (resulotion == null)
                throw new Exception("Could not find a resulotion with the name '" + name + "' inside of the collection");


            this.bounds = new SceneBounds(resulotion);
            this._currentResulotion = resulotion;

            this._graphicsDevice.PreferMultiSampling = resulotion.GetOption(SceneResulotionOption.MultiSampling);
            this._graphicsDevice.SynchronizeWithVerticalRetrace = resulotion.GetOption(SceneResulotionOption.VirticalSyncronization);

            this._graphicsDevice.PreferredBackBufferWidth = resulotion.Size.Width;
            this._graphicsDevice.PreferredBackBufferHeight = resulotion.Size.Height;

            this._graphicsDevice.IsFullScreen = resulotion.IsFullscreen;
            this._graphicsDevice.HardwareModeSwitch = resulotion.GetOption(SceneResulotionOption.HardwareSwitchMode);

            this._graphicsDevice.ApplyChanges();
        }
        
        public void ApplyResulotion(SceneResulotion resulotion)
        {
            SceneResulotion res = null;
            foreach (var item in _resulotions)
                if (item.Name == resulotion.Name)
                    res = item;

            if (res == null)
                throw new Exception("resulotion is not apart of the collection");
            else
                ApplyResulotion(res.Name);
        }
        
        public SceneResulotion this[int index] => _resulotions[index];

        public void AddResulotion(SceneResulotion resulotion)
        {
            if (resulotion == null)
                return;

            _resulotions.Add(resulotion);
        }
        public void RemoveResulotion(SceneResulotion resulotion)
        {
            if (resulotion == null)
                return;

            _resulotions.Remove(resulotion);
        }

        public bool LoadResulotionsFromXml(string xmlPath) => _resulotions.LoadFromXml(xmlPath);
        public void SaveResulotionsToXml(string xmlSavePath) => _resulotions.SaveToXml(xmlSavePath);

        public IEnumerator<SceneResulotion> GetEnumerator()
        {
            return _resulotions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _resulotions.GetEnumerator();
        }

        public void ClearResulotions() => _resulotions.Clear();
    }
}
