using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskbarButton : MonoBehaviour
{
    public string appName;
    public Image icon;

    public TMP_Text label;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(string appName)
    {
        icon.sprite = SpriteUtils.GetIcon(appName);
        label.text = appName;
    }
}
