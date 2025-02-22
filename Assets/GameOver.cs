using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Image image;

    public Sprite blue;

    public Sprite fired;
    public TMP_Text text;


    private void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_ui_lose_virus");
    }
}
