using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePDF : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OpenPDF());
    }
public IEnumerator OpenPDF()
{
    yield return new WaitForSeconds(1);
    FindObjectOfType<UIScreenShake>().StartShake();
    FindObjectOfType<UIScreenFlash>().StartFlash();
    yield return new WaitForSeconds(1);
    // var glitchEffect = FindObjectOfType<GlitchEffect>();
    //
    // glitchEffect.StartGlitch();
    // yield return new WaitForSeconds(0.3f);
    // glitchEffect.StartGlitch();
    // yield return new WaitForSeconds(0.4f);
    // glitchEffect.StartGlitch();
    // yield return new WaitForSeconds(0.2f);
    // glitchEffect.StartGlitch();
    // yield return new WaitForSeconds(0.3f);
    // glitchEffect.StartGlitch();
    // yield return new WaitForSeconds(0.1f);
    // glitchEffect.StartGlitch();
    // yield return new WaitForSeconds(0.2f);
    // glitchEffect.StopGlitch();
    
    //yield return new WaitForSeconds(0.5f);
    
    
    DeskTop.Instance. AddDesktopIcon("Chat");

    ChatManager.Instance.GenerateDialogue("tutorial1_chat");

    WindowManager.Instance.CloseApplication("fakePDF");
    DeskTop.Instance.RemoveDesktopIcon("Onboarding");
}
    // Update is called once per frame
    void Update()
    {
        
    }
}
