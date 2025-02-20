using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MeetingWindowController : MonoBehaviour
{
    public HPBar focusProgress;
    private RectTransform rect;
    private float progress = 100;
    public float maxProgress = 100;
    public float difficultScale = 1;
    public float progressIncreaseWhenOnTop = 1;
    public float progressDecreaseWhenNotOnTop = -0.5f;

    private float currentTime = 0;
    public float meetingTime = 10;
    
    public GameObject finishedOB;
    public GameObject finishedHideOB;

    private bool isFinished = false;
    // Start is called before the first frame update
    void Start()
    {
         rect = GetComponent<RectTransform>();
         progress = maxProgress;
         finishedOB.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinished)
        {
            return;
        }
        currentTime += Time.deltaTime;
        if (currentTime >= meetingTime)
        {
            finishedOB.SetActive(true);
            finishedHideOB.SetActive(false);
            isFinished = true;
        }
        if (IsOnTop())
        {
            progress += progressIncreaseWhenOnTop * Time.deltaTime;
        }
        else
        {
             progress += progressDecreaseWhenNotOnTop * difficultScale * Time.deltaTime;
        }

        progress = math.clamp(progress, 0, maxProgress);
        focusProgress.SetHP(progress, maxProgress);
    }
    public bool IsOnTop()
    {
        // 获取当前 UI 元素的父级 Transform
        Transform parent = transform.parent;

        if (parent == null)
        {
            Debug.LogWarning("UI 元素没有父级，无法判断是否置顶！");
            return false;
        }

        // 获取当前 UI 元素的索引
        int currentIndex = transform.GetSiblingIndex();

        // 获取父级的子对象总数
        int lastIndex = parent.childCount - 1;

        // 如果索引是最后一个，说明它在顶端
        return currentIndex == lastIndex;
    }
}
