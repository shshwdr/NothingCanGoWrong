using UnityEngine;

[ExecuteInEditMode]
public class GlitchEffect : MonoBehaviour
{
    public Material glitchMaterial;
    private float glitchTime = 0f;
    private bool isGlitching = false;

    // void Update()
    // {
    //     if (!isGlitching)
    //     {
    //         glitchTime -= Time.deltaTime;
    //         if (glitchTime <= 0)
    //         {
    //             StartGlitch();
    //         }
    //     }
    //     else
    //     {
    //         glitchTime -= Time.deltaTime;
    //         if (glitchTime <= 0)
    //         {
    //             StopGlitch();
    //         }
    //     }
    // }

    public void StartGlitch()
    {
        isGlitching = true;
        glitchTime = Random.Range(0.3f, 1.0f);
        
        // 随机 UV 偏移
        float randomX = Random.Range(-0.3f, 0.3f);  // 影响 UV 位置偏移
        float randomR = Random.Range(-0.5f, 0.5f);  // 影响红色通道偏移
        float randomB = Random.Range(-0.4f, 0.4f);  // 影响蓝色通道偏移

        glitchMaterial.SetVector("_RandomOffset", new Vector4(randomX, randomR, randomB, 0));
        glitchMaterial.SetFloat("_GlitchStrength", Random.Range(0.4f, 0.8f));
    }

    public void StopGlitch()
    {
        isGlitching = false;
        //glitchTime = Random.Range(2f, 5f);
        glitchMaterial.SetFloat("_GlitchStrength", 0f);
        glitchMaterial.SetVector("_RandomOffset", Vector4.zero);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (glitchMaterial != null)
        {
            Graphics.Blit(source, destination, glitchMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
