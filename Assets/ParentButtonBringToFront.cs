using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ParentButtonBringToFront : MonoBehaviour
{
    private List<GameObject> childButtons = new List<GameObject>();

    void Start()
    {
        // 获取所有子级 Button
        FindChildButtons(transform);
        
        // 给所有子 Button 添加点击事件监听
        foreach (var button in childButtons)
        {
            AddPointerDownListener(button);
        }
    }

    private void FindChildButtons(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.GetComponent<EventTrigger>()) // 仅查找 UI 按钮
            {
                childButtons.Add(child.gameObject);
            }

            // 递归搜索子层级
            if (child.childCount > 0)
            {
                FindChildButtons(child);
            }
        }
    }

    private void AddPointerDownListener(GameObject buttonObj)
    {
        EventTrigger trigger = buttonObj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = buttonObj.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        entry.callback.AddListener((data) => { OnChildButtonClicked(); });

        trigger.triggers.Add(entry);
    }

    private void OnChildButtonClicked()
    {
        // 让父级按钮置顶
        transform.SetAsLastSibling();
    }
}