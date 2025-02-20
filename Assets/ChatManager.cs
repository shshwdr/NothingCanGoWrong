using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pool;
using UnityEngine;

public enum ChatType
{
    chat,
    respond,
    download
    
}
public class ChatData
{
    public string text;
    public CharacterInfo sender;
    public ChatType type;
    
    public bool isFinished;
    public DialogueInfo dialogueInfo;

    public float angryTime = 20;
    public float angryTimer = 0;
    public bool isFailed;
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

    public float generateChatTimeMin = 6;
    public float generateChatTimeMax = 20;
    public float generateChatTime = 3;
    private float generateChatTimer = 0;

    public bool hasUnfinishedChat = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.Instance.isStarted)
        {
            return;
        }
        generateChatTimer += Time.deltaTime;
        if (generateChatTimer > generateChatTime)
        {
            generateChatTimer = 0;
            generateChatTime  = Random.Range(generateChatTimeMin, generateChatTimeMax);

            var id = Random.Range(1, 2);
            switch (id)
            {
                case 1:
                    GenerateRespondChat();
                    break;
                    case 2:
                        GenerateDownloadChat();
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
        GenerateChat(ChatType.respond, selectChat.text, speaker, selectChat,false,true);
    }
    
   public void GenerateChat(ChatType type,string text, CharacterInfo speaker, DialogueInfo selectChat,bool isFinished,bool noAngry = false)
    {
        var angryTime = type == ChatType.chat ? 20 : 100;
        if (noAngry)
        {
            angryTime = -1;
        }
        ChatData data = new ChatData()
            { text = text, sender = speaker, type = type, isFinished = isFinished, dialogueInfo = selectChat, angryTime = angryTime};
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

    }

    public void respond(string characterId)
    {
        var lastChat = chatDataMap[characterId].LastItem();
        var playerInfo = CSVLoader.Instance.CharacterInfoMap["player"];
        ChatData data = new ChatData()
            { text = lastChat.dialogueInfo.respond, sender = playerInfo, type = ChatType.respond, isFinished = true };
        
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
        ChatData data = new ChatData()
            { text = "I'm angry.", sender = lastChat.sender, type = ChatType.respond, isFinished = true };
        
        LevelManager.Instance.ReduceProductive(10);
        
        
        chatDataMap[characterId].Add(data);
        addChat(characterId);
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
        
                    chatDataMap[characterId].Add(data);
                    addChat(characterId);
                    EventPool.Trigger("UpdateChat");
                }
                else
                {
                    
                    LevelManager.Instance.ReduceProductive(10);
                    ChatData data = new ChatData()
                        { text = "It is NOT finished!!!", sender = lastChat.sender, type = ChatType.chat, isFinished = true };
        
                    chatDataMap[characterId].Add(data);
                    addChat(characterId);
                    EventPool.Trigger("UpdateChat");
                }
            }
            else
            {
                
                LevelManager.Instance.ReduceProductive(10);
                ChatData data = new ChatData()
                    { text = "This is not the file I want.", sender = lastChat.sender, type = ChatType.chat, isFinished = true };
        
                chatDataMap[characterId].Add(data);
                addChat(characterId);
                EventPool.Trigger("UpdateChat");
            }
        }
    }
}
