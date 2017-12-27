using System.IO;
using LocalizedText.Internal;
using LocalText.Internal;
using UnityEditor;
using UnityEngine;

namespace LocalizedText.Editor.Internal
{
    public static class CreateSingletonSettingsMenu
    {
        [MenuItem("Assets/Create/Localized Text/CreateAll Singleton Settings", false, 800)]
        public static void MenuItem()
        {
            var settings = AssetDataBaseHelper.FindScriptableObject<SingletonSettings>("LocalizedTextSettings");
            if(settings)
            {
                return;
            }

            var assetRootPath = AssetDataBaseHelper.FirstAssetPathOrDefault("t:Object", "LocalizedText");
            if(string.IsNullOrEmpty(assetRootPath))
            {
                Debug.LogError("LocalizedText directory not found");
                return;
            }

            LocalizedTextLogger.Log(LocalizedTextLogger.LogLevel.Debug,
                "CreateSingletonSettingsMenu assetRootPath " + assetRootPath);

            var singletonSettings = ScriptableObject.CreateInstance<SingletonSettings>();
            singletonSettings.name = "LocalizedTextSettings";
            singletonSettings.TextSetAssetName = "TextSet";
            singletonSettings.TextSetGenerateDirectory = Path.Combine(assetRootPath, "Resources");
            singletonSettings.KeyDefinitionClassName = "TextSetKey";
            singletonSettings.KeyClassGenerateDirectory = Path.Combine(assetRootPath, "Scripts/Generated");
            singletonSettings.SelectedDataSource = Settings.DataSource.GoogleSpreadSheetAsWeb;
            singletonSettings.SelectedDataFormat = Settings.DataFormat.Tsv;

            AssetDatabase.CreateAsset(singletonSettings, Path.Combine(assetRootPath,
                "Editor/Settings/LocalizedTextSettings.asset"));
            EditorUtility.SetDirty(singletonSettings);
            AssetDatabase.SaveAssets();
        }
    }
}