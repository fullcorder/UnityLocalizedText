﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using LocalizedText.Internal;
using LocalText;
using LocalText.Internal;
using UnityEditor;
using UnityEngine;

namespace LocalizedText.Importer
{
    public static class AssetCreator
    {
        public static Func<string, string> OnPreProcessImport;

        public static void CreateAll(Settings settings, string csv)
        {
            OnPreProcessImport += ReplaceNewline;

            CreteKeyDefinitionFile(settings, csv);
            try
            {
                CreateTextSet(settings, csv);
            }
            catch(IndexOutOfRangeException e)
            {
                LocalizedTextLogger.Error("Parse Fail.The number of columns may be incorrect");
            }

            AssetDatabase.Refresh();
        }

        private static void CreteKeyDefinitionFile(Settings settings, string csv)
        {
            const int indexOfKey = 0;
            const int indexOfComment = 1;

            var lines = new StringBuilder();
            lines.Append("// this class generated by LocalizedText");
            lines.AppendLine();
            lines.AppendFormat("public class {0}", settings.KeyDefinitionClassName);
            lines.AppendLine();
            lines.Append("{");
            lines.AppendLine();
            ForEachRows(csv, settings.Separator(), indexOfKey, indexOfComment, (key, comment) =>
            {
                lines.Append("    /// <summary>");
                lines.AppendLine();
                lines.AppendFormat("    /// {0}", comment);
                lines.AppendLine();
                lines.Append("    /// </summary>");
                lines.AppendLine();
                lines.AppendFormat("    public static readonly string {0} = \"{0}\";", key);
                lines.AppendLine();
            });
            lines.Append("}");

            var classString = lines.ToString();
            File.WriteAllText(settings.KeyAssetPath(), classString);
        }

        private static void CreateTextSet(Settings settings, string csv)
        {
            var textDataAssetPath = settings.TextSetAssetPath();

            var textData = AssetDatabase.LoadAssetAtPath<TextSet>(textDataAssetPath);
            if(!textData)
            {
                LocalizedTextLogger.Verbose("Create new TextSet");
                textData = ScriptableObject.CreateInstance<TextSet>();
                AssetDatabase.CreateAsset(textData, textDataAssetPath);
            }
            else
            {
                LocalizedTextLogger.Verbose("Update TextSet");
                textData.Clear();
            }

            const int indexOfKey = 0;
            var colIndex = 2;
            var settingList = settings.LanguageSettingList;
            var separator = settings.Separator();

            foreach(var info in settingList)
            {
                var textSet = new Dictionary<string, string>();
                ForEachRows(csv, separator, 0, colIndex, (key, text) =>
                {
                    if(OnPreProcessImport != null)
                    {
                        text = OnPreProcessImport(text);
                    }

                    textSet[key] = string.IsInterned(text) == null ? string.Intern(text) : text;
                });

                var language = new TextSet.Language(info.Language, textSet, info.IsDefault);
                textData.AddLanguage(language);
                colIndex++;
            }

            EditorUtility.SetDirty(textData);
            AssetDatabase.SaveAssets();
        }

        private static void ForEachRows(string csv, char separator, int firstColumn, int secondColumn, Action<string, string> action)
        {
            var lines = Regex.Split(csv, "\r\n");
            for(var i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Split(separator);
                action(line[firstColumn], line[secondColumn]);
            }
        }

        private static string ReplaceNewline(string rawText)
        {
            return rawText.Replace("<br>", "\n");
        }
    }
}