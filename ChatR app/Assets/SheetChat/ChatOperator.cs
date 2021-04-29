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
        //public static string sheetData;

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

        /// <summary>
        /// Reads data from the spreadsheet at a specific line and from a specific sheet
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public IList<object> ReadAtLine(string sheetName, int line, string range)
        {
            //var range = $"{sheetName}!A:B";
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
        public bool CreateNewSheet(string newSheetName, string description)
        {
            try
            {
                #region creates new sheet
                var addSheetRequest = new AddSheetRequest();
                addSheetRequest.Properties = new SheetProperties();
                addSheetRequest.Properties.Title = newSheetName;
                BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
                batchUpdateSpreadsheetRequest.Requests = new List<Request>();
                batchUpdateSpreadsheetRequest.Requests.Add(new Request { AddSheet = addSheetRequest });

                var batchUpdateRequest = service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, spreadSheetId);
                batchUpdateRequest.Execute();
                #endregion

                AddSheetDescription(newSheetName, description);

                return true;
            }
            catch (Exception e)
            {
                print("This event name is currently taken");
                return false;
            }
        }

        /// <summary>
        /// Adds a description on the G column in a specific sheet
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="description"></param>
        private void AddSheetDescription(string sheetName, string description)
        {
            var range = $"{sheetName}!G:G";
            var valueRange = new ValueRange();

            print(description);
            var objectList = new List<object>() { description };
            valueRange.Values = new List<IList<object>> { objectList };

            var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadSheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendResponse = appendRequest.Execute();
        }

        /// <summary>
        /// Deletes a selected event and its chat
        /// </summary>
        public void DeleteEventChat(ChatWindow window, Transform mainPage)
        {
            var deleteSheetRequest = new DeleteSheetRequest();
            Spreadsheet a = new Spreadsheet();
            a = service.Spreadsheets.Get(spreadSheetId).Execute();
            

            int? sheetIdToDelete = null;
            foreach (Sheet s in a.Sheets)
            {
                if (s.Properties.Title == window.roomName)
                    sheetIdToDelete = s.Properties.SheetId;
            }

            deleteSheetRequest.SheetId = sheetIdToDelete;
            BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
            batchUpdateSpreadsheetRequest.Requests = new List<Request>();
            batchUpdateSpreadsheetRequest.Requests.Add(new Request { DeleteSheet = deleteSheetRequest });
            var batchUpdateRequest = service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, spreadSheetId);
            batchUpdateRequest.Execute();
            
            window.manager.currentChatWindow = null;
            window.manager.chatClient.Disconnect();
            mainPage.gameObject.SetActive(true);
            Destroy(window.gameObject);
        }
    }
}
