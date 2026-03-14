using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 优化工具 - 性能分析、资源优化、代码检查
/// 使用方法：菜单栏 → Lobster Courier → 优化工具
/// </summary>
public class OptimizationTools : EditorWindow
{
    private Vector2 scrollPosition;
    private bool showPerformance = true;
    private bool showAssets = true;
    private bool showCode = true;
    
    [MenuItem("Lobster Courier/优化工具")]
    public static void ShowWindow()
    {
        var window = GetWindow<OptimizationTools>("优化工具");
        window.minSize = new Vector2(500, 700);
        window.Show();
    }
    
    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        GUILayout.Label("🦞 龙虾快递员 - 优化工具", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        // 性能分析
        showPerformance = EditorGUILayout.Foldout(showPerformance, "📊 性能分析");
        if (showPerformance)
        {
            PerformanceSection();
        }
        
        GUILayout.Space(10);
        
        // 资源优化
        showAssets = EditorGUILayout.Foldout(showAssets, "🎨 资源优化");
        if (showAssets)
        {
            AssetsSection();
        }
        
        GUILayout.Space(10);
        
        // 代码检查
        showCode = EditorGUILayout.Foldout(showCode, "💻 代码检查");
        if (showCode)
        {
            CodeSection();
        }
        
        EditorGUILayout.EndScrollView();
    }
    
    #region 性能分析
    
    void PerformanceSection()
    {
        EditorGUILayout.HelpBox(
            "分析场景性能，找出性能瓶颈",
            MessageType.Info);
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("🔍 分析当前场景", GUILayout.Height(35)))
        {
            AnalyzeScene();
        }
        
        if (GUILayout.Button("📈 检查对象数量", GUILayout.Height(35)))
        {
            CheckObjectCount();
        }
        
