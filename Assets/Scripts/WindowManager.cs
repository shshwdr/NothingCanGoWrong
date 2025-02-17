using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : Singleton<WindowManager>
{
    public Transform desktopArea;  // 桌面区域，存放应用图标
    public Transform taskbarArea;  // 任务栏，显示打开的应用
    public GameObject windowPrefab; // 应用窗口的预制体
    public GameObject taskbarButtonPrefab; // 任务栏按钮的预制体
    public Sprite defaultAppIcon; // 默认应用图标

    private Dictionary<string, GameObject> openWindows = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> taskbarButtons = new Dictionary<string, GameObject>();

    public RectTransform canvasRect;
    public GameObject OpenApplication(string appName,GameObject prefab = null)
    {
        // 如果应用已打开，则显示它
        if (prefab == null)
        {
            prefab = windowPrefab;
        }
        if (openWindows.ContainsKey(appName))
        {
            openWindows[appName].SetActive(true);
            
            return openWindows[appName];
        }

        // 创建新窗口
        GameObject newWindow = Instantiate(prefab, desktopArea);
        
        RectTransform popupRect = newWindow.GetComponent<RectTransform>();
        // 计算弹窗大小
        Vector2 popupSize = popupRect.sizeDelta;
        Vector2 safePosition = GetSafeRandomPosition(popupSize);

        // 设置弹窗位置
        popupRect.anchoredPosition = safePosition;
        
        // 添加到打开窗口列表
        openWindows[appName] = newWindow;

        // 创建任务栏按钮
        GameObject newTaskbarButton = Instantiate(taskbarButtonPrefab, taskbarArea);
        //newTaskbarButton.transform.Find("Text").GetComponent<Text>().text = appName;
        newTaskbarButton.GetComponent<Button>().onClick.AddListener(() => ToggleWindow(appName));

        newWindow.GetComponent<WindowController>().Init(appName,newTaskbarButton.transform);
        taskbarButtons[appName] = newTaskbarButton;
        return newWindow;
    }
    private Vector2 GetSafeRandomPosition(Vector2 popupSize)
    {
        Vector2 screenSize = canvasRect.sizeDelta;

        float halfPopupWidth = popupSize.x / 2;
        float halfPopupHeight = popupSize.y / 2;

        float xMin = -screenSize.x / 2 + halfPopupWidth;
        float xMax = screenSize.x / 2 - halfPopupWidth;
        float yMin = -screenSize.y / 2 + halfPopupHeight;
        float yMax = screenSize.y / 2 - halfPopupHeight;

        float randomX = Random.Range(xMin, xMax);
        float randomY = Random.Range(yMin, yMax);

        return new Vector2(randomX, randomY);
    }
    public void CloseApplication(string appName)
    {
        if (openWindows.ContainsKey(appName))
        {
            Destroy(openWindows[appName]);
            openWindows.Remove(appName);
        }

        if (taskbarButtons.ContainsKey(appName))
        {
            Destroy(taskbarButtons[appName]);
            taskbarButtons.Remove(appName);
        }
    }

    public void ToggleWindow(string appName)
    {
        if (openWindows.ContainsKey(appName))
        {
            bool isActive = openWindows[appName].activeSelf;
            openWindows[appName].SetActive(!isActive);
        }
    }
}
