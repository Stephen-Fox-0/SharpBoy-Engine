using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Utility
{
    public class EnumUtility
    {
        public static string[] GetValues(Type type)
        {
            return (string[])Enum.GetNames(type);
        }

        public static string[] GetResulotionValues(Screen.Graphics.SceneGraphicsDevice device)
        {
            List<string> values = new List<string>();
            foreach (var items in device)
                values.Add(items.Name);
            return values.ToArray();
        }
    }
}
