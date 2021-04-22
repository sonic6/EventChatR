using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostEvent : MonoBehaviour
{
    string roomPin;
    ChatManager manager;
    [SerializeField] InputField hostName;
    [SerializeField] InputField eventName;
    [SerializeField] InputField eventDescription;

    private void Start()
    {
        manager = FindObjectOfType<ChatManager>();
    }

    public void HostRoom()
    {
        int random = Random.Range(1000, 9999);
        roomPin = random.ToString();

        //manager.ConnectToChatAsHost(hostName.text, roomPin/*, eventName.text, eventDescription.text*/);
        //print(roomPin);
    }
}
