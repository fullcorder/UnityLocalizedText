using UnityEditor;

public static class ExportUnityPackage
{
    private static readonly string Version = "0.3.0";

    [MenuItem("LocalizeText/Export")]
    public static void Export()
    {
        AssetDatabase.ExportPackage("Assets/LocalizedText",
            string.Format("UnityLocalizeText{0}.unitypackage", Version), ExportPackageOptions.Recurse);
    }
}