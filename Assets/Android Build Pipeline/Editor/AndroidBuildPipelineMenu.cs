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

        [MenuItem("Build/Settings/Android", false, 0)]
        public static void OpenBuildSetting()
        {
            AndroidBuildPipelineSettingsWindow.OpenWindow();
        }

        [MenuItem("Build/Android", true, 10)]
        public static bool ValidateBuildAndroid()
        {
            return !EditorApplication.isCompiling && !EditorApplication.isUpdating && !UnityEditor.BuildPipeline.isBuildingPlayer && !EditorApplication.isPlaying;
        }

        [MenuItem("Build/Android", false, 10)]
        public static void Build()
        {
            string OpenBuildSavePanel(string path)
            {
                string newPath = EditorUtility.SaveFolderPanel("Choose Location of Build Game", path, null);
                if (string.IsNullOrEmpty(newPath))
                    return null;
                settings.buildPath = newPath;
                settings.Save();
                return newPath;
            }
            if (!ValidateBuildAndroid())
            {
                EditorUtility.DisplayDialog("Build Pipeline Android", "Cannot build because the editor is busy.", "OK");
                return;
            }
            settings ??= AndroidBuildPipelineSettings.Instance;
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
                PlayerSettings.Android.keystorePass = settings.KeystorePassword;
                PlayerSettings.Android.keyaliasName = settings.keyaliasName;
                PlayerSettings.Android.keyaliasPass = settings.KeyaliasPassword;
            }
            string fileName = settings.GetBuildFileName();
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
    }
}