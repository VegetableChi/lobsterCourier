using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 洋流管理系统 - 动态洋流影响玩家移动
/// </summary>
public class OceanCurrentManager : MonoBehaviour
{
    public static OceanCurrentManager Instance { get; private set; }
    
    [Header("洋流区域")]
    public List<CurrentZone> currentZones = new List<CurrentZone>();
    
    [Header("全局洋流")]
    public Vector2 globalCurrent = Vector2.zero;
    public float globalCurrentStrength = 0.5f;
    
    [Header("动态洋流")]
    public bool enableDynamicCurrents = true;
    public float dynamicCurrentChangeInterval = 10f;
    private float lastChangeTime = 0f;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        if (enableDynamicCurrents)
        {
            UpdateDynamicCurrents();
        }
    }
    
    void UpdateDynamicCurrents()
    {
        if (Time.time - lastChangeTime >= dynamicCurrentChangeInterval)
        {
            lastChangeTime = Time.time;
            // 可以添加随机洋流变化逻辑
        }
    }
    
    /// <summary>
    /// 获取指定位置的洋流影响
    /// </summary>
    public Vector2 GetCurrentFlow(Vector2 position)
    {
        Vector2 totalFlow = globalCurrent * globalCurrentStrength;
        
        foreach (var zone in currentZones)
        {
            if (zone.Contains(position))
            {
                totalFlow += zone.flowDirection * zone.flowStrength;
            }
        }
        
        return totalFlow;
    }
    
    /// <summary>
    /// 可视化洋流（编辑器用）
    /// </summary>
    void OnDrawGizmos()
    {
        foreach (var zone in currentZones)
        {
            zone.DrawGizmo();
        }
    }
}

[System.Serializable]
public class CurrentZone
{
    public Vector2 center;
    public float radius = 5f;
    public Vector2 flowDirection = Vector2.right;
    public float flowStrength = 2f;
    public Color debugColor = Color.cyan;
    
    public bool Contains(Vector2 position)
    {
        return Vector2.Distance(position, center) <= radius;
    }
    
    public void DrawGizmo()
    {
        Gizmos.color = debugColor;
        Gizmos.DrawWireSphere(center, radius);
        Gizmos.DrawRay(center, flowDirection.normalized * 2f);
    }
}
