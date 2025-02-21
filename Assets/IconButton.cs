using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconButton : MonoBehaviour
{
    public GameObject selectedGO;
    public Image image;
    public TMP_Text nameLabel;
    public GameObject redDot;
    public Button button;


    public void Select()
    {
        selectedGO.SetActive(true);
    }
    public void UnSelect()
    {
        selectedGO.SetActive(false);
    }
}
