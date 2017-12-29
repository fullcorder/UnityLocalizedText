using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using LocalizedText.Importer;
using LocalText.Internal;
using Object = UnityEngine.Object;

namespace LocalizedText.Internal
{
    [CustomEditor(typeof(Settings))]
    public class SettingsInspector : UnityEditor.Editor
    {
        protected ReorderableList reorderableList;

        private SerializedProperty languageSettingInfosProperty;

        private static GUIStyle boldLabelStyle;

        protected static GUIStyle BoldLabelStyle
        {
            get
            {
                if (boldLabelStyle == null)
                {
                    boldLabelStyle = new GUIStyle(EditorStyles.boldLabel)
                    {
                        fontSize = 12
                    };
                }
                return boldLabelStyle;
            }
        }

        protected void OnEnable()
        {
            languageSettingInfosProperty = serializedObject.FindProperty("_languageSettingList");
            reorderableList = new ReorderableList(serializedObject, languageSettingInfosProperty);

            reorderableList.drawHeaderCallback = DrawHeaderCallback;
            reorderableList.drawElementCallback = DrawElementCallback;
            reorderableList.onAddCallback = AddCallBack;
        }

        private void AddCallBack(ReorderableList list)
        {
            var settings = serializedObject.targetObject as Settings;
            settings.LanguageSettingList.Add(new Settings.LanguageSetting());
        }

        public override void OnInspectorGUI()
        {
            var settings = serializedObject.targetObject as Settings;

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            GUILayout.Label("TextSet name", BoldLabelStyle);

            settings.TextSetAssetName = EditorGUILayout.TextField(settings.TextSetAssetName);

            GUILayout.Label("TextSet Genrete Directory", BoldLabelStyle);
            EditorGUILayout.LabelField(settings.TextSetGenerateDirectory);

            var folderForTextSet =
                EditorGUILayout.ObjectField(settings.TextDataGenerateFolder, typeof(Object), false);

            if(folderForTextSet != settings.TextDataGenerateFolder)
            {
                settings.TextDataGenerateFolder = folderForTextSet;
                var assetPath = AssetDatabase.GetAssetPath(folderForTextSet);
                settings.TextSetGenerateDirectory = assetPath;
            }

            GUILayout.Label("Key definition class Name", BoldLabelStyle);

            settings.KeyDefinitionClassName = EditorGUILayout.TextField(settings.KeyDefinitionClassName);

            GUILayout.Label("Key definition Genrete Directory", BoldLabelStyle);

            EditorGUILayout.LabelField(settings.KeyClassGenerateDirectory);

            var folderForKeyFile = EditorGUILayout.ObjectField(settings.KeyClassGenerateFolder,
                typeof(UnityEngine.Object), false);

            if(folderForKeyFile != settings.KeyClassGenerateFolder)
            {
                settings.KeyClassGenerateFolder = folderForKeyFile;
                var assetPath = AssetDatabase.GetAssetPath(settings.KeyClassGenerateFolder);
                settings.KeyClassGenerateDirectory = assetPath;
            }

            EditorGUILayout.Space();

            GUILayout.Label("Language", BoldLabelStyle);
            reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            GUILayout.Label("Data Source", BoldLabelStyle);
            settings.SelectedDataSource = (Settings.DataSource)EditorGUILayout.EnumPopup(settings.SelectedDataSource);

            EditorGUILayout.Space();

            GUILayout.Label("Data Format", BoldLabelStyle);
            settings.SelectedDataFormat = (Settings.DataFormat)EditorGUILayout.EnumPopup(settings.SelectedDataFormat);

            EditorGUILayout.Space();

            GUILayout.Label("Google spread sheet URL");
            settings.GoogleSpreadSheetUrl = EditorGUILayout.TextField(settings.GoogleSpreadSheetUrl);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Create"))
            {
                LocalizedTextLogger.Verbose("Create Button Click");
                GoogleSpreadSheetImporter.SyncGoogleSpreadSheetApi(settings);
            }

            if(GUILayout.Button("Validate"))
            {
                LocalizedTextLogger.Verbose("Validate Button Click");

                Validate(settings);
            }
            EditorGUILayout.EndHorizontal();

            if(EditorGUI.EndChangeCheck())
            {
                LocalizedTextLogger.Verbose("Setting updated. Save Setting");
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
            }
        }

        protected static void Validate(Settings settings)
        {
            if(settings.Valid())
            {
                EditorUtility.DisplayDialog("Validation", "Success", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Validation Error", settings.ValidationErrorMessage()
                    .ToString(), "OK");
            }
        }

        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var settings = serializedObject.targetObject as Settings;

            var langSettingInfo = settings.LanguageSettingList[index];

            var padding = 4;

            var x = rect.x;
            var y = rect.y;

            var uiWidth = 160;
            var drawRect = new Rect(x, y, uiWidth, EditorGUIUtility.singleLineHeight);
            langSettingInfo.Language = (SystemLanguage) EditorGUI.EnumPopup(drawRect, langSettingInfo.Language);

            x += padding;
            x += uiWidth;

            uiWidth = 12;
            drawRect.x = x;
            drawRect.width = uiWidth;

            langSettingInfo.IsDefault = EditorGUI.Toggle(drawRect, langSettingInfo.IsDefault, EditorStyles.radioButton);
            if(langSettingInfo.IsDefault)
            {
                var languageSettingInfos = settings.LanguageSettingList
                    .Where(info => info.Language != langSettingInfo.Language);

                foreach(var info in languageSettingInfos)
                {
                    info.IsDefault = false;
                }
            }

            x += padding;
            x += uiWidth;

            uiWidth = 100;
            drawRect.x = x;
            drawRect.width = uiWidth;
            EditorGUI.LabelField(drawRect, "Default Language");
        }

        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, languageSettingInfosProperty.displayName);
        }
    }
}