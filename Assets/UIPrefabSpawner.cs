using UnityEngine;
using UnityEngine.UI;

public class UIPrefabSpawner : MonoBehaviour
{
    public RectTransform spawnArea;  // 生成范围（父级 RectTransform）
    public GameObject prefabToSpawn; // 需要生成的 Prefab
    public int spawnCount = 5;       // 生成数量（可选）

    public GameObject tutorialAttack;
    // void Start()
    // {
    //     for (int i = 0; i < spawnCount; i++)
    //     {
    //         SpawnPrefab();
    //     }
    // }

    public void ShowTutorialAttack()
    {
        tutorialAttack.SetActive(true);
    }
    /// <summary>
    /// 生成 Prefab，并确保不会超出 `RectTransform` 边界
    /// </summary>
    public GameObject SpawnPrefab(float destroyTime)
    {
        
        
        if (spawnArea == null || prefabToSpawn == null)
        {
            Debug.LogError("❌ SpawnArea 或 PrefabToSpawn 未设置！");
            return null;
        }

        GameObject newPrefab = Instantiate(prefabToSpawn, spawnArea,transform);
        
        newPrefab.GetComponent<Button>().onClick.AddListener(() =>
        {
            newPrefab.GetComponent<ImageAnimationController>().Play();
            newPrefab.GetComponent<Button>().interactable = false;
            Time.timeScale = 1;
            GameManager.Instance.finishVirusAttackTutorial = true;
            
            Destroy(newPrefab,1);
            GetComponent<VirusWindowController>().virus.DamageVirus();
            if (FindObjectOfType<ClipAnimationController>())
            {
                FindObjectOfType<ClipAnimationController>().PlayAttackAnim();
            }
        });
        Destroy(newPrefab, destroyTime);
        RectTransform prefabRect = newPrefab.GetComponent<RectTransform>();

        // 计算安全的随机位置
        Vector2 safePosition = GetSafeRandomPosition(prefabRect);

        // 设置 Prefab 位置
        prefabRect.anchoredPosition = safePosition;
        return newPrefab;
    }

    /// <summary>
    /// 计算不会超出边界的安全随机位置
    /// </summary>
    private Vector2 GetSafeRandomPosition(RectTransform prefabRect)
    {
        Vector2 areaSize = spawnArea.rect.size;
        Vector2 prefabSize = prefabRect.rect.size;

        float halfPrefabWidth = prefabSize.x / 2;
        float halfPrefabHeight = prefabSize.y / 2;

        float xMin = -areaSize.x / 2 + halfPrefabWidth;
        float xMax = areaSize.x / 2 - halfPrefabWidth;
        float yMin = -areaSize.y / 2 + halfPrefabHeight;
        float yMax = areaSize.y / 2 - halfPrefabHeight;

        float randomX = Random.Range(xMin, xMax);
        float randomY = Random.Range(yMin, yMax);

        return new Vector2(randomX, randomY);
    }
}
