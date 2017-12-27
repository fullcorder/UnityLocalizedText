using System;
using LocalizedText.Internal;
using UnityEngine;
using UnityEngine.Networking;

namespace LocalizedText.Importer
{
    public static class GoogleSpreadSheetImporter
    {
        private static readonly int TimeOutSec = 10;

        public static void SyncGoogleSpreadSheetApi(Settings settings)
        {
            if(string.IsNullOrEmpty(settings.GoogleSpreadSheetUrl))
            {
                Debug.LogError("GoogleSpreadSheetImporter Settings GoogleSpreadsheetUrl is Empty.");
                return;
            }

            FetchUrl(settings.GoogleSpreadSheetUrl, csv =>
            {
                AssetCreator.CreateAll(settings, csv);
            });
        }

        private static void FetchUrl(string url, Action<string> onComplete)
        {
            using(var webRequest = UnityWebRequest.Get(url))
            {
                webRequest.Send();

                var startTime = Time.realtimeSinceStartup;
                var timeOutSec = TimeOutSec;
                while(webRequest.responseCode == -1)
                {
                    if(Time.realtimeSinceStartup - startTime > timeOutSec)
                    {
                        Debug.LogError("GoogleSpreadSheetImporter : Timeout. Confirm spreadsheet setting.");
                        break;
                    }
                }

                //TODO support Unity2017 api
                if(webRequest.isError)
                {
                    Debug.LogError("GoogleSpreadSheetImporter : Api Network Error.");
                    return;
                }

                if(webRequest.responseCode != 200)
                {
                    Debug.LogError("GoogleSpreadSheetImporter : Response fail. responceCode " + webRequest.responseCode);
                    return;
                }

                var csv = webRequest.downloadHandler.text;
                if(string.IsNullOrEmpty(csv))
                {
                    Debug.LogError("GoogleSpreadSheetImporter : Response body is Empty.");
                    return;
                }

                onComplete(csv);
            }
        }
    }
}
