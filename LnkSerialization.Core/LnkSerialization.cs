using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ShellLink;

using Wororo.Utilities;

// Some useful links:
// https://stackoverflow.com/questions/37171394/how-to-create-shortcut-to-computer-on-the-users-desktop
// https://superuser.com/questions/217504/is-there-a-list-of-windows-special-directories-shortcuts-like-temp
// https://mypchell.com/guides/34-guides/69-156-useful-run-commands
// https://ss64.com/nt/syntax-variables.html
namespace LnkSerialization.Core
{
    public static class LnkSerialization
    {
        public static string DefaultBackupFilename { get; set; } = "LinksBackup.json";

        public static void SerializeLinkFolder(this string folderWithLinks)
        {
            if (!Directory.Exists(folderWithLinks)) return;
            var lnkFiles = Directory.GetFiles(folderWithLinks, "*.lnk");
            var list = lnkFiles.Select(lnkFile => new LinkModel(lnkFile));
            list.SerializeJson(DefaultBackupFilename);
        }

        public static void RestoreLinks(string outputFolder)
        {
            if (!File.Exists(DefaultBackupFilename))
                return;

            outputFolder.CreatePathIfNotExists();
            var links =
                JsonSerialization.DeserializeJson<IEnumerable<LinkModel>>(DefaultBackupFilename);
            foreach (var linkModel in links)
            {
                linkModel.ToLinkFile(outputFolder);
            }
        }

        public static void SerializeJson(string linkFile)
        {
            var lnkShortcut = Shortcut.ReadFromFile(linkFile);
            lnkShortcut.SerializeJson($"{Path.GetFileName(linkFile)}.json");
        }

        public static void ReadLnk(string path)
        {
            var lnkShortcut = Shortcut.ReadFromFile(path);
            var myPath = "C:\\Program Files\\Internet Explorer\\iexplore.exe";
            var myArgs = "-extoff";
            var workDir = "C:\\Program Files\\Internet Explorer";
            var myLnk = Shortcut.CreateShortcut(myPath, myArgs, workDir);
            myLnk.WriteToFile("ie_gen.lnk");
            lnkShortcut.SerializeJson("lnkShortcut.json");
            myLnk.SerializeJson("myLnk.json");
        }
    }



    public class LinkModel
    {
        public LinkModel() { }

        public LinkModel(string filename) => FromFilename(filename);

        private void FromFilename(string filename)
        {
            if (!File.Exists(filename))
                return;
            FromShortcut(Shortcut.ReadFromFile(filename), filename);
        }

        private void FromShortcut(Shortcut shortcut, string filename)
        {
            if (shortcut == null)
                return;
            var path = shortcut.LinkTargetIDList?.Path
                       ?? shortcut.ExtraData?.EnvironmentVariableDataBlock?.TargetUnicode
                       ?? shortcut.ExtraData?.EnvironmentVariableDataBlock?.TargetAnsi
                       ?? string.Empty;
            if (path.IsEmpty())
            {
                var propStoreValues = shortcut
                    .ExtraData?
                    .PropertyStoreDataBlock?
                    .PropertyStore?
                    .SelectMany(x => x.PropertyStorage)
                    .Select(x => x.TypedPropertyValue)
                    .Select(x => x.Value) 
                    ?? Array.Empty<string>();

                var values = propStoreValues
                    .Where(x => x != null)
                    .Select(x => x.ToString())
                    .Where(x => x.IsNotEmpty());

                var value = values.FirstOrDefault(x => x.StartsWith("::"));

                if (value.IsNotEmpty())
                    path = value;
            }
            var args = shortcut.StringData?.CommandLineArguments ?? string.Empty;
            var workingDir = shortcut.StringData?.WorkingDir ?? string.Empty;
            var name = System.IO.Path.GetFileName(filename);

            FromArguments(path, args, workingDir, name);
        }

        private void FromArguments(string path, string args, string workingDirectory, string filename)
        {
            Path = path;
            Args = args;
            WorkingDirectory = workingDirectory;
            Filename = filename;
        }

        public void ToLinkFile(string outputPath)
        {
            outputPath.CreatePathIfNotExists();
            Shortcut.CreateShortcut(Path, Args, WorkingDirectory)
                .WriteToFile(System.IO.Path.Combine(outputPath, Filename));
        }

        public string Path { get; set; }
        public string Args { get; set; }
        public string WorkingDirectory { get; set; }
        public string Filename { get; set; }
    }
}
