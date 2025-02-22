using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int level = 1;
    public bool finishVirusAttackTutorial = false;
    void Awake()
    {
        CSVLoader.Instance.Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLevel();
        }else 
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousLevel();
        }
        
    }
    public void NextLevel()
    {
        level++;
        if (level > CSVLoader.Instance.LevelInfoDict.Count)
        {
            level = 1;
        }
        LevelManager.Instance.StopMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PreviousLevel()
    {
        level--;
        
        LevelManager.Instance.StopMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void RestartLevel()
    {
        LevelManager.Instance.StopMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //LevelManager.Instance.LoadLevel(level);
    }
}
