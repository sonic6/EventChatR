using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    ChatClient chatClient;
    //[SerializeField] string userId;
    InputField Input;
    private string roomPin;
    [HideInInspector] public ChatWindow currentChatWindow;
    [SerializeField] GameObject roomConnectWindow;

    [SerializeField] GameObject chatBubble; //A reference to the chatBubble prefab
    [SerializeField] GameObject chatWindowPrefab; //A reference to the chatWindow prefab

    

    void Start()
    {
        chatClient = new ChatClient(this);
        
    }

    void Update()
    {
        chatClient.Service();
    }

    //Connects a user to a chat room with pin if the chat room exists
    public void ConnectToChat(string user, string pin)
    {
        roomPin = pin;
        print("Connecting now!");
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues(user);
        ChatAppSettings settings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(user));
    }

    public void ConnectToChatAsHost(string user, string pin/*, string eventName, string eventDesc*/)
    {
        roomPin = pin;
        print("Connecting now as host!");
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues(user);
        ChatAppSettings settings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(user));


    }

    public void SendPhotonMessage(InputField input)
    {
        chatClient.PublishMessage(roomPin, input.text);
        GameObject bubble = Instantiate(chatBubble, currentChatWindow.viewPortContent);
        bubble.GetComponent<ChatBubble>().bubble.text = input.text;
        bubble.GetComponent<ChatBubble>().userName.text = "Sent by: " + chatClient.UserId;

        input.text = null;
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

        //This message is a specifier for the event name
        if (messages[0].ToString().Contains(currentChatWindow.eventNameId.ToString()))
        {
            currentChatWindow.eventNameBillboard.text = messages[0].ToString().Remove(0, 2);
        }
        else if(messages[0].ToString().Contains(currentChatWindow.eventDescriptionId.ToString()))
        {

        }
        else
        {
            GameObject bubble = Instantiate(chatBubble, currentChatWindow.viewPortContent);
            bubble.GetComponent<ChatBubble>().bubble.text = messages[0].ToString();
            bubble.GetComponent<ChatBubble>().userName.text = "Sent by: " + senders[0];
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
