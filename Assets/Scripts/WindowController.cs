using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WindowController : MonoBehaviour, IDragHandler, IPointerDownHandler
{ 
    public Button fullWindow;
    
    public RectTransform content;

    public GameObject lockPanel;
    
    [HideInInspector]
    public string id;
    public RectTransform windowRect; // 窗口 RectTransform
    public Button closeButton;
    public Button minimizeButton;
    public Button maximizeButton;
public TMP_Text titleLabel;
    private bool isMaximized = false;
    private Vector2 originalSize= new Vector2(500, 300); // 默认窗口大小;
    private Vector3 originalPosition;
    private Transform parentCanvas;

    public bool hideAllButtons;

    private WindowManager windowManager;

    private Transform taskBarIcon;

    public bool isRansomed = false;

    public void Ransom()
    {
        GetComponentInChildren<Lock>(true).Init();
        isRansomed = true;
        lockPanel.SetActive(true);
        
        
    }

    public void Unlock()
    {
        isRansomed = false;
        lockPanel.SetActive(false);
    }
    
    public void SetToTop()
    {
        transform.SetAsLastSibling(); // 将当前 UI 置顶
    }
    public void SetToBottom()
    {
        transform.SetAsFirstSibling();
    }
    void Start()
    {
        lockPanel.SetActive(false);
        closeButton.onClick.AddListener(CloseWindow);
        minimizeButton.onClick.AddListener(MinimizeWindow);
        maximizeButton.onClick.AddListener(ToggleMaximize);
        fullWindow.onClick.AddListener(() =>
        {
            transform.SetAsLastSibling(); // 将当前 UI 置顶

        });
        //windowRect.sizeDelta = originalSize;
       // windowRect.anchoredPosition = Vector2.zero;
        parentCanvas = transform.parent;
        windowManager = WindowManager.Instance;

        if (hideAllButtons)
        {
            closeButton.gameObject.SetActive(false);
            minimizeButton.gameObject.SetActive(false);
            maximizeButton.gameObject.SetActive(false);
            
        }
    }

    public void Init(string name,string actualName,string appId,Transform taskBarIcon)
    {
        titleLabel.text = actualName;
        id = appId;
        this.taskBarIcon = taskBarIcon;
    }

    public void CloseWindow()
    {
        if (windowManager != null)
        {
            windowManager.CloseApplication(id);
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// 最小化窗口（隐藏，但可从任务栏恢复）
    /// </summary>
    public void MinimizeWindow()
    {
        
        // windowRect.DOAnchorPos(taskBarIcon.position, 0.5f).SetEase(Ease.InOutQuad);
        // windowRect.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad)
        //     .OnComplete(() => gameObject.SetActive(false));
        //
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 切换最大化/还原
    /// </summary>
    public void ToggleMaximize()
    {
        if (isMaximized)
        {
            // 还原窗口大小
            windowRect.sizeDelta = originalSize;
            windowRect.position = originalPosition;
        }
        else
        {
            // 记录当前大小 & 位置
            originalSize = windowRect.sizeDelta;
            originalPosition = windowRect.position;

            // 设置最大化（适应 Canvas 但不全屏）
            windowRect.sizeDelta = new Vector2(Screen.width , Screen.height);
            windowRect.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        }
        isMaximized = !isMaximized;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling(); // 确保窗口在最前面
    }
}