        if (GUILayout.Button("⚡ 检查组件性能", GUILayout.Height(35)))
        {
            CheckComponentPerformance();
        }
    }
    
    void AnalyzeScene()
    {
        Debug.Log("=== 场景性能分析 ===");
        
        // 对象统计
        int totalObjects = Object.FindObjectsOfType<GameObject>().Length;
        Debug.Log($"总对象数：{totalObjects}");
        
        // 组件统计
        var components = FindObjectsOfType<MonoBehaviour>();
        Debug.Log($"MonoBehaviour 数量：{components.Length}");
        
        // 物理组件
        int rigidbodies = Object.FindObjectsOfType<Rigidbody2D>().Length;
        int colliders = Object.FindObjectsOfType<Collider2D>().Length;
        Debug.Log($"Rigidbody2D: {rigidbodies}, Collider2D: {colliders}");
        
        // UI 组件
        int canvases = Object.FindObjectsOfType<Canvas>().Length;
        int uiTexts = Object.FindObjectsOfType<TextMeshProUGUI>().Length;
        Debug.Log($"Canvas: {canvases}, TextMeshProUGUI: {uiTexts}");
        
        EditorUtility.DisplayDialog("场景分析完成", 
            $"总对象：{totalObjects}\n" +
            $"MonoBehaviour: {components.Length}\n" +
            $"Rigidbody2D: {rigidbodies}\n" +
            $"Collider2D: {colliders}\n" +
            $"Canvas: {canvases}\n" +
            $"TextMeshProUGUI: {uiTexts}",
            "确定");
    }
    
    void CheckObjectCount()
    {
        Dictionary<string, int> counts = new Dictionary<string, int>();
        
        var gameObjects = Object.FindObjectsOfType<GameObject>();
        foreach (var obj in gameObjects)
        {
            string name = obj.name.Split('_')[0];
            if (!counts.ContainsKey(name))
            {
                counts[name] = 0;
            }
            counts[name]++;
        }
        
        Debug.Log("=== 对象数量统计 ===");
        foreach (var kvp in counts)
        {
            if (kvp.Value > 1)
            {
                Debug.Log($"{kvp.Key}: {kvp.Value}");
            }
        }
    }
    
    void CheckComponentPerformance()
    {
        Debug.Log("=== 组件性能检查 ===");
        
        // 检查没有使用的组件
        var renderers = FindObjectsOfType<Renderer>();
        foreach (var renderer in renderers)
        {
            if (!renderer.enabled && renderer.gameObject.activeInHierarchy)
            {
                Debug.LogWarning($"未启用的渲染器：{renderer.gameObject.name}");
            }
        }
        
        // 检查多余的物理组件
        var rigidbodies = FindObjectsOfType<Rigidbody2D>();
        foreach (var rb in rigidbodies)
        {
            if (rb.gravityScale == 0 && rb.isKinematic)
            {
                Debug.LogWarning($"可能不需要的 Rigidbody2D: {rb.gameObject.name}");
            }
        }
    }
    
    #endregion
    
    #region 资源优化
    
    void AssetsSection()
    {
        EditorGUILayout.HelpBox(
            "优化美术资源和音频资源",
            MessageType.Info);
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("🖼️ 检查纹理大小", GUILayout.Height(35)))
        {
            CheckTextureSizes();
        }
        
        if (GUILayout.Button("🎵 检查音频压缩", GUILayout.Height(35)))
        {
            CheckAudioCompression();
        }
        
        if (GUILayout.Button("🗑️ 清理未使用资源", GUILayout.Height(35)))
        {
            CleanUnusedAssets();
        }
        
        if (GUILayout.Button("📦 批量压缩纹理", GUILayout.Height(35)))
        {
            BatchCompressTextures();
        }
    }
    
    void CheckTextureSizes()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets" });
        
        Debug.Log("=== 纹理大小检查 ===");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            
            if (tex != null)
            {
                int size = tex.width * tex.height;
                if (size > 2048 * 2048)
                {
                    Debug.LogWarning($"大纹理：{path} ({tex.width}x{tex.height})");
                }
            }
        }
    }
    
    void CheckAudioCompression()
    {
        string[] guids = AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets" });
        
        Debug.Log("=== 音频压缩检查 ===");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            
            if (clip != null)
            {
                Debug.Log($"音频：{path} ({clip.length:F2}s, {clip.frequency}Hz)");
            }
        }
    }
    
    void CleanUnusedAssets()
    {
        if (EditorUtility.DisplayDialog("清理未使用资源",
            "这将删除未使用的资源文件，确定继续？",
            "确定",
            "取消"))
        {
            // 这里可以添加清理逻辑
            Debug.Log("清理完成（示例）");
        }
    }
    
    void BatchCompressTextures()
    {
        if (EditorUtility.DisplayDialog("批量压缩纹理",
            "这将为所有纹理应用压缩设置，确定继续？",
            "确定",
            "取消"))
        {
            string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets" });
            
            int count = 0;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                
                if (importer != null)
                {
                    importer.textureCompression = TextureImporterCompression.Compressed;
                    importer.SaveAndReimport();
                    count++;
                }
            }
            
            Debug.Log($"已压缩 {count} 个纹理");
            EditorUtility.DisplayDialog("压缩完成", $"已压缩 {count} 个纹理", "确定");
        }
    }
    
    #endregion
    
    #region 代码检查
    
    void CodeSection()
    {
        EditorGUILayout.HelpBox(
            "检查代码质量和潜在问题",
            MessageType.Info);
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("🔍 检查空引用", GUILayout.Height(35)))
        {
            CheckNullReferences();
        }
        
        if (GUILayout.Button("⚠️ 检查硬编码", GUILayout.Height(35)))
        {
            CheckHardcodedValues();
        }
        
        if (GUILayout.Button("📝 检查注释覆盖率", GUILayout.Height(35)))
        {
            CheckCommentCoverage();
        }
        
        if (GUILayout.Button("🔄 检查单例模式", GUILayout.Height(35)))
        {
            CheckSingletonPattern();
        }
    }
    
    void CheckNullReferences()
    {
        Debug.Log("=== 空引用检查 ===");
        
        string[] scripts = AssetDatabase.FindAssets("t:MonoScript", new[] { "Assets/Scripts" });
        
        foreach (string guid in scripts)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string code = System.IO.File.ReadAllText(path);
            
            // 简单检查 GetComponent 后没有空检查
            if (code.Contains("GetComponent") && !code.Contains("if (") && !code.Contains("?.") )
            {
                Debug.LogWarning($"可能存在空引用：{path}");
            }
        }
    }
    
    void CheckHardcodedValues()
    {
        Debug.Log("=== 硬编码值检查 ===");
        
        string[] scripts = AssetDatabase.FindAssets("t:MonoScript", new[] { "Assets/Scripts" });
        
        foreach (string guid in scripts)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string code = System.IO.File.ReadAllText(path);
            
            // 检查硬编码字符串
            if (System.Text.RegularExpressions.Regex.IsMatch(code, @"""[a-zA-Z0-9_]+"""))
            {
                // 可能是硬编码字符串
                Debug.Log($"检查硬编码：{path}");
            }
        }
    }
    
    void CheckCommentCoverage()
    {
        Debug.Log("=== 注释覆盖率检查 ===");
        
        string[] scripts = AssetDatabase.FindAssets("t:MonoScript", new[] { "Assets/Scripts" });
        
        int totalLines = 0;
        int commentLines = 0;
        
        foreach (string guid in scripts)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string[] lines = System.IO.File.ReadAllLines(path);
            
            totalLines += lines.Length;
            
            foreach (string line in lines)
            {
                if (line.Trim().StartsWith("//") || line.Trim().StartsWith("///"))
                {
                    commentLines++;
                }
            }
        }
        
        float coverage = (float)commentLines / totalLines * 100f;
        Debug.Log($"总行数：{totalLines}");
        Debug.Log($"注释行数：{commentLines}");
        Debug.Log($"注释覆盖率：{coverage:F1}%");
        
        if (coverage < 20f)
        {
            Debug.LogWarning("注释覆盖率低于 20%，建议增加注释");
        }
    }
    
    void CheckSingletonPattern()
    {
        Debug.Log("=== 单例模式检查 ===");
        
        string[] scripts = AssetDatabase.FindAssets("t:MonoScript", new[] { "Assets/Scripts" });
        
        foreach (string guid in scripts)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string code = System.IO.File.ReadAllText(path);
            
            if (code.Contains("Instance") && code.Contains("static"))
            {
                if (!code.Contains("DontDestroyOnLoad"))
                {
                    Debug.LogWarning($"单例但未设置 DontDestroyOnLoad: {path}");
                }
            }
        }
    }
    
    #endregion
}
