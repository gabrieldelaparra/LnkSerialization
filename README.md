# LnkSerialization

Tool to serialize/deserialize shortcut links as a human-friendly editable/readable json collection.

## Description

In my daily chores, I use the Win+R shortcuts **a lot** to access network folders, desktop, download, misc folders; or different applications.
I have a path env to a shortcuts folder, so Win+R can call the links from there.

My setup is the following:

1. Have a folder (`C:\shortcuts\`) with your links. (I have provided a `example.json` with some links. A deserialization is required. See below.)
2. Add that folder to your Windows `Path` Environment Variables. (possible via `setx path "%PATH%;C:\shortcuts"`).
3. Usage: In that (`C:\shortcuts\`) folder, add a `dl.lnk` shortcut to my `Downloads` folder.
4. Whenever I need to access the Downloads folder, I do a `Win+R` and run `dl`.
5. Voilà!

Now, sometimes I need to change my shortcuts in batches:

- A network path changes > multiple shortcuts are affected
- I want to programatically add new folders (e.g.: new project folders)
- I want to share my changes with other users: It is true that I can correctly create the shortcut with shortcuts as `%userProfile%`, but creating the shortcuts this way, takes longer.
- Update the binaries path of a tool that changes its folder. e.g.: `\App_v1\bin.exe` to `\App_v2\bin.exe`.
- Other reasons to update many shortcuts in batches (?)

With this tool, the shortcuts can be serialized to a `.json` file.\
This file can be easily edited and shared.\
You can call your favorite tools and folders without navigating on your file explorer.

## Tool usage

To serialize a folder with shortcuts to a .json file:

```
LnkSerialization serialize -i "Path\To\Shortcuts" -o "Path\To\backup.json"
```

To deserialize a .json file to a folder with shortcuts:

```
LnkSerialization deserialize -i "Path\To\backup.json" -o "Path\To\Shortcuts"
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
