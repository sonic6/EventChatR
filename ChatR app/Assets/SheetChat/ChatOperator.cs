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

namespace Assets.SheetChat
{
    public class ChatOperator : MonoBehaviour
    {
        public static string sheetData;

        public string[] Scopes = { SheetsService.Scope.Spreadsheets };
        public readonly string Applicationname = "ChatR";
        public static readonly string spreadSheetId = "1xtd8yhlRJ_OX3rRlmObU1iJO7aemv9asjbbgsezjykM";
        public static string sheet = "Chat1";

        public static SheetsService service;

        //UnityWebRequest path;

        //private void Awake()
        //{
        //    path = new UnityWebRequest("chatr-app-311418-f790eeb3b66e.json");
        //    print(path.url);
        //}

        public ChatOperator()
        {
            service = new SheetsService();
            GoogleCredential credential;


            //E:/Uni projects - uppsala/Agile Methods/EventChatR/ChatR app/Assets/SheetChat/chatr-app-311418-f790eeb3b66e.json
            using (var stream = new FileStream(/*path.url*/"E:/Uni projects - uppsala/Agile Methods/EventChatR/ChatR app/Assets/SheetChat/chatr-app-311418-f790eeb3b66e.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Applicationname,
            });

            print(service.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ReadEntry();
            //NewSheet();
        }

        public void ReadEntry()
        {
            var range = $"{sheet}!A1:B2";
            var request = service.Spreadsheets.Values.Get(spreadSheetId, range);

            var response = request.Execute();
            var values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    sheetData = row[0].ToString() + " " + row[1].ToString();
                    //Console.WriteLine("{0} says {1}", row[0], row[1]);
                }
            }
            else
            {
                Console.WriteLine("It didn't work");
            }
        }
    }
}
