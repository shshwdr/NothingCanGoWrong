using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAdjust : MonoBehaviour
{
    void Start()
    {
        // Texture2D cursorTexture = Resources.Load<Texture2D>("CustomCursor"); // Load your cursor texture
        // Vector2 hotSpot = new Vector2(0,0);
        // //cursorTexture.width / 2, cursorTexture.height / 2
        // // ✅ Check platform and set cursor size properly
        // if (Application.platform == RuntimePlatform.WebGLPlayer)
        // {
        //     Cursor.SetCursor(ResizeTexture(cursorTexture, cursorTexture.width / 2, cursorTexture.height / 2), hotSpot, CursorMode.Auto);
        // }
        // else
        // {
        //     Cursor.SetCursor(ResizeTexture(cursorTexture, cursorTexture.width / 2, cursorTexture.height / 2), hotSpot, CursorMode.Auto);
        //
        //     //Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        // }
    }

// Helper function to resize texture
    Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        Graphics.Blit(source, rt);
        Texture2D result = new Texture2D(newWidth, newHeight);
        RenderTexture.active = rt;
        result.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        result.Apply();
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);
        return result;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
