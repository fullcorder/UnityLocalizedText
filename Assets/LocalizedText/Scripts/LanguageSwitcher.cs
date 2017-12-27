using UnityEngine;

namespace LocalizedText
{
    public class LanguageSwitcher : MonoBehaviour
    {
        private static LanguageSwitcher languageSwitcher;

        public static void Create()
        {
            if(languageSwitcher != null)
            {
                return;
            }

            var o = new GameObject()
            {
                name = "~LanguageSwitcher",
                hideFlags = HideFlags.HideInHierarchy
            };
            DontDestroyOnLoad(o);
            languageSwitcher = o.AddComponent<LanguageSwitcher>();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if(pauseStatus)
            {
                return;
            }

            var currentLangueage = Application.systemLanguage;
            var textSet = SingletonTextSet.Instance;

            if(textSet.CurrentSystemLanguage == currentLangueage)
            {
                return;
            }

            textSet.CurrentSystemLanguage = currentLangueage;
        }
    }
}