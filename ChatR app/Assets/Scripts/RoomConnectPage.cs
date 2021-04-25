using Photon.Chat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomConnectPage : MonoBehaviour
{
    [SerializeField] InputField pinCode;
    [SerializeField] InputField userName;
    [SerializeField] Button connectionButton;

    [SerializeField] GameObject pinError;
    [SerializeField] GameObject nameError;

    ChatManager chatManager;

    bool pin = true /*false*/, user = false;

    private void Start()
    {
        chatManager = GameObject.FindObjectOfType<ChatManager>();
    }

    public void WaitForPin()
    {
        //pin = false;
    }

    public void CheckPin()
    {
        ActivateButton();
    }

    public void WaitForUserName()
    {
        user = false;
    }

    public void CheckUserName()
    {
        userName.text.Replace(" ", "");
        if (userName.text == "")
        {
            nameError.SetActive(true);
        }
        else
        {
            user = true;
            nameError.SetActive(false);
        }

        ActivateButton();
    }

    private void ActivateButton()
    {
        if (pin && user)
        {
            chatManager.userId = userName.text;
            chatManager.roomPin = pinCode.text;
            connectionButton.interactable = true;
        }
        else
            connectionButton.interactable = false;
    }

    public void DebugLog()
    {
        print("button was pressed");
    }

    public void ConnectToRoom()
    {
        chatManager.ConnectToSheet();
    }
}
