using System.Collections;
using DG.Tweening;
using Pool;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ComputerManager : Singleton<ComputerManager>
{

    public int playerMaxHealth = 100; // 玩家血量
    [HideInInspector]
    public int currentPlayerHealth = 100;


    private void Start()
    {
        currentPlayerHealth = playerMaxHealth;
        ammoCount = ammoStart;
    }

    /// <summary>
    /// 让病毒对玩家造成伤害
    /// </summary>
    public void InflictDamage(int damage)
    {
        if (LevelManager.Instance.isFinished)
        {
            return;
        }
        currentPlayerHealth -= damage;
        if (currentPlayerHealth < 0) currentPlayerHealth = 0;
        UpdateUI();

        if (currentPlayerHealth <= 0)
        {
            Debug.Log("💀 玩家电脑崩溃！");
            LevelManager.Instance.isFinished = true;
            failVirus();
        }
    }

    void failVirus()
    {
        ChatManager.Instance.ClearDialogue();
        foreach (var window in FindObjectsOfType<WindowController>())
        {
            if (!window.GetComponent<VirusWindowController>())
            {
                window.MinimizeWindow();
            }
        }
        
        if (FindObjectOfType<VirusWindowController>())
        {
            FindObjectOfType<VirusWindowController>().GetComponent<RectTransform>().anchoredPosition =
                new Vector2(0, 0);
            FindObjectOfType<VirusAnimationController>().PlayAnimation("Idle",true);
            FindObjectOfType<VirusAnimationController>().targetImage.transform.parent = DeskTop.Instance.dragFileArea;
            FindObjectOfType<VirusAnimationController>().targetImage.transform.DOScale(7, 2f);
            StartCoroutine(test());
        }
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(1);
        FindObjectOfType<GameOver>(true).gameObject.SetActive(true);
    }

    public float ammoRefillTime = 2;
    private float ammoRefillTimer = 0;
    public int ammoStart = 0;
    public int ammoCount = 0;
    public int ammoMax = 5;
    public void Update()
    {
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
        
        ComputerManager.Instance.  ammoCount = Mathf.Clamp(ComputerManager.Instance. ammoCount, 0, ComputerManager.Instance. ammoMax);
    }
    /// <summary>
    /// 玩家使用杀毒软件恢复健康
    /// </summary>
    public void RestoreHealth(int amount)
    {
        currentPlayerHealth += amount;
        if (currentPlayerHealth > playerMaxHealth) currentPlayerHealth = playerMaxHealth;
        UpdateUI();
    }

    /// <summary>
    /// 更新 UI
    /// </summary>
    private void UpdateUI()
    {
        EventPool.Trigger("OnPlayerHealthChange");
    }
}