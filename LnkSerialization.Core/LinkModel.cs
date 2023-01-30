using System;
using System.IO;
using System.Linq;
using ShellLink;
using ShellLink.Flags;
using Wororo.Utilities;

namespace LnkSerialization.Core
{
    public class LinkModel
    {
        public LinkModel() { }

        public LinkModel(string filename)
        {
            FromFilename(filename);
        }

        public string Args { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }
        public string WorkingDirectory { get; set; }
        public bool RunAsAdmin { get; set; }

        public void ToLinkFile(string outputPath)
        {
            if (Path.Count(x => x == '%') % 2 == 0)
            {
                Path = Environment.ExpandEnvironmentVariables(Path);
            }
            var s = Shortcut.CreateShortcut(Path, Args, WorkingDirectory);
            
            if (RunAsAdmin) 
                s.LinkFlags |= ShellLink.Flags.LinkFlags.RunAsUser;

            if (!Path.ToLower().StartsWith("http"))
            {
                s.LinkFlags = s.LinkFlags | ShellLink.Flags.LinkFlags.IsUnicode | ShellLink.Flags.LinkFlags.HasLinkTargetIDList;
                s.LinkTargetIDList = new ShellLink.Structures.LinkTargetIDList
                {
                    Path = Path
                };
            }
            s.WriteToFile(System.IO.Path.Combine(outputPath, Filename));
        }

        private void FromArguments(string path, string args, string workingDirectory, string filename, bool runAsAdmin)
        {
            Path = path;
            Args = args;
            WorkingDirectory = workingDirectory;
            Filename = filename;
            RunAsAdmin = runAsAdmin;
        }

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
            var runAsAdmin = (shortcut.LinkFlags & LinkFlags.RunAsUser) != 0;
            var workingDir = shortcut.StringData?.WorkingDir ?? string.Empty;
            var name = System.IO.Path.GetFileName(filename);

            FromArguments(path, args, workingDir, name, runAsAdmin);
        }
    }
}