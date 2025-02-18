using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pool;
using UnityEngine;
using UnityEngine.UI;

public class AntiVirusWindowController : MonoBehaviour
{
    
    public HPBar playerHealthBar;

    public Button scanButton;
    public Button spawnButton;

    public float spawnAntiTimer = 0;
    public float spawnAntiInterval = 3;

    public HPBar ammo;

    public int ammoMax = 5;
    private int ammoCount = 0;
    public float ammoRefillTime = 2;
    private float ammoRefillTimer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        ammoCount = ammoMax;
        playerHealthBar.SetHP(ComputerManager.Instance.currentPlayerHealth, ComputerManager.Instance.playerMaxHealth);
        EventPool.OptIn("OnPlayerHealthChange", () =>
        {
            playerHealthBar.SetHP(ComputerManager.Instance.currentPlayerHealth, ComputerManager.Instance.playerMaxHealth);
        });
        scanButton.onClick.AddListener(() =>
        {
            Instantiate(Resources.Load<GameObject>( "enemy/popupVirus"),null);
        });
        
        spawnButton.onClick.AddListener(() =>
        {
            
            addAntiVirusBug(true);
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
            virus.PickItem().SpawnPrefab(5);
            spawnAntiTimer = 0;

            if (useAmmo)
            {
                
                ammoCount -= 1;
                ammoCount = Mathf.Clamp(ammoCount, 0, ammoMax);
                ammo.SetHP(ammoCount, ammoMax);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
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
