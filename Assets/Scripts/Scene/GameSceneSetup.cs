using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏场景设置器 - 一键配置完整的游戏场景
/// 整合所有美术资源和系统
/// </summary>
public class GameSceneSetup : MonoBehaviour
{
    [Header("场景配置")]
    public SceneTheme sceneTheme = SceneTheme.ShallowSea;
    public bool generateDecorations = true;
    public bool enableParticles = true;
    
    [Header("预制体引用")]
    public GameObject playerPrefab;
    public GameObject[] obstaclePrefabs;
    public GameObject[] decorationPrefabs;
    
    [Header("生成参数")]
    public Vector2 spawnArea = new Vector2(40, 20);
    public int obstacleCount = 15;
    public int decorationCount = 20;
    
    [Header("相机设置")]
    public Camera mainCamera;
    public float cameraOrthographicSize = 10f;
    
    [Header("光照设置")]
    public Light mainLight;
    public Color shallowSeaLight = new Color(0.3f, 0.6f, 0.9f, 1f);
    public Color deepSeaLight = new Color(0.1f, 0.2f, 0.4f, 1f);
    public Color coralReefLight = new Color(0.7f, 0.4f, 0.5f, 1f);
    public Color nightLight = new Color(0.1f, 0.1f, 0.3f, 1f);
    
    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        SetupCamera();
        SetupLighting();
        SetupBackground();
    }
    
    void Start()
    {
        if (generateDecorations)
        {
            GenerateScene();
        }
        
        if (enableParticles)
        {
            SetupParticles();
        }
        
        Debug.Log("[GameSceneSetup] 场景配置完成");
    }
    
    #region 相机设置
    
    void SetupCamera()
    {
        if (mainCamera == null) return;
        
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = cameraOrthographicSize;
        mainCamera.backgroundColor = GetThemeColor();
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        
        // 设置相机层级
        mainCamera.cullingMask = ~LayerMask.GetMask("UI");
    }
    
    #endregion
    
    #region 光照设置
    
    void SetupLighting()
    {
        if (mainLight == null)
        {
            // 创建方向光
            GameObject lightObj = new GameObject("MainLight");
            mainLight = lightObj.AddComponent<Light>();
            mainLight.type = LightType.Directional;
            mainLight.rotation = Quaternion.Euler(50, -30, 0);
        }
        
        mainLight.color = GetThemeColor();
        mainLight.intensity = GetThemeIntensity();
        mainLight.shadows = LightShadows.Soft;
    }
    
    Color GetThemeColor()
    {
        switch (sceneTheme)
        {
            case SceneTheme.ShallowSea:
                return shallowSeaLight;
            case SceneTheme.DeepSea:
                return deepSeaLight;
            case SceneTheme.CoralReef:
                return coralReefLight;
            case SceneTheme.Night:
                return nightLight;
            default:
                return shallowSeaLight;
        }
    }
    
    float GetThemeIntensity()
    {
        switch (sceneTheme)
        {
            case SceneTheme.ShallowSea:
                return 0.8f;
            case SceneTheme.DeepSea:
                return 0.4f;
            case SceneTheme.CoralReef:
                return 0.7f;
            case SceneTheme.Night:
                return 0.2f;
            default:
                return 0.8f;
        }
    }
    
    #endregion
    
    #region 背景设置
    
    void SetupBackground()
    {
        // 创建背景精灵
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.position = new Vector3(0, 0, 10);
        
        SpriteRenderer bgRenderer = bgObj.AddComponent<SpriteRenderer>();
        bgRenderer.sortingOrder = -100;
        bgRenderer.color = GetThemeColor() * 0.5f;
        
        // 尝试加载生成的背景
        Sprite bgSprite = Resources.Load<Sprite>("Sprites/environment/seabed_background");
        if (bgSprite != null)
        {
            bgRenderer.sprite = bgSprite;
        }
        else
        {
            // 创建纯色背景
            Texture2D bgTexture = CreateColorTexture(512, 512, GetThemeColor() * 0.5f);
            Sprite sprite = Sprite.Create(bgTexture, new Rect(0, 0, 512, 512), Vector2.one * 0.5f);
            bgRenderer.sprite = sprite;
        }
        
        // 平铺背景
        bgObj.transform.localScale = new Vector3(8, 8, 1);
    }
    
    #endregion
    
    #region 场景生成
    
    void GenerateScene()
    {
        // 生成障碍物
        for (int i = 0; i < obstacleCount; i++)
        {
            SpawnObstacle();
        }
        
        // 生成装饰物
        for (int i = 0; i < decorationCount; i++)
        {
            SpawnDecoration();
        }
    }
    
    void SpawnObstacle()
    {
        Vector3 pos = GetRandomSpawnPosition();
        pos.y = Mathf.Lerp(-spawnArea.y / 2, -spawnArea.y / 4, Random.value); // 底部区域
        
        if (obstaclePrefabs != null && obstaclePrefabs.Length > 0)
        {
            int index = Random.Range(0, obstaclePrefabs.Length);
            Instantiate(obstaclePrefabs[index], pos, Quaternion.identity);
        }
        else
        {
            // 创建占位障碍物
            CreatePlaceholderObstacle(pos);
        }
    }
    
    void SpawnDecoration()
    {
        Vector3 pos = GetRandomSpawnPosition();
        
        if (decorationPrefabs != null && decorationPrefabs.Length > 0)
        {
            int index = Random.Range(0, decorationPrefabs.Length);
            Instantiate(decorationPrefabs[index], pos, Quaternion.identity);
        }
    }
    
    void CreatePlaceholderObstacle(Vector3 position)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.position = position;
        obj.transform.localScale = new Vector3(
            Random.Range(1f, 3f),
            Random.Range(1f, 2f),
            1
        );
        obj.GetComponent<Renderer>().material.color = new Color(
            Random.Range(0.3f, 0.6f),
            Random.Range(0.5f, 0.8f),
            Random.Range(0.6f, 0.9f)
        );
        obj.layer = LayerMask.NameToLayer("Obstacle");
    }
    
    Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(
            Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
            Random.Range(-spawnArea.y / 2, spawnArea.y / 2),
            0
        );
    }
    
    #endregion
    
    #region 粒子效果
    
    void SetupParticles()
    {
        // 创建气泡粒子系统
        GameObject bubbleObj = new GameObject("BubbleParticles");
        bubbleObj.transform.position = Vector3.zero;
        
        ParticleSystem ps = bubbleObj.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.startLifetime = 5f;
        main.startSpeed = 2f;
        main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.3f);
        main.startColor = new Color(0.8f, 0.9f, 1f, 0.6f);
        main.emissionRate = 10f;
        main.gravityModifier = -0.1f; // 向上飘
        
        var emission = ps.emission;
        emission.enabled = true;
        emission.rateOverTime = 10f;
        
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(spawnArea.x, spawnArea.y, 1);
        
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = GetDefaultParticleMaterial();
    }
    
    Material GetDefaultParticleMaterial()
    {
        // 创建简单的粒子材质
        Shader shader = Shader.Find("Particles/Standard Unlit");
        if (shader == null)
        {
            shader = Shader.Find("Particles/Alpha Blended");
        }
        
        Material mat = new Material(shader);
        mat.color = new Color(0.8f, 0.9f, 1f, 0.6f);
        return mat;
    }
    
    #endregion
    
    #region 工具方法
    
    Texture2D CreateColorTexture(int width, int height, Color color)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];
        
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
    
    #endregion
    
    #region 公共方法
    
    /// <summary>
    /// 切换场景主题
    /// </summary>
    public void ChangeTheme(SceneTheme newTheme)
    {
        sceneTheme = newTheme;
        mainCamera.backgroundColor = GetThemeColor();
        mainLight.color = GetThemeColor();
        mainLight.intensity = GetThemeIntensity();
    }
    
    /// <summary>
    /// 重新生成场景
    /// </summary>
    public void RegenerateScene()
    {
        // 清理现有装饰
        GameObject[] decorations = GameObject.FindGameObjectsWithTag("Decoration");
        foreach (GameObject obj in decorations)
        {
            Destroy(obj);
        }
        
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obj in obstacles)
        {
            Destroy(obj);
        }
        
        // 重新生成
        GenerateScene();
    }
    
    #endregion
}
