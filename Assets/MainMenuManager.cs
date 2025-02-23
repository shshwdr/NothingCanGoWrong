using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string playableScene;
    [SerializeField] private GameObject mainMenuObject;
    [SerializeField] private GameObject levelMenuObject;
    [SerializeField] private GameObject hasNotPlayedObject;
    [SerializeField] private GameObject hasPlayedObject;

    void Start()
    {
        if(PlayerPrefs.GetInt("hasPlayed") == 0)
        {
            hasNotPlayedObject.SetActive(true);
        }

        else if(PlayerPrefs.GetInt("hasPlayed") == 1)
        {
            hasPlayedObject.SetActive(true);
        }
    }

    public void StartButton()
    {
        PlayerPrefs.SetInt("lastLevelPlayed", 1);
        PlayerPrefs.SetInt("hasPlayed", 1);
        SceneManager.LoadScene(playableScene);
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene(playableScene);
    }

    public void LevelSelectionButton()
    {
        mainMenuObject.SetActive(false);
        levelMenuObject.SetActive(true);
    }

    public void BackToMainMenuButton()
    {
        mainMenuObject.SetActive(true);
        levelMenuObject.SetActive(false);
    }

    public void LoadLevelFromSelection(int levelNumber)
    {
        PlayerPrefs.SetInt("lastLevelPlayed", levelNumber);
        SceneManager.LoadScene(playableScene);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_ui_mouse_click");
        }
    }
}
