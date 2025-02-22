using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ShutdownBlit : MonoBehaviour
{
    public Material shutdownMaterial;
    private float progress = 0f;
    private bool shuttingDown = false;

    void Start()
    {
        shutdownMaterial.SetFloat("_Progress", 0f);
    }

    public void StartShutdown()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_ui_shutdown_win");

        if (!shuttingDown)
        {
            shuttingDown = true;
            StartCoroutine(ShutdownSequence());
        }
    }

    IEnumerator ShutdownSequence()
    {
        while (progress < 1f)
        {
            progress += Time.deltaTime * 1.5f;
            shutdownMaterial.SetFloat("_Progress", progress);
            yield return null;
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (shutdownMaterial != null)
        {
            Graphics.Blit(source, destination, shutdownMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}