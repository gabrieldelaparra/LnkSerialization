using System;

using ShellLink;

using Wororo.Utilities;

namespace LnkSerialization.Core
{
    public static class LnkSerialization
    {
        public static void ReadLnk(string path)
        {
            var lnkShortcut = Shortcut.ReadFromFile(path);
            var myPath = "C:\\Program Files\\Internet Explorer\\iexplore.exe";
            var myArgs = "-extoff";
            var workDir = "C:\\Program Files\\Internet Explorer";
            var myLnk = Shortcut.CreateShortcut(myPath, myArgs, workDir);
            lnkShortcut.SerializeJson("lnkShortcut.json");
            myLnk.SerializeJson("myLnk.json");
        }
    }
}
