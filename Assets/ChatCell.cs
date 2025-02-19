using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatCell : MonoBehaviour
{
    public Image icon;

    public TMP_Text text;

    public Button fileButton;
    public TMP_Text fileName;

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
            if (data.isFinished)
            {
                fileButton.interactable = false;
            }
            else
            {
                fileButton.onClick.AddListener(() =>
                {
                    
                    controller.GetComponent<WindowController>().GetComponent<RectTransform>(). SetAsLastSibling();
                    DeskTop.Instance.AddDesktopIcon("pdf", "important_" + data.sender.name);
                    fileButton.interactable = false;
                });
            }
            text.gameObject.SetActive(false);
        }
        else
        {
            fileButton.gameObject.SetActive(false);
            text.gameObject.SetActive(true);
            
        }

        if (isPlayer)
        {
            icon.GetComponent<RectTransform>().SetAsLastSibling();
        }
    }
}
