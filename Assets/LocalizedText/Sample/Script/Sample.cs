using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace LocalizedText
{
    public class Sample : MonoBehaviour
    {
        [SerializeField]
        private Text _text;

        private void Start()
        {
            var textSet = SingletonTextSet.Instance;

            _text.text += textSet.Text(TextSetKey.title);
            _text.text += "\n";
            _text.text += textSet.Format(TextSetKey.hello_text, Application.systemLanguage.ToString());
            _text.text += "\n";
            _text.text += textSet[TextSetKey.dummy_text]; //Text method also define as indexer method.
        }
    }
}
