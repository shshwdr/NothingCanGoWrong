using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DesktopIcon : MonoBehaviour
{
    public string appName;
    private WindowManager windowManager;

    public GameObject prefab;
public TMP_Text nameLabel;
    void Start()
    {
        nameLabel.text = appName;
        windowManager = FindObjectOfType<WindowManager>();
        GetComponent<Button>().onClick.AddListener(() => windowManager.OpenApplication(appName,prefab));
        
    }
}