using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingManager : MonoBehaviour
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
        if (!LevelManager.Instance.isStarted)
        {
            return;
        }
        generateChatTimer += Time.deltaTime;
        if (generateChatTimer > generateChatTime)
        {
            generateChatTimer = 0;
            generateChatTime  = Random.Range(generateChatTimeMin, generateChatTimeMax);

            GameObject prefab = Resources.Load<GameObject>("Application/Meeting");
            WindowManager.Instance.OpenApplication("Meeting","Meeting",prefab);
        }
    }
}
