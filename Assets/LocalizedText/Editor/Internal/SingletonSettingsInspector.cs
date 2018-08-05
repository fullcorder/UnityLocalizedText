using LocalizedText.Importer;
using LocalizedText.Internal;
using UnityEditor;
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

            //AssetName
            GUILayout.Label(Constant.Inspactor.TextSetName, BoldLabelStyle);
            EditorGUILayout.LabelField(settings.TextSetAssetName);

            GUILayout.Label("Instance Type", BoldLabelStyle);
            EditorGUILayout.LabelField("Singleton (Use Resources folder)");

            GUILayout.Label("Generate TextSet Path", BoldLabelStyle);
            EditorGUILayout.LabelField(settings.TextSetAssetPath());

            GUILayout.Label("Genrete KeyFile Path", BoldLabelStyle);
            EditorGUILayout.LabelField(settings.KeyAssetPath());

            Separator();

            EditorGUI.BeginChangeCheck();

            //Language
            GUILayout.Label(Constant.Inspactor.Language, BoldLabelStyle);
            reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();

            //Data
            GUILayout.Label(Constant.Inspactor.DataSouce, BoldLabelStyle);
            EditorGUILayout.LabelField(settings.SelectedDataSource.ToString());

            GUILayout.Label(Constant.Inspactor.SpreadSheetURL, BoldLabelStyle);
            settings.GoogleSpreadSheetUrl = EditorGUILayout.TextField(settings.GoogleSpreadSheetUrl);

            Separator();

            //Button
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button(Constant.Inspactor.ButtonGenerate))
            {
                LocalTextLogger.Verbose("Create Button Click");
                GoogleSpreadSheetImporter.SyncGoogleSpreadSheetApi(settings);
            }

            if(GUILayout.Button(Constant.Inspactor.ButtonValidate))
            {
                LocalTextLogger.Verbose("Validate Button Click");

                Validate(settings);
            }
            EditorGUILayout.EndHorizontal();

            if(EditorGUI.EndChangeCheck())
            {
                LocalTextLogger.Verbose("Setting updated. Save Setting");
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
            }
        }
    }
}