using System;
using System.Collections.Generic;
using UnityEngine;
using Google.Apis.Sheets.v4;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4.Data;
using Photon.Chat;

namespace SheetChat
{
    public class ChatOperator : MonoBehaviour
    {
        public static string sheetData;

        string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string Applicationname = "ChatR";
        static readonly string spreadSheetId = "1Pmn9iIETFglHlMC3JROAnmJ0DKX3IWAz8hMTWHoScYM";
        
        public static SheetsService service;

        [SerializeField] TextAsset json;

        private void Awake()
        {
            service = new SheetsService();
            GoogleCredential credential = GoogleCredential.FromJson(json.text).CreateScoped(Scopes);
            
            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Applicationname,
            });
        }

        /// <summary>
        /// Reads all the chat bubbles from history
        /// </summary>
        public IList<IList<object>> ReadHistory(string sheetName)
        {
            var range = $"{sheetName}!A:B";
            var request = service.Spreadsheets.Values.Get(spreadSheetId, range);

            var response = request.Execute();
            var values = response.Values;
            if (values != null && values.Count > 0)
            {
                
                return values;
            }
            else
            {
                Console.WriteLine("It didn't work");
                return null;
            }
        }

        public IList<object> ReadMessageAtLine(string sheetName, int line)
        {
            var range = $"{sheetName}!A:B";
            var request = service.Spreadsheets.Values.Get(spreadSheetId, range);

            var response = request.Execute();
            var values = response.Values;
            return values[line];
        }

        public void WriteMessage(string name, string message, string sheetName, ChatClient photon)
        {
            var range = $"{sheetName}!A:B";
            var valueRange = new ValueRange();

            var objectList = new List<object>() { name, message };
            valueRange.Values = new List<IList<object>> { objectList };
            
            try
            {
                var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadSheetId, range);
                appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
                var appendResponse = appendRequest.Execute();

                photon.PublishMessage(sheetName, "Message recieved");
            }
            catch (Exception e)
            {
                print("Chatroom does not exist");
            }
            
        }

        /// <summary>
        /// Sheets are used as chatrooms. Creating a new sheet means creating a new chatroom. This method returns false if the chatroom already exists
        /// </summary>
        public bool CreateNewSheet(string newSheetName)
        {
            try
            {
                var addSheetRequest = new AddSheetRequest();
                addSheetRequest.Properties = new SheetProperties();
                addSheetRequest.Properties.Title = newSheetName;
                BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
                batchUpdateSpreadsheetRequest.Requests = new List<Request>();
                batchUpdateSpreadsheetRequest.Requests.Add(new Request { AddSheet = addSheetRequest });

                var batchUpdateRequest = service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, spreadSheetId);
                batchUpdateRequest.Execute();
                return true;
            }
            catch (Exception e)
            {
                print("This event name is currently taken");
                return false;
            }
        }
    }
}
