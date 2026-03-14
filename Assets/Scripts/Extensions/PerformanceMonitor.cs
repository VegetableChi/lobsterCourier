using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// 性能监控器 - 实时监控游戏性能
/// 使用方法：在场景中添加 PerformanceMonitor 对象
/// </summary>
public class PerformanceMonitor : MonoBehaviour
{
    public static PerformanceMonitor Instance { get; private set; }
    
    [Header("显示设置")]
    public bool showStats = true;
    public KeyCode toggleKey = KeyCode.F1;
    public int updateInterval = 10; // 帧数
    
    [Header("UI 引用")]
    public TextMeshProUGUI statsText;
    
    [Header("性能阈值")]
    public float targetFPS = 60f;
    public float warningFPS = 45f;
    public float criticalFPS = 30f;
    
    // 性能数据
    private float deltaTime = 0f;
    private float updateTime = 0f;
    private int frameCount = 0;
    private float fps = 0f;
    private float ms = 0f;
    
    // 内存监控
    private long lastGCAlloc = 0;
    private long gcAllocDelta = 0;
    
    // 对象计数
    private Dictionary<string, int> objectCounts = new Dictionary<string, int>();
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        CreateStatsUI();
        lastGCAlloc = System.GC.GetTotalMemory(false);
    }
    
    void Update()
    {
        // 切换显示
        if (Input.GetKeyDown(toggleKey))
        {
            showStats = !showStats;
            if (statsText != null)
            {
                statsText.gameObject.SetActive(showStats);
            }
        }
        
        if (!showStats) return;
        
        // 计算 FPS
        deltaTime += Time.unscaledDeltaTime;
        frameCount++;
        
        updateTime += Time.unscaledDeltaTime;
        if (updateTime >= 1f / updateInterval)
        {
            fps = frameCount / updateTime;
            ms = deltaTime * 1000f / frameCount;
            
            // GC 分配
            long currentGCAlloc = System.GC.GetTotalMemory(false);
            gcAllocDelta = currentGCAlloc - lastGCAlloc;
            lastGCAlloc = currentGCAlloc;
            
            // 更新 UI
            UpdateStatsUI();
            
            // 重置
            deltaTime = 0f;
            frameCount = 0;
            updateTime = 0f;
        }
        
        // 检查性能警告
        CheckPerformanceWarnings();
    }
    
    void CreateStatsUI()
    {
        if (statsText != null) return;
        
        // 创建 Canvas（如果没有）
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("PerformanceCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }
        
        // 创建文本
        GameObject textObj = new GameObject("PerformanceStats");
        textObj.transform.SetParent(canvas.transform, false);
        textObj.transform.SetAsFirstSibling(); // 放在最前面
        
        statsText = textObj.AddComponent<TextMeshProUGUI>();
        statsText.fontSize = 14;
        statsText.color = Color.white;
        statsText.alignment = TextAlignmentOptions.TopLeft;
        
        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(10, -10);
        rect.sizeDelta = new Vector2(300, 200);
        
        // 背景
        UnityEngine.UI.Image bg = textObj.AddComponent<UnityEngine.UI.Image>();
        bg.color = new Color(0, 0, 0, 0.5f);
        
        textObj.SetActive(showStats);
    }
    
    void UpdateStatsUI()
    {
        if (statsText == null) return;
        
        // 颜色根据 FPS 变化
        Color fpsColor = fps >= targetFPS ? Color.green :
                        fps >= warningFPS ? Color.yellow : Color.red;
        
        // 对象计数
        UpdateObjectCounts();
        
        string stats = $"<color={ColorUtility.ToHtmlStringRGB(fpsColor)}>FPS: {fps:F1}</color>\n" +
                      $"MS: {ms:F2}ms\n" +
                      $"GC: {FormatBytes(gcAllocDelta)}/frame\n" +
                      $"Memory: {FormatBytes(System.GC.GetTotalMemory(false))}\n" +
                      $"\n" +
                      $"Objects:\n" +
                      $"  Total: {objectCounts["Total"]}\n" +
                      $"  Players: {objectCounts["Player"]}\n" +
                      $"  Packages: {objectCounts["Package"]}\n" +
                      $"  UI: {objectCounts["UI"]}\n";
        
        statsText.text = stats;
    }
    
    void UpdateObjectCounts()
    {
        objectCounts["Total"] = FindObjectsOfType<GameObject>().Length;
        objectCounts["Player"] = GameObject.FindGameObjectsWithTag("Player").Length;
        objectCounts["Package"] = GameObject.FindGameObjectsWithTag("Package").Length;
        
        int uiCount = 0;
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.layer == LayerMask.NameToLayer("UI"))
            {
                uiCount++;
            }
        }
        objectCounts["UI"] = uiCount;
    }
    
    void CheckPerformanceWarnings()
    {
        if (fps < criticalFPS)
        {
            Debug.LogWarning($"⚠️ 性能警告：FPS 低于 {criticalFPS} (当前：{fps:F1})");
        }
    }
    
    string FormatBytes(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        
        return $"{len:0.##} {sizes[order]}";
    }
    
    public void LogPerformance(string message)
    {
        Debug.Log($"[性能] {message}");
    }
}

/// <summary>
/// 性能分析器 - 代码块性能分析
/// </summary>
public static class PerformanceProfiler
{
    private static Dictionary<string, long> timings = new Dictionary<string, long>();
    private static Dictionary<string, int> counts = new Dictionary<string, int>();
    
    public static void Begin(string name)
    {
        string key = $"{name}_start";
        timings[key] = System.Diagnostics.Stopwatch.GetTimestamp();
    }
    
    public static void End(string name)
    {
        string startKey = $"{name}_start";
        if (!timings.ContainsKey(startKey)) return;
        
        long start = timings[startKey];
        long end = System.Diagnostics.Stopwatch.GetTimestamp();
        long elapsed = end - start;
        double ms = elapsed * 1000.0 / System.Diagnostics.Stopwatch.Frequency;
        
        if (!counts.ContainsKey(name)) counts[name] = 0;
        counts[name]++;
        
        Debug.Log($"[性能] {name}: {ms:F3}ms (调用：{counts[name]})");
    }
}
