using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScreenFlash : MonoBehaviour
{
    public Image flashImage; // UI Image 组件
    public float flashSpeed = 0.1f; // 闪烁速度
    public int flashCount = 5; // 闪烁次数

    public void StartFlash()
    {
        StartCoroutine(FlashEffect());
    }

    IEnumerator FlashEffect()
    {
        for (int i = 0; i < flashCount; i++)
        {
            flashImage.color = new Color(1, 1, 1, 1); // 变为全白
            yield return new WaitForSeconds(flashSpeed);
            flashImage.color = new Color(1, 1, 1, 0); // 变透明
            yield return new WaitForSeconds(flashSpeed);
        }
    }
}