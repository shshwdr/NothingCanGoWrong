using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMinionVirusWindow : MonoBehaviour
{
    public HPBar progress;
    public float damageTime = 5;
     float damageTimer = 0;
    public int damage = 5;

    private Virus virus;

    private WindowMover mover;
    // Start is called before the first frame update
    void Start()
    {
        virus = FindObjectOfType<PopupVirus>().GetComponent<Virus>();
        mover = GetComponent<WindowMover>();

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_virus_summon_minion");
    }

    // Update is called once per frame
    void Update()
    {
        if (virus && !virus.isDead && virus.hpRatio < 0.5)
        {
            mover.enabled = true;
        }
        
        progress.SetHP(damageTimer, damageTime);
         damageTimer += Time.deltaTime;
         if (damageTimer > damageTime)
         {
             if (virus&& !virus.isDead)
             {
                 virus.AddProgress(10);
            }
             //ComputerManager.Instance.InflictDamage(damage);
             damageTimer = 0;
         }
    }
}
