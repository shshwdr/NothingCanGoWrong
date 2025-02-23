using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WormVirus : MonoBehaviour
{
    [FormerlySerializedAs("popupTime")] public float spawnTime = 5f;

    private float popupTimer = 0;

    public WindowController currentWindow;

    public WindowController mainWindow;

    public bool canBeAttacked()
    {
        return currentWindow == mainWindow;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        popupTimer += Time.deltaTime;
        if (popupTimer > spawnTime)
        {
            if (!mainWindow)
            {
                mainWindow = GetComponent<Virus>().virusWindow.GetComponent<WindowController>();
                currentWindow = mainWindow;
                mainWindow.GetComponent<VirusAnimationController>().targetImage.transform.GetChild(0).GetComponentInChildren<Button>(true).onClick.AddListener(GetHit);
            }

            var allWindows = FindObjectsOfType<WindowController>().ToList();
            allWindows.Remove(mainWindow);
            allWindows.Remove(currentWindow);
            if (allWindows.Count == 0)
            {
                moveCoroutine = StartCoroutine(MoveToWindow(mainWindow));
            }
            else
            {
                moveCoroutine = StartCoroutine(MoveToWindow(allWindows.RandomItem()));
            }
            
            
            popupTimer = 0;
        }
    }

    private Coroutine moveCoroutine;
    
    
    public IEnumerator MoveToWindow(WindowController nextWindow)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_worm_virus_hide");

        mainWindow.GetComponent<VirusAnimationController>().PlayAnimation("Hide",false);
        var time2 = mainWindow.GetComponent<VirusAnimationController>().getAnimationTime("Hide");
        yield return new WaitForSeconds(time2);
        if (currentWindow == mainWindow)
        {

            //显示按钮
            mainWindow.GetComponent<VirusWindowController>().HideVirus();
         
            
            
        }
        else
        {
            
        }

        if (nextWindow!=null && nextWindow.gameObject && nextWindow.gameObject.activeSelf)
        {
            currentWindow = nextWindow;
        }
        else
        {
            
            currentWindow = mainWindow;
        }
        
        
        
        
        if (currentWindow == mainWindow)
        {
            mainWindow.GetComponent<VirusWindowController>().hintText.gameObject.SetActive(false);
            mainWindow.GetComponent<VirusAnimationController>().targetImage.transform.parent =
                mainWindow.VirusCommon;
            mainWindow.GetComponent<VirusWindowController>().ShowVirus();
            
            mainWindow.GetComponent<VirusAnimationController>().PlayAnimation("Reappear",false);
            var time = mainWindow.GetComponent<VirusAnimationController>().getAnimationTime("Reappear");

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_worm_virus_appear");
            yield return new WaitForSeconds(time);
        }
        else
        {
            
            mainWindow.GetComponent<VirusWindowController>().hintText.gameObject.SetActive(true);
            GetComponent<WormVirusSpawner>().spawnArea = currentWindow.content;
            var position = GetComponent<WormVirusSpawner>().spawnPosition(spawnTime,mainWindow.GetComponent<VirusAnimationController>().targetImage.GetComponent<RectTransform>());
            mainWindow.GetComponent<VirusAnimationController>().targetImage.transform.parent = currentWindow.content;
            mainWindow.GetComponent<VirusAnimationController>().targetImage.GetComponent<RectTransform>()
                .anchoredPosition = position;
            
            
            mainWindow.GetComponent<VirusAnimationController>().PlayAnimation("Reappear",false);
            var time = mainWindow.GetComponent<VirusAnimationController>().getAnimationTime("Reappear");
            yield return new WaitForSeconds(time);
            
            
            mainWindow.GetComponent<VirusAnimationController>().PlayAnimation("Minimize",false);
            time = mainWindow.GetComponent<VirusAnimationController>().getAnimationTime("Minimize");
            yield return new WaitForSeconds(time);
            
            if (currentWindow.minimizeButton.gameObject.activeSelf)
            {
                currentWindow.MinimizeWindow();
            }
        }
    }
    
    public void GetHit()
    {
        mainWindow.GetComponent<VirusWindowController>().hintText.gameObject.SetActive(false);
       mainWindow. GetComponent<VirusAnimationController>().targetImage.transform.GetChild(0).gameObject.SetActive(false);
        if (moveCoroutine!=null)
        {
            StopCoroutine(moveCoroutine);
        }

        StartCoroutine(Hide());
    }
    
    IEnumerator Hide()
    {
        popupTimer = 0;
        mainWindow.GetComponent<VirusAnimationController>().PlayAnimation("Hurt",false);
        var time = mainWindow.GetComponent<VirusAnimationController>().getAnimationTime("Hurt");
        yield return new WaitForSeconds(time);
        
        StartCoroutine(MoveToWindow(mainWindow));
        popupTimer = 0;
    }
    
}
