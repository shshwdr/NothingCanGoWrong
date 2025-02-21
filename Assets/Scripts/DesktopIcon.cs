using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DesktopIcon : MonoBehaviour
{
    public string appName;
    public string actualName;
    private WindowManager windowManager;

public TMP_Text nameLabel;
public GameObject finishedIcon;
public GameObject redDotIcon;

    public void Init( string actualName, string appName)
    {
        nameLabel.text = actualName;
        this.appName = appName;
        this.actualName = actualName;
        windowManager = FindObjectOfType<WindowManager>();
        GetComponent<Button>().onClick.AddListener(OpenApplication);
        
        
        GetComponent<Image>().sprite = SpriteUtils.GetIcon(appName);
        
        EventPool.OptIn("UpdateDot",UpdateDot);
    }

    public void OpenApplication()
    {
        GameObject prefab = Resources.Load<GameObject>("Application/"+appName);
        windowManager.OpenApplication(appName, actualName, prefab);
    }
    public void UpdateDot()
    {
        if (appName == "Chat")
        {
            redDotIcon.SetActive(ChatManager.Instance.hasUnreadMessage());
        }
    }
}