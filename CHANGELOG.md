# Change Log

## 2.4.3
- namespace **LuviKunG.Console** is required. (```using LuviKunG.Console;```)
- Make ```LuviConsole``` as plugin manager and separate with **LuviTools** git.
- Remove requirement of ```StringBuilderRichTextExtension``` and using internal coloring rich text.

## 2.4.2
- Fix changes are not save via Unity Inspector.

## 2.4.1
- New **Command Group** that using for grouping your command.
- New **Execute Command Immediately** option that will execute the command instantly when press the command button.
- Remove internal Rich Text display for Log. But...
- Require extension of ```StringBuilderRichTextExtension``` to display rich text in Log.
- New **Command Log** to display your executed command in Log.

## 2.4.0
- namespace **LuviKunG** is required. (```using LuviKunG;```)
- WebGL support.
- New drag scroll view on log. (working on all platform)
- New LuviCommand syntax.
    - Now you can use string in your command by using quote "Your string here" to get full string without serparate by space.
    - Fix bugs that execute by double quote and got an error.
- Add realtime update window size and orientation.
- Add ```LuviConsoleException``` to throw error during execute the command.

## 2.3.6
- Upgrade compatible with Unity version 5, 2018 and 2019.
- Rearrange the inspectator.
- Add new unity instantiate menu on **GameObject > LuviKunG > LuviConsole.**

## Older Version
- Combine ```LuviDebug``` and ```LuviCommand``` into one as ```LuviConsole```.