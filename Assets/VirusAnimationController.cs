using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class VirusAnimationController : MonoBehaviour
{
    public Image targetImage; // UI Image 组件
    public float frameRate = 0.1f; // 每帧间隔时间

    private Dictionary<string, Sprite[]> animations = new Dictionary<string, Sprite[]>(); // 存储所有动画
    private Coroutine animationCoroutine; // 当前动画协程
    private string currentState = ""; // 当前动画状态
    private int currentFrame = 0;
    public string animationName = "Trojan";
    void Start()
    {
        // 加载所有动画

        // 播放默认 Idle 动画
    }

    public void LoadAnimations()
    {
        animations["Idle"] = Resources.LoadAll<Sprite>("virus/"+animationName+"/Idle");
        animations["Attack"] = Resources.LoadAll<Sprite>("virus/"+animationName+"/Attack");
        animations["Hurt"] = Resources.LoadAll<Sprite>("virus/"+animationName+"/Hurt");
        animations["Death"] = Resources.LoadAll<Sprite>("virus/"+animationName+"/Death");
        animations["PowerUp"] = Resources.LoadAll<Sprite>("virus/"+animationName+"/PowerUp");
        animations["Spawn"] = Resources.LoadAll<Sprite>("virus/"+animationName+"/Spawn");
        PlayAnimation("Spawn",false);
    }

    public float getAnimationTime(string state)
    {
        if (!animations.ContainsKey(state))
        {
            return 0;
        }
        return animations[currentState].Length * frameRate;
    }
    public void PlayAnimation(string state,bool loop)
    {
        if (!animations.ContainsKey(state)) return;

        // 如果当前状态和要播放的状态相同，则不重复播放
        if (currentState == state) return;

        if (animations[state].Length == 0)
        {
            return;
        }
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