using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Google.Apis;
using Google.Apis.Sheets.v4;
using Google.Apis.Auth.OAuth2;
using System.IO;
using UnityEngine.Networking;
using System.Net;

namespace SheetChat
{
    public class ChatOperator : MonoBehaviour
    {
        public static string sheetData;

        public string[] Scopes = { SheetsService.Scope.Spreadsheets };
        public readonly string Applicationname = "ChatR";
        public static readonly string spreadSheetId = "1xtd8yhlRJ_OX3rRlmObU1iJO7aemv9asjbbgsezjykM";
        public static string sheet = "Chat1";

        public static SheetsService service;

        public TextAsset json; //Making a reference to a json file keeps unity from deleting it when making a build
        
        public ChatOperator()
        {
            
            service = new SheetsService();
            GoogleCredential credential;

            
            using (var stream = new FileStream("Assets/SheetChat/chatr-app-311418-f790eeb3b66e.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Applicationname,
            });

        }

        /// <summary>
        /// Reads all the chat bubbles from history
        /// </summary>
        public IList<IList<object>> ReadHistory()
        {
            var range = $"{sheet}!A1:B2";
            var request = service.Spreadsheets.Values.Get(spreadSheetId, range);

            var response = request.Execute();
            var values = response.Values;
            if (values != null && values.Count > 0)
            {
                //foreach (var row in values)
                //{
                //    sheetData = row[0].ToString() + " " + row[1].ToString();
                //}
                return values;
            }
            else
            {
                Console.WriteLine("It didn't work");
                return null;
            }
        }
    }
}
