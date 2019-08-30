using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace LuviKunG.BuildPipeline.Android
{
    public sealed class AndroidBuildPipelineSettings
    {
        private const string ALIAS = "unity.editor.luvikung.buildpipeline.android.";
        private static readonly string PREFS_SETTINGS_BUILD_PATH = ALIAS + "buildpath";
        private static readonly string PREFS_SETTINGS_NAME_FORMAT = ALIAS + "nameformat";
        private static readonly string PREFS_SETTINGS_DATE_TIME_FORMAT = ALIAS + "datetimeformat";
        private static readonly string PREFS_SETTINGS_INCREMENT_BUNDLE = ALIAS + "incrementbundle";
        private static readonly string PREFS_SETTINGS_USE_KEYSTORE = ALIAS + "usekeystore";
        private static readonly string PREFS_SETTINGS_KEYSTORE_NAME = ALIAS + "keystorename";
        private static readonly string PREFS_SETTINGS_KEYSTORE_PASS = ALIAS + "keystorepass";
        private static readonly string PREFS_SETTINGS_KEYALIAS_NAME = ALIAS + "keyaliasname";
        private static readonly string PREFS_SETTINGS_KEYALIAS_PASS = ALIAS + "keyaliaspass";

        public string buildPath;
        public string nameFormat;
        public string dateTimeFormat;
        public bool incrementBundle;
        public bool useKeystore;

        public string keystoreName;
        public string keystorePass;
        public string keyaliasName;
        public string keyaliasPass;

        public AndroidBuildPipelineSettings()
        {
            Load();
        }

        public void Load()
        {
            buildPath = PlayerPrefs.GetString(PREFS_SETTINGS_BUILD_PATH, EditorUserBuildSettings.GetBuildLocation(BuildTarget.Android));
            nameFormat = PlayerPrefs.GetString(PREFS_SETTINGS_NAME_FORMAT, "{package}_{date}");
            dateTimeFormat = PlayerPrefs.GetString(PREFS_SETTINGS_DATE_TIME_FORMAT, "yyyyMMddHHmmss");
            incrementBundle = PlayerPrefs.GetString(PREFS_SETTINGS_INCREMENT_BUNDLE, bool.FalseString) == bool.TrueString;
            useKeystore = PlayerPrefs.GetString(PREFS_SETTINGS_USE_KEYSTORE, bool.FalseString) == bool.TrueString;

            keystoreName = PlayerPrefs.GetString(PREFS_SETTINGS_KEYSTORE_NAME, PlayerSettings.Android.keystoreName);

            keyaliasName = PlayerPrefs.GetString(PREFS_SETTINGS_KEYALIAS_NAME, string.Empty);

            var keystorePassEncoded = PlayerPrefs.GetString(PREFS_SETTINGS_KEYSTORE_PASS, string.Empty);
            if (!string.IsNullOrEmpty(keystorePassEncoded))
                keystorePass = Encoding.ASCII.GetString(Convert.FromBase64String(keystorePassEncoded));

            var keyaliasPassEncoded = PlayerPrefs.GetString(PREFS_SETTINGS_KEYALIAS_PASS, string.Empty);
            if (!string.IsNullOrEmpty(keyaliasPassEncoded))
                keyaliasPass = Encoding.ASCII.GetString(Convert.FromBase64String(keyaliasPassEncoded));
        }

        public void Save()
        {
            PlayerPrefs.SetString(PREFS_SETTINGS_NAME_FORMAT, nameFormat);
            PlayerPrefs.SetString(PREFS_SETTINGS_DATE_TIME_FORMAT, dateTimeFormat);
            PlayerPrefs.SetString(PREFS_SETTINGS_INCREMENT_BUNDLE, incrementBundle ? bool.TrueString : bool.FalseString);
            PlayerPrefs.SetString(PREFS_SETTINGS_USE_KEYSTORE, useKeystore ? bool.TrueString : bool.FalseString);

            if (!string.IsNullOrEmpty(keystoreName))
                PlayerPrefs.SetString(PREFS_SETTINGS_KEYSTORE_NAME, keystoreName);
            else if (PlayerPrefs.HasKey(PREFS_SETTINGS_KEYSTORE_NAME))
                PlayerPrefs.DeleteKey(PREFS_SETTINGS_KEYSTORE_NAME);

            if (!string.IsNullOrEmpty(keyaliasName))
                PlayerPrefs.SetString(PREFS_SETTINGS_KEYALIAS_NAME, keyaliasName);
            else if (PlayerPrefs.HasKey(PREFS_SETTINGS_KEYALIAS_NAME))
                PlayerPrefs.DeleteKey(PREFS_SETTINGS_KEYALIAS_NAME);

            if (!string.IsNullOrEmpty(keystorePass))
            {
                var keystorePassEncoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(keystorePass));
                PlayerPrefs.SetString(PREFS_SETTINGS_KEYSTORE_PASS, keystorePassEncoded);
            }
            else if (PlayerPrefs.HasKey(PREFS_SETTINGS_KEYSTORE_PASS))
                PlayerPrefs.DeleteKey(PREFS_SETTINGS_KEYSTORE_PASS);

            if (!string.IsNullOrEmpty(keyaliasPass))
            {
                var keyaliasPassEncoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(keyaliasPass));
                PlayerPrefs.SetString(PREFS_SETTINGS_KEYALIAS_PASS, keyaliasPassEncoded);
            }
            else if (PlayerPrefs.HasKey(PREFS_SETTINGS_KEYALIAS_PASS))
                PlayerPrefs.DeleteKey(PREFS_SETTINGS_KEYALIAS_PASS);
        }

        public string GetFileName()
        {
            StringBuilder s = new StringBuilder();
            s.Append(nameFormat);
            s.Replace("{name}", Application.productName);
            s.Replace("{package}", PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android));
            s.Replace("{version}", Application.version);
            s.Replace("{bundle}", PlayerSettings.Android.bundleVersionCode.ToString());
            s.Replace("{date}", DateTime.Now.ToString(dateTimeFormat));
            s.Append(".apk");
            return s.ToString();
        }
    }
}