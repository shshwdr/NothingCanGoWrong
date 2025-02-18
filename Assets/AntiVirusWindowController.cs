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
    
    // Start is called before the first frame update
    void Start()
    {
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
            addAntiVirusBug();
        });
        
    }

    public void addAntiVirusBug()
    {
        var virus = FindObjectsOfType<UIPrefabSpawner>().ToList();
        if (virus.Count > 0)
        {
            virus.PickItem().SpawnPrefab(5);
            spawnAntiTimer = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
         spawnAntiTimer += Time.deltaTime;
         if (spawnAntiTimer > spawnAntiInterval)
         {
             addAntiVirusBug();
         }
    }
}
