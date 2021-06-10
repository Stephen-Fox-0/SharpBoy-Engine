using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Graphics
{
    public class SceneResulotion
    {
        Dictionary<SceneResulotionOption, bool> options;
        bool fullScreen;
        SceneSize size;
        SceneAspectSize aspectRatio;
        string name;
        public string Name => name;

        public SceneAspectSize AspectRatio => aspectRatio;
        public SceneSize Size => size;
        public bool IsFullscreen
        {
            get => fullScreen;
            set => fullScreen = value;
        }

        public SceneResulotion(string name, SceneSize size, SceneAspectSize aspect, bool fullScreen)
        {
            this.size = size;
            this.aspectRatio = aspect;
            this.fullScreen = fullScreen;
            this.name = name;

            LoadDefaults();
        }

        public SceneResulotion(string name, SceneSize size, SceneAspectSize aspect)
        {
            this.size = size;
            this.aspectRatio = aspect;
            this.fullScreen = false;
            this.name = name;

            LoadDefaults();
        }

        public void SetOption(SceneResulotionOption option, bool value)
        {
            options[option] = value;
        }
        public bool GetOption(SceneResulotionOption option) => options[option];
        void LoadDefaults()
        {
            options = new Dictionary<SceneResulotionOption, bool>();
            options.Add(SceneResulotionOption.VirticalSyncronization, true);
            options.Add(SceneResulotionOption.MultiSampling, false);
            options.Add(SceneResulotionOption.HardwareSwitchMode, false);
        }
    }
}
