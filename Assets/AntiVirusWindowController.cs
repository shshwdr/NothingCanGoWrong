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

    public HPBar ammo;

    public int ammoMax = 5;
    private int ammoCount = 0;
    public float ammoRefillTime = 2;
    private float ammoRefillTimer = 0;

    public bool allBugsAtOnce = true;
    public float antivirusBugLifeTime = 5;
    public TMP_Text dayLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        dayLevel.text =   "Day " + LevelManager.Instance.level;
        if (FindObjectOfType<ChatWindowController>())
        {
            FindObjectOfType<ChatWindowController>().UpdateInputStates();
        }
        ammoCount = ammoMax;
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
            
            addAntiVirusBug(true);
        });
        
        shutdownButton.onClick.AddListener(() =>
        {
            GameManager.Instance.NextLevel();
        });
        
    }

    public void addAntiVirusBug(bool useAmmo)
    {
        if (ammoCount <= 0)
        {
            return;
        }
        var virus = FindObjectsOfType<UIPrefabSpawner>().ToList();
        if (virus.Count > 0)
        {
            //Spend all the ammo at once and spawns bugs
            if(allBugsAtOnce)
            {
                UIPrefabSpawner mainVirusItem = virus.PickItem();

                for (int i = 0; i < ammoCount; i++)
                {
                    mainVirusItem.SpawnPrefab(antivirusBugLifeTime);
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
        
        //shutdownButton.gameObject.SetActive(LevelManager.Instance.isFinished);
        
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
             ammoCount = Mathf.Clamp(ammoCount, 0, ammoMax);
             ammo.SetHP(ammoCount, ammoMax);
         }
    }
}
