using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
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

    private FMOD.Studio.EventInstance gameplayMusic;

    public List<Virus> virusList = new List<Virus>();
    private void Start()
    {
        LoadLevel(GameManager.Instance.level);
 
        gameplayMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/mus_gameplay");
        gameplayMusic.start();
    }

    public FMOD.Studio.EventInstance GetGameplayMusicInstance()
    {
        return gameplayMusic;
    }

    public void ReduceProductive(int value)
    {
         productive -= value;
         EventPool.Trigger("UpdateProductive");


         if (productive <= 0)
         {
             
             FindObjectOfType<GameOver>(true).gameObject.SetActive(true);
             FindObjectOfType<GameOver>(true).image.sprite = FindObjectOfType<GameOver>(true).fired;
             LevelManager.Instance.isFinished = true;
             //ChatManager.Instance.GenerateDialogue();
         }
    }
    public void LoadLevel(int levelName)
    {
        isFinished = false;
        gameTime = CSVLoader.Instance.LevelInfoDict[level].totalTime;
        level = levelName;
        currentLevelInfo = CSVLoader.Instance.LevelInfoDict[level];
        gameTimer = 0;
        productive = productiveMax;
        virusDataList = new List<VirusData>();
        if (levelName == 1)
        {
            DeskTop.Instance. AddDesktopIcon("Chat");
            ChatManager.Instance.GenerateDialogue("tutorial1_chat1");
            //DeskTop.Instance. AddDesktopIcon("fakePDF","Onboarding");
        }else if (levelName == 2)
        {
            
            DeskTop.Instance. AddDesktopIcon("Chat");
            DeskTop.Instance. AddDesktopIcon("Anti Virus");
            ChatManager.Instance.GenerateDialogue("tutorial2_chat1");
        }else if (levelName == 3)
        {
            
            DeskTop.Instance. AddDesktopIcon("Chat");
            DeskTop.Instance. AddDesktopIcon("Anti Virus");
            ChatManager.Instance.GenerateDialogue("tutorial3_chat1");
        }
        else
        {
            
            //DeskTop.Instance. AddDesktopIcon("My Computer");
            DeskTop.Instance. AddDesktopIcon("Chat");
            DeskTop.Instance. AddDesktopIcon("Anti Virus");
            StartGame();
        }
    }

    public void StartGame()
    {
        ChatManager.Instance.generateChatTime = currentLevelInfo.chatInterval[0];
        ChatManager.Instance.generateChatTimeMin = currentLevelInfo.chatInterval[1];
        ChatManager.Instance.generateChatTimeMax = currentLevelInfo.chatInterval[2];
        ChatManager.Instance.generateFileTimeMin = currentLevelInfo.chatInterval[3];
        ChatManager.Instance.generateFileTimeMax = currentLevelInfo.chatInterval[4];

        MeetingManager.Instance.generateChatTime = currentLevelInfo.meeting[0];
        MeetingManager.Instance.generateChatTimeMin = currentLevelInfo.meeting[1];
        MeetingManager.Instance.generateChatTimeMax = currentLevelInfo.meeting[2];
        for (int i = 0; i < currentLevelInfo.virus.Count; i += 4)
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

        if (Input.GetKeyDown(KeyCode.Home))
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
            StopMusic();
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

    public void StopMusic()
    {
        
        gameplayMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        gameplayMusic.release();
    }
    public void CreateVirus(string virusId)
    {
        gameplayMusic.setParameterByName("Game Mode", 1);

        var virusData = virusDataList[0];
        if (FindObjectOfType<ClipAnimationController>())
        {
            
            FindObjectOfType<ClipAnimationController>().PlayDetectAnim();
        }
        var virus = Instantiate(Resources.Load<GameObject>( "enemy/"+virusId),null);
        virus.GetComponent<Virus>().virusMaxHealth = virusData.hp;
        virus.GetComponent<Virus>().corruptionInterval = virusData.stayTime;
        virusList.Add(virus.GetComponent<Virus>());
    }

    public void Restart()
    {
        GameManager.Instance.RestartLevel();

        gameplayMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        gameplayMusic.release();
    }
}
