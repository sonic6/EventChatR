using Photon.Chat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindow : MonoBehaviour
{
    [HideInInspector] public ChatManager manager { get; private set; } //The ChatManger in the scene
    ChatClient client; //A ChatClient from the ChatManager
    public Transform viewPortContent; //The transform of the 'content' gameobject inside ChatWindow gameobject

    public Text eventNameBillboard;
    [SerializeField] Text eventDescriptionBillboard;
    [SerializeField] GameObject deleteChat_Box;
    
    [HideInInspector] public string roomName;
    [HideInInspector] public int messageCount = 0;

    bool clearWindow = true;

    void Start()
    {
        manager = FindObjectOfType<ChatManager>();
        roomName = manager.roomPin;
        manager.ConnectToPhoton(manager.userId, roomName);

        print(messageCount);
    }

    private void Update()
    {
        // Check if Back was pressed this frame
        if (Input.GetKeyUp(KeyCode.Escape) && !clearWindow)
        {
            eventDescriptionBillboard.transform.parent.gameObject.SetActive(false);
            deleteChat_Box.SetActive(false);
            clearWindow = true;
        }
        else if(Input.GetKeyUp(KeyCode.Escape) && clearWindow)
        {
            manager.currentChatWindow = null;
            manager.chatClient.Disconnect();
            manager.mainPage.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
    //Used by UI buttons
    public void SetClearWindow()
    {
        clearWindow = false;
    }

    //Sends a user written message to the ChatManger which in turn sends it online
    public void MessageToManager(InputField input)
    {
        if (input.text != string.Empty)
        {
            manager.operand.WriteMessage(manager.userId, input.text, roomName, client);
            input.text = string.Empty;
        }
    }

    public void CreateChatBubble()
    {
        string range = $"{roomName}!A:B";
        IList<object> messageLine = manager.operand.ReadAtLine(roomName, messageCount, $"{roomName}!A:B");

        GameObject bubble = Instantiate(manager.chatBubble, manager.currentChatWindow.viewPortContent);
        bubble.GetComponent<ChatBubble>().bubble.text = messageLine[1].ToString();
        bubble.GetComponent<ChatBubble>().userName.text = "Sent by: " + messageLine[0].ToString();
        manager.currentChatWindow.messageCount++;
    }

    public void ReadDescription()
    {
        try
        {
            string range = $"{roomName}!G:G";
            IList<object> descriptionLine = manager.operand.ReadAtLine(roomName, 0, range);
            eventDescriptionBillboard.text = descriptionLine[0].ToString();
        }
        catch
        {
            eventDescriptionBillboard.text = "The host of this event did not write a description";
        }
        
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

    //This method has to exist here because Unity's inspector can't get access to the ChatOperator from an instance of a chatwindow prefab
    /// <summary>
    /// Calls a method in ChatOperator that deletes this ChatWindow and its chat history 
    /// </summary>
    public void DeleteThisWindow()
    {
        manager.operand.DeleteEventChat(this, manager.mainPage);
    }
}
