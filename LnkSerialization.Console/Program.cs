using System.Collections.Generic;
using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace LnkSerialization.Console
{
    public class Program
    {
        private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            var helpText = HelpText.AutoBuild(result, h => {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = "Links Serialization Util";
                h.AddDashesToOption = true;
                h.AutoHelp = true;
                h.AutoVersion = true;
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);
            System.Console.WriteLine(helpText);
        }

        private static void Main(string[] args)
        {
            System.Console.WriteLine($"Args: {string.Join(' ', args.Select(x => $"\"{x}\""))}");
            var parser = new Parser(with => with.HelpWriter = null);
            var parserResult = parser.ParseArguments<SerializeOptions, DeserializeOptions>(args);

            Parser.Default.ParseArguments<SerializeOptions, DeserializeOptions>(args)
                .WithParsed<SerializeOptions>(RunSerialize)
                .WithParsed<DeserializeOptions>(RunDeserialize)
                .WithNotParsed(errs => DisplayHelp(parserResult, errs));
        }

        private static void RunDeserialize(DeserializeOptions options)
        {
            Core.LnkSerialization.DeserializeLinksToFolder(options.InputFilename, options.OutputPath);
        }

        private static void RunSerialize(SerializeOptions options)
        {
            Core.LnkSerialization.SerializeLinkFolder(options.InputPath, options.OutputFilename);
        }

        [Verb("serialize",
            HelpText =
                "Serializes a folder with *.lnk files to a .json file.\nUses a custom link model for the serialization.")]
        private class SerializeOptions
        {
            [Option('i', "inputPath", Required = true,
                HelpText = "Input folder with *.lnk files.\nExample: 'c:\\dirSource'")]
            public string InputPath { get; set; }

            [Option('o', "outputfile", Required = true,
                HelpText = "Output path to json filename.\nExample: 'c:\\dirBackup\\links.json'")]
            public string OutputFilename { get; set; }
        }

        [Verb("deserialize",
            HelpText =
                "Takes a .json file with a collection of link models.\nDeserializes each link model to a *.lnk link.")]
        private class DeserializeOptions
        {
            [Option('i', "inputfile", Required = true,
                HelpText = "Path to json filename that contains links.\nExample: 'c:\\dirBackup\\links.json'")]
            public string InputFilename { get; set; }

            [Option('o', "outputPath", Required = true,
                HelpText = "Output folder where the *.lnk files will be created.\nExample: 'c:\\dirRestore'")]
            public string OutputPath { get; set; }
        }
    }
}