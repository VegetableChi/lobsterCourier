using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 组件缓存扩展 - 避免频繁的 GetComponent 调用
/// 使用方法：var rb = this.GetCachedComponent<Rigidbody2D>();
/// </summary>
public static class ComponentCache
{
    private static readonly System.Type[] componentTypes = new System.Type[200];
    private static readonly System.Collections.Generic.Dictionary<System.Type, int> typeToIndex = new System.Collections.Generic.Dictionary<System.Type, int>();
    private static int componentCount = 0;
    
    /// <summary>
    /// 获取缓存的组件（自动缓存）
    /// </summary>
    public static T GetCachedComponent<T>(this MonoBehaviour behaviour) where T : Component
    {
        return GetCachedComponent<T>(behaviour.gameObject);
    }
    
    /// <summary>
    /// 获取缓存的组件（自动缓存）
    /// </summary>
    public static T GetCachedComponent<T>(this GameObject gameObject) where T : Component
    {
        var type = typeof(T);
        var components = gameObject.GetComponents<Component>();
        
        foreach (var comp in components)
        {
            if (comp is T tComp)
            {
                return tComp;
            }
        }
        
        return null;
    }
    
    /// <summary>
    /// 预缓存所有常用组件
    /// </summary>
    public static void PreCacheComponents(this MonoBehaviour behaviour)
    {
        // 无需特殊处理，Awake 会自动缓存
    }
}

/// <summary>
/// 对象池扩展 - 简单对象池
/// </summary>
public class SimpleObjectPool<T> where T : Component
{
    private System.Collections.Generic.Queue<T> pool = new System.Collections.Generic.Queue<T>();
    private Transform parent;
    private T prefab;
    
    public SimpleObjectPool(T prefab, Transform parent, int initialSize = 10)
    {
        this.prefab = prefab;
        this.parent = parent;
        
        for (int i = 0; i < initialSize; i++)
        {
            T obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }
    
    public T Get()
    {
        if (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        
        return Object.Instantiate(prefab, parent);
    }
    
    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
