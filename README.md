# LnkSerialization

Project to handle my shortcut links as a human-friendly editable/readable collection.

In my work, I use the Win+R shortcuts **a lot** to access network folders, desktop, download or my different applications.

Sometimes I need to change my shortcuts batches:

- A network path changes
- I want to programatically add new folders (new project folders)
- I want to share my changes with other users: It is true that I can correctly create the shortcut with shortcuts as `%userProfile%`, but creating the shortcuts this way, takes longer.
- Update the binaries path of a tool that changes its folder: `\App_v1\bin.exe\` to `\App_v2\bin.exe`.
- Others

My setup is the following:

1. Have a folder (`C:\shortcuts\`) that is on the `Path` Windows Environment Variable.
2. In that folder, there is a `dl.lnk` shortcut to my `Downloads` folder.
3. Whenever I need to access the Downloads folder, I do a `Win+R` and run `dl`.
4. Voilà!

With this tool, the shortcuts can be serialized as a `.json` file. This file can be easily edited and shared. You can call your favorite tools and folders without navigating on your file explorer.

## Usage

To serialize a shortcuts folder to a .json file:

```
LnkSerialization serialize -i "Path\To\Shortcuts" -o "Path\To\backup.json"
```

To deserialize a .json file to a shortcuts folder:

```
LnkSerialization deserialize -i "Path\To\backup.json" -o "Path\To\backup.json" "Path\To\Shortcuts"
```

The repo comes with an `example.json` file with some starting links.

## Dependencies

This tool uses:

- CommandLineParser (https://github.com/commandlineparser/commandline)
- securifybv.ShellLink (https://github.com/securifybv/ShellLink)
- Wororo.Utilities (https://github.com/gabrieldelaparra/Wororo.Utilities)

## Notes

Please note that the tool is not throughly tested.
Please create an issue if you find any bugs.
