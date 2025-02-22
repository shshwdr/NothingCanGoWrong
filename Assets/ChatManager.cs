using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pool;
using UnityEngine;

public enum ChatType
{
    chat,
    respond,
    download,
    meeting,
    
}
public class ChatData
{
    public string text;
    public CharacterInfo sender;
    public ChatType type;

    public bool isDownloaded;
    public bool isFinished;
    public DialogueInfo dialogueInfo;

    public float angryTime = 10;
    public float angryTimer = 0;
    public bool isFailed;
    public bool isRead = false;
}

public class ChatCharacterStatus
{
    public int status = 0;
}

public class ChatManager : Singleton<ChatManager>
{
    public Dictionary<string, List<ChatData>> chatDataMap = new Dictionary<string, List<ChatData>>();
    public List<string> chatCharacters = new List<string>();
   // public Dictionary<string, ChatCharacterStatus> chatCharacterStatusMap = new Dictionary<string, ChatCharacterStatus>();

   public void ClearDialogue()
   {
       chatCharacters.Clear();
   }
   public float generateChatTimeMin = 6;
   public float generateChatTimeMax = 20;
   public float generateFileTimeMin = 6;
   public float generateFileTimeMax = 20;
   public float generateMeetingTimeMin = 6;
   public float generateMeetingTimeMax = 20;
    public float generateChatTime = 3;
    private float generateChatTimer = 0;

    public bool hasUnfinishedChat = false;

    public bool hasUnreadMessage()
    {
        foreach (var characterID in ChatManager.Instance.chatCharacters)
        {
            var isRead = ChatManager.Instance.chatDataMap[characterID].LastItem().isRead;
            if (!isRead)
            {
                return true;
            }
        }

        return false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool isFirst = true;
    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.Instance.isStarted || LevelManager.Instance.isFinished)
        {
            return;
        }
        generateChatTimer += Time.deltaTime;
        if (generateChatTimer > generateChatTime)
        {
            generateChatTimer = 0;


            var id = Random.Range(1, LevelManager.Instance.currentLevelInfo.chatType+1);
            //id = 2;
            if (isFirst && LevelManager.Instance.currentLevelInfo.chatType == LevelManager.Instance.level)
            {
                isFirst = false;
                id = LevelManager.Instance.currentLevelInfo.chatType;
            }
            switch (id)
            {
                case 1:
                    
                    generateChatTime  = Random.Range(generateChatTimeMin, generateChatTimeMax);
                    GenerateRespondChat();
                    break;
                case 2:
                        
                    generateChatTime  = Random.Range(generateFileTimeMin, generateFileTimeMax);
                        GenerateDownloadChat();
                    break;
                case 3:
                    generateChatTime  = Random.Range(generateMeetingTimeMin, generateMeetingTimeMax);
                    GenerateMeetingChat();
                    break;
            }
        }
        
