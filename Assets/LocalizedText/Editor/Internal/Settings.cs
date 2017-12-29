using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using MenuName = Constant.MenuName;

namespace LocalizedText
{
    [CreateAssetMenu(fileName = MenuName.AssetMenuFileName, menuName = MenuName.AssetMenuName, order = 801)]
    public class Settings : ScriptableObject
    {
        private static Settings _instance;

        public enum DataSource
        {
            GoogleSpreadSheetAsWeb
        }

        public enum DataFormat
        {
            Tsv,
            Csv
        }

        private static readonly Dictionary<DataFormat, char> _separatorMap = new Dictionary<DataFormat, char>
        {
            {DataFormat.Tsv, '\t'},
            {DataFormat.Csv, ','}
        };

        [SerializeField] private string _textSetAssetName;

        [SerializeField] private string _textSetGenerateDirectory;

        [SerializeField] private Object _textDataGenerateFolder;

        [SerializeField] private string _keyDefinitionClassName;

        [SerializeField] private string _keyClassGenerateDirectory;

        [SerializeField] private Object _keyClassGenerateFolder;

        [SerializeField] private List<LanguageSetting> _languageSettingList = new List<LanguageSetting>();

        [SerializeField] private DataSource _selectedDataSource;

        [SerializeField] private DataFormat _selectedDataFormat;

        [SerializeField] private string _googleSpreadSheetUrl;

        public IList<LanguageSetting> LanguageSettingList
        {
            get { return _languageSettingList; }
            set { _languageSettingList = value.ToList(); }
        }

        public string TextSetAssetName
        {
            get { return _textSetAssetName; }
            set { _textSetAssetName = value; }
        }

        public string TextSetGenerateDirectory
        {
            get { return _textSetGenerateDirectory; }
            set { _textSetGenerateDirectory = value; }
        }

        public Object TextDataGenerateFolder
        {
            get { return _textDataGenerateFolder; }
            set { _textDataGenerateFolder = value; }
        }

        public string KeyDefinitionClassName
        {
            get { return _keyDefinitionClassName; }
            set { _keyDefinitionClassName = value; }
        }

        public string KeyClassGenerateDirectory
        {
            get { return _keyClassGenerateDirectory; }
            set { _keyClassGenerateDirectory = value; }
        }

        public Object KeyClassGenerateFolder
        {
            get { return _keyClassGenerateFolder; }
            set { _keyClassGenerateFolder = value; }
        }

        public DataSource SelectedDataSource
        {
            get { return _selectedDataSource; }
            set { _selectedDataSource = value; }
        }

        public DataFormat SelectedDataFormat
        {
            get { return _selectedDataFormat; }
            set { _selectedDataFormat = value; }
        }

        public string GoogleSpreadSheetUrl
        {
            get { return _googleSpreadSheetUrl; }
            set { _googleSpreadSheetUrl = value; }
        }

        public bool Valid()
        {
            return ValidationErrorMessage()
                       .Length ==
                   0;
        }

        public StringBuilder ValidationErrorMessage()
        {
            var errorMessages = new StringBuilder();
            if(LanguageSettingList.Count == 0)
            {
                errorMessages.Append(Constant.ErrorMessage.EmptyLangageList);
                errorMessages.AppendLine();
            }

            if(string.IsNullOrEmpty(TextSetAssetName))
            {
                errorMessages.Append(Constant.ErrorMessage.NullAssetName);
                errorMessages.AppendLine();
            }

            if(string.IsNullOrEmpty(TextSetGenerateDirectory))
            {
                errorMessages.Append(Constant.ErrorMessage.NullTextSetDirectory);
                errorMessages.AppendLine();
            }

            if(string.IsNullOrEmpty(KeyDefinitionClassName))
            {
                errorMessages.Append(Constant.ErrorMessage.NullKeyClassName);
                errorMessages.AppendLine();
            }

            if(string.IsNullOrEmpty(KeyClassGenerateDirectory))
            {
                errorMessages.Append(Constant.ErrorMessage.NullKeyDirectory);
                errorMessages.AppendLine();
            }

            if(string.IsNullOrEmpty(GoogleSpreadSheetUrl))
            {
                errorMessages.Append(Constant.ErrorMessage.NullSpreadSheetURL);
                errorMessages.AppendLine();
            }

            return errorMessages;
        }

        public string TextSetAssetPath()
        {
            if(string.IsNullOrEmpty(TextSetAssetName))
            {
                return "";
            }

            return Path.Combine(TextSetGenerateDirectory, string.Format("{0}.asset", TextSetAssetName));
        }

        public string KeyAssetPath()
        {
            if(string.IsNullOrEmpty(KeyClassGenerateDirectory))
            {
                return "";
            }

            return Path.Combine(KeyClassGenerateDirectory, string.Format("{0}.cs", KeyDefinitionClassName));
        }

        public char Separator()
        {
            return _separatorMap[SelectedDataFormat];
        }

        [Serializable]
        public class LanguageSetting
        {
            [SerializeField] private SystemLanguage _language = SystemLanguage.Unknown;

            [SerializeField] private int _indexOfColumns;

            [SerializeField] private bool _isDefault;

            public SystemLanguage Language
            {
                get { return _language; }
                set { _language = value; }
            }

            public int IndexOfColumns
            {
                get { return _indexOfColumns; }
                set { _indexOfColumns = value; }
            }

            public bool IsDefault
            {
                get { return _isDefault; }
                set { _isDefault = value; }
            }
        }
    }
}