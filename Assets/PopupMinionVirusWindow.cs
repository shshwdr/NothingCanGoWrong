using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMinionVirusWindow : MonoBehaviour
{
    public HPBar progress;
    public float damageTime = 5;
     float damageTimer = 0;
    public int damage = 5;
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        progress.SetHP(damageTimer, damageTime);
         damageTimer += Time.deltaTime;
         if (damageTimer > damageTime)
         {
             ComputerManager.Instance.InflictDamage(damage);
             damageTimer = 0;
         }
    }
}
