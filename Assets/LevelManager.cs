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
        var levelInfo = CSVLoader.Instance.LevelInfoDict[level];
        ChatManager.Instance.generateChatTime = levelInfo.chatInterval[0];
        ChatManager.Instance.generateChatTimeMin = levelInfo.chatInterval[1];
        ChatManager.Instance.generateChatTimeMax = levelInfo.chatInterval[2];

        for (int i = 0; i < levelInfo.virus.Count; i += 3)
        {
            virusDataList.Add(new VirusData
            {
                id = levelInfo.virus[i],
                level = int.Parse(levelInfo.virus[i + 1]),
                spawnTime = int.Parse(levelInfo.virus[i + 2]),
            });
        }
        
        
        isStarted = true;
    }

    private void Update()
    {
        if (isStarted)
        {
            gameTimer += Time.deltaTime;
        }

        if (gameTimer > gameTime)
        {
            isFinished = true;
            //GameManager.Instance.NextLevel();
        }

        if (virusDataList.Count > 0)
        {
            if (gameTimer > virusDataList[0].spawnTime)
            {
                var virusData = virusDataList[0];
                virusDataList.RemoveAt(0);
                CreateVirus(virusData.id);
            }
        }
    }

    public void CreateVirus(string virusId)
    {
        FindObjectOfType<ClipAnimationController>().PlayDetectAnim();
        Instantiate(Resources.Load<GameObject>( "enemy/"+virusId),null);
    }
}
