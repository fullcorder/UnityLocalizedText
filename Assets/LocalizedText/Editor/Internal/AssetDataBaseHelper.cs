using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LocalizedText.Internal
{
    public static class AssetDataBaseHelper
    {
        public static string FirstAssetPathOrDefault(string filterType, string assetFileName)
        {
            var assetPath = AssetDatabase.FindAssets(filterType)
                .Select(AssetDatabase.GUIDToAssetPath)
                .FirstOrDefault(path => Path.GetFileName(path) == assetFileName);

            return assetPath;
        }

        public static T FindAssets<T>(string filterType, string assetFileName) where T : UnityEngine.Object
        {
            var assetPath = FirstAssetPathOrDefault(filterType, assetFileName);
            if(!string.IsNullOrEmpty(assetPath))
            {
                return AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }
            return null;
        }

        public static T FindScriptableObject<T>(string assetName) where T : ScriptableObject
        {
            return FindAssets<T>("t:ScriptableObject", string.Format("{0}.asset", assetName));
        }
    }
}
