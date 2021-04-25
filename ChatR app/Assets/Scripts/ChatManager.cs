using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using SheetChat;
using System;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public ChatOperator operand;
    public Text TestText;


    ChatClient chatClient;
    [HideInInspector] public string userId;
    [HideInInspector] public string roomPin;
    [HideInInspector] public ChatWindow currentChatWindow;
    [SerializeField] GameObject roomConnectWindow;
    [SerializeField] HostEvent hostRoomWindow;
    [SerializeField] Text enterRoomError;

    #region prefab references
    public GameObject chatBubble; //A reference to the chatBubble prefab
    public GameObject chatWindowPrefab; //A reference to the chatWindow prefab
    #endregion


    void Start()
    {
        chatClient = new ChatClient(this);
    }

    void Update()
    {
        chatClient.Service();
    }
    

    public void ConnectToSheet()
    {
        try
        {
            var sheetData = operand.ReadHistory(roomPin);
            roomConnectWindow.SetActive(false);
            CreateChatWindow();
            foreach (var row in sheetData)
            {
                GameObject bubble = Instantiate(chatBubble, currentChatWindow.viewPortContent);
                bubble.GetComponent<ChatBubble>().bubble.text = row[1].ToString();
                bubble.GetComponent<ChatBubble>().userName.text = "Sent by: " + row[0].ToString();
                currentChatWindow.messageCount++;
            }
        }
        catch (Exception e)
        {
            enterRoomError.gameObject.SetActive(true);
        }
        
    }
    

    /// <summary>
    /// Connects a user to a chat room with pin
    /// </summary>
    /// <param name="user">represents a username</param>
    /// <param name="pin">The name of the chatroom/sheet</param>
    public void ConnectToPhoton(string user, string pin)
    {
        roomPin = pin;
        print("Connecting now!");
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues(user);
        ChatAppSettings settings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();

        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(user));

        //ChatBot bot = new ChatBot(roomPin);
    }

    public void ConnectToChatAsHost(string user, string sheetName/*, string eventName, string eventDesc*/)
    {
        roomPin = sheetName;
        print("Connecting now as host!");
        bool roomAvialable = operand.CreateNewSheet(sheetName); //Checks if the room name is already taken and creates it if not
        hostRoomWindow.warningText.SetActive(!roomAvialable);

        if(roomAvialable)
        {
            chatClient.AuthValues = new Photon.Chat.AuthenticationValues(user);
            ChatAppSettings settings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
            chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(user));

            CreateChatWindow();
        }

        
    }

    void CreateChatWindow()
    {
        Transform canvas = FindObjectOfType<Canvas>().transform;
        ChatWindow chatWindow = Instantiate(chatWindowPrefab, canvas).GetComponent<ChatWindow>();
        currentChatWindow = chatWindow;
        currentChatWindow.SetChatClient(chatClient);
        currentChatWindow.eventNameBillboard.text = roomPin;
    }

    #region Photon Callback Methods

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnConnected()
    {
        print("Connected to Photon");
        chatClient.Subscribe(roomPin);
        roomConnectWindow.SetActive(false);
        hostRoomWindow.gameObject.SetActive(false);

        
    }

    public void OnDisconnected()
    {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        print("Recieved " + messages[0] + " by " + senders[0]);

        currentChatWindow.CreateChatBubble();
        //GameObject bubble = Instantiate(chatBubble, currentChatWindow.viewPortContent);
        //bubble.GetComponent<ChatBubble>().bubble.text = messages[0].ToString();
        //bubble.GetComponent<ChatBubble>().userName.text = "Sent by: " + senders[0];

        //foreach (object message in messages)
        //{
        //    print(message.ToString());
        //}
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        //print("Subscribed to channel " + channels[0]);
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
