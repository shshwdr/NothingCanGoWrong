using System;
using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;
using UnityEngine.Serialization;

public class VirusData
{
    public string id;
    public float spawnTime;
    public int level;
    public int hp;
    public int stayTime;
    public int otherValues;
}
public class LevelManager : Singleton<LevelManager>
{
    public bool isFinished = false;
    public float gameTimer = 0;
    public bool isStarted = false;
    public int level ;

    public int productiveMax = 100;
    public float productive = 100;
    
    List<VirusData> virusDataList = new List<VirusData>();
    public float gameTime ;
    public LevelInfo currentLevelInfo;
    private void Start()
    {
        LoadLevel(GameManager.Instance.level);
    }

    public void ReduceProductive(int value)
    {
         productive -= value;
         EventPool.Trigger("UpdateProductive");
    }
    public void LoadLevel(int levelName)
    {
        isFinished = false;
        gameTime = CSVLoader.Instance.LevelInfoDict[level].totalTime;
        level = levelName;
        gameTimer = 0;
        productive = productiveMax;
        virusDataList = new List<VirusData>();
        if (levelName == 1)
        {
            DeskTop.Instance. AddDesktopIcon("fakePDF","Onboarding");
        }
        else
        {
            
            DeskTop.Instance. AddDesktopIcon("My Computer");
            DeskTop.Instance. AddDesktopIcon("Chat");
            DeskTop.Instance. AddDesktopIcon("Anti Virus");
            StartGame();
        }
    }

    public void StartGame()
    {
        currentLevelInfo = CSVLoader.Instance.LevelInfoDict[level];
        ChatManager.Instance.generateChatTime = currentLevelInfo.chatInterval[0];
        ChatManager.Instance.generateChatTimeMin = currentLevelInfo.chatInterval[1];
        ChatManager.Instance.generateChatTimeMax = currentLevelInfo.chatInterval[2];

        MeetingManager.Instance.generateChatTime = currentLevelInfo.meeting[0];
        MeetingManager.Instance.generateChatTimeMin = currentLevelInfo.meeting[1];
        MeetingManager.Instance.generateChatTimeMax = currentLevelInfo.meeting[2];
        for (int i = 0; i < currentLevelInfo.virus.Count; i += 5)
        {
            virusDataList.Add(new VirusData
            {
                id = currentLevelInfo.virus[i],
                spawnTime = int.Parse(currentLevelInfo.virus[i + 1]),
                hp = int.Parse(currentLevelInfo.virus[i + 2]),
                stayTime = int.Parse    (currentLevelInfo.virus[i + 3]),
            });
        }

        StartCoroutine(startEnumerator());
    }

    IEnumerator startEnumerator()
    {
        yield return new WaitForSeconds(2);
        
        isStarted = true;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        if (!isStarted || isFinished)
        {
            return;
        }
        if (isStarted)
        {
            gameTimer += Time.deltaTime;
        }

        if (gameTimer > gameTime)
        {
            isFinished = true;
            if( FindObjectOfType<ClipAnimationController>())
            FindObjectOfType<ClipAnimationController>().PlayEndOfDay();
            //GameManager.Instance.NextLevel();
        }

        if (virusDataList.Count > 0)
        {
            if (gameTimer > virusDataList[0].spawnTime)
            {
                var virusData = virusDataList[0];
                CreateVirus(virusData.id);
                virusDataList.RemoveAt(0);
            }
        }
    }

    public void CreateVirus(string virusId)
    {
        var virusData = virusDataList[0];
        FindObjectOfType<ClipAnimationController>().PlayDetectAnim();
        var virus = Instantiate(Resources.Load<GameObject>( "enemy/"+virusId),null);
        virus.GetComponent<Virus>().virusMaxHealth = virusData.hp;
        virus.GetComponent<Virus>().corruptionInterval = virusData.stayTime;
    }

    public void Restart()
    {
        GameManager.Instance.RestartLevel();
    }
}
