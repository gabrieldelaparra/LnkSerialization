using System;
using System.IO;
using System.Linq;
using ShellLink;
using Wororo.Utilities;

namespace LnkSerialization.Core
{
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