using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VirusAnimationController : MonoBehaviour
{
    public Image targetImage; // UI Image 组件
    public float frameRate = 0.1f; // 每帧间隔时间

    private Dictionary<string, Sprite[]> animations = new Dictionary<string, Sprite[]>(); // 存储所有动画
    private Coroutine animationCoroutine; // 当前动画协程
    private string currentState = ""; // 当前动画状态
    private int currentFrame = 0;
    private string name = "Trojan";
    void Start()
    {
        // 加载所有动画
        LoadAnimations();

        // 播放默认 Idle 动画
        PlayAnimation("Idle",true);
    }

    void LoadAnimations()
    {
        animations["Idle"] = Resources.LoadAll<Sprite>("virus/"+name+"/Idle");
        animations["Attack"] = Resources.LoadAll<Sprite>("virus/"+name+"/Attack");
        animations["Hurt"] = Resources.LoadAll<Sprite>("virus/"+name+"/Hurt");
        animations["Death"] = Resources.LoadAll<Sprite>("virus/"+name+"/Death");
    }

    public void PlayAnimation(string state,bool loop)
    {
        if (!animations.ContainsKey(state)) return;

        // 如果当前状态和要播放的状态相同，则不重复播放
        if (currentState == state) return;

        // 停止当前动画
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);

        currentState = state;
        animationCoroutine = StartCoroutine(PlayAnimationCoroutine(state,loop));
    }

    IEnumerator PlayAnimationCoroutine(string state,bool  loop)
    {
        Sprite[] frames = animations[state];
        currentFrame = 0;

        while (true)
        {
            targetImage.sprite = frames[currentFrame];
            currentFrame = (currentFrame + 1) % frames.Length;
            yield return new WaitForSeconds(frameRate);
            if (currentFrame == 0 && !loop)
            {
                break;
            }
        }

        if (loop != true)
        {
            if (state != "Death")
            {
                PlayAnimation("Idle",true);
            }
        }
    }
}