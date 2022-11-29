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
        private const string HELPBOX_USE_KEYSTORE_INFO = "Please note that all keystore information will be stored which include the keystore password. When you're finish your fast-build process, please remove your keystore information from the device by pressing the clear keystore information button below.";

        private static readonly string[] KEYSTORE_EXTENSIONS = new string[4] { "Keystore files", "jks,keystore", "All files", "*" };

        private Vector2 m_scrollViewPosition;
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
            EditorGUIUtility.labelWidth = 200.0f;
            using (new EditorGUI.DisabledGroupScope(UnityEditor.BuildPipeline.isBuildingPlayer))
            {
                using (var scrollView = new GUILayout.ScrollViewScope(m_scrollViewPosition))
                {
                    m_scrollViewPosition = scrollView.scrollPosition;
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        settings.nameFormat = EditorGUILayout.TextField("File name formatting", settings.nameFormat);
                        if (changeScope.changed)
                        {
                            settings.Save();
                            Repaint();
                        }
                    }
                    EditorGUILayout.HelpBox(HELPBOX_NAME_FORMATTING_INFO, MessageType.Info, true);
                    EditorGUILayout.LabelField("Formatted name", settings.GetFileName(), EditorStyles.helpBox);
                    GUILayout.Space(16.0f);
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        settings.dateTimeFormat = EditorGUILayout.TextField("Date time format", settings.dateTimeFormat);
                        if (changeScope.changed)
                        {
                            settings.Save();
                            Repaint();
                        }
                    }
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        settings.buildOptions = (BuildOptions)EditorGUILayout.EnumFlagsField("Build options", settings.buildOptions);
                        if (changeScope.changed)
                        {
                            settings.Save();
                            Repaint();
                        }
                    }
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        settings.incrementBundle = EditorGUILayout.Toggle("Increase Bundle Version", settings.incrementBundle);
                        if (changeScope.changed)
                        {
                            settings.Save();
                            Repaint();
                        }
                    }
#if BUILD_PIPELINE_ANDROID_ENABLE_BUNDLE_VERSION_CONFIG
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        PlayerSettings.Android.bundleVersionCode = EditorGUILayout.IntField("Current Bundle Version", PlayerSettings.Android.bundleVersionCode);
                    }
#endif
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        settings.isBuildAppBundle = EditorGUILayout.Toggle("Build App Bundle for Google Play", settings.isBuildAppBundle);
                        settings.isSplitAppBinary = EditorGUILayout.Toggle("Split Application Binary", settings.isSplitAppBinary);
                        if (changeScope.changed)
                        {
                            EditorUserBuildSettings.buildAppBundle = settings.isBuildAppBundle;
                            PlayerSettings.Android.useAPKExpansionFiles = settings.isSplitAppBinary;
                            settings.Save();
                            Repaint();
                        }
                    }
                    using (var verticalScope = new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        using (var changeScope = new EditorGUI.ChangeCheckScope())
                        {
                            settings.useKeystore = GUILayout.Toggle(settings.useKeystore, "Use Keystore", EditorStyles.miniButton);
                            if (changeScope.changed)
                            {
                                settings.Save();
                                Repaint();
                            }
                        }
                        if (settings.useKeystore)
                        {
                            EditorGUILayout.HelpBox(HELPBOX_USE_KEYSTORE_INFO, MessageType.Warning, true);
                            using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.LabelField("Keystore Name", settings.keystoreName);
                                if (GUILayout.Button("Change...", GUILayout.Width(80.0f)))
                                {
                                    var path = EditorUtility.OpenFilePanelWithFilters("Choose Location of Build Game", string.IsNullOrEmpty(settings.keystoreName) ? string.Empty : settings.keystoreName, KEYSTORE_EXTENSIONS);
                                    if (string.IsNullOrEmpty(path))
                                        return;
                                    settings.keystoreName = path;
                                    settings.Save();
                                    Repaint();
                                }
                            }
                            using (var changeScope = new EditorGUI.ChangeCheckScope())
                            {
                                settings.keystorePass = EditorGUILayout.PasswordField("Keystore Password", settings.keystorePass);
                                settings.keyaliasName = EditorGUILayout.TextField("Keyalias Name", settings.keyaliasName);
                                settings.keyaliasPass = EditorGUILayout.PasswordField("Keyalias Password", settings.keyaliasPass);
                                if (changeScope.changed)
                                {
                                    settings.Save();
                                    Repaint();
                                }
                            }
                            GUILayout.Space(16.0f);
                            using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                            {
                                GUILayout.FlexibleSpace();
                                Color cacheColor = GUI.color;
                                GUI.color = Color.red;
                                if (GUILayout.Button("Clear Keystore Information", GUILayout.MaxWidth(256.0f)))
                                {
                                    settings.keystoreName = string.Empty;
                                    settings.keystorePass = string.Empty;
                                    settings.keyaliasName = string.Empty;
                                    settings.keyaliasPass = string.Empty;
                                    settings.Save();
                                    Repaint();
                                }
                                GUI.color = cacheColor;
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.Space(16.0f);
                        }
                    }
                    using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField("Build location", settings.buildPath);
                        if (GUILayout.Button("Change...", GUILayout.Width(80.0f)))
                        {
                            var path = OpenBuildSavePanel(settings.buildPath);
                            if (!string.IsNullOrEmpty(path))
                                settings.buildPath = path;
                            settings.Save();
                            Repaint();
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