        foreach (var characterID in ChatManager.Instance.chatCharacters.ToList())
        {
            var lastItem = ChatManager.Instance.chatDataMap[characterID].LastItem();
            var notFinished = lastItem.isFinished == false;
            if (notFinished && lastItem.angryTime>0)
            {
                
                lastItem.angryTimer+=Time.deltaTime;
                if (lastItem.angryTimer > lastItem.angryTime)
                {
                    lastItem.angryTimer = 0;
                    lastItem.isFailed = true;
                    ChatManager.Instance.failedRespond(characterID);
                    
                    //EventPool.Trigger("UpdateChat");
                }
                else
                {
                }
            }
            else
            {
            }
        }
    }

    CharacterInfo randomSpeaker()
    {
        
        var npcs = CSVLoader.Instance.NPCs;
        npcs = npcs.Where(x => !chatDataMap.ContainsKey(x.id) || chatDataMap[x.id].LastItem().isFinished ).ToList();
        if (npcs.Count == 0)
        {
            return null;
        }
        var speaker = npcs.RandomItem();
        return speaker;
    }

    void GenerateRespondChat()
    {
        var speaker = randomSpeaker();
        if (speaker == null)
        {
            return;
        }

        var type = ChatType.respond;
        var selectChat = CSVLoader.Instance.DialogueInfoMap[(int)(type)].RandomItem();
        GenerateChat(type, selectChat.text, speaker, selectChat,false);
    }

    void GenerateMeetingChat()
    {
        
        var speaker = randomSpeaker();
        if (speaker == null)
        {
            return;
        }

        var type = ChatType.meeting;
        var selectChat = CSVLoader.Instance.DialogueInfoMap[(int)(type)].RandomItem();
        GenerateChat(ChatType.chat, selectChat.text, speaker, selectChat,true);
        GenerateChat(ChatType.meeting,"",speaker,selectChat,false);
    }
    
    void GenerateDownloadChat()
    {
        var speaker = randomSpeaker();
        if (speaker == null)
        {
            return;
        }

        var type = ChatType.download;
        var selectChat = CSVLoader.Instance.DialogueInfoMap[(int)(type)].RandomItem();
        GenerateChat(ChatType.chat, selectChat.text, speaker, selectChat,true);
        GenerateChat(ChatType.download,"",speaker,selectChat,false);
    }

    public void GenerateDialogue(string key)
    {
        var selectChat = CSVLoader.Instance.DialogueInfoMapById[key];
        var speaker = CSVLoader.Instance.CharacterInfoMap[selectChat.speaker];

        if (selectChat.respond != null && selectChat.respond.Length > 0)
        {
            
            GenerateChat(ChatType.respond, selectChat.text, speaker, selectChat,false,true);
        }
        else
        {
            GenerateChat(ChatType.respond, selectChat.text, speaker, selectChat,true,true);
            if (selectChat.next != "")
            {
                StartCoroutine(chatNext(selectChat.next));
            }
        }
        
    }
    
   public void GenerateChat(ChatType type,string text, CharacterInfo speaker, DialogueInfo selectChat,bool isFinished,bool noAngry = false)
   {
       float angryTime = -1;
        if (noAngry || type == ChatType.chat)
        {
            angryTime = -1;
        }
        else
        {
            
            angryTime = LevelManager.Instance.currentLevelInfo.angry[((int)type) -1 ] ;
        }
        ChatData data = new ChatData()
            { text = text, sender = speaker, type = type, isFinished = isFinished, dialogueInfo = selectChat, angryTime = angryTime};

        if (FindObjectOfType<ChatWindowController>())
        {
            data.isRead = speaker.id == FindObjectOfType<ChatWindowController>().selectedCharacter;
        }
       
        if (!chatDataMap.ContainsKey(speaker.id))
        {
            chatDataMap.Add(speaker.id, new List<ChatData>());
        }
        chatDataMap[speaker.id].Add(data);
        hasUnfinishedChat = true;

        addChat(speaker.id);
        
        EventPool.Trigger("UpdateChat");
    }

    void addChat(string characterId)
    {
        
        if (chatCharacters.Contains(characterId))
        {
            chatCharacters.Remove(characterId);
        }
        chatCharacters.Insert(0, characterId);

        EventPool.Trigger("UpdateDot");
    }

    public void respond(string characterId)
    {
        var lastChat = chatDataMap[characterId].LastItem();
        var playerInfo = CSVLoader.Instance.CharacterInfoMap["player"];
        ChatData data = new ChatData()
            { text = lastChat.dialogueInfo.respond, sender = playerInfo, type = ChatType.respond, isFinished = true };
        
        if(FindObjectOfType<ChatWindowController>())
        data.isRead = lastChat.sender.id == FindObjectOfType<ChatWindowController>().selectedCharacter;
        
        chatDataMap[characterId].Add(data);
        addChat(characterId);
        EventPool.Trigger("UpdateChat");
        
        if (lastChat.dialogueInfo.next != "")
        {
            StartCoroutine(chatNext(lastChat.dialogueInfo.next));
        }
        if (lastChat.dialogueInfo.otherEvent != "")
        {
            switch (lastChat.dialogueInfo.otherEvent)
            {
                case "startgame":
                    LevelManager.Instance.StartGame();
                    break;
                case "installAnti":
                    DeskTop.Instance. AddDesktopIcon("Anti Virus");
                    break;
                case "installFake":
                    DeskTop.Instance. AddDesktopIcon("fakePDF","Onboarding");
                    break;
            }
        }
        
    }

    IEnumerator chatNext(string key)
    {
        yield return new WaitForSeconds(1);
        
        ChatManager.Instance.GenerateDialogue(key);
    }

    public void failedRespond(string characterId)
    {
        
        //ComputerManager.Instance.InflictDamage(5);
        
        var lastChat = chatDataMap[characterId].LastItem();




        var text = CSVLoader.Instance.DialogueInfoMap[10].RandomItem().text;
        ChatData data = new ChatData()
            { text = text, sender = lastChat.sender, type = ChatType.respond, isFinished = true };
        
        if(FindObjectOfType<ChatWindowController>())
        data.isRead = lastChat.sender.id == FindObjectOfType<ChatWindowController>().selectedCharacter;

        var reduceProductive = 10;
        if (lastChat.type == ChatType.meeting)
        {
            reduceProductive = 20;
        }

        if (lastChat.type == ChatType.download)
        {
            var fileName = "important_" + data.sender.name;
            if ( DeskTop.Instance.desktopIcons.ContainsKey(fileName))
            {
                DeskTop.Instance.desktopIcons[fileName].GetComponent<DesktopIcon>().failedIcon.SetActive(true);
            }
            else
            {
                Debug.LogError(fileName +" not found");
            }
        }
        
        LevelManager.Instance.ReduceProductive(reduceProductive);
        
        
        chatDataMap[characterId].Add(data);
        addChat(characterId);
        EventPool.Trigger("UpdateChat");
    }

    public void joinMeeting(string characterId)
    {
        var lastChat = chatDataMap[characterId].LastItem();
        lastChat.isFinished = true;
        EventPool.Trigger("UpdateChat");
    }

    public void addFile(string fileName,bool isFinished)
    {
        var characterId = FindObjectOfType<ChatWindowController>().selectedCharacter;
        if (characterId != "")
        {
            var lastChat = chatDataMap[characterId].LastItem();
            var playerInfo = CSVLoader.Instance.CharacterInfoMap["player"];
            var characterInfo = CSVLoader.Instance.CharacterInfoMap[characterId];
            {
                ChatData data = new ChatData()
                {
                    text = "" /*lastChat.dialogueInfo.respond*/, sender = playerInfo, type = ChatType.download,
                    isFinished = true
                };

                if(FindObjectOfType<ChatWindowController>())
                data.isRead = lastChat.sender.id == FindObjectOfType<ChatWindowController>().selectedCharacter;
                chatDataMap[characterId].Add(data);
                addChat(characterId);
                EventPool.Trigger("UpdateChat");
            }
            
            if (fileName.Contains(characterInfo.name))
            {
                if (isFinished)
                {
                    
                    ChatData data = new ChatData()
                        { text = "Great!", sender = lastChat.sender, type = ChatType.chat, isFinished = true };
        
                    if(FindObjectOfType<ChatWindowController>())
                    data.isRead = lastChat.sender.id == FindObjectOfType<ChatWindowController>().selectedCharacter;
                    chatDataMap[characterId].Add(data);
                    addChat(characterId);
                    EventPool.Trigger("UpdateChat");
                }
                else
                {
                    
                    LevelManager.Instance.ReduceProductive(20);
                    ChatData data = new ChatData()
                        { text = "It is NOT finished!!!", sender = lastChat.sender, type = ChatType.chat, isFinished = true };
        
                    if(FindObjectOfType<ChatWindowController>())
                    data.isRead = lastChat.sender.id == FindObjectOfType<ChatWindowController>().selectedCharacter;
                    chatDataMap[characterId].Add(data);
                    addChat(characterId);
                    EventPool.Trigger("UpdateChat");
                }
            }
            else
            {
                
                LevelManager.Instance.ReduceProductive(20);
                ChatData data = new ChatData()
                    { text = "This is not the file I want.", sender = lastChat.sender, type = ChatType.chat, isFinished = true };
        
                if(FindObjectOfType<ChatWindowController>())
                data.isRead = lastChat.sender.id == FindObjectOfType<ChatWindowController>().selectedCharacter;
                chatDataMap[characterId].Add(data);
                addChat(characterId);
                EventPool.Trigger("UpdateChat");
            }
        }
    }
}
