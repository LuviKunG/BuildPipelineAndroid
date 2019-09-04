# Build Pipeline for Android

This is a build tools for Unity Editor that easier to use. Include custom format name and auto increment bundle every perform a build.
Created by Thanut Panichyotai (@[LuviKunG]((https://github.com/LuviKunG)))

## How to use?

New menu will be add to your Unity Editor named 'Build'. In the menu that will include two elements.

- **Android** will perform a build. If you're not select the directory before, it will popup the directory selection window and start perform a build.
- **Settings/Android** will open the build pipeline settings for android.

In the settings, you can set a format name, date format and other options for build. (It's already include descriptions)

## How to install?

### UPM Install via manifest.json

Locate to your Unity Project. In *Packages* folder, you will see a file named **manifest.json**. Open it with your text editor (such as Notepad++ or Visual Studio Code or Legacy Notepad)

Then merge this json format below.

(Do not just copy & paste the whole json! If you are not capable to merge json, please using online JSON merge tools like [this](https://tools.knowledgewalls.com/onlinejsonmerger))

```json
{
  "dependencies": {
    "com.luvikung.buildpipelineandroid": "https://github.com/LuviKunG/AndroidBuildPipeline.git#1.0.3"
  }
}
```

If you want to install the older version, please take a look at release tag in this git, then change the path after **#** to the version tag that you want.

### Unity UPM Git Extension

If you doesn't have this package before, please redirect to this git [https://github.com/mob-sakai/UpmGitExtension](https://github.com/mob-sakai/UpmGitExtension) then follow the instruction in README.md to install the **UPM Git Extension** to your Unity.

If you already installed. Open the **Package Manager UI**, you will see the git icon around the bottom left connor, Open it then follow the instruction using this git URL to perform package install.

![Install LuviConsole with UPM Git Extension](images/install01.png)

Make sure that you're select the latest version.