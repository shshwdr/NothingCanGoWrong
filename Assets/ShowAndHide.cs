using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShowAndHide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        GetComponentInChildren<Image>(true).transform.DOScale(1.5f,0.5f). SetLoops(-1,LoopType.Yoyo);
        GetComponentInChildren<Image>(true).DOFade(1f,0.5f). SetLoops(-1,LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
