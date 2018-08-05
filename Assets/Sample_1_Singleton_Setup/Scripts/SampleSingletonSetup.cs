using UnityEngine;
using UnityEngine.UI;

namespace LocalizedText
{
    public class SampleSingletonSetup : MonoBehaviour
    {
        [SerializeField]
        private Text _text;

        private void Start()
        {
            //1. Asset Menu / Create / LocalizedText / Create Singleton Settings
            //2. Generated LocalizedTextSettings at Assets/LoclizedText/Editor/Settings/LocalizedTextSettings
            //3. Configure Language at LocalizedTextSettings inspector
            //4. Configure Google Spread Sheet URL at LocalizedTextSettings inspector
            // ex https://docs.google.com/spreadsheets/d/e/2PACX-1vRVG09sHjgpAKLrC4gK7tr4dKlm0CTi8jOy1E8tLqb9_gAvEiRt4_rprcjsRLGv5mGXW6c7tWbWz0m0/pub?gid=0&single=true&output=tsv
            //5. Generate / Update  TextSet Assets by Settings Inspector

            /*
            var textSet = SingletonTextSet.Instance;

            _text.text += textSet.Text(TextSetKey.title);
            _text.text += "\n";
            _text.text += textSet.Format(TextSetKey.hello_text, Application.systemLanguage.ToString());
            _text.text += "\n";
            _text.text += textSet[TextSetKey.dummy_text]; //Text method also define as indexer method.
            */
        }
    }
}
