using System.IO;
using System.Runtime.Serialization;
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
                        var nameFormat = EditorGUILayout.TextField("File name formatting", settings.nameFormat);
                        if (changeScope.changed)
                        {
                            settings.nameFormat = nameFormat;
                            settings.Save();
                            Repaint();
                        }
                    }
                    EditorGUILayout.HelpBox(HELPBOX_NAME_FORMATTING_INFO, MessageType.Info, true);
                    EditorGUILayout.LabelField("Formatted name", settings.GetBuildFileName(), EditorStyles.helpBox);
                    GUILayout.Space(16.0f);
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        var dateTimeFormat = EditorGUILayout.TextField("Date time format", settings.dateTimeFormat);
                        if (changeScope.changed)
                        {
                            settings.dateTimeFormat = dateTimeFormat;
                            settings.Save();
                            Repaint();
                        }
                    }
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        var buildOptions = (BuildOptions)EditorGUILayout.EnumFlagsField("Build options", settings.buildOptions);
                        if (changeScope.changed)
                        {
                            settings.buildOptions = buildOptions;
                            settings.Save();
                            Repaint();
                        }
                    }
                    using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                    {
                        using (var changeScope = new EditorGUI.ChangeCheckScope())
                        {
                            var incrementBundle = EditorGUILayout.Toggle("Increase Bundle Version", settings.incrementBundle);
                            if (changeScope.changed)
                            {
                                settings.incrementBundle = incrementBundle;
                                settings.Save();
                                Repaint();
                            }
                        }
                        GUILayout.FlexibleSpace();
                        if (settings.incrementBundle)
                        {
                            EditorGUILayout.LabelField($"Next build app bundle will be version {PlayerSettings.Android.bundleVersionCode + 1}", EditorStyles.helpBox, GUILayout.ExpandWidth(false));
                        }
                    }
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        bool cacheEnable = GUI.enabled;
                        GUI.enabled = !settings.incrementBundle;
                        var bundleVersion = EditorGUILayout.IntField("Current Bundle Version", PlayerSettings.Android.bundleVersionCode);
                        GUI.enabled = cacheEnable;
                        if (changeScope.changed)
                        {
                            if (bundleVersion < 1)
                                bundleVersion = 1;
                            PlayerSettings.Android.bundleVersionCode = bundleVersion;
                            Repaint();
                        }
                    }
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        var isBuildAppBundle = EditorGUILayout.Toggle("Build App Bundle for Google Play", EditorUserBuildSettings.buildAppBundle);
                        if (changeScope.changed)
                        {
                            EditorUserBuildSettings.buildAppBundle = isBuildAppBundle;
                            Repaint();
                        }
                    }
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        var isSplitAppBinary = EditorGUILayout.Toggle("Split Application Binary", PlayerSettings.Android.useAPKExpansionFiles);
                        if (changeScope.changed)
                        {
                            PlayerSettings.Android.useAPKExpansionFiles = isSplitAppBinary;
                            Repaint();
                        }
                    }
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        var createSymbols = (AndroidCreateSymbols)EditorGUILayout.EnumPopup("Create Symbols", EditorUserBuildSettings.androidCreateSymbols);
                        if (changeScope.changed)
                        {
                            EditorUserBuildSettings.androidCreateSymbols = createSymbols;
                            Repaint();
                        }
                    }
                    using (var verticalScope = new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        using (var changeScope = new EditorGUI.ChangeCheckScope())
                        {
                            var useKeystore = GUILayout.Toggle(settings.useKeystore, "Use Keystore", EditorStyles.miniButton);
                            if (changeScope.changed)
                            {
                                settings.useKeystore = useKeystore;
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
                                if (GUILayout.Button("Retrive", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                                {
                                    settings.keystoreName = PlayerSettings.Android.keystoreName;
                                    settings.Save();
                                    Repaint();
                                }
                                if (GUILayout.Button("Change", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
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
                                var keystorePass = EditorGUILayout.PasswordField("Keystore Password", settings.KeystorePassword);
                                if (changeScope.changed)
                                {
                                    settings.KeystorePassword = keystorePass;
                                    settings.Save();
                                    Repaint();
                                }
                            }
                            using (var changeScope = new EditorGUI.ChangeCheckScope())
                            {
                                var keyaliasName = EditorGUILayout.TextField("Keyalias Name", settings.keyaliasName);
                                if (changeScope.changed)
                                {
                                    settings.keyaliasName = keyaliasName;
                                    settings.Save();
                                    Repaint();
                                }
                            }
                            using (var changeScope = new EditorGUI.ChangeCheckScope())
                            {
                                var keyaliasPass = EditorGUILayout.PasswordField("Keyalias Password", settings.KeyaliasPassword);
                                if (changeScope.changed)
                                {
                                    settings.KeyaliasPassword = keyaliasPass;
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
                                if (GUILayout.Button("Clear Keystore Information", GUILayout.MaxWidth(250.0f), GUILayout.Height(30.0f)))
                                {
                                    settings.keystoreName = string.Empty;
                                    settings.KeystorePassword = string.Empty;
                                    settings.keyaliasName = string.Empty;
                                    settings.KeyaliasPassword = string.Empty;
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
                        if (GUILayout.Button("Change", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
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
                        if (GUILayout.Button("Open Build Location", GUILayout.MaxWidth(250.0f), GUILayout.Height(30.0f)))
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
            return newPath;
        }
    }
}