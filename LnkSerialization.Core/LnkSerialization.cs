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
    public static string DefaultJsonFilename { get; set; } = "links.json";
    public static string DefaultLinksFolder { get; set; } = @"%userProfile%\shortcuts\";

    public static void DeserializeLinksToFolder(string jsonFilename, string outputFolder)
    {
      if (jsonFilename.IsEmpty()) {
        jsonFilename = DefaultJsonFilename;
      }

      if (outputFolder.IsEmpty()) {
        outputFolder = DefaultLinksFolder;
      }

      if (!outputFolder.EndsWith("\\")) {
        outputFolder = $"{outputFolder}\\";
      }

      if (!File.Exists(jsonFilename)) {
        return;
      }

      outputFolder.CreatePathIfNotExists();
      var links = JsonSerialization.DeserializeJson<IEnumerable<LinkModel>>(jsonFilename);

      var count = links.Count();
      Console.WriteLine($"{count} entries found.");

      var i = 0;

      foreach (var linkModel in links) {
        Console.WriteLine($"Deserializing {linkModel.Filename} ({++i}/{count})");
        linkModel.ToLinkFile(outputFolder);
      }
    }

    public static void SerializeAsShortcutJson(string linkFile)
    {
      var lnkShortcut = Shortcut.ReadFromFile(linkFile);
      lnkShortcut.SerializeJson($"{Path.GetFileName(linkFile)}.json");
    }

    public static void SerializeLinkFolder(string folderWithLinks, string outputJsonFilename = "")
    {
      if (folderWithLinks.IsEmpty()) {
        folderWithLinks = DefaultLinksFolder;
      }

      if (outputJsonFilename.IsEmpty()) {
        outputJsonFilename = Path.Combine(folderWithLinks, DefaultJsonFilename);
      }

      if (!Directory.Exists(folderWithLinks)) {
        return;
      }

      var lnkFiles = Directory.GetFiles(folderWithLinks, "*.lnk");
      var list = lnkFiles.Select(lnkFile => new LinkModel(lnkFile));
      list.SerializeJson(outputJsonFilename);
    }
  }
}