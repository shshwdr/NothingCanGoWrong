using UnityEngine;

public class UIScreenShake : MonoBehaviour
{
    public RectTransform canvasTransform;
    public float shakeDuration = 0.5f; // 抖动持续时间
    public float shakeIntensity = 10f; // 抖动强度

    private float elapsedTime = 0f;

    void Update()
    {
        if (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            Vector3 shakeOffset = new Vector3(Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity), 0);
            canvasTransform.anchoredPosition = shakeOffset;
        }
        else
        {
            canvasTransform.anchoredPosition = Vector3.zero;
        }
    }

    public void StartShake()
    {
        elapsedTime = shakeDuration;
    }
}