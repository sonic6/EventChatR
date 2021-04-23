using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SheetChat;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public ChatOperator operand;
    public Text TestText;


    ChatClient chatClient;
    public string userId;
    InputField Input;
    [HideInInspector] public string roomPin;
    [HideInInspector] public ChatWindow currentChatWindow;
    [SerializeField] GameObject roomConnectWindow;

    [SerializeField] GameObject chatBubble; //A reference to the chatBubble prefab
    [SerializeField] GameObject chatWindowPrefab; //A reference to the chatWindow prefab
    
    

    void Start()
    {
        //operand.ReadHistory();
        //print(ChatOperator.sheetData);
        //TestText.text = ChatOperator.sheetData;
        

        //chatClient = new ChatClient(this);
        
    }

    void Update()
    {
        //chatClient.Service();
    }

    #region our own code for Google sheets methods

    public void ConnectToSheet()
    {
        roomConnectWindow.SetActive(false);

        Transform canvas = FindObjectOfType<Canvas>().transform;
        ChatWindow chatWindow = Instantiate(chatWindowPrefab, canvas).GetComponent<ChatWindow>();
        currentChatWindow = chatWindow;

        operand.sheet = roomPin;
        var sheetData  = operand.ReadHistory();

        foreach (var row in sheetData)
        {
            //sheetData = row[0].ToString() + " " + row[1].ToString();
            GameObject bubble = Instantiate(chatBubble, currentChatWindow.viewPortContent);
            bubble.GetComponent<ChatBubble>().bubble.text = row[1].ToString();
            bubble.GetComponent<ChatBubble>().userName.text = "Sent by: " + row[0].ToString();
        }
    }

    #endregion

    //#region owr code for photon

    ////Connects a user to a chat room with pin if the chat room exists
    //public void ConnectToChat(string user, string pin)
    //{
    //    roomPin = pin;
    //    print("Connecting now!");
    //    chatClient.AuthValues = new Photon.Chat.AuthenticationValues(user);
    //    ChatAppSettings settings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();

    //    chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(user));

    //    //ChatBot bot = new ChatBot(roomPin);
    //}

    public void ConnectToChatAsHost(string user, string sheetName/*, string eventName, string eventDesc*/)
    {
        roomPin = sheetName;
        print("Connecting now as host!");
        operand.CreateNewSheet(sheetName);
        //chatClient.AuthValues = new Photon.Chat.AuthenticationValues(user);
        //ChatAppSettings settings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        //chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(user));


    }

    //public void SendPhotonMessage(InputField input)
    //{
    //    chatClient.PublishMessage(roomPin, input.text);
    //    GameObject bubble = Instantiate(chatBubble, currentChatWindow.viewPortContent);
    //    bubble.GetComponent<ChatBubble>().bubble.text = input.text;
    //    bubble.GetComponent<ChatBubble>().userName.text = "Sent by: " + chatClient.UserId;

    //    input.text = null;
    //}

    //#endregion

    #region Photon Callback Methods

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnConnected()
    {
        print("Connected");
        chatClient.Subscribe(roomPin);
        roomConnectWindow.SetActive(false);

        Transform canvas = FindObjectOfType<Canvas>().transform;
        ChatWindow chatWindow = Instantiate(chatWindowPrefab, canvas).GetComponent<ChatWindow>();
        currentChatWindow = chatWindow;
    }

    public void OnDisconnected()
    {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        print("Recieved " + messages[0] + " by " + senders[0]);

        ////This message is a specifier for the event name
        //if (messages[0].ToString().Contains(currentChatWindow.eventNameId.ToString()))
        //{
        //    currentChatWindow.eventNameBillboard.text = messages[0].ToString().Remove(0, 2);
        //}
        //else if(messages[0].ToString().Contains(currentChatWindow.eventDescriptionId.ToString()))
        //{

        //}
        //else
        //{
            
        //}

        GameObject bubble = Instantiate(chatBubble, currentChatWindow.viewPortContent);
        bubble.GetComponent<ChatBubble>().bubble.text = messages[0].ToString();
        bubble.GetComponent<ChatBubble>().userName.text = "Sent by: " + senders[0];

        foreach(object message in messages)
        {
            print(message.ToString());
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        print("Subscribed to channel " + channels[0]);
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    public void OnUserSubscribed(string channel, string user)
    {
        print(user + " Subscribed to channel " + channel);
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
       
    }

    #endregion
}
