using System.Linq;
using LocalizedText.Importer;
using LocalizedText.Internal;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

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
                if(boldLabelStyle == null)
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
            var languageSetting = new Settings.LanguageSetting();
            if(settings.LanguageSettingList.Count == 0)
            {
                languageSetting.IsDefault = true;
            }
            settings.LanguageSettingList.Add(languageSetting);
        }

        public override void OnInspectorGUI()
        {
            var settings = serializedObject.targetObject as Settings;

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            //Asset Name
            GUILayout.Label(Constant.Inspactor.TextSetName, BoldLabelStyle);
            settings.TextSetAssetName = EditorGUILayout.TextField(settings.TextSetAssetName);

            GUILayout.Label(Constant.Inspactor.TextSetDir, BoldLabelStyle);
            EditorGUILayout.LabelField(string.IsNullOrEmpty(settings.TextSetGenerateDirectory)
                ? Constant.Inspactor.FolderNotSelected
                : settings.TextSetGenerateDirectory);

            var folderForTextSet = EditorGUILayout.ObjectField(Constant.Inspactor.Folder,
                settings.TextDataGenerateFolder, typeof(Object), false);

            if(folderForTextSet != settings.TextDataGenerateFolder)
            {
                settings.TextDataGenerateFolder = folderForTextSet;
                var assetPath = AssetDatabase.GetAssetPath(folderForTextSet);
                settings.TextSetGenerateDirectory = assetPath;
            }

            Separator();

            //Key Class Name
            GUILayout.Label(Constant.Inspactor.KeyClass, BoldLabelStyle);
            settings.KeyDefinitionClassName = EditorGUILayout.TextField(settings.KeyDefinitionClassName);

            GUILayout.Label(Constant.Inspactor.KeyClassDir, BoldLabelStyle);
            EditorGUILayout.LabelField(string.IsNullOrEmpty(settings.KeyClassGenerateDirectory)
                ? Constant.Inspactor.KeyClassDir
                : settings.KeyClassGenerateDirectory);

            var folderForKeyFile = EditorGUILayout.ObjectField(Constant.Inspactor.Folder,
                settings.KeyClassGenerateFolder, typeof(Object), false);

            if(folderForKeyFile != settings.KeyClassGenerateFolder)
            {
                settings.KeyClassGenerateFolder = folderForKeyFile;
                var assetPath = AssetDatabase.GetAssetPath(settings.KeyClassGenerateFolder);
                settings.KeyClassGenerateDirectory = assetPath;
            }

            Separator();

            //Language List
            GUILayout.Label(Constant.Inspactor.Language, BoldLabelStyle);
            reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();

            Separator();

            //Data
            GUILayout.Label(Constant.Inspactor.DataSouce, BoldLabelStyle);
            settings.SelectedDataSource = (Settings.DataSource) EditorGUILayout.EnumPopup(settings.SelectedDataSource);

            GUILayout.Label(Constant.Inspactor.DataFormat, BoldLabelStyle);
            settings.SelectedDataFormat = (Settings.DataFormat) EditorGUILayout.EnumPopup(settings.SelectedDataFormat);

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

        protected static void Separator()
        {
            GUILayoutUtility.GetRect(6f, 18f);
        }

        protected static void Validate(Settings settings)
        {
            if(settings.Valid())
            {
                EditorUtility.DisplayDialog("Validation", "Success", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Validation Error", settings.ValidationErrorMessage().ToString(), "OK");
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
            EditorGUI.LabelField(drawRect, Constant.Inspactor.DefaultLanguage);
        }

        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, languageSettingInfosProperty.displayName);
        }
    }
}