using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipAnimationController : MonoBehaviour
{
    public Animator animator;
    
    private float idleTime = 0f;
    private float nextFidgetTime;

    public float fidgeTimeMin = 5;
    public float fidgeTimeMax = 10;
    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
        ScheduleNextFidget();
    }
    
    void Update()
    {
        idleTime += Time.deltaTime;

        // 到达随机 Fidget 触发时间
        if (idleTime >= nextFidgetTime)
        {
            PlayRandomFidget();
            ScheduleNextFidget();
        }
    }
    void PlayRandomFidget()
    {
        int fidgetIndex = Random.Range(1, 4); // 1, 2, 3
        animator.SetTrigger("idle"+fidgetIndex);
        idleTime = 0f; // 重置 Idle 计时
    }
    void ScheduleNextFidget()
    {
        nextFidgetTime = Random.Range(fidgeTimeMin,fidgeTimeMax); // 3~7秒后触发 Fidget
    }
    
}
