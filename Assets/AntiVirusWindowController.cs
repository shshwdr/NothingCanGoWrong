using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;
using UnityEngine.UI;

public class AntiVirusWindowController : MonoBehaviour
{
    
    public HPBar playerHealthBar;

    public Button scanButton;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
