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
    }
}
