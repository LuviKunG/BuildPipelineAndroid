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
            settings = settings ?? new AndroidBuildPipelineSettings();
            string loadedBuildPath = settings.buildPath;
            string directoryPath;
            if (string.IsNullOrEmpty(loadedBuildPath))
            {
                directoryPath = EditorUtility.SaveFolderPanel("Choose Location of Build Game", loadedBuildPath, null);
                if (string.IsNullOrEmpty(directoryPath))
                    return;
            }
            else
                directoryPath = loadedBuildPath;
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
            string buildPath = Path.Combine(directoryPath, fileName);
            BuildReport report = UnityEditor.BuildPipeline.BuildPlayer(scenes.ToArray(), buildPath, BuildTarget.Android, BuildOptions.None);
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded at '{buildPath}' using {summary.totalTime.TotalSeconds.ToString("N2")} seconds with size of {summary.totalSize} bytes.");
                Application.OpenURL(directoryPath);
            }
            if (summary.result == BuildResult.Failed)
            {
                Debug.LogError($"Build failed...");
            }
        }

        [MenuItem("Build/Settings/Android/Set Build Directory...", false, 1)]
        public static void SetDirectory()
        {
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
                return;
            settings = settings ?? new AndroidBuildPipelineSettings();
            string loadedBuildPath = settings.buildPath;
            string newPath = EditorUtility.SaveFolderPanel("Choose Location of Build Game", loadedBuildPath, null);
            if (string.IsNullOrEmpty(newPath))
                return;
            EditorUserBuildSettings.SetBuildLocation(BuildTarget.Android, newPath);
            Debug.Log($"Build directory has been set to: {newPath}");
        }

        [MenuItem("Build/Settings/Android/Open Directory...", true, 2)]
        public static bool OpenDirectoryValidate()
        {
            settings = settings ?? new AndroidBuildPipelineSettings();
            string loadedBuildPath = settings.buildPath;
            return !string.IsNullOrEmpty(loadedBuildPath);
        }

        [MenuItem("Build/Settings/Android/Open Directory...", false, 2)]
        public static void OpenDirectory()
        {
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
                return;
            settings = settings ?? new AndroidBuildPipelineSettings();
            string loadedBuildPath = settings.buildPath;
            if (string.IsNullOrEmpty(loadedBuildPath))
                return;
            Application.OpenURL(loadedBuildPath);
        }


        [MenuItem("Build/Settings/Android/Open Name Settings...", false, 3)]
        public static void OpenSetting()
        {
            AndroidBuildPipelineSettingsWindow.OpenWindow();
        }
    }
}