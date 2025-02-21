using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Lock : MonoBehaviour
{
    private int add1;
    private int add2;

    private int sum;

    public TMP_Text label1;
    public TMP_Text label2;
    public TMP_InputField labelSum;

    private void Start()
    {
        
        labelSum.onValueChanged.AddListener((string value) =>
        {
            
             
            if (value == sum.ToString().Trim())
            {
                GetComponentInParent<WindowController>().Unlock();
            }
        });
    }

    // Start is called before the first frame update
    public void Init()
    {
        var minValue = 3;
        var maxValue = 10;
        
        add1 = Random.Range(minValue, maxValue);
        
        if (FindObjectOfType<RansomVirus>())
        {
            if (FindObjectOfType<RansomVirus>().GetComponent<Virus>().hpRatio < 0.5f)
            {
                minValue = 15;
                maxValue = 50;
            }
        }
        
        add2 = Random.Range(minValue, maxValue);
        sum = add1 + add2;
        label1.text = add1.ToString();
        label2.text = add2.ToString();
        labelSum.text = "";
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
