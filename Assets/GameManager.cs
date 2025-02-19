using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int level = 1;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PreviousLevel()
    {
        level--;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //LevelManager.Instance.LoadLevel(level);
    }
}
