using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupVirus : MonoBehaviour
{
    
    public float popupTime = 5f;

    private float popupTimer = 0;

    public GameObject minionPrefab;

    private int id = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         popupTimer += Time.deltaTime;
         if (popupTimer > popupTime)
         {
             GetComponent<Virus>().virusWindow.GetComponent<VirusAnimationController>().PlayAnimation("Attack",false);
             StartCoroutine(test());
             id++;
             popupTimer = 0;
         }
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(1);
        
        WindowManager.Instance.OpenApplication("Ads","Ads",minionPrefab,true,true);
    }
}
