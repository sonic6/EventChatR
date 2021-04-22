using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatBot : MonoBehaviour, IChatClientListener
{
    List<string> chatHistory = new List<string>();
    string roomPin;
    ChatClient client;

    public ChatBot(string roomPin)
    {
        this.roomPin = roomPin;
        client = new ChatClient(this);
        ConnectBotToRoom();
    }

    void Update()
    {
        client.Service();
    }

    private void ConnectBotToRoom()
    {
        client.AuthValues = new Photon.Chat.AuthenticationValues("Bot");
        ChatAppSettings settings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        client.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues("Bot"));
    }


    public void DebugReturn(DebugLevel level, string message)
    {
        
    }

    public void OnChatStateChange(ChatState state)
    {
        
    }

    public void OnConnected()
    {
        client.Subscribe(roomPin);
    }

    public void OnDisconnected()
    {
        
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        print("The chat bot recieved something");
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        
    }

    public void OnUnsubscribed(string[] channels)
    {
        
    }

    public void OnUserSubscribed(string channel, string user)
    {
        
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        
    }
}
