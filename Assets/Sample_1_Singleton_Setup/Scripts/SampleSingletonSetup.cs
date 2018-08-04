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
            //2. Configure Settings
            //3. Generate TextSet Assets by Settings Button
            //4. Uncomment

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
