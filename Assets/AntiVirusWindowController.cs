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
    public GameObject noVirusGO;


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
        
        noVirusGO.SetActive( FindObjectsOfType<TargetPointSpawner>().ToList().Count==0);
        updateAmmoCount();
        spawnGo.SetActive(LevelManager.Instance.currentLevelInfo.day != 1);
        
        //DeskTop.Instance.pet.SetActive(true);
        dayLevel.text =   "Day " + LevelManager.Instance.level;
        if (FindObjectOfType<ChatWindowController>())
        {
            FindObjectOfType<ChatWindowController>().UpdateInputStates();
        }
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
            GetComponent<WindowController>().SetToTop();
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
        if (ComputerManager.Instance. ammoCount <= 0)
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

                for (int i = 0; i < ComputerManager.Instance. ammoCount; i++)
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
                    ComputerManager.Instance.  ammoCount -= ComputerManager.Instance. ammoCount;
                    ComputerManager.Instance. ammoCount = Mathf.Clamp(ComputerManager.Instance. ammoCount, 0, ComputerManager.Instance. ammoMax);
                    ammo.SetHP(ComputerManager.Instance. ammoCount, ComputerManager.Instance. ammoMax);
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
                
                    ComputerManager.Instance. ammoCount -= 1;
                    ComputerManager.Instance. ammoCount = Mathf.Clamp(ComputerManager.Instance. ammoCount, 0, ComputerManager.Instance. ammoMax);
                    ammo.SetHP(ComputerManager.Instance. ammoCount, ComputerManager.Instance. ammoMax);
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

        shutdownButton.gameObject.SetActive(LevelManager.Instance.isFinished);
        if (!LevelManager.Instance.isStarted || LevelManager.Instance.isFinished)
        {
            return;
        }
        
        if (LevelManager.Instance.virusList.Count > 0)
        {
            
            noVirusGO.SetActive(false);
            if (LevelManager.Instance.level == 1)
            {
                spawnGo.SetActive(true);
            }
            
            noVirusGO.SetActive(false);

            bool hasAttackableVirus = false;
            foreach (var virus in LevelManager.Instance.virusList)
            {
                if (virus && virus.canBeAttacked())
                {
                    hasAttackableVirus = true;
                    break;
                }   
            }

            spawnButton.interactable = hasAttackableVirus && ComputerManager.Instance. ammoCount > 0;
            
        }
        else
        {
            spawnButton.interactable = false;
            noVirusGO.SetActive(true);
            
        }

        updateAmmoCount();
        
         spawnAntiTimer += Time.deltaTime;
         if (spawnAntiTimer > spawnAntiInterval)
         {
             addAntiVirusBug(false);
             addAntiVirusBug(false);
             addAntiVirusBug(false);
         }
         
         spawnButton.interactable = ComputerManager.Instance. ammoCount > 0;
         
         
    }

    void updateAmmoCount()
    {
        
        ammo.SetHP(ComputerManager.Instance. ammoCount, ComputerManager.Instance. ammoMax);
    }
}
