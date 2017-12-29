using System.Linq;
using LocalizedText.Editor.Internal;
using LocalizedText.Importer;
using LocalText.Internal;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LocalizedText.Internal
{
    [CustomEditor(typeof(SingletonSettings))]
    public class SingletonSettingsInspector : SettingsInspector
    {
        public override void OnInspectorGUI()
        {
            var settings = serializedObject.targetObject as Settings;

            serializedObject.Update();

            GUILayout.Label("Asset Name", BoldLabelStyle);
            EditorGUILayout.LabelField(settings.TextSetAssetName);

            GUILayout.Label("Instance Type", BoldLabelStyle);
            EditorGUILayout.LabelField("Singleton (Use Resources folder)");

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            GUILayout.Label("Language", BoldLabelStyle);
            reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();

            GUILayout.Label("Genrete TextSet Path", BoldLabelStyle);
            EditorGUILayout.LabelField(settings.TextSetAssetPath());

            GUILayout.Label("Genrete KeyFile Path", BoldLabelStyle);
            EditorGUILayout.LabelField(settings.KeyAssetPath());

            EditorGUILayout.Space();

            GUILayout.Label("Data Source", BoldLabelStyle);
            EditorGUILayout.LabelField(settings.SelectedDataSource.ToString());

            EditorGUILayout.Space();

            if(settings.SelectedDataSource == Settings.DataSource.GoogleSpreadSheetAsWeb)
            {
                GUILayout.Label("Google spread sheet URL");
                settings.GoogleSpreadSheetUrl = EditorGUILayout.TextField(settings.GoogleSpreadSheetUrl);

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                if(GUILayout.Button("CreateAll or Update"))
                {
                    LocalizedTextLogger.Verbose("CreateAll Button Click");
                    GoogleSpreadSheetImporter.SyncGoogleSpreadSheetApi(settings);
                }
                EditorGUILayout.EndHorizontal();
            }

            if(EditorGUI.EndChangeCheck())
            {
                LocalizedTextLogger.Verbose("Save Setting Asset");
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
            }
        }
    }
}