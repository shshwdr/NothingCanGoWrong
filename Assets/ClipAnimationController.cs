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
    void Awake()
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
        int fidgetIndex = Random.Range(0, 4); // 1, 2, 3
        animator.SetTrigger("idle"+fidgetIndex);
        idleTime = 0f; // 重置 Idle 计时
    }

    public void PlayStart()
    {
        
        animator.SetTrigger("talk");
        GetComponentInParent<ClipController>().ShowDialogue("clip_start",10);
    }
    public void PlayDetectAnim()
    {
        animator.SetTrigger("detect");
        GetComponentInParent<ClipController>().ShowDialogue("clip_detect");
        StartCoroutine(detectNext());
    }

    public void PlayDetectAnim3()
    {
        
        animator.SetTrigger("talk");
        GetComponentInParent<ClipController>().ShowDialogue("clip_detect3",-1);
    }
    IEnumerator detectNext()
    {
        yield return new WaitForSeconds(5);
        
        animator.SetTrigger("talk");
        GetComponentInParent<ClipController>().ShowDialogue("clip_detect2",-1);
        if (!GameManager.Instance.finishVirusAttackTutorial)
        {
            //Time.timeScale = 0;
            if (!FindObjectOfType<AntiVirusWindowController>())
            {
                DeskTop.Instance.openIcon("Anti Virus");
                StartCoroutine(test());
            }
            else
            {
                FindObjectOfType<AntiVirusWindowController>().ShowTutorialSpawn();
            }
        }
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(1);
        
        FindObjectOfType<AntiVirusWindowController>().ShowTutorialSpawn();
    }
    
    public void PlayAttackAnim()
    {
        animator.SetTrigger("attack");
        GetComponentInParent<ClipController>().ShowDialogue("clip_attack",5);
    }

    public void PlayWin()
    {
        animator.SetTrigger("talk");
        GetComponentInParent<ClipController>().ShowDialogue("clip_win",10);
    }
    
    
    public void PlayEndOfDay()
    {
        animator.SetTrigger("talk");
        GetComponentInParent<ClipController>().ShowDialogue("clip_endDay",20);
    }
    void ScheduleNextFidget()
    {
        nextFidgetTime = Random.Range(fidgeTimeMin,fidgeTimeMax); // 3~7秒后触发 Fidget
    }
    
}
