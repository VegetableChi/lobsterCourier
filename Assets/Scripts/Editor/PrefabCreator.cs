using UnityEngine;
using UnityEditor;

/// <summary>
/// 预制体批量创建工具
/// 使用方法：菜单栏 → Lobster Courier → 创建预制体
/// </summary>
public class PrefabCreator : EditorWindow
{
    [MenuItem("Lobster Courier/创建预制体/玩家")]
    public static void CreatePlayerPrefab()
    {
        EnsureFolderExists("Assets/Prefabs");
        
        // 创建玩家对象
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        
        // 添加组件
        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        sr.color = Color.red;
        
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        CircleCollider2D collider = player.AddComponent<CircleCollider2D>();
        collider.radius = 0.5f;
        
        player.AddComponent<LobsterController>();
        player.AddComponent<Animator>();
        
        // 创建持握点
        GameObject holdPoint = new GameObject("HoldPoint");
        holdPoint.transform.parent = player.transform;
        holdPoint.transform.localPosition = new Vector3(0.8f, 0, 0);
        
        // 保存预制体
        SavePrefab(player, "Assets/Prefabs/Player.prefab");
    }
    
    [MenuItem("Lobster Courier/创建预制体/包裹")]
    public static void CreatePackagePrefab()
    {
        EnsureFolderExists("Assets/Prefabs");
        
        // 创建包裹
        GameObject package = new GameObject("Package");
        package.tag = "Package";
        
        SpriteRenderer sr = package.AddComponent<SpriteRenderer>();
        sr.color = Color.brown;
        
        Rigidbody2D rb = package.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        
        CircleCollider2D collider = package.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        
        package.AddComponent<Package>();
        
        // 保存预制体
        SavePrefab(package, "Assets/Prefabs/Package.prefab");
    }
    
    [MenuItem("Lobster Courier/创建预制体/送货点")]
    public static void CreateDeliveryPointPrefab()
    {
        EnsureFolderExists("Assets/Prefabs");
        
        // 创建送货点
        GameObject deliveryPoint = new GameObject("DeliveryPoint");
        deliveryPoint.tag = "DeliveryPoint";
        
        SpriteRenderer sr = deliveryPoint.AddComponent<SpriteRenderer>();
        sr.color = Color.green;
        
        CircleCollider2D collider = deliveryPoint.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 2f;
        
        deliveryPoint.AddComponent<DeliveryPoint>();
        
        // 保存预制体
        SavePrefab(deliveryPoint, "Assets/Prefabs/DeliveryPoint.prefab");
    }
    
    [MenuItem("Lobster Courier/创建预制体/障碍物")]
    public static void CreateObstaclePrefab()
    {
        EnsureFolderExists("Assets/Prefabs");
        
        // 创建障碍物
        GameObject obstacle = new GameObject("Obstacle");
        
        SpriteRenderer sr = obstacle.AddComponent<SpriteRenderer>();
        sr.color = new Color(0.3f, 0.2f, 0.1f, 1f); // 棕色
        
        CircleCollider2D collider = obstacle.AddComponent<CircleCollider2D>();
        collider.radius = 1f;
        
        // 保存预制体
        SavePrefab(obstacle, "Assets/Prefabs/Obstacle.prefab");
    }
    
    [MenuItem("Lobster Courier/创建预制体/装饰物")]
    public static void CreateDecorationPrefab()
    {
        EnsureFolderExists("Assets/Prefabs");
        
        // 创建装饰物
        GameObject decoration = new GameObject("Decoration");
        
        SpriteRenderer sr = decoration.AddComponent<SpriteRenderer>();
        sr.color = new Color(0.2f, 0.6f, 0.3f, 1f); // 绿色
        
        // 保存预制体
        SavePrefab(decoration, "Assets/Prefabs/Decoration.prefab");
    }
    
    [MenuItem("Lobster Courier/创建预制体/所有基础预制体")]
    public static void CreateAllPrefabs()
    {
        CreatePlayerPrefab();
        CreatePackagePrefab();
        CreateDeliveryPointPrefab();
        CreateObstaclePrefab();
        CreateDecorationPrefab();
        
        Debug.Log("✅ 所有基础预制体创建完成！");
        EditorUtility.DisplayDialog("预制体创建完成", 
            "已创建以下预制体：\n" +
            "• Player\n" +
            "• Package\n" +
            "• DeliveryPoint\n" +
            "• Obstacle\n" +
            "• Decoration\n\n" +
            "现在可以替换美术资源了。", "好的");
    }
    
    static void EnsureFolderExists(string path)
    {
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
    }
    
    static void SavePrefab(GameObject obj, string path)
    {
        PrefabUtility.SaveAsPrefabAsset(obj, path);
        Debug.Log($"✅ 预制体已保存：{path}");
    }
}
