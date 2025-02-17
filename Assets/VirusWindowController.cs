using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusWindowController : MonoBehaviour
{
    private Virus virus;
    // public Text virusHealthText;

    public HPBar attackProgressBar;
    public HPBar beAttackProgressBar;
    public HPBar hpProgressBar;
    public HPBar progressProgressBar;
    // Start is called before the first frame update
    public void Init(Virus virus)
    {
        this.virus = virus;
        UpdateUI();
    }
    void Start()
    {
        
        attackProgressBar.GetComponentInChildren<Button>().onClick.AddListener(OnAttackButton);
        beAttackProgressBar.GetComponentInChildren<Button>().onClick.AddListener(OnDefenseButton);
    }

    protected virtual void Update()
    {
        float timeNow = Time.time;

        progressProgressBar.SetHP(timeNow - virus.spawnTime,virus.corruptionInterval);
        if (timeNow - virus.spawnTime > virus.corruptionInterval)
        {
            virus.AttackPlayer();
            virus.spawnTime = timeNow;
        }
        //玩家攻击病毒
        //if (virus.actionMode == 1)
        {
            beAttackProgressBar.SetHP(timeNow - virus.lastGetAttackTime,virus.getDamageInterval);
            
                
                if (timeNow - virus.lastGetAttackTime > virus.getDamageInterval)
                {
                    if (virus.actionMode == 0)
                    {
                    virus.DamageVirus();
                    }
                    virus.lastGetAttackTime = timeNow;
                    UpdateUI();
                }
        }
        
        
        // 病毒定期攻击玩家
        {
            attackProgressBar.SetHP(timeNow - virus.lastAttackTime,virus.attackInterval);
            if (timeNow - virus.lastAttackTime > virus.attackInterval)
            {

                if (virus.actionMode != 1)
                {
                    virus.AttackPlayer();
                }
                virus.lastAttackTime = timeNow;
                UpdateUI();
            }
        }

    }

    public void OnAttackButton()
    {
        virus.actionMode = 1;
        attackProgressBar.GetComponentInChildren<SelectableItem>().OnSelect(true);
        beAttackProgressBar.GetComponentInChildren<SelectableItem>().OnSelect(false);
    }
    public void OnDefenseButton()
    {
        virus.actionMode = 0;
        attackProgressBar.GetComponentInChildren<SelectableItem>().OnSelect(false);
        beAttackProgressBar.GetComponentInChildren<SelectableItem>().OnSelect(true);
    }
    
    protected void UpdateUI()
    {
        //if (virusHealthText) virusHealthText.text = "病毒血量: " + virusHealth;
        
        hpProgressBar.SetHP(virus.virusHealth, virus.virusMaxHealth);
    }
}
