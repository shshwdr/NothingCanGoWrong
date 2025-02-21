using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

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
            }

            var allWindows = FindObjectsOfType<WindowController>().ToList();
            allWindows.Remove(mainWindow);
            allWindows.Remove(currentWindow);
            if (allWindows.Count == 0)
            {
                MoveToWindow(mainWindow);
            }
            else
            {
                MoveToWindow(allWindows.RandomItem());
            }
            
            
            popupTimer = 0;
        }
    }

    public void MoveToWindow(WindowController nextWindow)
    {

        if (currentWindow == mainWindow)
        {
            mainWindow.GetComponent<VirusWindowController>().HideVirus();
        }
        
        
        currentWindow = nextWindow;
        
        
        
        if (currentWindow == mainWindow)
        {
            mainWindow.GetComponent<VirusWindowController>().ShowVirus();
        }
        else
        {
            GetComponent<WormVirusSpawner>().spawnArea = currentWindow.content;
            GetComponent<WormVirusSpawner>().SpawnPrefab(spawnTime);
            if (currentWindow.minimizeButton.gameObject.activeSelf)
            {
                currentWindow.MinimizeWindow();
            }
        }
    }
    
    public void GetHit()
    {
        MoveToWindow(mainWindow);
        popupTimer = 0;
    }
}
