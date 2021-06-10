using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SharpBoyEngine.Screen.Graphics
{
    public class SceneResulotionCollection : List<SceneResulotion>
    {
        public bool LoadFromXml(string xmlFilePath)
        {
            bool flag = false;
            if (!File.Exists(xmlFilePath))
                return false;
            try
            {

                XDocument doc = XDocument.Load(xmlFilePath);
                XElement element = doc.Element("Resulotions");
                IEnumerable<XElement> elements = element.Elements("item");

                foreach (var elem in elements)
                {
                    var width = elem.Attribute("width").Value;
                    var height = elem.Attribute("height").Value;
                    var aWidth = elem.Attribute("aspect-width").Value;
                    var aHeight = elem.Attribute("aspect-height").Value;
                    var name = elem.Value;

                    var multiSampling = elem.Attribute("multiSampling").Value == "1";
                    var vsync = elem.Attribute("vsync").Value == "1";
                    var hwsm = elem.Attribute("hardwareSwitchMode").Value == "1";

                    var full = elem.Attribute("fullScreen").Value == "1";

                    var size = new SceneSize(int.Parse(width), int.Parse(height));
                    var aspect = new SceneAspectSize(size);
                    var res = new SceneResulotion(name, size, aspect);

                    res.IsFullscreen = full;
                    res.SetOption(SceneResulotionOption.HardwareSwitchMode, hwsm);
                    res.SetOption(SceneResulotionOption.MultiSampling, multiSampling);
                    res.SetOption(SceneResulotionOption.VirticalSyncronization, vsync);

                    Add(res);
                }
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        public void SaveToXml(string xmlSavePath)
        {
            if (string.IsNullOrWhiteSpace(xmlSavePath))
                return;

            XElement element = new XElement("Resulotions");

            foreach (var res in this)
            {
                XElement resElement = new XElement("item", res.Name);
                XAttribute width = new XAttribute("width", res.Size.Width);
                XAttribute height = new XAttribute("height", res.Size.Height);
                XAttribute aWidth = new XAttribute("aspect-width", res.AspectRatio.Width);
                XAttribute aHeight = new XAttribute("aspect-height", res.AspectRatio.Height);
                XAttribute multi = new XAttribute("multiSampling", res.GetOption(SceneResulotionOption.MultiSampling) ? "1" : "0");
                XAttribute v = new XAttribute("vsync", res.GetOption(SceneResulotionOption.VirticalSyncronization) ? "1" : "0");
                XAttribute h = new XAttribute("hardwareSwitchMode", res.GetOption(SceneResulotionOption.HardwareSwitchMode) ? "1" : "0");
                XAttribute f = new XAttribute("fullScreen", res.IsFullscreen ? "1" : "0");
                resElement.Add(new XAttribute[] { width, height, aWidth, aHeight, multi, v, h,f });
                element.Add(resElement);
            }
            element.Save(xmlSavePath);
        }
    }
}
