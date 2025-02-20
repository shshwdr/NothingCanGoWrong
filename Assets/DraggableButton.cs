using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableButton : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public RectTransform targetArea;
    
    ChatWindowController chatWindowController;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        // 记录按钮的原始位置
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 让按钮半透明，提升交互体验
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false; // 让按钮不会阻挡射线

        chatWindowController = FindObjectOfType<ChatWindowController>();
        if (chatWindowController)
        {
            targetArea = chatWindowController.input.GetComponent<RectTransform>();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 在 UI 空间内移动按钮
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        
        if (IsPointerOverTarget(eventData))
        {
            targetArea.GetComponent<Image>().color = Color.blue;
            
        }
        else
        {
            if (targetArea)
            {
                targetArea.GetComponent<Image>().color = Color.white;
                
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true; // 允许按钮再次接受事件
        if (targetArea)
        {
            targetArea.GetComponent<Image>().color = Color.white;
        }
        // 检测是否拖拽到了目标区域
        if (IsPointerOverTarget(eventData))
        {
            Destroy(gameObject); // 删除按钮
            
            ChatManager.Instance.addFile(GetComponent<DesktopIcon>().actualName,GetComponent<DesktopIcon>().finishedIcon.activeSelf);
        }
        else
        {
           // rectTransform.anchoredPosition = originalPosition; // 复位到原始位置
        }
    }

    private bool IsPointerOverTarget(PointerEventData eventData)
    {
        if (targetArea == null) return false;

        if (!chatWindowController.takeFile())
        {
            return false;
        }
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetArea,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        return targetArea.rect.Contains(localPoint);
    }
}