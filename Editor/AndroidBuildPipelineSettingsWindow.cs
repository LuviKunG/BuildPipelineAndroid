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

        private AndroidBuildPipelineSettings format;

        public static AndroidBuildPipelineSettingsWindow OpenWindow()
        {
            var window = GetWindow<AndroidBuildPipelineSettingsWindow>(true, "Android Build Pipeline Setting", true);
            window.Show();
            return window;
        }

        private void OnEnable()
        {
            format = new AndroidBuildPipelineSettings();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("File name formatting", EditorStyles.boldLabel);
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                format.nameFormat = EditorGUILayout.TextField(format.nameFormat);
                if (changeScope.changed)
                    format.Save();
            }
            EditorGUILayout.HelpBox(HELPBOX_NAME_FORMATTING_INFO, MessageType.Info, true);
            EditorGUILayout.LabelField("Formatted name", format.GetFileName(), EditorStyles.helpBox);
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                format.dateTimeFormat = EditorGUILayout.TextField("Date time format", format.dateTimeFormat);
                format.incrementBundle = EditorGUILayout.Toggle("Increase Bundle Version", format.incrementBundle);
                format.useKeystore = EditorGUILayout.Toggle("Use Keystore", format.useKeystore);
                if (changeScope.changed)
                    format.Save();
            }
            if (format.useKeystore)
            {
                using (var verticalScope = new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.HelpBox(HELPBOX_USE_KEYSTORE_INFO, MessageType.Warning, true);
                    using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField("Keystore Name", format.keystoreName);
                        if (GUILayout.Button("Change...", GUILayout.Width(96.0f)))
                        {
                            var path = EditorUtility.OpenFilePanelWithFilters("Choose Location of Build Game", string.IsNullOrEmpty(format.keystoreName) ? string.Empty : format.keystoreName, KEYSTORE_EXTENSIONS);
                            if (string.IsNullOrEmpty(path))
                                return;
                            format.keystoreName = path;
                            format.Save();
                        }
                    }
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        format.keystorePass = EditorGUILayout.PasswordField("Keystore Password", format.keystorePass);
                        format.keyaliasName = EditorGUILayout.TextField("Keyalias Name", format.keyaliasName);
                        format.keyaliasPass = EditorGUILayout.PasswordField("Keyalias Password", format.keyaliasPass);
                        if (changeScope.changed)
                            format.Save();
                    }
                    GUILayout.Space(16.0f);
                    using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();
                        Color cacheColor = GUI.color;
                        GUI.color = Color.red;
                        if (GUILayout.Button("Clear Keystore Settings", GUILayout.MaxWidth(256.0f)))
                        {
                            format.keystoreName = string.Empty;
                            format.keystorePass = string.Empty;
                            format.keyaliasName = string.Empty;
                            format.keyaliasPass = string.Empty;
                            format.Save();
                        }
                        GUI.color = cacheColor;
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.Space(16.0f);
                }
            }
        }
    }
}