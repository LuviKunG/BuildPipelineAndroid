using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace LuviKunG.BuildPipeline.Android
{
    public sealed class AndroidBuildPipelineSettings : ScriptableObject
    {
        // 'Editor/Resources/' are special folders that all assets will be exclude from build.
        // https://docs.unity3d.com/Manual/SpecialFolders.html
        private const string EDITOR_DEFAULT_RESOURCE_PATH = "Assets/Editor/Resources/AndroidBuildPipelineSettings.asset";

        private static AndroidBuildPipelineSettings instance;
        public static AndroidBuildPipelineSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GetEditorDefaultResourceInstance();
                }
                return instance;
            }
        }

        private static AndroidBuildPipelineSettings GetEditorDefaultResourceInstance()
        {
            void CreateFolderIfNotExist(string path)
            {
                var folderPath = path.Substring(0, path.LastIndexOf('/'));
                if (!AssetDatabase.IsValidFolder(folderPath))
                {
                    CreateFolderIfNotExist(folderPath);
                    AssetDatabase.CreateFolder(folderPath.Substring(0, folderPath.LastIndexOf('/')), folderPath.Substring(folderPath.LastIndexOf('/') + 1));
                }
            }
            var asset = AssetDatabase.LoadAssetAtPath<AndroidBuildPipelineSettings>(EDITOR_DEFAULT_RESOURCE_PATH);
            if (asset == null)
            {
                asset = CreateInstance<AndroidBuildPipelineSettings>();
                // recursive function that wil check folders from path that isn't exist and will create folder.
                CreateFolderIfNotExist(EDITOR_DEFAULT_RESOURCE_PATH);
                AssetDatabase.CreateAsset(asset, EDITOR_DEFAULT_RESOURCE_PATH);
                AssetDatabase.SaveAssets();
            }
            return asset;
        }

        private static StringBuilder cachedStringBuilder;

        public string buildPath;
        public string nameFormat;
        public string dateTimeFormat;
        public bool incrementBundle;
        public BuildOptions buildOptions;
        public bool useKeystore;

        public string keystoreName;
        public string keyaliasName;

        [SerializeField]
        private string keystoreEncodedPassword;
        public string KeystorePassword
        {
            get => DecodePassword(keystoreEncodedPassword);
            set => keystoreEncodedPassword = EncodePassword(value);
        }

        [SerializeField]
        public string keyaliasEncodedPassword;
        public string KeyaliasPassword
        {
            get => DecodePassword(keyaliasEncodedPassword);
            set => keyaliasEncodedPassword = EncodePassword(value);
        }

        public AndroidBuildPipelineSettings()
        {
            buildPath = string.Empty;
            nameFormat = "{package}_{date}";
            dateTimeFormat = "yyyyMMddHHmm";
            incrementBundle = true;
            buildOptions = BuildOptions.CompressWithLz4;
            useKeystore = false;
            keystoreName = string.Empty;
            keyaliasName = string.Empty;
            keystoreEncodedPassword = string.Empty;
            keyaliasEncodedPassword = string.Empty;
        }

        public string GetBuildFileName()
        {
            cachedStringBuilder ??= new StringBuilder();
            cachedStringBuilder.Clear();
            cachedStringBuilder.Append(nameFormat);
            cachedStringBuilder.Replace("{name}", Application.productName);
            cachedStringBuilder.Replace("{package}", PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android));
            cachedStringBuilder.Replace("{version}", Application.version);
            cachedStringBuilder.Replace("{bundle}", PlayerSettings.Android.bundleVersionCode.ToString());
            cachedStringBuilder.Replace("{date}", DateTime.Now.ToString(dateTimeFormat));
            cachedStringBuilder.Append(EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk");
            return cachedStringBuilder.ToString();
        }

        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        private string DecodePassword(string encodedPassword)
        {
            if (string.IsNullOrEmpty(encodedPassword))
                return string.Empty;
            var bytes = Convert.FromBase64String(encodedPassword);
            return Encoding.UTF8.GetString(bytes);
        }

        private string EncodePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;
            var bytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(bytes);
        }
    }
}