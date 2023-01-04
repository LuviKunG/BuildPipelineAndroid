using UnityEditor;
using UnityEngine;

namespace LuviKunG.BuildPipeline.Android
{
    [CustomEditor(typeof(AndroidBuildPipelineSettings))]
    public sealed class AndroidBuildPipelineSettingsDrawer : Editor
    {
        public override bool UseDefaultMargins() => false;

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Android Build Pipeline Settings", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
                AndroidBuildPipelineSettingsWindow.OpenWindow();
        }
    }
}