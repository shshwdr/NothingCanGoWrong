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
}
public class ChatManager : Singleton<ChatManager>
{
    public Dictionary<string, List<ChatData>> chatDataMap = new Dictionary<string, List<ChatData>>();
    public List<string> chatCharacters = new List<string>();
    public GameObject reddot;

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
        generateChatTimer += Time.deltaTime;
        if (generateChatTimer > generateChatTime)
        {
            generateChatTimer = 0;
            generateChatTime  = Random.Range(generateChatTimeMin, generateChatTimeMax);
            GenerateRespondChat();
        }
    }

    void GenerateRespondChat()
    {
        var selectChat = CSVLoader.Instance.DialogueInfoMap[1].RandomItem();
        var npcs = CSVLoader.Instance.NPCs;
        npcs = npcs.Where(x => !chatDataMap.ContainsKey(x.id) || chatDataMap[x.id].LastItem().isFinished ).ToList();
        if (npcs.Count == 0)
        {
            return;
        }
        var speaker = npcs.RandomItem();
        ChatData data = new ChatData()
            { text = selectChat.text, sender = speaker, type = ChatType.respond, isFinished = false, dialogueInfo = selectChat};
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
        
    }

    public void failedRespond(string characterId)
    {
        
        var lastChat = chatDataMap[characterId].LastItem();
        ChatData data = new ChatData()
            { text = "I'm angry.", sender = lastChat.sender, type = ChatType.respond, isFinished = true };
        
        chatDataMap[characterId].Add(data);
        addChat(characterId);
        EventPool.Trigger("UpdateChat");
    }
}
