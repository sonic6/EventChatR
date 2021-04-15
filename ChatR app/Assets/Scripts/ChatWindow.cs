using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindow : MonoBehaviour
{
    ChatManager manager; //The ChatManger in the scene
    public Transform viewPortContent; //The transform of the 'content' gameobject inside ChatWindow gameobject
 
    void Start()
    {
        manager = FindObjectOfType<ChatManager>();
    }

    //Sends a user written message to the ChatManger which in turn sends it online
    public void MessageToManager(InputField input)
    {
        manager.SendPhotonMessage(input);
    }

    private void OnEnable()
    {
        if(manager != null)
            manager.currentChatWindow = this;
    }
}
