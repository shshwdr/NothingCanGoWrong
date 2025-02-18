using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatCell : MonoBehaviour
{
    public Image icon;

    public TMP_Text text;

    public bool isPlayer = false;


    public void Init(ChatData data)
    {
        this.isPlayer =  data.sender.id == "player";
        icon.sprite = data.sender.icon;
        this.text.text = data.text;

        if (isPlayer)
        {
            icon.GetComponent<RectTransform>().SetAsFirstSibling();
        }
    }
}
