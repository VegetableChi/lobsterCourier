using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// 场景自动搭建工具 - 一键创建游戏场景
/// 使用方法：菜单栏 → Lobster Courier → 自动搭建场景
/// 
/// 优化记录:
/// - 2026-03-14: 初始版本
/// - 修复：添加 SceneManager 引用
/// - 优化：改进错误处理
/// </summary>
public class SceneSetup : EditorWindow
{
    private Vector2 scrollPosition;
    
    [MenuItem("Lobster Courier/自动搭建场景")]
    public static void ShowWindow()
    {
        var window = GetWindow<SceneSetup>("场景搭建工具");
        window.minSize = new Vector2(400, 600);
        window.Show();
    }
    
    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        GUILayout.Label("🦞 龙虾快递员 - 场景自动搭建工具", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "本工具将自动创建完整的游戏场景，包括：\n" +
            "• 玩家对象（龙虾）\n" +
            "• 游戏管理器\n" +
            "• UI 界面\n" +
            "• 环境元素\n" +
            "• 灯光和摄像机",
            MessageType.Info);
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("🎬 创建主游戏场景", GUILayout.Height(40)))
        {
            CreateGameScene();
        }
        
        if (GUILayout.Button("🏠 创建主菜单场景", GUILayout.Height(40)))
        {
            CreateMainMenuScene();
        }
        
        if (GUILayout.Button("🎮 创建玩家对象", GUILayout.Height(30)))
        {
            CreatePlayer();
        }
        
        if (GUILayout.Button("🎨 创建 UI 界面", GUILayout.Height(30)))
        {
            CreateUI();
        }
        
        if (GUILayout.Button("🌊 创建环境元素", GUILayout.Height(30)))
        {
            CreateEnvironment();
        }
        
        if (GUILayout.Button("📦 创建测试包裹", GUILayout.Height(30)))
        {
            CreateTestPackages();
        }
        
        EditorGUILayout.EndScrollView();
    }
    
    #region 创建主场景
    
    public void CreateGameScene()
    {
        try
        {
            string scenePath = "Assets/Scenes/GameScene.unity";
            
            // 创建场景目录
            if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            {
                AssetDatabase.CreateFolder("Assets", "Scenes");
            }
            
            // 创建新场景
            Scene scene = SceneManager.CreateScene("GameScene");
            SceneManager.SetActiveScene(scene);
            
            // 显示进度
            EditorUtility.DisplayProgressBar("搭建场景", "创建玩家对象...", 0.1f);
            
            // 创建场景元素
            CreatePlayer();
            EditorUtility.DisplayProgressBar("搭建场景", "创建摄像机...", 0.2f);
            CreateCamera();
            EditorUtility.DisplayProgressBar("搭建场景", "创建管理器...", 0.4f);
            CreateManagers();
            EditorUtility.DisplayProgressBar("搭建场景", "创建 UI...", 0.6f);
            CreateUI();
            EditorUtility.DisplayProgressBar("搭建场景", "创建环境...", 0.8f);
            CreateEnvironment();
            CreateLighting();
            
            // 保存场景
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName("Assets/" + scenePath));
            EditorSceneManager.SaveScene(scene);
            
            // 清除进度
            EditorUtility.ClearProgressBar();
            
            Debug.Log("✅ 主游戏场景创建完成！");
            EditorUtility.DisplayDialog("场景创建完成", 
                "主游戏场景已创建完成！\n\n" +
                "包含内容:\n" +
                "• 玩家对象（带控制器）\n" +
                "• 15 个游戏管理器\n" +
                "• 完整 UI 界面\n" +
                "• 环境光照\n\n" +
                "现在可以添加美术资源了。", "好的");
        }
        catch (System.Exception e)
        {
            EditorUtility.ClearProgressBar();
            Debug.LogError($"❌ 场景创建失败：{e.Message}");
            EditorUtility.DisplayDialog("错误", $"场景创建失败:\n{e.Message}", "确定");
        }
    }
    
    #endregion
    
    #region 创建主菜单场景
    
    void CreateMainMenuScene()
    {
        string scenePath = "Assets/Scenes/MainMenu.unity";
        
        // 创建场景目录
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
        {
            AssetDatabase.CreateFolder("Assets", "Scenes");
        }
        
        // 创建新场景
        UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.CreateScene("MainMenu");
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
        
        // 创建摄像机
        CreateCamera();
        
        // 创建主菜单 UI（CreateMainMenuUI 会自己创建 Canvas）
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        CreateMainMenuUI(canvas);
        
        // 创建管理器
        CreateAudioManager();
        
        // 保存场景
        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName("Assets/" + scenePath));
        EditorSceneManager.SaveScene(scene);
        
        Debug.Log("✅ 主菜单场景创建完成！");
        EditorUtility.DisplayDialog("场景创建完成", "主菜单场景已创建完成！", "好的");
    }
    
    #endregion
    
    #region 创建玩家
    
    void CreatePlayer()
    {
        // 检查是否已存在玩家
        if (GameObject.FindWithTag("Player") != null)
        {
            Debug.LogWarning("⚠️ 玩家对象已存在，跳过创建");
            return;
        }
        
        // 创建玩家对象
        GameObject player = new GameObject("Player");
        player.transform.position = Vector3.zero;
        
        // 设置 Tag（如果不存在则创建）
        try
        {
            player.tag = "Player";
        }
        catch
        {
            Debug.LogWarning("⚠️ Player Tag 不存在，请手动创建");
        }
        
        // 添加精灵渲染器
        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        sr.color = new Color(1, 0.3f, 0.3f, 1); // 临时红色占位
        
        // 添加刚体
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        // 添加碰撞体
        CircleCollider2D collider = player.AddComponent<CircleCollider2D>();
        collider.radius = 0.5f;
        
        // 添加玩家控制器
        player.AddComponent<LobsterController>();
        
        // 添加动画组件
        player.AddComponent<Animator>();
        
        // 添加持握点
        GameObject holdPoint = new GameObject("HoldPoint");
        holdPoint.transform.parent = player.transform;
        holdPoint.transform.localPosition = new Vector3(0.8f, 0, 0);
        
        // 设置层级
        player.layer = LayerMask.NameToLayer("Default");
        
        Debug.Log("✅ 玩家对象创建完成！");
    }
    
    #endregion
    
    #region 创建摄像机
    
    void CreateCamera()
    {
        Camera mainCamera = Camera.main;
        
        if (mainCamera == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            mainCamera = cameraObj.AddComponent<Camera>();
            cameraObj.tag = "MainCamera";
        }
        
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = 10;
        mainCamera.backgroundColor = new Color(0.1f, 0.3f, 0.5f, 1f); // 海底蓝色
        
        // 添加摄像机跟随脚本
        if (mainCamera.GetComponent<CameraFollow>() == null)
        {
            mainCamera.gameObject.AddComponent<CameraFollow>();
        }
        
        Debug.Log("✅ 摄像机创建完成！");
    }
    
    #endregion
    
    #region 创建管理器
    
    void CreateManagers()
    {
        // 管理器列表
        System.Type[] managerTypes = new System.Type[]
        {
            typeof(GameManager),
            typeof(OceanCurrentManager),
            typeof(LevelGenerator),
            typeof(PackageSpawner),
            typeof(SaveSystem),
            typeof(AchievementSystem),
            typeof(AudioManager),
            typeof(ParticleManager),
            typeof(ComboSystem),
            typeof(TutorialSystem),
            typeof(DifficultyManager),
            typeof(GameEndSystem),
            typeof(WeatherSystem),
            typeof(DailyChallenge)
        };
        
        int count = 0;
        foreach (var managerType in managerTypes)
        {
            string managerName = managerType.Name;
            
            // 检查是否已存在
            if (GameObject.Find(managerName) != null)
            {
                Debug.LogWarning($"⚠️ {managerName} 已存在，跳过");
                continue;
            }
            
            CreateManagerObject(managerName, managerType);
            count++;
        }
        
        Debug.Log($"✅ 所有管理器创建完成！共创建 {count} 个管理器");
    }
    
    GameObject CreateManagerObject(string name, System.Type scriptType)
    {
        GameObject managerObj = new GameObject(name);
        managerObj.AddComponent(scriptType);
        
        // 设置为不销毁（对于单例模式）
        if (scriptType == typeof(GameManager) || 
            scriptType == typeof(SaveSystem) || 
            scriptType == typeof(AchievementSystem) ||
            scriptType == typeof(AudioManager) ||
            scriptType == typeof(ParticleManager))
        {
            MonoBehaviour.DontDestroyOnLoad(managerObj);
        }
        
        return managerObj;
    }
    
    void CreateAudioManager()
    {
        GameObject audioManager = new GameObject("AudioManager");
        audioManager.AddComponent<AudioManager>();
    }
    
    #endregion
    
    #region 创建 UI
    
    void CreateUI()
    {
        // 检查是否已存在 Canvas
        Canvas existingCanvas = FindObjectOfType<Canvas>();
        if (existingCanvas != null)
        {
            Debug.LogWarning("⚠️ Canvas 已存在，跳过创建");
            return;
        }
        
        // 创建 Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        
        canvasObj.AddComponent<GraphicRaycaster>();
        canvasObj.layer = LayerMask.NameToLayer("UI");
        
        // 创建 UI 事件系统
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            
            // 使用 Standalone Input Module
            eventSystem.AddComponent<StandaloneInputModule>();
        }
        
        // 创建 UI 元素
        CreateHUD(canvas);
        CreatePauseMenu(canvas);
        CreateShopPanel(canvas);
        CreateAchievementPanel(canvas);
        CreateTutorialPanel(canvas);
        
        Debug.Log("✅ UI 界面创建完成！");
    }
    
    void CreateHUD(Canvas canvas)
    {
        GameObject hudObj = new GameObject("HUD");
        hudObj.transform.SetParent(canvas.transform, false);
        
        // 金钱显示
        CreateTextElement(hudObj, "MoneyText", "💰 $0", new Vector2(-800, 500), 36, Color.yellow);
        
        // 声誉显示
        CreateTextElement(hudObj, "ReputationText", "⭐ 0", new Vector2(-800, 460), 36, Color.white);
        
        // 订单计数
        CreateTextElement(hudObj, "OrderCountText", "📦 0", new Vector2(800, 500), 36, Color.white);
        
        // 连击显示
        CreateTextElement(hudObj, "ComboText", "", new Vector2(0, 500), 48, new Color(1f, 0.5f, 0f));
        
        // 体力条背景
        GameObject staminaBg = CreateImageElement(hudObj, "StaminaBarBg", new Vector2(0, -500), new Vector2(300, 30), Color.gray);
        
        // 体力条填充
        GameObject staminaFill = CreateImageElement(staminaBg, "Fill", Vector2.zero, new Vector2(300, 30), Color.green);
        Image fillImage = staminaFill.GetComponent<Image>();
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        fillImage.fillAmount = 1f;
        
        // 添加体力条脚本
        GameObject staminaBarObj = new GameObject("StaminaBar");
        staminaBarObj.transform.SetParent(hudObj.transform, false);
        StaminaBar staminaBar = staminaBarObj.AddComponent<StaminaBar>();
        staminaBar.fillImage = fillImage;
    }
    
    void CreatePauseMenu(Canvas canvas)
    {
        GameObject pausePanel = CreatePanel(canvas, "PausePanel", new Color(0, 0, 0, 0.8f));
        pausePanel.SetActive(false);
        
        CreateTextElement(pausePanel, "PauseTitle", "⏸️ 游戏暂停", Vector2.zero, 64, Color.white);
        
        // 继续按钮
        CreateButton(pausePanel, "ResumeButton", "继续游戏", new Vector2(0, 100), 200, 50);
        
        // 重新开始按钮
        CreateButton(pausePanel, "RestartButton", "重新开始", new Vector2(0, 30), 200, 50);
        
        // 主菜单按钮
        CreateButton(pausePanel, "MainMenuButton", "返回主菜单", new Vector2(0, -40), 200, 50);
        
        // 添加暂停菜单脚本
        pausePanel.AddComponent<PauseMenu>();
    }
    
    void CreateShopPanel(Canvas canvas)
    {
        GameObject shopPanel = CreatePanel(canvas, "ShopPanel", new Color(0.1f, 0.1f, 0.2f, 0.95f));
        shopPanel.SetActive(false);
        
        CreateTextElement(shopPanel, "ShopTitle", "🏪 商店", new Vector2(0, 500), 64, Color.yellow);
        CreateTextElement(shopPanel, "MoneyText", "💰 $0", new Vector2(700, 500), 36, Color.yellow);
        
        // 商品列表容器
        GameObject content = new GameObject("Content");
        content.transform.SetParent(shopPanel.transform, false);
        content.transform.localPosition = new Vector3(0, 0, 0);
        
        VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 10;
        layout.childAlignment = TextAnchor.UpperCenter;
        
        ContentSizeFitter fitter = content.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        // 关闭按钮
        CreateButton(shopPanel, "CloseButton", "关闭", new Vector2(0, -500), 200, 50);
    }
    
    void CreateAchievementPanel(Canvas canvas)
    {
        GameObject achievementPanel = CreatePanel(canvas, "AchievementPanel", new Color(0.1f, 0.1f, 0.2f, 0.95f));
        achievementPanel.SetActive(false);
        
        CreateTextElement(achievementPanel, "AchievementTitle", "🏆 成就", new Vector2(0, 500), 64, Color.yellow);
        
        // 成就列表容器
        GameObject content = new GameObject("Content");
        content.transform.SetParent(achievementPanel.transform, false);
        
        // 关闭按钮
        CreateButton(achievementPanel, "CloseButton", "关闭", new Vector2(0, -500), 200, 50);
    }
    
    void CreateTutorialPanel(Canvas canvas)
    {
        GameObject tutorialPanel = CreatePanel(canvas, "TutorialPanel", new Color(0, 0, 0, 0.9f));
        tutorialPanel.SetActive(false);
        
        CreateTextElement(tutorialPanel, "TutorialTitle", "教程标题", new Vector2(0, 200), 48, Color.yellow);
        CreateTextElement(tutorialPanel, "TutorialContent", "教程内容", new Vector2(0, 50), 32, Color.white);
        CreateTextElement(tutorialPanel, "TutorialTip", "提示", new Vector2(0, -200), 24, Color.gray);
        
        // 下一步按钮
        CreateButton(tutorialPanel, "NextButton", "下一步", new Vector2(100, -400), 150, 50);
        
        // 完成按钮
        CreateButton(tutorialPanel, "CompleteButton", "完成", new Vector2(100, -400), 150, 50);
    }
    
    void CreateMainMenuUI(Canvas canvas)
    {
        // 标题
        CreateTextElement(canvas.gameObject, "GameTitle", "🦞 龙虾快递员", new Vector2(0, 300), 96, Color.yellow);
        CreateTextElement(canvas.gameObject, "GameSubtitle", "海底最快外卖员", new Vector2(0, 220), 48, Color.white);
        
        // 开始按钮
        CreateButton(canvas.gameObject, "StartButton", "开始游戏", new Vector2(0, 50), 300, 60);
        
        // 设置按钮
        CreateButton(canvas.gameObject, "SettingsButton", "设置", new Vector2(0, -30), 300, 60);
        
        // 退出按钮
        CreateButton(canvas.gameObject, "QuitButton", "退出", new Vector2(0, -110), 300, 60);
        
        // 版本信息
        CreateTextElement(canvas.gameObject, "VersionText", "v0.8.0 Beta", new Vector2(0, -500), 24, Color.gray);
    }
    
    #endregion
    
    #region 创建环境
    
    void CreateEnvironment()
    {
        // 创建海底背景
        GameObject background = new GameObject("Background");
        SpriteRenderer bgSr = background.AddComponent<SpriteRenderer>();
        bgSr.color = new Color(0.1f, 0.3f, 0.5f, 1f);
        bgSr.sortingOrder = -10;
        
        // 缩放背景
        background.transform.localScale = new Vector3(200, 200, 1);
        
        // 创建光源
        CreateLighting();
        
        Debug.Log("✅ 环境元素创建完成！");
    }
    
    void CreateLighting()
    {
        // 创建平行光
        GameObject lightObj = new GameObject("Directional Light");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.color = new Color(0.8f, 0.9f, 1f, 1f); // 淡蓝色光
        light.intensity = 0.8f;
        lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);
        
        // 添加环境光
        RenderSettings.ambientLight = new Color(0.2f, 0.4f, 0.6f, 1f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.1f, 0.3f, 0.5f, 1f);
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 20f;
        RenderSettings.fogEndDistance = 80f;
        
        Debug.Log("✅ 灯光设置完成！");
    }
    
    #endregion
    
    #region 创建测试包裹
    
    void CreateTestPackages()
    {
        // 创建包裹预制体目录
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        
        // 创建基础包裹
        GameObject package = new GameObject("Package");
        package.tag = "Package";
        
        SpriteRenderer sr = package.AddComponent<SpriteRenderer>();
        sr.color = new Color(0.55f, 0.27f, 0.07f);
        
        Rigidbody2D rb = package.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        
        CircleCollider2D collider = package.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        
        // 添加包裹脚本
        package.AddComponent<Package>();
        
        // 保存为预制体
        SaveAsPrefab(package, "Prefabs/Package");
        
        // 在场景中生成几个测试包裹
        for (int i = 0; i < 5; i++)
        {
            GameObject testPackage = Instantiate(package);
            testPackage.name = $"TestPackage_{i}";
            testPackage.transform.position = new Vector3(UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(-20f, 20f), 0);
        }
        
        Debug.Log("✅ 测试包裹创建完成！");
    }
    
    #endregion
    
    #region 辅助方法
    
    GameObject CreatePanel(Canvas canvas, string name, Color color)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(canvas.transform, false);
        
        Image image = panel.AddComponent<Image>();
        image.color = color;
        
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
        
        return panel;
    }
    
    GameObject CreateTextElement(GameObject parent, string name, string text, Vector2 position, int fontSize, Color color)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent.transform, false);
        textObj.transform.localPosition = position;
        
        TextMeshProUGUI textComponent = textObj.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = fontSize;
        textComponent.color = color;
        textComponent.alignment = TextAlignmentOptions.Center;
        
        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(600, 60);
        
        return textObj;
    }
    
    GameObject CreateImageElement(GameObject parent, string name, Vector2 position, Vector2 size, Color color)
    {
        GameObject imgObj = new GameObject(name);
        imgObj.transform.SetParent(parent.transform, false);
        imgObj.transform.localPosition = position;
        
        Image image = imgObj.AddComponent<Image>();
        image.color = color;
        
        RectTransform rect = imgObj.GetComponent<RectTransform>();
        rect.sizeDelta = size;
        
        return imgObj;
    }
    
    GameObject CreateButton(GameObject parent, string name, string text, Vector2 position, int width, int height)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent.transform, false);
        buttonObj.transform.localPosition = position;
        
        // 背景
        Image bg = buttonObj.AddComponent<Image>();
        bg.color = new Color(0.2f, 0.5f, 0.8f, 1f);
        
        RectTransform rect = buttonObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, height);
        
        // 添加按钮组件
        Button button = buttonObj.AddComponent<Button>();
        
        // 文本
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        TextMeshProUGUI textComponent = textObj.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = 28;
        textComponent.color = Color.white;
        textComponent.alignment = TextAlignmentOptions.Center;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        
        return buttonObj;
    }
    
    void SaveAsPrefab(GameObject obj, string path)
    {
        // 创建预制体目录
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        
        // 保存预制体
        string fullPath = $"Assets/{path}.prefab";
        PrefabUtility.SaveAsPrefabAsset(obj, fullPath);
        
        Debug.Log($"✅ 预制体已保存：{fullPath}");
    }
    
    #endregion
}
