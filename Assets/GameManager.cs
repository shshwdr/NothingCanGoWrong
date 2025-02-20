using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int level = 2;
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
        
    }
    public void NextLevel()
    {
        level++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        LevelManager.Instance.LoadLevel(level);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //LevelManager.Instance.LoadLevel(level);
    }
}
