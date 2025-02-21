using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AntiVirusWindowController : MonoBehaviour
{
    
    public HPBar playerHealthBar;

    public Button scanButton;
    public Button spawnButton;

    public float spawnAntiTimer = 0;
    public float spawnAntiInterval = 3;

    public Button shutdownButton;
    public GameObject spawnGo;
    public HPBar ammo;

    public int ammoMax = 5;
    public int ammoStart = 0;
    private int ammoCount = 0;
    public float ammoRefillTime = 2;
    private float ammoRefillTimer = 0;

    public bool allBugsAtOnce = true;
    public float antivirusBugLifeTime = 5;
    public TMP_Text dayLevel;

    public GameObject tutorialSpawn;
    

    public void ShowTutorialSpawn()
    {
        tutorialSpawn.SetActive(true);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
        updateAmmoCount();
        spawnGo.SetActive(LevelManager.Instance.currentLevelInfo.day != 1);
        
        DeskTop.Instance.pet.SetActive(true);
        dayLevel.text =   "Day " + LevelManager.Instance.level;
        if (FindObjectOfType<ChatWindowController>())
        {
            FindObjectOfType<ChatWindowController>().UpdateInputStates();
        }
        ammoCount = ammoStart;
        playerHealthBar.SetHP(ComputerManager.Instance.currentPlayerHealth, ComputerManager.Instance.playerMaxHealth);
        EventPool.OptIn("OnPlayerHealthChange", () =>
        {
            playerHealthBar.SetHP(ComputerManager.Instance.currentPlayerHealth, ComputerManager.Instance.playerMaxHealth);
        });
        // scanButton.onClick.AddListener(() =>
        // {
        //     Instantiate(Resources.Load<GameObject>( "enemy/popupVirus"),null);
        // });
        
        spawnButton.onClick.AddListener(() =>
        {
            tutorialSpawn.SetActive(false);
            addAntiVirusBug(true);

        });
        
        shutdownButton.onClick.AddListener(() =>
        {
            Camera.main.GetComponent<ShutdownBlit>().StartShutdown();
            StartCoroutine(startNext());
        });
        
    }

    IEnumerator startNext()
    {
        yield return new WaitForSeconds(1);
        GameManager.Instance.NextLevel();
    }

    public void addAntiVirusBug(bool useAmmo)
    {
        if (ammoCount <= 0)
        {
            return;
        }
        var virus = FindObjectsOfType<TargetPointSpawner>().ToList();
        if (virus.Count > 0)
        {
            //Spend all the ammo at once and spawns bugs
            if(allBugsAtOnce)
            {
                TargetPointSpawner mainVirusItem = virus.PickItem();

                for (int i = 0; i < ammoCount; i++)
                {
                    var go = mainVirusItem.SpawnPrefab(antivirusBugLifeTime);
                    
                    if(i == 0 && !GameManager.Instance.finishVirusAttackTutorial)
            {
                
                FindObjectOfType<ClipAnimationController>().PlayDetectAnim3();
                go.transform.Find("tutorialAttack").gameObject.SetActive(true);
            }
                }

                spawnAntiTimer = 0;

                if (useAmmo)
                {                
                    ammoCount -= ammoCount;
                    ammoCount = Mathf.Clamp(ammoCount, 0, ammoMax);
                    ammo.SetHP(ammoCount, ammoMax);
                }
            }

            //Spend one ammo to spawn one bug
            else
            {
                print("None");

                virus.PickItem().SpawnPrefab(antivirusBugLifeTime);
                spawnAntiTimer = 0;

                if (useAmmo)
                {
                
                    ammoCount -= 1;
                    ammoCount = Mathf.Clamp(ammoCount, 0, ammoMax);
                    ammo.SetHP(ammoCount, ammoMax);
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!LevelManager.Instance.isStarted || LevelManager.Instance.isFinished)
        {
            return;
        }
        
        if (LevelManager.Instance.virusList.Count > 0)
        {
            
            if (LevelManager.Instance.level == 1)
            {
                spawnGo.SetActive(LevelManager.Instance.currentLevelInfo.day != 1);
            }

            bool hasAttackableVirus = false;
            foreach (var virus in LevelManager.Instance.virusList)
            {
                if (virus && virus.canBeAttacked())
                {
                    hasAttackableVirus = true;
                    break;
                }   
            }

            spawnButton.interactable = hasAttackableVirus && ammoCount > 0;
            
        }
        else
        {
            spawnButton.interactable = false;
            
        }
        
        
        shutdownButton.gameObject.SetActive(LevelManager.Instance.isFinished);
        
         spawnAntiTimer += Time.deltaTime;
         if (spawnAntiTimer > spawnAntiInterval)
         {
             addAntiVirusBug(false);
             addAntiVirusBug(false);
             addAntiVirusBug(false);
         }
         
         spawnButton.interactable = ammoCount > 0;
         
         ammoRefillTimer += Time.deltaTime;
         if (ammoRefillTimer > ammoRefillTime)
         {
             ammoRefillTimer = 0;
             ammoCount += 1;
             updateAmmoCount();
         }
    }

    void updateAmmoCount()
    {
        
        ammoCount = Mathf.Clamp(ammoCount, 0, ammoMax);
        ammo.SetHP(ammoCount, ammoMax);
    }
}
