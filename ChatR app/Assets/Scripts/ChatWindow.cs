using Photon.Chat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindow : MonoBehaviour
{
    ChatManager manager; //The ChatManger in the scene
    ChatClient client; //A ChatClient from the ChatManager
    public Transform viewPortContent; //The transform of the 'content' gameobject inside ChatWindow gameobject

    public Text eventNameBillboard;
    public Text eventDescriptionBillboard;

    [HideInInspector] public string eventName;
    [HideInInspector] public string eventDescription;
    [HideInInspector] public string roomPin;

    [HideInInspector] public int eventNameId;
    [HideInInspector] public int eventDescriptionId;

    [HideInInspector] public int messageCount = 0;

    void Start()
    {
        manager = FindObjectOfType<ChatManager>();
        roomPin = manager.roomPin;
        manager.ConnectToPhoton(manager.userId, roomPin);

        print(messageCount);
    }

    //Sends a user written message to the ChatManger which in turn sends it online
    public void MessageToManager(InputField input)
    {
        manager.operand.WriteMessage( manager.userId, input.text, roomPin, client);
        
    }

    public void CreateChatBubble()
    {
        IList<object> messageLine = manager.operand.ReadMessageAtLine(roomPin, messageCount);

        GameObject bubble = Instantiate(manager.chatBubble, manager.currentChatWindow.viewPortContent);
        bubble.GetComponent<ChatBubble>().bubble.text = messageLine[1].ToString();
        bubble.GetComponent<ChatBubble>().userName.text = "Sent by: " + messageLine[0].ToString();
        manager.currentChatWindow.messageCount++;
    }

    private void OnEnable()
    {
        if(manager != null)
            manager.currentChatWindow = this;
    }

    public void SetChatClient(ChatClient newClient)
    {
        client = newClient;
    }
}
