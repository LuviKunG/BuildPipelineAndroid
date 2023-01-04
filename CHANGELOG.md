# Change Log

## 1.0.7
- Refractor the code in **Android Build Pipeline Settings**
    - Remove all usage of **PlayerPrefs** that holding values of build pipeline.
    - Using as serialized **Scriptable Object Asset** in **Editor/Resources** instead.
    - Still able to call for instance and save the value as serialized asset.
- Improve the **Android Build Pipeline Window** a little bit.
    - Change size of button as unexpanded buttons.
    - Increase size of execution buttons.
    - Add new button of **Retrive** in keystore name for retrive the keystore path from **PlayerSettings.Android.keystoreName**.

## 1.0.6
- Add new popup field of **'Create Symbols'** that will create `symbols.zip` on build, using for upload to Google Play Console.
- Remove usage of `BUILD_PIPELINE_ANDROID_ENABLE_BUNDLE_VERSION_CONFIG` and always enable app bundle version field.
- Add label for display next app bundle version when enabling **'Increment Bundle Version'**.
- **'Build App Bundle'** and **'Split Application Binary'** will use value from Unity Editor.

## 1.0.5
- Require minimal version of Unity Editor 2021.3 LTS. Older of Unity Editor version are no longer support.
- Change UI.
- Add toggle of **'Build App Bundle'**.
- Add toggle of **'Split Application Binary'**.
- Add Scripting Define Symbols of `BUILD_PIPELINE_ANDROID_ENABLE_BUNDLE_VERSION_CONFIG` for enable bundle version field in settings.

## 1.0.4
- Add new 'Build options'.

## 1.0.3
- Fix 'set build location' isn't working properly.
- Change the menu structure.
    - Move 'set build location' and 'open build location' into settings window.
    - Move 'open settings' to 'Settings/Android'.

## 1.0.2
- Add the option of **'Use Keystore'**

## 1.0.1
- Add the option of **'Increment Bundle Version'**

## 1.0.0
- Create the plugins.