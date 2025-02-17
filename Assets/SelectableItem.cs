using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableItem : MonoBehaviour
{
    public bool isSelected = false;
    public GameObject selectOb;
    public void OnSelect(bool isSelected)
    {
        this.isSelected = !isSelected;
        if (isSelected)
        {
            selectOb.SetActive(true);
        }
        else
        {
            selectOb.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
