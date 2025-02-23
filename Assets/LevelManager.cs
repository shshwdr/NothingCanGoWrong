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
    public bool isLastLevel => GameManager.Instance.level == CSVLoader.Instance.LevelInfoDict.Count;
    public bool isFinalFinished=>isFinished && isLastLevel;
    public bool isFailed = false;
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

        gameplayMusic.setParameterByName("Game Over", 0);
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
             //
             // FindObjectOfType<GameOver>(true).gameObject.SetActive(true);
             // FindObjectOfType<GameOver>(true).image.sprite = FindObjectOfType<GameOver>(true).fired;
             LevelManager.Instance.isFinished = true;
             LevelManager.Instance.isFailed = true;
             productiveLose();
             //ChatManager.Instance.GenerateDialogue();
             
             
         }
    }

    void productiveLose()
    {
        ChatManager.Instance.ClearDialogue();
        foreach (var window in FindObjectsOfType<WindowController>())
        {
            if (window.id != "Anti Virus" && window.id != "Chat")
            {
                window.MinimizeWindow();
            }
        }

        if (FindObjectOfType<ChatWindowController>())
        {
            FindObjectOfType<ChatWindowController>().GetComponent<RectTransform>().anchoredPosition =
                new Vector2(-350, 0); 
            ChatManager.Instance.GenerateDialogue("end_lowProductive");
        }
        else
        {
            DeskTop.Instance.openIcon("Chat");
            StartCoroutine(test3());
        }
        
        if (FindObjectOfType<AntiVirusWindowController>())
        {
            FindObjectOfType<AntiVirusWindowController>().GetComponent<RectTransform>().anchoredPosition =
                new Vector2(350, 100); 
        }
        else
        {
            DeskTop.Instance.openIcon("Anti Virus");
            StartCoroutine(test4());
        }
        
    }
    IEnumerator test3()
    {
        yield return new WaitForSeconds(0.1f);
        FindObjectOfType<ChatWindowController>().GetComponent<RectTransform>().anchoredPosition =
            new Vector2(-350, 0); 
        
        ChatManager.Instance.GenerateDialogue("end_lowProductive");
    }
    
    IEnumerator test4()
    {
        yield return new WaitForSeconds(0.1f);
        FindObjectOfType<AntiVirusWindowController>().GetComponent<RectTransform>().anchoredPosition =
            new Vector2(350, 100); 
    }
    public void LoadLevel(int levelName)
    {
        isFinished = false;
        level = levelName;
        gameTime = CSVLoader.Instance.LevelInfoDict[level].totalTime;
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
        }
        else if (levelName == 3)
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
        ChatManager.Instance.generateMeetingTimeMin = currentLevelInfo.chatInterval[5];
        ChatManager.Instance.generateMeetingTimeMax = currentLevelInfo.chatInterval[6];
        

        // MeetingManager.Instance.generateChatTime = currentLevelInfo.meeting[0];
        // MeetingManager.Instance.generateChatTimeMin = currentLevelInfo.meeting[1];
        // MeetingManager.Instance.generateChatTimeMax = currentLevelInfo.meeting[2];
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
        if (Input.GetMouseButtonDown(0))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_ui_mouse_click");
        }

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
            endADay();

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

    void endADay()
    {
        //Save the highscore
        if(level == 1)
        {
            PlayerPrefs.SetInt("level1Highscore", (int) productive);
        }

        if(level == 2)
        {
            PlayerPrefs.SetInt("level2Highscore", (int) productive);
        }

        if(level == 3)
        {
            PlayerPrefs.SetInt("level3Highscore", (int) productive);
        }

        if(level == 4)
        {
            PlayerPrefs.SetInt("level4Highscore", (int) productive);
        }

        if(level == 5)
        {
            PlayerPrefs.SetInt("level5Highscore", (int) productive);
        }


        isFinished = true;
        if( FindObjectOfType<ClipAnimationController>())
            FindObjectOfType<ClipAnimationController>().PlayEndOfDay();
        //GameManager.Instance.NextLevel();
        StopMusic();

        if (isLastLevel)
        {
            ChatManager.Instance.ClearDialogue();
            foreach (var window in FindObjectsOfType<WindowController>())
            {
                if ( window.id != "Chat")
                {
                    window.MinimizeWindow();
                }
            }

            if (FindObjectOfType<ChatWindowController>())
            {
                FindObjectOfType<ChatWindowController>().GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(0, 0); 
                ChatManager.Instance.GenerateDialogue("end_win1");
            }
            else
            {
                DeskTop.Instance.openIcon("Chat");
                StartCoroutine(test2());
            }
            
            // if (FindObjectOfType<AntiVirusWindowController>())
            // {
            //     FindObjectOfType<AntiVirusWindowController>().GetComponent<RectTransform>().anchoredPosition =
            //         new Vector2(500, 200);
            // }
            // else
            // {
            //     
            //     DeskTop.Instance.openIcon("Anti Virus");
            // }
        }
    }
    IEnumerator test2()
    {
        yield return new WaitForSeconds(0.1f);
        
        FindObjectOfType<ChatWindowController>().GetComponent<RectTransform>().anchoredPosition =
            new Vector2(0, 0); 
        ChatManager.Instance.GenerateDialogue("end_win1");
    }
    public void StopMusic()
    {
        
        gameplayMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        
    }

    public void MusicLose()
    {
        gameplayMusic.setParameterByName("Game Over", 2);
    }
    public void CreateVirus(string virusId)
    {
        gameplayMusic.setParameterByName("Game Mode", 1);

        var virusData = virusDataList[0];

        if (!FindObjectOfType<AntiVirusWindowController>())
        {
            
            DeskTop.Instance.openIcon("Anti Virus");
            //WindowManager.Instance.OpenApplication("Anti Virus","Anti Virus");
            StartCoroutine(test());
        }
        else
        {
            
            if (FindObjectOfType<ClipAnimationController>())
            {
            
                FindObjectOfType<ClipAnimationController>().PlayDetectAnim();
            }
        }
        
        var virus = Instantiate(Resources.Load<GameObject>( "enemy/"+virusId),null);
        virus.GetComponent<Virus>().virusMaxHealth = virusData.hp;
        virus.GetComponent<Virus>().corruptionInterval = virusData.stayTime;
        virusList.Add(virus.GetComponent<Virus>());
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(0.1f);
        if (FindObjectOfType<ClipAnimationController>())
        {
            
            FindObjectOfType<ClipAnimationController>().PlayDetectAnim();
        }
    }

    public void Restart()
    {
        GameManager.Instance.RestartLevel();

        gameplayMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        gameplayMusic.release();
    }

    public void MainMenu()
    {
        GameManager.Instance.MainMenu();
    }
}
