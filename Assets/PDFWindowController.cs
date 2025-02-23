using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PDFWindowController : MonoBehaviour
{
    private bool isFinished = false;
    public GameObject setNumberGame;

    public TMP_Text descriptionText;

    public TMP_Text number;
    private int targetNumber;
    public Button next;
    public Button back;
    public Button save;
    public string description;

    public GameObject finishedOB;
    // Start is called before the first frame update
    void Start()
    {
        setNumberGame.SetActive(true);

        targetNumber = Random.Range(10, 35);
        descriptionText .text = string.Format(description, targetNumber);
        this.number.text = 0.ToString();
        
        save.onClick.AddListener( Save);


        {
            
            EventTrigger trigger = next.gameObject.GetComponent<EventTrigger>();
            // 添加 PointerDown 事件
            EventTrigger.Entry entryDown = new EventTrigger.Entry();
            entryDown.eventID = EventTriggerType.PointerDown;
            entryDown.callback.AddListener((data) => { OnButtonDown((PointerEventData)data); });
            trigger.triggers.Add(entryDown);

            // 添加 PointerUp 事件
            EventTrigger.Entry entryUp = new EventTrigger.Entry();
            entryUp.eventID = EventTriggerType.PointerUp;
            entryUp.callback.AddListener((data) => { OnButtonUp((PointerEventData)data); });
            trigger.triggers.Add(entryUp);
        }
        
        {
            
            EventTrigger trigger = back.gameObject.GetComponent<EventTrigger>();
            // 添加 PointerDown 事件
            EventTrigger.Entry entryDown = new EventTrigger.Entry();
            entryDown.eventID = EventTriggerType.PointerDown;
            entryDown.callback.AddListener((data) => { OnButtonDown2((PointerEventData)data); });
            trigger.triggers.Add(entryDown);

            // 添加 PointerUp 事件
            EventTrigger.Entry entryUp = new EventTrigger.Entry();
            entryUp.eventID = EventTriggerType.PointerUp;
            entryUp.callback.AddListener((data) => { OnButtonUp2((PointerEventData)data); });
            trigger.triggers.Add(entryUp);
        }
        
    }

    private bool isHoldingIncrease;
    private bool isHoldingDecrease;
    private Coroutine holdCoroutine;
    private float incrementInterval = 0.1f;
    public void OnButtonDown(PointerEventData eventData)
    {
        isHoldingIncrease = true;
        holdCoroutine = StartCoroutine(HoldIncrement());
        GetComponent<WindowController>().windowRect. SetAsLastSibling();
    }

    // 鼠标抬起时调用
    public void OnButtonUp(PointerEventData eventData)
    {
        isHoldingIncrease = false;
        if (holdCoroutine != null)
            StopCoroutine(holdCoroutine);
    }
    
    
    public void OnButtonDown2(PointerEventData eventData)
    {
        isHoldingDecrease = true;
        holdCoroutine = StartCoroutine(HoldDecrease());
        GetComponent<WindowController>().windowRect. SetAsLastSibling();
    }

    // 鼠标抬起时调用
    public void OnButtonUp2(PointerEventData eventData)
    {
        isHoldingDecrease = false;
        if (holdCoroutine != null)
            StopCoroutine(holdCoroutine);
    }

    private IEnumerator HoldIncrement()
    {
        while (isHoldingIncrease)
        {
            this.number.text = (int.Parse(this.number.text) + 1).ToString();
            yield return new WaitForSeconds(incrementInterval);
        }
    }

    private IEnumerator HoldDecrease()
    {
        while (isHoldingDecrease)
        {
            this.number.text = (int.Parse(this.number.text) - 1).ToString();
            yield return new WaitForSeconds(incrementInterval);
        }
    }
    // public void AddNumber()
    // {
    // }
    // public void SubNumber()
    // {
    //     this.number.text = (int.Parse(this.number.text) - 1).ToString();
    // }

    public void Save()
    {
        if (this.number.text == targetNumber.ToString())
        {
            isFinished = true;
            finishedOB.SetActive(true);
            setNumberGame.SetActive(false);
            
            if(!DeskTop.Instance.desktopIcons[GetComponent<WindowController>().titleLabel.text].GetComponent<DesktopIcon>().failedIcon.activeSelf)
            DeskTop.Instance.desktopIcons[GetComponent<WindowController>().titleLabel.text].GetComponent<DesktopIcon>().finishedIcon.SetActive(true);
        }
        
        GetComponent<WindowController>().SetToTop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
