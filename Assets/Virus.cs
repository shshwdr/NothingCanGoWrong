using UnityEngine;
using UnityEngine.UI;

public class Virus : MonoBehaviour
{
    public int virusMaxHealth = 30;
    
    [HideInInspector]
    public int virusHealth = 30;  // 病毒血量
    public int attackDamage = 5;   // 病毒攻击玩家的伤害
    public float attackInterval = 7f; // 病毒攻击玩家的间隔
    public float hpRatio => virusHealth / (float)virusMaxHealth;
    //
    // public float getDamageInterval = 3f;
    
    public float corruptionInterval = 30f;

    public bool isDead = false;
    
// [HideInInspector]
//     public float lastAttackTime;
//     [HideInInspector]
//     public float lastGetAttackTime;

   // WindowController windowController;
    
    [HideInInspector]
    public float spawnTime = 0;
    public int actionMode = -1;//0 attack, 1 defense

    public GameObject virusWindowPrefab;
    public GameObject virusWindow;
    public string applicationName = "Virus";
    public string animationName = "Trojan";
    private FMOD.Studio.EventInstance instance;

    public bool canBeAttacked()
    {
        var res = true;
        if (isDead)
        {
            res = false;
        }

        if (GetComponent<WormVirus>())
        {
            if (!GetComponent<WormVirus>().canBeAttacked())
            {
                res = false;
            }
        }

        return res;
    }
    protected virtual void Start()
    {
        //attackButton.onClick.AddListener(DamageVirus);
        spawnTime = Time.time;
        virusHealth = virusMaxHealth;

        createWindow();
        instance = LevelManager.Instance.GetGameplayMusicInstance();
    }

    public void AddProgress(int value)
    {
        virusWindow.GetComponent<VirusAnimationController>().PlayAnimation("PowerUp",false);
        spawnTime -= value;
       // virusWindow.GetComponent<VirusWindowController>().
    }

    void createWindow()
    {
       virusWindow =  WindowManager.Instance. OpenApplication(applicationName, applicationName, virusWindowPrefab,true);
       virusWindow.GetComponent<VirusWindowController>().Init(this);
    }

    

    /// <summary>
    /// 伤害病毒
    /// </summary>
    public virtual void DamageVirus()
    {
        if (isDead)
        {
            return;
        }
        virusHealth -= 10;
        virusWindow.GetComponent<VirusWindowController>().TakeDamage();
        virusWindow.GetComponent<VirusWindowController>().hpProgressBar.SetHP(virusHealth, virusMaxHealth);
        if (virusHealth <= 0)
        {
            
            if( FindObjectOfType<ClipAnimationController>())
                FindObjectOfType<ClipAnimationController>().PlayWin();
            Destroy(gameObject);
            //Destroy(virusWindow);
            isDead = true;
            virusWindow.GetComponent<VirusWindowController>().Die();

            LevelManager.Instance.virusList.Remove(this);
            if (LevelManager.Instance.virusList.Count <= 0)
            {
                instance.setParameterByName("Game Mode", 0);
            }
        }
    }

    /// <summary>
    /// 病毒攻击玩家（调用 ComputerManager）
    /// </summary>
    public virtual void AttackPlayer()
    {
        if (ComputerManager.Instance != null)
        {
            ComputerManager.Instance.InflictDamage(attackDamage);
        }
    }

    /// <summary>
    /// 更新病毒窗口 UI
    /// </summary>
}