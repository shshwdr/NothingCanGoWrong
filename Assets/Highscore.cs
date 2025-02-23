using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Highscore : MonoBehaviour
{
    [SerializeField] private int levelNumber;

    // Start is called before the first frame update
    void Start()
    {
        if(levelNumber == 1)
        {
            GetComponent<TextMeshProUGUI>().text = "Score: " + PlayerPrefs.GetInt("level1Highscore") + "/100";
        }

        if(levelNumber == 2)
        {
            GetComponent<TextMeshProUGUI>().text = "Score: " + PlayerPrefs.GetInt("level2Highscore") + "/100";
        }

        if(levelNumber == 3)
        {
            GetComponent<TextMeshProUGUI>().text = "Score: " + PlayerPrefs.GetInt("level3Highscore") + "/100";
        }

        if(levelNumber == 4)
        {
            GetComponent<TextMeshProUGUI>().text = "Score: " + PlayerPrefs.GetInt("level4Highscore") + "/100";
        }

        if(levelNumber == 5)
        {
            GetComponent<TextMeshProUGUI>().text = "Score: " + PlayerPrefs.GetInt("level5Highscore") + "/100";
        }

    }


}
