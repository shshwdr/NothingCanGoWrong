using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusWindowController : MonoBehaviour
{
    [HideInInspector]
    public Virus virus;
    // public Text virusHealthText;

    public HPBar attackProgressBar;
    public HPBar beAttackProgressBar;
    public HPBar hpProgressBar;
    public HPBar progressProgressBar;
    // Start is called before the first frame update
    public void Init(Virus virus)
    {
        this.virus = virus;
        GetComponent<VirusAnimationController>().animationName = virus.animationName;
        GetComponent<VirusAnimationController>().LoadAnimations();
        UpdateUI();
    }

    public void HideVirus()
    {
        GetComponent<VirusAnimationController>().targetImage.gameObject.SetActive(false);
    }
    public void ShowVirus()
    {
        GetComponent<VirusAnimationController>().targetImage.gameObject.SetActive(true);
    }

    public void Die()
    {
        
        GetComponent<VirusAnimationController>().PlayAnimation("Death",false);
        StartCoroutine(StartCoroutine(1f + GetComponent<VirusAnimationController>().getAnimationTime("Death")));
    }
    
    IEnumerator StartCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        WindowManager.Instance.CloseApplication(GetComponent<WindowController>().id);
    }
    void Start()
    {
        
        // attackProgressBar.GetComponentInChildren<Button>().onClick.AddListener(OnAttackButton);
        // beAttackProgressBar.GetComponentInChildren<Button>().onClick.AddListener(OnDefenseButton);
    }

    public void TakeDamage()
    {
        GetComponent<VirusAnimationController>().PlayAnimation("Hurt",false);
    }
    protected virtual void Update()
    {
        if (virus==null || virus.isDead)
        {
            return;
        }
        float timeNow = Time.time;

        progressProgressBar.SetHP(timeNow - virus.spawnTime,virus.corruptionInterval);
        if (timeNow - virus.spawnTime > virus.corruptionInterval)
        {
            virus.AttackPlayer();
            virus.spawnTime = timeNow;
        }
        //玩家攻击病毒
        //if (virus.actionMode == 1)
        // {
        //     beAttackProgressBar.SetHP(timeNow - virus.lastGetAttackTime,virus.getDamageInterval);
        //     
        //         
        //         if (timeNow - virus.lastGetAttackTime > virus.getDamageInterval)
        //         {
        //             if (virus.actionMode == 0)
        //             {
        //             virus.DamageVirus();
        //             }
        //             virus.lastGetAttackTime = timeNow;
        //             UpdateUI();
        //         }
        // }
        //
        //
        // // 病毒定期攻击玩家
        // {
           // attackProgressBar.SetHP(timeNow - virus.lastAttackTime,virus.attackInterval);
            if (timeNow - virus.spawnTime > virus.attackInterval)
            {
        
                //if (virus.actionMode != 1)
                {
                    virus.AttackPlayer();
                }
                //UpdateUI();
            }
        //}

    }

    
    protected void UpdateUI()
    {
        //if (virusHealthText) virusHealthText.text = "病毒血量: " + virusHealth;
        
        hpProgressBar.SetHP(virus.virusHealth, virus.virusMaxHealth);
    }
}
