using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

public class Productive : MonoBehaviour
{
    public HPBar dayRemain;

    public HPBar productiveBar;
    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("UpdateProductive", UpdateProductive);
        UpdateProductive();
    }

    // Update is called once per frame
    void Update()
    {
        dayRemain.SetHP(LevelManager.Instance.gameTime - LevelManager.Instance.gameTimer, LevelManager.Instance.gameTime);
    }
    public void UpdateProductive()
    {
        productiveBar.SetHP(LevelManager.Instance.productive, LevelManager.Instance.productiveMax);
    }
}
