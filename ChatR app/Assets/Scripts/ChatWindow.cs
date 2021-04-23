using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindow : MonoBehaviour
{
    ChatManager manager; //The ChatManger in the scene
    public Transform viewPortContent; //The transform of the 'content' gameobject inside ChatWindow gameobject

    public Text eventNameBillboard;
    public Text eventDescriptionBillboard;

    [HideInInspector] public string eventName;
    [HideInInspector] public string eventDescription;
    [HideInInspector] public string roomPin;

    [HideInInspector] public int eventNameId;
    [HideInInspector] public int eventDescriptionId;

    void Start()
    {
        manager = FindObjectOfType<ChatManager>();
        eventNameId = Random.Range(100, 999);
        eventDescriptionId = Random.Range(100, 999);
    }

    //Sends a user written message to the ChatManger which in turn sends it online
    public void MessageToManager(InputField input)
    {
        //manager.SendPhotonMessage(input);
        manager.operand.WriteMessage( manager.userId, input.text);
    }

    private void OnEnable()
    {
        if(manager != null)
            manager.currentChatWindow = this;
    }
}
