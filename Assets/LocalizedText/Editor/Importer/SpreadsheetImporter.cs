using System;
using LocalizedText.Internal;
using LocalText.Internal;
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
                LocalizedTextLogger.Error("GoogleSpreadSheetImporter : Settings GoogleSpreadsheetUrl is Empty.");
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
                        LocalizedTextLogger.Error("GoogleSpreadSheetImporter : Timeout. Confirm spreadsheet setting.");
                        break;
                    }
                }

                #if UNITY_5
                var isError = webRequest.isError;
                #else
                var isError = webRequest.isNetworkError;
                #endif

                if(isError)
                {
                    LocalizedTextLogger.Error("GoogleSpreadSheetImporter : Api Network Error.");
                    return;
                }

                if(webRequest.responseCode != 200)
                {
                    LocalizedTextLogger.ErrorFormat("GoogleSpreadSheetImporter : Response fail. responceCode {0}",
                         webRequest.responseCode);
                    return;
                }

                var csv = webRequest.downloadHandler.text;
                if(string.IsNullOrEmpty(csv))
                {
                    LocalizedTextLogger.Error("GoogleSpreadSheetImporter : Response body is Empty.");
                    return;
                }

                onComplete(csv);
            }
        }
    }
}
