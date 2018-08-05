using System.Collections.Generic;
using System.Linq;
using LocalizedText;
using UnityEngine;

namespace LocalizedText
{
    public static class SingletonTextSet
    {
        private const string ResourcesPath = "TextSet";

        private static TextSet instance;

        private static readonly object lockObject = new Object();

        public static TextSet Instance
        {
            get
            {
                if(instance == null)
                {
                    lock(lockObject)
                    {
                        if(instance == null)
                        {
                            instance = Resources.Load<TextSet>(ResourcesPath);
                            LanguageSwitcher.Create();
                        }
                    }
                }
                return instance;
            }
        }
    }
}