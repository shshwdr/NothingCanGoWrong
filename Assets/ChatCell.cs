using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatCell : MonoBehaviour
{
    public Image icon;

    public TMP_Text text;

    public Image dialogueBubbleBK;
    public Sprite playerBK;
    public Button fileButton;
    public TMP_Text fileName;
    
    public Button meetingButton;

    public GameObject meetingGO;

    public bool isPlayer = false;


    public void Init(ChatData data,ChatWindowController controller)
    {
        this.isPlayer =  data.sender.id == "player";
        icon.sprite = data.sender.icon;
        this.text.text = data.text;
        if (data.type == ChatType.download)
        {
            fileButton.gameObject.SetActive(true);
            fileName.text = "important_"+data.sender.name;
            if (data.isFinished || data.isDownloaded)
            {
                fileButton.interactable = false;
            }
            else
            {
                fileButton.onClick.AddListener(() =>
                {
                    data.isDownloaded = true;
                    controller.GetComponent<WindowController>().GetComponent<RectTransform>(). SetAsLastSibling();
                    DeskTop.Instance.AddDesktopIcon("pdf", "important_" + data.sender.name);
                    fileButton.interactable = false;
                });
            }
            text.transform.parent.gameObject.SetActive(false);
            meetingGO.SetActive(false);
        }
        else if (data.type == ChatType.respond || data.type == ChatType.chat)
        {
            fileButton.gameObject.SetActive(false);
            text.transform.parent.gameObject.SetActive(true);
            meetingGO.SetActive(false);
            
        }
        else
        {
            fileButton.gameObject.SetActive(false);
            text.transform.parent.gameObject.SetActive(false);
            meetingGO.SetActive(true);
            if (data.isFinished)
            {
                meetingButton.interactable = false;
            }
            else
            {
                
                meetingButton.interactable = true;
                meetingButton.onClick.AddListener(() =>
                {
                    ChatManager.Instance.joinMeeting(data.sender.id);
                    controller.GetComponent<WindowController>().GetComponent<RectTransform>(). SetAsLastSibling();
                    MeetingManager.Instance.joinMeeting(data.sender.name);
                    //DeskTop.Instance.AddDesktopIcon("pdf", "important_" + data.sender.name);
                    fileButton.interactable = false;
                });
            }
        }

        if (isPlayer)
        {
            GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleRight;
            icon.GetComponent<RectTransform>().SetAsLastSibling();
            dialogueBubbleBK.sprite = playerBK;
            text.color = Color.white;
            
        }
    }
}
