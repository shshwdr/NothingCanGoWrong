using System;
using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindowController : MonoBehaviour
{
    public Transform sideBar;

    public GameObject sideBarIcon;

    public Transform content;
    public GameObject chatContentCell;
    
    public TMP_InputField input;

    public Button sendButton;

    public string selectedCharacter = "";
    // Start is called before the first frame update
    void Start()
    {
        UpdateView();
        EventPool.OptIn("UpdateChat", () =>
        {
            UpdateView();
        });
        
        sendButton.onClick.AddListener(() =>
        {
            input.GetComponent<FakeInputField>().Clear();
            ChatManager.Instance.respond(selectedCharacter);
        });
    }

    public void UpdateViewAll()
    {
        input.GetComponent<FakeInputField>().Clear();
        UpdateView();
    }

    private Dictionary<string, GameObject> chatIconMap = new Dictionary<string, GameObject>();
    public void UpdateView()
    {
        //input.GetComponent<FakeInputField>().Clear();
        //sidebar
        chatIconMap.Clear();
        foreach (Transform child in sideBar)
        {
            Destroy(child.gameObject);
        }
        foreach (var characterID in ChatManager.Instance.chatCharacters)
        {
            var characterInfo =  CSVLoader.Instance.CharacterInfoMap[characterID];
            var characterName = characterInfo.name;
            var icon = Instantiate(sideBarIcon, sideBar);
            icon.GetComponent<IconButton>().nameLabel.text = characterName;
            icon.GetComponent<IconButton>().image.sprite = characterInfo.icon;
            icon.GetComponent<IconButton>().redDot.SetActive(false);//ChatManager.Instance.chatDataMap[characterID].LastItem().isFinished == false);
            icon.GetComponent<IconButton>().button.onClick.AddListener(() =>
            {
                selectedCharacter = characterID;
                UpdateViewAll();
            });
            chatIconMap[characterID]= icon;
        }
        
        //content
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        if (ChatManager.Instance.chatCharacters.Contains(selectedCharacter))
        {
            foreach (var chatData in ChatManager.Instance.chatDataMap[selectedCharacter])
            {
                var chatContent = Instantiate(chatContentCell, content);
                chatContent.GetComponent<ChatCell>().Init(chatData);
            }

            var canInput = ChatManager.Instance.chatDataMap[selectedCharacter].LastItem().isFinished == false &&
                           ChatManager.Instance.chatDataMap[selectedCharacter].LastItem().type == ChatType.respond;
            input.interactable = canInput;
            sendButton.gameObject.SetActive(canInput);
            if (canInput)
            {
                input.GetComponent<FakeInputField>().predefinedText = ChatManager.Instance.chatDataMap[selectedCharacter]
                    .LastItem().dialogueInfo.respond;
                var canSend = canInput && input.GetComponent<FakeInputField>().isFinishedType();
                sendButton.interactable =(canSend);
            }
        }
        else
        {
            
            sendButton.gameObject. SetActive(false);
            input.interactable = false;
        }
    }

    public void updateInputArrow(bool active)
    {
        sendButton.interactable =(active);
    }

    private void Update()
    {
        foreach (var characterID in ChatManager.Instance.chatCharacters)
        {
            var canInput = ChatManager.Instance.chatDataMap[characterID].LastItem().isFinished == false &&
                           ChatManager.Instance.chatDataMap[characterID].LastItem().type == ChatType.respond;
            var lastItem = ChatManager.Instance.chatDataMap[characterID].LastItem();
            if (canInput)
            {
                chatIconMap[characterID].GetComponentInChildren<HPBar>().gameObject.SetActive(true);
                lastItem.angryTimer+=Time.deltaTime;
                if (lastItem.angryTimer > lastItem.angryTime)
                {
                    lastItem.angryTimer = 0;
                    lastItem.isFinished = true;
                    ChatManager.Instance.failedRespond(characterID);
                    //EventPool.Trigger("UpdateChat");
                }
                else
                {
                     chatIconMap[characterID].GetComponentInChildren<HPBar>().SetHP(lastItem.angryTimer , lastItem.angryTime);
                }
            }
            else
            {
                chatIconMap[characterID].GetComponentInChildren<HPBar>(true).gameObject.SetActive(false);
            }
        }
    }
}
