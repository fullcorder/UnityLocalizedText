using System;
using System.IO;
using System.Linq;
using LocalizedText.Internal;
using LocalText.Internal;
using UnityEditor;
using UnityEngine;

namespace LocalizedText.Editor.Internal
{
    public static class CreateSingletonSettingsMenu
    {
        [MenuItem(Constant.MenuName.MenuBarItemName, false, 800)]
        public static void MenuItem()
        {
            var assetRootPath = AssetDataBaseHelper.FirstAssetPathOrDefault("t:Object", "LocalizedText");

            if(string.IsNullOrEmpty(assetRootPath))
            {
                LocalizedTextLogger.Error("LocalizedText directory not found. " +
                                          "LocalizedText assets must be placed under LocalizedText directory.");
                return;
            }

            LocalizedTextLogger.VerboseFormat("CreateSingletonSettingsMenu assetRootPath {0}", assetRootPath);

            var singletonSettings = ScriptableObject.CreateInstance<SingletonSettings>();
            singletonSettings.name = "LocalizedTextSettings";
            singletonSettings.TextSetAssetName = "TextSet";

            var resourcesDirectory = Path.Combine(assetRootPath, "Resources");
            if(!Directory.Exists(resourcesDirectory)) AssetDatabase.CreateFolder(assetRootPath, "Resources");
            singletonSettings.TextSetGenerateDirectory = resourcesDirectory;

            singletonSettings.KeyDefinitionClassName = "TextSetKey";

            const string scripts = "Scripts";
            const string generated = "Generated";

            var generatedDirectory = new[] {assetRootPath, scripts, generated}.Aggregate(Path.Combine);

            if(!Directory.Exists(generatedDirectory))
            {
                AssetDatabase.CreateFolder(Path.Combine(assetRootPath, scripts), generated);
            }
            singletonSettings.KeyClassGenerateDirectory = generatedDirectory;

            singletonSettings.SelectedDataSource = Settings.DataSource.GoogleSpreadSheetAsWeb;
            singletonSettings.SelectedDataFormat = Settings.DataFormat.Tsv;

            const string editor = "Editor";
            const string settings = "Settings";

            var settingsDirectory = new[] {assetRootPath, editor, settings}.Aggregate(Path.Combine);

            if(!Directory.Exists(settingsDirectory))
            {
                AssetDatabase.CreateFolder(Path.Combine(assetRootPath, editor), settings);
            }

            var settingsPath = Path.Combine(settingsDirectory, "LocalizedTextSettings.asset");
            AssetDatabase.CreateAsset(singletonSettings, settingsPath);

            EditorUtility.SetDirty(singletonSettings);
            AssetDatabase.SaveAssets();
        }
    }
}