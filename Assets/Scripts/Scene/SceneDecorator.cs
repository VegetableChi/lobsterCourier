using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 场景装饰器 - 使用新生成的美术资源美化游戏场景
/// 基于 ART_STYLE_GUIDE.md 和 UI_DESIGN_SYSTEM.md
/// </summary>
public class SceneDecorator : MonoBehaviour
{
    [Header("背景配置")]
    public Sprite seabedBackground;
    public GameObject backgroundPrefab;
    
    [Header("障碍物预设")]
    public GameObject coralReefPrefab;
    public GameObject seaweedPrefab;
    public GameObject rockPrefab;
    public GameObject shipwreckPrefab;
    public GameObject shellPrefab;
    
    [Header("装饰配置")]
    public int coralCount = 5;
    public int seaweedCount = 10;
    public int rockCount = 8;
    public int shellCount = 15;
    public int shipwreckCount = 1;
    
    [Header("场景边界")]
    public float minX = -20f;
    public float maxX = 20f;
    public float minY = -10f;
    public float maxY = 10f;
    
    [Header("粒子效果")]
    public GameObject bubblePrefab;
    public float bubbleSpawnRate = 0.5f;
    private float bubbleTimer = 0;
    
    private List<GameObject> spawnedObjects = new List<GameObject>();
    
    void Start()
    {
        DecorateScene();
    }
    
    /// <summary>
    /// 装饰场景
    /// </summary>
    public void DecorateScene()
    {
        Debug.Log("[SceneDecorator] 开始装饰场景...");
        
        // 清理旧装饰
        ClearDecorations();
        
        // 生成背景
        CreateBackground();
        
        // 生成障碍物
        CreateObstacles();
        
        // 生成装饰物
        CreateDecorations();
        
        Debug.Log($"[SceneDecorator] 场景装饰完成，共生成 {spawnedObjects.Count} 个对象");
    }
    
    /// <summary>
    /// 清理所有装饰物
    /// </summary>
    public void ClearDecorations()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear();
    }
    
    /// <summary>
    /// 创建背景
    /// </summary>
    void CreateBackground()
    {
        if (backgroundPrefab != null)
        {
            GameObject bg = Instantiate(backgroundPrefab, Vector3.zero, Quaternion.identity);
            spawnedObjects.Add(bg);
            Debug.Log("[SceneDecorator] 背景已创建");
        }
        else
        {
            // 创建纯色背景作为后备
            GameObject bg = new GameObject("Background");
            SpriteRenderer sr = bg.AddComponent<SpriteRenderer>();
            sr.color = new Color(0.29f, 0.56f, 0.78f); // 海洋蓝
            sr.sortingOrder = -10;
            spawnedObjects.Add(bg);
        }
    }
    
    /// <summary>
    /// 创建障碍物
    /// </summary>
    void CreateObstacles()
    {
        // 珊瑚礁
        for (int i = 0; i < coralCount; i++)
        {
            Vector3 pos = GetRandomPosition();
            pos.y = minY + Random.Range(0f, 3f); // 底部区域
            CreateObject(coralReefPrefab, pos, "CoralReef");
        }
        
        // 海草
        for (int i = 0; i < seaweedCount; i++)
        {
            Vector3 pos = GetRandomPosition();
            pos.y = minY + Random.Range(0f, 5f);
            CreateObject(seaweedPrefab, pos, "Seaweed");
        }
        
        // 岩石
        for (int i = 0; i < rockCount; i++)
        {
            Vector3 pos = GetRandomPosition();
            pos.y = minY + Random.Range(0f, 4f);
            CreateObject(rockPrefab, pos, "Rock");
        }
        
        // 沉船
        for (int i = 0; i < shipwreckCount; i++)
        {
            Vector3 pos = GetRandomPosition();
            pos.y = minY + 2f;
            CreateObject(shipwreckPrefab, pos, "Shipwreck");
        }
    }
    
    /// <summary>
    /// 创建装饰物
    /// </summary>
    void CreateDecorations()
    {
        // 贝壳
        for (int i = 0; i < shellCount; i++)
        {
            Vector3 pos = GetRandomPosition();
            pos.y = minY + Random.Range(0f, 2f);
            CreateObject(shellPrefab, pos, "Shell");
        }
    }
    
    /// <summary>
    /// 创建单个对象
    /// </summary>
    void CreateObject(GameObject prefab, Vector3 position, string name)
    {
        if (prefab != null)
        {
            GameObject obj = Instantiate(prefab, position, Quaternion.identity);
            obj.name = $"{name}_{spawnedObjects.Count}";
            spawnedObjects.Add(obj);
        }
        else
        {
            // 创建占位符
            GameObject placeholder = GameObject.CreatePrimitive(PrimitiveType.Cube);
            placeholder.name = $"{name}_Placeholder";
            placeholder.transform.position = position;
            placeholder.transform.localScale = Vector3.one * 0.5f;
            placeholder.GetComponent<Renderer>().material.color = Color.gray;
            spawnedObjects.Add(placeholder);
        }
    }
    
    /// <summary>
    /// 获取随机位置
    /// </summary>
    Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            0
        );
    }
    
    /// <summary>
    /// 生成气泡效果
    /// </summary>
    void Update()
    {
        bubbleTimer += Time.deltaTime;
        if (bubbleTimer >= bubbleSpawnRate)
        {
            bubbleTimer = 0;
            SpawnBubble();
        }
    }
    
    /// <summary>
    /// 生成单个气泡
    /// </summary>
    void SpawnBubble()
    {
        if (bubblePrefab != null)
        {
            Vector3 pos = new Vector3(
                Random.Range(minX, maxX),
                minY - 1,
                0
            );
            Instantiate(bubblePrefab, pos, Quaternion.identity);
        }
    }
    
    /// <summary>
    /// 设置场景主题
    /// </summary>
    public void SetSceneTheme(SceneTheme theme)
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            switch (theme)
            {
                case SceneTheme.ShallowSea:
                    cam.backgroundColor = new Color(0.29f, 0.56f, 0.78f); // 海洋蓝
                    break;
                case SceneTheme.DeepSea:
                    cam.backgroundColor = new Color(0f, 0f, 0.2f); // 深海蓝
                    break;
                case SceneTheme.CoralReef:
                    cam.backgroundColor = new Color(0.6f, 0.2f, 0.4f); // 珊瑚色
                    break;
                case SceneTheme.Night:
                    cam.backgroundColor = new Color(0f, 0f, 0.1f); // 夜晚
                    break;
            }
        }
    }
}

/// <summary>
/// 场景主题枚举
/// </summary>
public enum SceneTheme
{
    ShallowSea,    // 浅海
    DeepSea,       // 深海
    CoralReef,     // 珊瑚礁
    Night          // 夜晚
}
