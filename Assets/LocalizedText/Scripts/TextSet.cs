using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LocalText.Internal;
using UnityEngine;

namespace LocalText
{
    public partial class TextSet : ScriptableObject
    {
        [SerializeField] private List<Language> languageList = new List<Language>();

        [SerializeField] private Dictionary<string, string> currentLanguageTextSet;

        private static SystemLanguage currentSystemLanguage;


        public SystemLanguage CurrentSystemLanguage
        {
            get { return currentSystemLanguage; }
            set
            {
                currentSystemLanguage = value;

                var language = FindLanguage(value);

                if(language != null)
                {
                    currentLanguageTextSet = language.TextDictionary;
                    return;
                }

                language = DefaultLanguage();
                if(language!= null)
                {
                    currentLanguageTextSet = language.TextDictionary;
                }
            }
        }

        private void OnEnable()
        {
            CurrentSystemLanguage = Application.systemLanguage;
        }

        public string this[string key]
        {
            get { return Text(key); }
        }

        public string Text(string key)
        {
            var text = "";
            try
            {
                text = currentLanguageTextSet[key];
            }
            catch(NullReferenceException e)
            {
                LocalizedTextLogger.ErrorFormat("No value found for key {0}. Confirm TextSet Inspector.", key);
            }
            return text;
        }

        public string Format(string key, params object[] parasmsObjects)
        {
            return string.Format(Text(key), parasmsObjects);
        }

        private Language DefaultLanguage()
        {
            return languageList.FirstOrDefault(langSet => langSet.IsDefaultLanguage);
        }

        private Language FindLanguage(SystemLanguage systemLanguage)
        {
            return languageList.FirstOrDefault(langSet => langSet.SystemLanguage == systemLanguage);
        }

        #region UnityEditor

        [Conditional("UNITY_EDITOR")]
        public void Clear()
        {
            languageList.Clear();
        }

        [Conditional("UNITY_EDITOR")]
        public void AddLanguage(Language language)
        {
            languageList.Add(language);
        }

        #endregion

        [Serializable]
        public struct StringKeyValuePair
        {
            public string Key;
            public string Value;
        }

        [Serializable]
        public class Language : ISerializationCallbackReceiver
        {
            [SerializeField]private SystemLanguage systemLanguage;

            [SerializeField] private bool isDefaultLanguage;

            [SerializeField] private List<StringKeyValuePair> serializeTextList = new List<StringKeyValuePair>();

            private Dictionary<string, string> textDictionary;

            public SystemLanguage SystemLanguage
            {
                get { return systemLanguage; }
            }

            public Dictionary<string, string> TextDictionary
            {
                get { return textDictionary; }
            }

            public bool IsDefaultLanguage
            {
                get { return isDefaultLanguage; }
            }

            public Language(SystemLanguage systemLanguage, Dictionary<string, string> textDictionary,
                bool isDefaultLanguage = false)
            {
                this.systemLanguage = systemLanguage;
                this.textDictionary = textDictionary;
                this.isDefaultLanguage = isDefaultLanguage;
            }
            public void OnBeforeSerialize()
            {
                serializeTextList.Clear();
                foreach(var keyValuePair in textDictionary)
                {
                    serializeTextList.Add(new StringKeyValuePair()
                    {
                        Key = keyValuePair.Key,
                        Value = keyValuePair.Value
                    });
                }
            }

            public void OnAfterDeserialize()
            {
                if(serializeTextList == null)
                {
                    return;
                }
                textDictionary = serializeTextList.ToDictionary(element => element.Key, element => element.Value);
            }
        }
    }
}
