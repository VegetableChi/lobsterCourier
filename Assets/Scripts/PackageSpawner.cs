using UnityEngine;

/// <summary>
/// 包裹生成器 - 在指定地点生成包裹
/// </summary>
public class PackageSpawner : MonoBehaviour
{
    [Header("生成配置")]
    public Package[] packagePrefabs;
    public float spawnInterval = 30f;
    public int maxActivePackages = 5;
    
    [Header("生成点")]
    public Transform spawnPoint;
    
    [Header("当前状态")]
    public int activePackageCount = 0;
    private float timeSinceLastSpawn = 0f;
    
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        
        if (timeSinceLastSpawn >= spawnInterval && activePackageCount < maxActivePackages)
        {
            SpawnPackage();
            timeSinceLastSpawn = 0f;
        }
    }
    
    public void SpawnPackage()
    {
        if (packagePrefabs.Length == 0 || spawnPoint == null) return;
        
        // 随机选择包裹类型
        Package prefab = packagePrefabs[Random.Range(0, packagePrefabs.Length)];
        
        // 生成包裹
        Package newPackage = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        
        // 随机属性
        newPackage.isFragile = Random.value < 0.2f; // 20% 概率易碎
        newPackage.isUrgent = Random.value < 0.15f; // 15% 概率紧急
        
        // 设置价值
        newPackage.value = Random.Range(10f, 50f);
        if (newPackage.isFragile) newPackage.value *= 1.5f;
        if (newPackage.isUrgent) newPackage.value *= 2f;
        
        activePackageCount++;
        
        Debug.Log($"📦 生成包裹：{newPackage.packageType} (价值：${newPackage.value})");
    }
    
    public void OnPackageCollected()
    {
        activePackageCount--;
    }
    
    void OnDrawGizmos()
    {
        if (spawnPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(spawnPoint.position, 1f);
        }
    }
}
