using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FakePDF : MonoBehaviour
{
    public TMP_Text text;
    public TMP_Text text2;

    public string textValue;
    public string textValue2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OpenPDF());
    }
    
    
public IEnumerator OpenPDF()
{
    int length = textValue.Length;

    int lengthPerTime =(int)( length / (float)20);
    for (int i = 0; i < length; i += lengthPerTime)
    {
        text.text = textValue.Substring(0, i);
        yield return new WaitForSeconds(0.1f);
    }

    text.text = textValue;
    
    length = textValue2.Length;

    lengthPerTime = length;
    for (int i = 0; i < length; i += lengthPerTime)
    {
        text2.text = textValue2.Substring(0, i);
        yield return new WaitForSeconds(0.2f);
    }

    yield return new WaitForSeconds(0.5f);
    text2.text = textValue2;
    
    yield return new WaitForSeconds(1f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_ui_onboarding_message");

        StartCoroutine(MoveRandomly());
        
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
    
    
    //DeskTop.Instance. AddDesktopIcon("Chat");

    ChatManager.Instance.GenerateDialogue("tutorial1_chat3");

    WindowManager.Instance.CloseApplication("fakePDF");
    DeskTop.Instance.RemoveDesktopIcon("Onboarding");
}

IEnumerator MoveRandomly()
{
    float time = 0;
    while (time < 2)
    {
        var randomTime =  Random.Range(0.01f, 0.3f);
        time += randomTime;
        if (time > 2)
        {
            break;
        }
        var randomPosition = new Vector2(Random.Range(-1000f, 1000f), Random.Range(-800f, 800f));
        
        GetComponent<RectTransform>().anchoredPosition = randomPosition;
        yield return new WaitForSeconds(randomTime);

    }
}
    // Update is called once per frame
    void Update()
    {
        
    }
}
