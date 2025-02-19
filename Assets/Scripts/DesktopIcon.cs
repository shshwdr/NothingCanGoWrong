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

    public void Init( string actualName, string appName)
    {
        nameLabel.text = actualName;
        this.appName = appName;
        this.actualName = actualName;
        windowManager = FindObjectOfType<WindowManager>();
        GameObject prefab = Resources.Load<GameObject>("Application/"+appName);
        GetComponent<Button>().onClick.AddListener(() => windowManager.OpenApplication(appName,actualName,prefab));
        
        
        GetComponent<Image>().sprite = SpriteUtils.GetIcon(appName);
    }
}