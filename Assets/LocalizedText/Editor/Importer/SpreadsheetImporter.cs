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
                LocalTextLogger.Error("GoogleSpreadSheetImporter : Settings GoogleSpreadsheetUrl is Empty.");
                return;
            }

            FetchUrl(settings.GoogleSpreadSheetUrl, csv =>
            {
                AssetCreator.CreateAll(settings, csv);
            });
        }

        private static void FetchUrl(string url, Action<string> onComplete)
        {
            using(var www = new WWW(url))
            {
                var startTime = Time.realtimeSinceStartup;
                var timeOutSec = TimeOutSec;
                while(!www.isDone)
                {
                    if(Time.realtimeSinceStartup - startTime > timeOutSec)
                    {
                        LocalTextLogger.Error("GoogleSpreadSheetImporter : Timeout. Confirm spreadsheet setting.");
                        break;
                    }
                }

                if(!string.IsNullOrEmpty(www.error))
                {
                    LocalTextLogger.Error("GoogleSpreadSheetImporter : Api Network Error.");
                    return;
                }

                var csv = www.text;
                if(string.IsNullOrEmpty(csv))
                {
                    LocalTextLogger.Error("GoogleSpreadSheetImporter : Response body is Empty.");
                    return;
                }

                onComplete(csv);
            }
        }
    }
}
