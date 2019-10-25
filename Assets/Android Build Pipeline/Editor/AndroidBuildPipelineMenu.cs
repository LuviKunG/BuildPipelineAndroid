using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace LuviKunG.BuildPipeline.Android
{
    public static class AndroidBuildPipelineMenu
    {
        private static AndroidBuildPipelineSettings settings;

        [MenuItem("Build/Android", false, 0)]
        public static void Build()
        {
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
                return;
            settings = AndroidBuildPipelineSettings.Instance;
            string path;
            if (string.IsNullOrEmpty(settings.buildPath))
                path = OpenBuildSavePanel(settings.buildPath);
            else
                path = settings.buildPath;
            var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            for (int i = 0; i < scenes.Count; i++)
                if (!scenes[i].enabled)
                    scenes.RemoveAt(i--);
            if (!(scenes.Count > 0))
                return;
            if (settings.incrementBundle)
            {
                int bundleVersion = PlayerSettings.Android.bundleVersionCode;
                bundleVersion++;
                PlayerSettings.Android.bundleVersionCode = bundleVersion;
            }
            if (settings.useKeystore)
            {
                PlayerSettings.Android.keystoreName = settings.keystoreName;
                PlayerSettings.Android.keystorePass = settings.keystorePass;
                PlayerSettings.Android.keyaliasName = settings.keyaliasName;
                PlayerSettings.Android.keyaliasPass = settings.keyaliasPass;
            }
            string fileName = settings.GetFileName();
            string buildPath = Path.Combine(path, fileName);
            BuildReport report = UnityEditor.BuildPipeline.BuildPlayer(scenes.ToArray(), buildPath, BuildTarget.Android, settings.buildOptions);
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded at '{buildPath}' using {summary.totalTime.TotalSeconds.ToString("N2")} seconds with size of {summary.totalSize} bytes.");
                Application.OpenURL(path);
            }
            if (summary.result == BuildResult.Failed)
            {
                Debug.LogError($"Build failed...");
            }
        }

#if BUILD_PIPELINE_ANDROID_OLD_MENU
        [MenuItem("Build/Settings/Android/Set Build Directory...", false, 1)]
        public static void SetDirectory()
        {
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
                return;
            settings = AndroidBuildPipelineSettings.Instance;
            string newPath = OpenBuildSavePanel(settings.buildPath);
            if (!string.IsNullOrEmpty(newPath))
                EditorUtility.DisplayDialog("Android Build Pipeline", $"Build directory has been set to: {newPath}", "Okay");
        }

        [MenuItem("Build/Settings/Android/Open Directory...", true, 2)]
        public static bool OpenDirectoryValidate()
        {
            settings = AndroidBuildPipelineSettings.Instance;
            string loadedBuildPath = settings.buildPath;
            return !string.IsNullOrEmpty(loadedBuildPath);
        }

        [MenuItem("Build/Settings/Android/Open Directory...", false, 2)]
        public static void OpenDirectory()
        {
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
                return;
            settings = AndroidBuildPipelineSettings.Instance;
            string loadedBuildPath = settings.buildPath;
            if (string.IsNullOrEmpty(loadedBuildPath))
                return;
            Application.OpenURL(loadedBuildPath);
        }

        [MenuItem("Build/Settings/Android/Open Build Settings...", false, 3)]
        public static void OpenBuildSetting()
        {
            AndroidBuildPipelineSettingsWindow.OpenWindow();
        }
#else
        [MenuItem("Build/Settings/Android", false, 0)]
        public static void OpenBuildSetting()
        {
            AndroidBuildPipelineSettingsWindow.OpenWindow();
        }
#endif
        private static string OpenBuildSavePanel(string path)
        {
            string newPath = EditorUtility.SaveFolderPanel("Choose Location of Build Game", path, null);
            if (string.IsNullOrEmpty(newPath))
                return null;
            settings.buildPath = newPath;
            settings.Save();
            return newPath;
        }
    }
}