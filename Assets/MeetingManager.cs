using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingManager : Singleton<MeetingManager>
{
    
    public float generateChatTimeMin = 11;
    public float generateChatTimeMax = 20;
    public float generateChatTime = 5;
    private float generateChatTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        // if (!LevelManager.Instance.isStarted || LevelManager.Instance.isFinished)
        // {
        //     return;
        // }
        // generateChatTimer += Time.deltaTime;
        // if (generateChatTimer > generateChatTime)
        // {
        //     generateChatTimer = 0;
        //     generateChatTime  = Random.Range(generateChatTimeMin, generateChatTimeMax);
        //
        //     GameObject prefab = Resources.Load<GameObject>("Application/Meeting");
        //     prefab.GetComponent<MeetingWindowController>().meetingTime = LevelManager.Instance.currentLevelInfo.meeting[0];
        //     prefab.GetComponent<MeetingWindowController>().maxProgress = LevelManager.Instance.currentLevelInfo.meeting[1];
        //     prefab.GetComponent<MeetingWindowController>().difficultScale = LevelManager.Instance.currentLevelInfo.meeting[2];
        //     WindowManager.Instance.OpenApplication("Meeting","Meeting",prefab);
        // }
    }
    
    public void joinMeeting(string characterId)
    {
        GameObject prefab = Resources.Load<GameObject>("Application/Meeting");
        prefab.GetComponent<MeetingWindowController>().meetingTime = LevelManager.Instance.currentLevelInfo.meeting[0];
        prefab.GetComponent<MeetingWindowController>().maxProgress = LevelManager.Instance.currentLevelInfo.meeting[1];
        prefab.GetComponent<MeetingWindowController>().difficultScale = LevelManager.Instance.currentLevelInfo.meeting[2];
        WindowManager.Instance.OpenApplication("Meeting","Meeting - With "+characterId,prefab,true);
    }
}
