using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject systemChatContentCell;
    public RectTransform dropArea;
    public TMP_InputField input;
    public GameObject inputArea;
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
            GetComponent<WindowController>().SetToTop();
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

        var charactersOrder =
            ChatManager.Instance.chatCharacters.OrderBy(x => ChatManager.Instance.chatDataMap[x].LastItem().isFinished);
        foreach (var characterID in charactersOrder)
        {
            var characterInfo =  CSVLoader.Instance.CharacterInfoMap[characterID];
            var characterName = characterInfo.name;
            var icon = Instantiate(sideBarIcon, sideBar);
            icon.GetComponent<IconButton>().nameLabel.text = characterName;
            icon.GetComponent<IconButton>().image.sprite = characterInfo.icon;
            var isRead = ChatManager.Instance.chatDataMap[characterID].LastItem().isRead;
            if (selectedCharacter == characterID)
            {
                icon.GetComponent<IconButton>().Select();
            }
            icon.GetComponent<IconButton>().redDot.SetActive(!isRead);//ChatManager.Instance.chatDataMap[characterID].LastItem().isFinished == false);
            icon.GetComponent<IconButton>().button.onClick.AddListener(() =>
            {
                
                GetComponent<WindowController>().SetToTop();
                selectedCharacter = characterID;
                ChatManager.Instance.chatDataMap[characterID].LastItem().isRead = true;
                UpdateViewAll();
                
                EventPool.Trigger("UpdateDot");      
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
                if (chatData.type == ChatType.system)
                {
                    var chatContent = Instantiate(systemChatContentCell, content);
                    chatContent.GetComponentInChildren<TMP_Text>().text = chatData.text;
                }
                else
                {
                    
                    var chatContent = Instantiate(chatContentCell, content);
                    chatContent.GetComponent<ChatCell>().Init(chatData,this);
                }
            }

           var lastItem=  ChatManager.Instance.chatDataMap[selectedCharacter].LastItem();
            var canInput = lastItem.isFinished == false &&
                           lastItem.type == ChatType.respond;

            if (lastItem.dialogueInfo!=null && lastItem.dialogueInfo.respondCheck != null && lastItem.dialogueInfo.respondCheck.Length > 0)
            {
                switch (lastItem.dialogueInfo.respondCheck )
                {
                    case "openAnti":
                        if (!FindObjectOfType<AntiVirusWindowController>())
                        {
                            canInput = false;
                        }
                        break;
                }
            }
            
            input.interactable = canInput;
            sendButton.gameObject.SetActive(canInput);
            inputArea.gameObject.SetActive(canInput);
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
            inputArea.gameObject.SetActive(false);
        }
    }

    public void UpdateInputStates()
    {
        if (!chatIconMap.ContainsKey(selectedCharacter))
        {
            return;
        }
        var lastItem=  ChatManager.Instance.chatDataMap[selectedCharacter].LastItem();
        
        var canInput = lastItem.isFinished == false &&
                       lastItem.type == ChatType.respond;
        if (lastItem.dialogueInfo!=null && lastItem.dialogueInfo.respondCheck != null && lastItem.dialogueInfo.respondCheck.Length > 0)
        {
            switch (lastItem.dialogueInfo.respondCheck )
            {
                case "openAnti":
                    if (!FindObjectOfType<AntiVirusWindowController>())
                    {
                        canInput = false;
                    }
                    break;
            }
        }
        
        input.interactable = canInput;
        sendButton.gameObject.SetActive(canInput);
        inputArea.gameObject.SetActive(canInput);
        if (canInput)
        {
            input.GetComponent<FakeInputField>().predefinedText = ChatManager.Instance.chatDataMap[selectedCharacter]
                .LastItem().dialogueInfo.respond;
            var canSend = canInput && input.GetComponent<FakeInputField>().isFinishedType();
            sendButton.interactable =(canSend);
        }
    }

    public void updateInputArrow(bool active)
    {
        sendButton.interactable =(active);
    }

    private void Update()
    {
        
        
        foreach (var characterID in ChatManager.Instance.chatCharacters.ToList())
        {
            var canInput = ChatManager.Instance.chatDataMap[characterID].LastItem().isFinished == false;
            var lastItem = ChatManager.Instance.chatDataMap[characterID].LastItem();
            if (canInput)
            {
                chatIconMap[characterID].GetComponentInChildren<HPBar>().gameObject.SetActive(true);
                
                if (lastItem.isFailed == true)
                {
                    input.GetComponent<FakeInputField>().Clear();
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

    public bool takeFile()
    {
        if (selectedCharacter == null)
        {
            return false;
        }

        if (ChatManager.Instance.chatCharacters.Contains(selectedCharacter))
        {
            var lastChat = ChatManager.Instance.chatDataMap[selectedCharacter].LastItem();
            var canInput = !lastChat.isFinished && lastChat.type == ChatType.download;
            return canInput;
        }

        return false;
    }
}
