using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopySpriteToImage : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        image.sprite = spriteRenderer.sprite;
    }
}
