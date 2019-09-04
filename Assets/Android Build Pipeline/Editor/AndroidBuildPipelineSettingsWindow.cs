using UnityEditor;
using UnityEngine;

namespace LuviKunG.BuildPipeline.Android
{
    public sealed class AndroidBuildPipelineSettingsWindow : EditorWindow
    {
        private const string HELPBOX_NAME_FORMATTING_INFO = @"How to format the file name.

{name} = App Name.
{package} = Android Package Name.
{version} = App Version.
{bundle} = App Bundle.
{date} = Date time. (format)";
        private const string HELPBOX_USE_KEYSTORE_INFO = "Note that this method will not safety! Just for helpful when doing fast build.";

        private static readonly string[] KEYSTORE_EXTENSIONS = new string[4] { "Keystore files", "jks,keystore", "All files", "*" };

        private AndroidBuildPipelineSettings settings;

        public static AndroidBuildPipelineSettingsWindow OpenWindow()
        {
            var window = GetWindow<AndroidBuildPipelineSettingsWindow>(true, "Android Build Pipeline Setting", true);
            window.Show();
            return window;
        }

        private void OnEnable()
        {
            settings = AndroidBuildPipelineSettings.Instance;
        }

        private void OnGUI()
        {
            GUI.enabled = !UnityEditor.BuildPipeline.isBuildingPlayer;
            EditorGUILayout.LabelField("File name formatting", EditorStyles.boldLabel);
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                settings.nameFormat = EditorGUILayout.TextField(settings.nameFormat);
                if (changeScope.changed)
                    settings.Save();
            }
            EditorGUILayout.HelpBox(HELPBOX_NAME_FORMATTING_INFO, MessageType.Info, true);
            EditorGUILayout.LabelField("Formatted name", settings.GetFileName(), EditorStyles.helpBox);
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                settings.dateTimeFormat = EditorGUILayout.TextField("Date time format", settings.dateTimeFormat);
                settings.incrementBundle = EditorGUILayout.Toggle("Increase Bundle Version", settings.incrementBundle);
                settings.useKeystore = EditorGUILayout.Toggle("Use Keystore", settings.useKeystore);
                if (changeScope.changed)
                    settings.Save();
            }
            if (settings.useKeystore)
            {
                using (var verticalScope = new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.HelpBox(HELPBOX_USE_KEYSTORE_INFO, MessageType.Warning, true);
                    using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField("Keystore Name", settings.keystoreName);
                        if (GUILayout.Button("Change...", GUILayout.Width(96.0f)))
                        {
                            var path = EditorUtility.OpenFilePanelWithFilters("Choose Location of Build Game", string.IsNullOrEmpty(settings.keystoreName) ? string.Empty : settings.keystoreName, KEYSTORE_EXTENSIONS);
                            if (string.IsNullOrEmpty(path))
                                return;
                            settings.keystoreName = path;
                            settings.Save();
                        }
                    }
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        settings.keystorePass = EditorGUILayout.PasswordField("Keystore Password", settings.keystorePass);
                        settings.keyaliasName = EditorGUILayout.TextField("Keyalias Name", settings.keyaliasName);
                        settings.keyaliasPass = EditorGUILayout.PasswordField("Keyalias Password", settings.keyaliasPass);
                        if (changeScope.changed)
                            settings.Save();
                    }
                    GUILayout.Space(16.0f);
                    using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();
                        Color cacheColor = GUI.color;
                        GUI.color = Color.red;
                        if (GUILayout.Button("Clear Keystore Settings", GUILayout.MaxWidth(256.0f)))
                        {
                            settings.keystoreName = string.Empty;
                            settings.keystorePass = string.Empty;
                            settings.keyaliasName = string.Empty;
                            settings.keyaliasPass = string.Empty;
                            settings.Save();
                        }
                        GUI.color = cacheColor;
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.Space(16.0f);
                }
            }
            using (var verticalScope = new EditorGUILayout.VerticalScope())
            {
                using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Build location", settings.buildPath);
                    if (GUILayout.Button("Change...", GUILayout.Width(96.0f)))
                    {
                        var path = OpenBuildSavePanel(settings.buildPath);
                        if (!string.IsNullOrEmpty(path))
                            settings.buildPath = path;
                    }
                }
                using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    bool cacheEnable = GUI.enabled;
                    GUI.enabled = !string.IsNullOrWhiteSpace(settings.buildPath);
                    if (GUILayout.Button("Open Build Location", GUILayout.MaxWidth(256.0f)))
                    {
                        Application.OpenURL(settings.buildPath);
                    }
                    GUI.enabled = cacheEnable;
                    GUILayout.FlexibleSpace();
                }
            }
        }

        private string OpenBuildSavePanel(string path)
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