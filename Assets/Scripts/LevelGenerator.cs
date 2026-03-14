using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 关卡生成器 - 程序化生成海底世界
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    [Header("关卡配置")]
    public int levelSeed;
    public float worldSize = 100f;
    public int obstacleCount = 20;
    public int currentZoneCount = 5;
    public int deliveryPointCount = 8;
    
    [Header("预制体")]
    public GameObject[] obstaclePrefabs;
    public GameObject[] decorationPrefabs;
    public GameObject deliveryPointPrefab;
    public GameObject currentZonePrefab;
    
    [Header("生成区域")]
    public List<Vector2> availableSpawnPoints = new List<Vector2>();
    public List<Vector2> obstaclePositions = new List<Vector2>();
    
    [Header("已生成对象")]
    public List<GameObject> generatedObjects = new List<GameObject>();
    
    void Start()
    {
        GenerateLevel();
    }
    
    public void GenerateLevel()
    {
        // 清理旧对象
        ClearGeneratedObjects();
        
        // 设置随机种子
        if (levelSeed == 0)
        {
            levelSeed = Random.Range(0, 10000);
        }
        Random.InitState(levelSeed);
        
        Debug.Log($"🌊 生成关卡，种子：{levelSeed}");
        
        // 生成障碍物
        GenerateObstacles();
        
        // 生成装饰物
        GenerateDecorations();
        
        // 生成送货点
        GenerateDeliveryPoints();
        
        // 生成洋流区域
        GenerateCurrentZones();
        
        // 保存可用生成点
        GenerateSpawnPoints();
    }
    
    void ClearGeneratedObjects()
    {
        foreach (var obj in generatedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        generatedObjects.Clear();
        availableSpawnPoints.Clear();
        obstaclePositions.Clear();
    }
    
    void GenerateObstacles()
    {
        for (int i = 0; i < obstacleCount; i++)
        {
            Vector2 pos = GetRandomPosition();
            
            // 确保不重叠
            if (IsPositionValid(pos, 3f))
            {
                GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                GameObject obstacle = Instantiate(prefab, pos, Quaternion.identity);
                generatedObjects.Add(obstacle);
                obstaclePositions.Add(pos);
                
                // 添加碰撞体
                if (obstacle.GetComponent<Collider2D>() == null)
                {
                    obstacle.AddComponent<CircleCollider2D>().radius = 1f;
                }
            }
        }
    }
    
    void GenerateDecorations()
    {
        int decorationCount = obstacleCount * 2;
        for (int i = 0; i < decorationCount; i++)
        {
            Vector2 pos = GetRandomPosition();
            
            if (IsPositionValid(pos, 1f))
            {
                GameObject prefab = decorationPrefabs[Random.Range(0, decorationPrefabs.Length)];
                GameObject decoration = Instantiate(prefab, pos, Quaternion.identity);
                generatedObjects.Add(decoration);
                
                // 随机缩放
                float scale = Random.Range(0.5f, 1.5f);
                decoration.transform.localScale = Vector3.one * scale;
                
                // 随机旋转
                decoration.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            }
        }
    }
    
    void GenerateDeliveryPoints()
    {
        for (int i = 0; i < deliveryPointCount; i++)
        {
            Vector2 pos = GetRandomPosition();
            
            // 送货点需要更大的间距
            if (IsPositionValid(pos, 8f))
            {
                GameObject deliveryPoint = Instantiate(deliveryPointPrefab, pos, Quaternion.identity);
                generatedObjects.Add(deliveryPoint);
                
                // 设置客户信息
                DeliveryPoint point = deliveryPoint.GetComponent<DeliveryPoint>();
                if (point != null)
                {
                    point.customerName = GetRandomCustomerName();
                    point.customerType = (CustomerType)Random.Range(0, System.Enum.GetNames(typeof(CustomerType)).Length);
                }
                
                obstaclePositions.Add(pos);
            }
        }
    }
    
    void GenerateCurrentZones()
    {
        OceanCurrentManager oceanManager = FindObjectOfType<OceanCurrentManager>();
        if (oceanManager == null)
        {
            GameObject managerObj = new GameObject("OceanCurrentManager");
            oceanManager = managerObj.AddComponent<OceanCurrentManager>();
        }
        
        for (int i = 0; i < currentZoneCount; i++)
        {
            Vector2 center = GetRandomPosition();
            float radius = Random.Range(3f, 8f);
            Vector2 flowDir = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
            float strength = Random.Range(1f, 3f);
            
            CurrentZone zone = new CurrentZone
            {
                center = center,
                radius = radius,
                flowDirection = flowDir,
                flowStrength = strength
            };
            
            oceanManager.currentZones.Add(zone);
        }
    }
    
    void GenerateSpawnPoints()
    {
        // 创建一些固定的包裹生成点
        for (int i = 0; i < 5; i++)
        {
            Vector2 pos = GetRandomPosition();
            if (IsPositionValid(pos, 5f))
            {
                availableSpawnPoints.Add(pos);
            }
        }
    }
    
    Vector2 GetRandomPosition()
    {
        float x = Random.Range(-worldSize / 2f, worldSize / 2f);
        float y = Random.Range(-worldSize / 2f, worldSize / 2f);
        return new Vector2(x, y);
    }
    
    bool IsPositionValid(Vector2 pos, float minDistance)
    {
        foreach (Vector2 existing in obstaclePositions)
        {
            if (Vector2.Distance(pos, existing) < minDistance)
            {
                return false;
            }
        }
        return true;
    }
    
    string GetRandomCustomerName()
    {
        string[] names = new string[]
        {
            "星星", "八爪", "珊瑚", "钳子",
            "卷卷", "飘飘", "老海", "深深",
            "彩彩", "壳壳", "藻藻", "星宝"
        };
        return names[Random.Range(0, names.Length)];
    }
    
    void OnDrawGizmos()
    {
        // 绘制世界边界
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(worldSize, worldSize, 0));
        
        // 绘制生成点
        Gizmos.color = Color.green;
        foreach (Vector2 pos in availableSpawnPoints)
        {
            Gizmos.DrawSphere(pos, 0.5f);
        }
    }
}
