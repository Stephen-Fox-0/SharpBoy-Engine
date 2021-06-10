using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Utility
{
    public enum SpecialFolderEx
    {
        Config,
        Downloads,
        Library,
        SaveFolder,
        Plugins,
        Screenshot,
    }
    public class PathUtility
    {
        static string ConfigFolder = Environment.CurrentDirectory + "/Content/Config";
        static string DownloadsFolder = Environment.CurrentDirectory + "/Content/Emulator/Downloads";
        static string LibraryFolder = Environment.CurrentDirectory + "/Content/Emulator/Library";
        static string SaveFolder = Environment.CurrentDirectory + "/Content/Emulator/Saves";
        static string PluginsFolder = Environment.CurrentDirectory + "/Content/Emulator/Plugins";
        static string ScreenshotFolder = Environment.CurrentDirectory + "/Content/Emulator/Screenshots";

        public static string GetSpecialFolder(SpecialFolderEx specialFolder)
        {
            switch (specialFolder)
            {
                case SpecialFolderEx.Config:
                    return ConfigFolder;
                case SpecialFolderEx.Downloads:
                    return DownloadsFolder;
                case SpecialFolderEx.Library:
                    return LibraryFolder;
                case SpecialFolderEx.Plugins:
                    return PluginsFolder;
                case SpecialFolderEx.SaveFolder:
                    return SaveFolder;
                case SpecialFolderEx.Screenshot:
                    return ScreenshotFolder;
                default:
                    return "";
            }
        }

        public static bool Exists(SpecialFolderEx specialFolder)
        {
            return System.IO.Directory.Exists(GetSpecialFolder(specialFolder));
        }

        public static void IfNotCreate()
        {
            var items = (string[])Enum.GetNames(typeof(SpecialFolderEx));
            foreach(var item in items)
            {
                var specialFolder = (SpecialFolderEx)Enum.Parse(typeof(SpecialFolderEx), item);
                if(!Exists(specialFolder))
                {
                    System.IO.Directory.CreateDirectory(GetSpecialFolder(specialFolder));
                }
            }    
        }
    }
}
