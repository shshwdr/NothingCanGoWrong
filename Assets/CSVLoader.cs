using System.Collections;
using System.Collections.Generic;
using Sinbad;
using UnityEngine;

public class DialogueInfo
{
    public string id;
    public string text;
    public string speaker;
    public int type;
    public string otherEvent;
    public string next;
    public string respond;
    

}

public class CharacterInfo
{
    public string id;
    public string name;
    public bool isMan;
    public Sprite icon=>Resources.Load<Sprite>("characterIcons/"+id);
}

public class CSVLoader : Singleton<CSVLoader>
{
    public Dictionary<int, List<DialogueInfo>> DialogueInfoMap = new Dictionary<int, List<DialogueInfo>>();
public Dictionary<string, CharacterInfo> CharacterInfoMap = new Dictionary<string, CharacterInfo>();
public List<CharacterInfo> NPCs = new List<CharacterInfo>();
    public void Init()
    {
        var heroInfos =
            CsvUtil.LoadObjects<DialogueInfo>(GetFileNameWithABTest("dialogue"));
        foreach (var info in heroInfos)
        {
            if (!DialogueInfoMap.ContainsKey(info.type))
            {
                DialogueInfoMap.Add(info.type, new List<DialogueInfo>());
            }
            DialogueInfoMap[info.type].Add(info);
        }
        var characterInfos =
            CsvUtil.LoadObjects<CharacterInfo>(GetFileNameWithABTest("character"));
        foreach (var info in characterInfos)
        {
            CharacterInfoMap.Add(info.id, info);
            if (info.id != "player")
            {
                NPCs.Add(info);
            }
        }
    }
    string GetFileNameWithABTest(string name)
    {
        // if (ABTestManager.Instance.testVersion != 0)
        // {
        //     var newName = $"{name}_{ABTestManager.Instance.testVersion}";
        //     //check if file in resource exist
        //      
        //     var file = Resources.Load<TextAsset>("csv/" + newName);
        //     if (file)
        //     {
        //         return newName;
        //     }
        // }
        return name;
    }
}

