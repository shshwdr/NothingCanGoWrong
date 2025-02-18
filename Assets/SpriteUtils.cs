using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteUtils
{
    public static Sprite GetIcon(string name)
    {
        return Resources.Load<Sprite>("icons/" + name);
    }
}
