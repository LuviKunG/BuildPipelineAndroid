# Change Log

## v1.0.7 January 4th 2023 - Latest

`https://github.com/LuviKunG/BuildPipelineAndroid.git#1.0.7`

- Refractor the code in **Android Build Pipeline Settings**
    - Remove all usage of **PlayerPrefs** that holding values of build pipeline.
    - Using as serialized **Scriptable Object Asset** in **Editor/Resources** instead.
    - Still able to call for instance and save the value as serialized asset.
- Improve the **Android Build Pipeline Window** a little bit.
    - Change size of button as unexpanded buttons.
    - Increase size of execution buttons.
    - Add new button of **Retrive** in keystore name for retrive the keystore path from **PlayerSettings.Android.keystoreName**.

## v1.0.6 December 12th 2022

`https://github.com/LuviKunG/BuildPipelineAndroid.git#1.0.6`

- Add new popup field of **'Create Symbols'** that will create `symbols.zip` on build, using for upload to Google Play Console.
- Remove usage of `BUILD_PIPELINE_ANDROID_ENABLE_BUNDLE_VERSION_CONFIG` and always enable app bundle version field.
- Add label for display next app bundle version when enabling **'Increment Bundle Version'**.
- **'Build App Bundle'** and **'Split Application Binary'** will use value from Unity Editor.

## v1.0.5 November 29th 2022

`https://github.com/LuviKunG/BuildPipelineAndroid.git#1.0.5`

- Require minimal version of Unity Editor 2021.3 LTS. Older of Unity Editor version are no longer support.
- Change UI.
- Add toggle of **'Build App Bundle'**.
- Add toggle of **'Split Application Binary'**.
- Add Scripting Define Symbols of `BUILD_PIPELINE_ANDROID_ENABLE_BUNDLE_VERSION_CONFIG` for enable bundle version field in settings.

## v1.0.4 October 25th 2019

`https://github.com/LuviKunG/BuildPipelineAndroid.git#1.0.4`

- Add new 'Build options'.

## v1.0.3 September 4th 2019

`https://github.com/LuviKunG/BuildPipelineAndroid.git#1.0.3`

- Fix 'set build location' isn't working properly.
- Change the menu structure.
    - Move 'set build location' and 'open build location' into settings window.
    - Move 'open settings' to 'Settings/Android'.

## v1.0.2 September 2nd 2019

`https://github.com/LuviKunG/BuildPipelineAndroid.git#1.0.2`

- Add the option of **'Use Keystore'**

## v1.0.1 August 30th 2019

`https://github.com/LuviKunG/BuildPipelineAndroid.git#1.0.1`

- Add the option of **'Increment Bundle Version'**

## v1.0.0 August 30th 2019

`https://github.com/LuviKunG/BuildPipelineAndroid.git#1.0.0`

- Create the plugins.