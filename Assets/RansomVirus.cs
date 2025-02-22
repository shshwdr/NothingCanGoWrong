using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RansomVirus : MonoBehaviour
{
    public float ransomTime = 10f;

    private float popupTimer = 0;


    public WindowController mainWindow;
    private int id = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        popupTimer += Time.deltaTime;
        if (popupTimer > ransomTime)
        {
            
            if (!mainWindow)
            {
                mainWindow = GetComponent<Virus>().virusWindow.GetComponent<WindowController>();
            }
            
            var allWindows = FindObjectsOfType<WindowController>().ToList();
            allWindows.Remove(mainWindow);

            allWindows = allWindows.Where(x => x.isRansomed == false).ToList();
            if (allWindows.Count > 0)
            {
                var selectedRansom = allWindows.RandomItem();
                selectedRansom.Ransom();
                
                mainWindow.GetComponent<VirusAnimationController>().PlayAnimation("Lock",false);
                
            }
            
            popupTimer = 0;
        }
    }
}
