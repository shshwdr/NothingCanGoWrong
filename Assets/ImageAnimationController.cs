using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimationController : MonoBehaviour
{
    public Image targetImage; // UI Image 组件
    public float frameRate = 0.3f; // 每帧间隔时间

    private int currentFrame = 0;
    private Coroutine animationCoroutine; // 当前动画协程
    public Sprite[] animationSprites;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(bool loop = false)
    {

        // 停止当前动画
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(PlayAnimationCoroutine(loop));
    }
    
    IEnumerator PlayAnimationCoroutine(bool  loop)
    {
        Sprite[] frames = animationSprites;
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

    }
}
