using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskTop : Singleton<DeskTop>
{
public Transform desktopParent;
public GameObject desktopIcon;

public Dictionary<string, GameObject> desktopIcons = new Dictionary<string, GameObject>();
public void AddDesktopIcon(string appName)
{
    AddDesktopIcon(appName, appName);
}
    public void AddDesktopIcon(string appName, string actualName)
    {
        GameObject icon = Instantiate(desktopIcon, desktopParent);
        icon.GetComponent<DesktopIcon>().Init(actualName, appName);
        desktopIcons[actualName] = icon;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        AddDesktopIcon("My Computer");
        AddDesktopIcon("Chat");
        AddDesktopIcon("Anti Virus");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
