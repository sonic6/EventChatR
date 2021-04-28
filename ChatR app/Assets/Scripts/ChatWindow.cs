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
    
    [HideInInspector] public string roomPin;
    [HideInInspector] public int messageCount = 0;

    void Start()
    {
        manager = FindObjectOfType<ChatManager>();
        roomPin = manager.roomPin;
        manager.ConnectToPhoton(manager.userId, roomPin);

        print(messageCount);
    }

    private void Update()
    {
        // Check if Back was pressed this frame
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            eventDescriptionBillboard.transform.parent.gameObject.SetActive(false);
        }
    }

    //Sends a user written message to the ChatManger which in turn sends it online
    public void MessageToManager(InputField input)
    {
        if (input.text != string.Empty)
        {
            manager.operand.WriteMessage(manager.userId, input.text, roomPin, client);
            input.text = string.Empty;
        }
    }

    public void CreateChatBubble()
    {
        string range = $"{roomPin}!A:B";
        IList<object> messageLine = manager.operand.ReadAtLine(roomPin, messageCount, $"{roomPin}!A:B");

        GameObject bubble = Instantiate(manager.chatBubble, manager.currentChatWindow.viewPortContent);
        bubble.GetComponent<ChatBubble>().bubble.text = messageLine[1].ToString();
        bubble.GetComponent<ChatBubble>().userName.text = "Sent by: " + messageLine[0].ToString();
        manager.currentChatWindow.messageCount++;
    }

    public void ReadDescription()
    {
        string range = $"{roomPin}!G:G";
        IList<object> descriptionLine = manager.operand.ReadAtLine(roomPin, 0, range);
        eventDescriptionBillboard.text = descriptionLine[0].ToString();
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
