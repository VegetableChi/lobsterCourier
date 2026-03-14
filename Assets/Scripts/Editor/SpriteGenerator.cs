using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// 程序化精灵生成器 - 生成占位美术资源
/// 使用方法：菜单栏 → Lobster Courier → 生成美术资源
/// </summary>
public class SpriteGenerator : EditorWindow
{
    private Vector2 scrollPosition;
    
    [MenuItem("Lobster Courier/生成美术资源")]
    public static void ShowWindow()
    {
        var window = GetWindow<SpriteGenerator>("美术资源生成");
        window.minSize = new Vector2(400, 600);
        window.Show();
    }
    
    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        GUILayout.Label("🦞 龙虾快递员 - 程序化美术资源生成", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "本工具将生成程序化占位美术资源，\n" +
            "可以在 Unity 中立即运行游戏。\n" +
            "后续可以替换为精美美术。",
            MessageType.Info);
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("🎨 生成所有美术资源", GUILayout.Height(40)))
        {
            GenerateAllAssets();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("🦞 生成主角精灵", GUILayout.Height(35)))
        {
            GeneratePlayerSprites();
        }
        
        if (GUILayout.Button("🐠 生成 NPC 精灵", GUILayout.Height(35)))
        {
            GenerateNPCSprites();
        }
        
        if (GUILayout.Button("🌊 生成环境资源", GUILayout.Height(35)))
        {
            GenerateEnvironmentAssets();
        }
        
        if (GUILayout.Button("🎨 生成 UI 资源", GUILayout.Height(35)))
        {
            GenerateUIAssets();
        }
        
        if (GUILayout.Button("✨ 生成特效资源", GUILayout.Height(35)))
        {
            GenerateEffectAssets();
        }
        
        EditorGUILayout.EndScrollView();
    }
    
    public void GenerateAllAssets()
    {
        try
        {
            EditorUtility.DisplayProgressBar("生成美术资源", "创建文件夹...", 0f);
            CreateFolders();
            
            EditorUtility.DisplayProgressBar("生成美术资源", "生成主角精灵...", 0.2f);
            GeneratePlayerSprites();
            
            EditorUtility.DisplayProgressBar("生成美术资源", "生成 NPC 精灵...", 0.4f);
            GenerateNPCSprites();
            
            EditorUtility.DisplayProgressBar("生成美术资源", "生成环境资源...", 0.6f);
            GenerateEnvironmentAssets();
            
            EditorUtility.DisplayProgressBar("生成美术资源", "生成 UI 资源...", 0.8f);
            GenerateUIAssets();
            
            EditorUtility.DisplayProgressBar("生成美术资源", "生成特效资源...", 0.9f);
            GenerateEffectAssets();
            
            EditorUtility.ClearProgressBar();
            
            AssetDatabase.Refresh();
            
            Debug.Log("✅ 所有美术资源生成完成！");
            EditorUtility.DisplayDialog("生成完成", 
                "所有程序化美术资源已生成！\n\n" +
                "包含:\n" +
                "• 主角精灵 (12 个)\n" +
                "• NPC 精灵 (24 个)\n" +
                "• 环境资源 (11 个)\n" +
                "• UI 资源 (14 个)\n" +
                "• 特效资源 (5 个)\n\n" +
                "现在可以在 Unity 中运行游戏了！", "好的");
        }
        catch (System.Exception e)
        {
            EditorUtility.ClearProgressBar();
            Debug.LogError($"❌ 生成失败：{e.Message}");
            EditorUtility.DisplayDialog("错误", $"生成失败:\n{e.Message}", "确定");
        }
    }
    
    void CreateFolders()
    {
        CreateFolder("Assets/Sprites");
        CreateFolder("Assets/Sprites/Characters");
        CreateFolder("Assets/Sprites/Environment");
        CreateFolder("Assets/Sprites/UI");
        CreateFolder("Assets/Sprites/Effects");
    }
    
    void CreateFolder(string path)
    {
        if (!AssetDatabase.IsValidFolder(path.Replace("Assets/", "")))
        {
            string[] parts = path.Split('/');
            string currentPath = "Assets";
            
            for (int i = 1; i < parts.Length; i++)
            {
                string newPath = Path.Combine(currentPath, parts[i]);
                if (!AssetDatabase.IsValidFolder(newPath))
                {
                    AssetDatabase.CreateFolder(currentPath, parts[i]);
                }
                currentPath = newPath;
            }
        }
    }
    
    #region 生成主角
    
    void GeneratePlayerSprites()
    {
        string folder = "Assets/Sprites/Characters";
        
        // 龙虾颜色
        Color bodyColor = new Color(1f, 0.42f, 0.29f); // #FF6B4A
        Color bellyColor = new Color(1f, 0.71f, 0.76f); // #FFB6C1
        Color eyeColor = Color.white;
        Color pupilColor = Color.black;
        
        // 生成 4 方向×3 帧 = 12 个精灵
        string[] directions = { "Down", "Up", "Left", "Right" };
        
        for (int d = 0; d < 4; d++)
        {
            for (int f = 0; f < 3; f++)
            {
                Texture2D texture = CreateLobsterSprite(bodyColor, bellyColor, eyeColor, pupilColor, d, f);
                SaveTexture(texture, folder, $"Player_{directions[d]}_Frame{f}");
            }
        }
        
        Debug.Log("✅ 主角精灵生成完成 (12 个)");
    }
    
    Texture2D CreateLobsterSprite(Color bodyColor, Color bellyColor, Color eyeColor, Color pupilColor, int direction, int frame)
    {
        Texture2D texture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
        
        // 清空为透明
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        // 根据方向翻转
        bool flipX = (direction == 2); // Left
        
        // 绘制身体 (椭圆形)
        int centerY = 32 + (frame - 1) * 2; // 呼吸动画
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                int drawX = flipX ? 63 - x : x;
                
                // 身体
                if (IsInEllipse(drawX, y, 32, centerY, 20, 24))
                {
                    texture.SetPixel(x, y, bodyColor);
                }
                
                // 腹部
                if (IsInEllipse(drawX, y, 32, centerY + 5, 12, 15))
                {
                    texture.SetPixel(x, y, bellyColor);
                }
                
                // 眼睛
                if (IsInCircle(drawX, y, 26, centerY - 8, 5))
                {
                    texture.SetPixel(x, y, eyeColor);
                }
                if (IsInCircle(drawX, y, 26, centerY - 8, 2))
                {
                    texture.SetPixel(x, y, pupilColor);
                }
                
                // 钳子 (根据帧动画)
                int clawOffset = (frame - 1) * 3;
                if (IsInEllipse(drawX, y, 48 + clawOffset, centerY + 8, 8, 10))
                {
                    texture.SetPixel(x, y, bodyColor);
                }
                
                // 触角
                if (drawX > 28 && drawX < 36 && y < centerY - 15 && y > centerY - 30)
                {
                    texture.SetPixel(x, y, bodyColor);
                }
            }
        }
        
        texture.Apply();
        return texture;
    }
    
    #endregion
    
    #region 生成 NPC
    
    void GenerateNPCSprites()
    {
        string folder = "Assets/Sprites/Characters";
        
        // 8 种 NPC 颜色
        Color[] npcColors = new Color[]
        {
            new Color(1f, 0.84f, 0f),   // 星星 - 金黄
            new Color(0.61f, 0.35f, 0.71f), // 八爪 - 紫色
            new Color(1f, 0.41f, 0.71f), // 珊瑚 - 粉红
            new Color(0.86f, 0.08f, 0.24f), // 钳子 - 红色
            new Color(0.53f, 0.81f, 0.92f), // 卷卷 - 淡蓝
            new Color(0.9f, 0.9f, 0.98f), // 飘飘 - 淡紫
            new Color(0.13f, 0.55f, 0.13f), // 老海 - 绿色
            new Color(0f, 0f, 0.55f)    // 深深 - 深蓝
        };
        
        string[] npcNames = { "Star", "Octo", "Coral", "Crab", "Seahorse", "Jelly", "Turtle", "Shark" };
        
        for (int i = 0; i < 8; i++)
        {
            // 每个 NPC 生成 3 帧
            for (int f = 0; f < 3; f++)
            {
                Texture2D texture = CreateNPCSprite(npcColors[i], i, f);
                SaveTexture(texture, folder, $"{npcNames[i]}_Frame{f}");
            }
        }
        
        Debug.Log("✅ NPC 精灵生成完成 (24 个)");
    }
    
    Texture2D CreateNPCSprite(Color mainColor, int npcType, int frame)
    {
        Texture2D texture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
        
        // 清空
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        int centerY = 32 + (frame - 1) * 2;
        
        // 根据类型绘制不同形状 - 使用简单几何图形
        switch (npcType)
        {
            case 0: // 星星 - 五角星
                DrawStar(texture, 32, centerY, 24, mainColor);
                break;
            case 1: // 八爪 - 圆形
                DrawCircle(texture, 32, centerY, 24, mainColor);
                break;
            case 2: // 珊瑚鱼 - 椭圆
                DrawEllipse(texture, 32, centerY, 24, 16, mainColor);
                break;
            case 3: // 钳子蟹 - 扁圆
                DrawEllipse(texture, 32, centerY, 28, 18, mainColor);
                break;
            case 4: // 海马 - S 形
                DrawCircle(texture, 32, centerY, 20, mainColor);
                break;
            case 5: // 水母 - 半透明伞状
                DrawCircle(texture, 32, centerY, 24, new Color(mainColor.r, mainColor.g, mainColor.b, 0.5f));
                break;
            case 6: // 海龟 - 椭圆 + 壳
                DrawEllipse(texture, 32, centerY, 26, 20, mainColor);
                break;
            case 7: // 鲨鱼 - 流线型
                DrawEllipse(texture, 32, centerY, 32, 14, mainColor);
                break;
        }
        
        texture.Apply();
        return texture;
    }
    
    void DrawEllipse(Texture2D texture, int centerX, int centerY, int radiusX, int radiusY, Color color)
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                float dx = (x - centerX) / (float)radiusX;
                float dy = (y - centerY) / (float)radiusY;
                if (dx * dx + dy * dy <= 1f)
                {
                    texture.SetPixel(x, y, color);
                }
            }
        }
    }
    
    #endregion
    
    #region 生成环境
    
    void GenerateEnvironmentAssets()
    {
        string folder = "Assets/Sprites/Environment";
        
        // 背景
        Texture2D background = CreateSeabedBackground();
        SaveTexture(background, folder, "SeabedBackground");
        
        // 障碍物
        Texture2D coral = CreateCoral();
        SaveTexture(coral, folder, "CoralObstacle");
        
        Texture2D seaweed = CreateSeaweed();
        SaveTexture(seaweed, folder, "Seaweed");
        
        Texture2D rock = CreateRock();
        SaveTexture(rock, folder, "Rock");
        
        Texture2D shell = CreateShell();
        SaveTexture(shell, folder, "Shell");
        
        Texture2D shipwreck = CreateShipwreck();
        SaveTexture(shipwreck, folder, "Shipwreck");
        
        // 装饰物
        for (int i = 0; i < 5; i++)
        {
            Texture2D decoration = CreateDecoration(i);
            SaveTexture(decoration, folder, $"Decoration_{i}");
        }
        
        Debug.Log("✅ 环境资源生成完成 (11 个)");
    }
    
    Texture2D CreateSeabedBackground()
    {
        Texture2D texture = new Texture2D(512, 512, TextureFormat.RGBA32, false);
        
        // 渐变背景
        for (int y = 0; y < 512; y++)
        {
            float t = y / 512f;
            Color top = new Color(0.29f, 0.56f, 0.78f); // #4A90C8
            Color bottom = new Color(0f, 0f, 0.2f);
            Color gradient = Color.Lerp(bottom, top, t);
            
            for (int x = 0; x < 512; x++)
            {
                texture.SetPixel(x, y, gradient);
            }
        }
        
        // 添加一些气泡
        for (int i = 0; i < 50; i++)
        {
            int x = Random.Range(0, 512);
            int y = Random.Range(0, 512);
            int radius = Random.Range(2, 5);
            DrawCircle(texture, x, y, radius, new Color(0.88f, 1f, 1f, 0.3f));
        }
        
        texture.Apply();
        return texture;
    }
    
    Texture2D CreateCoral()
    {
        Texture2D texture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        // 绘制珊瑚形状
        Color coralColor = new Color(1f, 0.42f, 0.42f);
        for (int x = 24; x < 40; x++)
        {
            for (int y = 32; y < 60; y++)
            {
                if (IsInEllipse(x, y, 32, 50, 10, 20))
                {
                    texture.SetPixel(x, y, coralColor);
                }
            }
        }
        
        texture.Apply();
        return texture;
    }
    
    Texture2D CreateSeaweed()
    {
        Texture2D texture = new Texture2D(32, 96, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 96; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        Color seaweedColor = new Color(0.35f, 0.73f, 0.42f);
        for (int x = 12; x < 20; x++)
        {
            for (int y = 0; y < 96; y++)
            {
                int wave = Mathf.FloorToInt(Mathf.Sin(y * 0.1f) * 3);
                if (x + wave >= 12 && x + wave < 20)
                {
                    texture.SetPixel(x, y, seaweedColor);
                }
            }
        }
        
        texture.Apply();
        return texture;
    }
    
    Texture2D CreateRock()
    {
        Texture2D texture = new Texture2D(48, 48, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 48; x++)
        {
            for (int y = 0; y < 48; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        Color rockColor = new Color(0.41f, 0.41f, 0.41f);
        for (int x = 8; x < 40; x++)
        {
            for (int y = 12; y < 44; y++)
            {
                if (IsInEllipse(x, y, 24, 28, 18, 20))
                {
                    texture.SetPixel(x, y, rockColor);
                }
            }
        }
        
        texture.Apply();
        return texture;
    }
    
    Texture2D CreateShell()
    {
        Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        Color shellColor = new Color(1f, 0.84f, 0f);
        DrawCircle(texture, 16, 16, 12, shellColor);
        
        texture.Apply();
        return texture;
    }
    
    Texture2D CreateShipwreck()
    {
        Texture2D texture = new Texture2D(128, 64, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 128; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        Color woodColor = new Color(0.55f, 0.27f, 0.07f);
        for (int x = 16; x < 112; x++)
        {
            for (int y = 32; y < 60; y++)
            {
                if (IsInEllipse(x, y, 64, 50, 50, 20))
                {
                    texture.SetPixel(x, y, woodColor);
                }
            }
        }
        
        texture.Apply();
        return texture;
    }
    
    Texture2D CreateDecoration(int index)
    {
        Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        Color[] colors = new Color[]
        {
            new Color(1f, 0.42f, 0.42f),
            new Color(0.35f, 0.73f, 0.42f),
            new Color(1f, 0.84f, 0f),
            new Color(0.53f, 0.81f, 0.92f),
            new Color(0.86f, 0.08f, 0.24f)
        };
        
        DrawCircle(texture, 16, 16, 10, colors[index]);
        texture.Apply();
        return texture;
    }
    
    #endregion
    
    #region 生成 UI
    
    void GenerateUIAssets()
    {
        string folder = "Assets/Sprites/UI";
        
        // 按钮
        Texture2D buttonNormal = CreateButton(new Color(0.53f, 0.81f, 0.92f));
        SaveTexture(buttonNormal, folder, "Button_Normal");
        
        Texture2D buttonHover = CreateButton(new Color(0f, 0.75f, 1f));
        SaveTexture(buttonHover, folder, "Button_Hover");
        
        Texture2D buttonPressed = CreateButton(new Color(0.27f, 0.51f, 0.71f));
        SaveTexture(buttonPressed, folder, "Button_Pressed");
        
        // 面板背景
        Texture2D panelBg = CreatePanelBackground();
        SaveTexture(panelBg, folder, "Panel_Background");
        
        // 图标
        Texture2D coinIcon = CreateCoinIcon();
        SaveTexture(coinIcon, folder, "Icon_Coin");
        
        Texture2D starIcon = CreateStarIcon();
        SaveTexture(starIcon, folder, "Icon_Star");
        
        Texture2D packageIcon = CreatePackageIcon();
        SaveTexture(packageIcon, folder, "Icon_Package");
        
        Debug.Log("✅ UI 资源生成完成 (7 个)");
    }
    
    Texture2D CreateButton(Color color)
    {
        Texture2D texture = new Texture2D(200, 50, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 200; x++)
        {
            for (int y = 0; y < 50; y++)
            {
                // 圆角矩形
                float dist = Mathf.Sqrt(
                    Mathf.Pow(Mathf.Max(0, x - 8, 192 - x), 2) +
                    Mathf.Pow(Mathf.Max(0, y - 8, 42 - y), 2)
                );
                
                if (dist <= 8)
                {
                    texture.SetPixel(x, y, color);
                }
                else
                {
                    texture.SetPixel(x, y, Color.clear);
                }
            }
        }
        
        texture.Apply();
        return texture;
    }
    
    Texture2D CreatePanelBackground()
    {
        Texture2D texture = new Texture2D(400, 300, TextureFormat.RGBA32, false);
        
        Color bgColor = new Color(0f, 0f, 0.2f, 0.9f);
        for (int x = 0; x < 400; x++)
        {
            for (int y = 0; y < 300; y++)
            {
                texture.SetPixel(x, y, bgColor);
            }
        }
        
        texture.Apply();
        return texture;
    }
    
    Texture2D CreateCoinIcon()
    {
        Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        DrawCircle(texture, 16, 16, 14, new Color(1f, 0.84f, 0f));
        texture.Apply();
        return texture;
    }
    
    Texture2D CreateStarIcon()
    {
        Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        DrawStar(texture, 16, 16, 14, new Color(1f, 0.84f, 0f));
        texture.Apply();
        return texture;
    }
    
    Texture2D CreatePackageIcon()
    {
        Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        Color packageColor = new Color(0.55f, 0.27f, 0.07f);
        for (int x = 8; x < 24; x++)
        {
            for (int y = 8; y < 24; y++)
            {
                texture.SetPixel(x, y, packageColor);
            }
        }
        
        texture.Apply();
        return texture;
    }
    
    #endregion
    
    #region 生成特效
    
    void GenerateEffectAssets()
    {
        string folder = "Assets/Sprites/Effects";
        
        // 气泡
        Texture2D bubble = CreateBubble();
        SaveTexture(bubble, folder, "Bubble");
        
        // 金币闪光
        Texture2D coinSparkle = CreateCoinSparkle();
        SaveTexture(coinSparkle, folder, "CoinSparkle");
        
        // 爱心
        Texture2D heart = CreateHeart();
        SaveTexture(heart, folder, "Heart");
        
        // 星星
        Texture2D sparkle = CreateSparkle();
        SaveTexture(sparkle, folder, "Sparkle");
        
        // 拖尾
        Texture2D trail = CreateTrail();
        SaveTexture(trail, folder, "Trail");
        
        Debug.Log("✅ 特效资源生成完成 (5 个)");
    }
    
    Texture2D CreateBubble()
    {
        Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        DrawCircle(texture, 16, 16, 12, new Color(0.88f, 1f, 1f, 0.3f));
        DrawCircle(texture, 16, 16, 10, new Color(0.88f, 1f, 1f, 0.1f));
        
        texture.Apply();
        return texture;
    }
    
    Texture2D CreateCoinSparkle()
    {
        Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        DrawStar(texture, 16, 16, 14, new Color(1f, 0.84f, 0f, 0.8f));
        texture.Apply();
        return texture;
    }
    
    Texture2D CreateHeart()
    {
        Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        Color heartColor = new Color(1f, 0.41f, 0.71f);
        DrawCircle(texture, 12, 14, 6, heartColor);
        DrawCircle(texture, 20, 14, 6, heartColor);
        
        for (int x = 8; x < 24; x++)
        {
            for (int y = 14; y < 26; y++)
            {
                if (IsInEllipse(x, y, 16, 18, 10, 10))
                {
                    texture.SetPixel(x, y, heartColor);
                }
            }
        }
        
        texture.Apply();
        return texture;
    }
    
    Texture2D CreateSparkle()
    {
        Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        Color sparkleColor = new Color(1f, 0.84f, 0f);
        for (int i = 0; i < 4; i++)
        {
            int angle = i * 90;
            float rad = angle * Mathf.Deg2Rad;
            int x = Mathf.FloorToInt(16 + Mathf.Cos(rad) * 10);
            int y = Mathf.FloorToInt(16 + Mathf.Sin(rad) * 10);
            DrawCircle(texture, x, y, 3, sparkleColor);
        }
        
        texture.Apply();
        return texture;
    }
    
    Texture2D CreateTrail()
    {
        Texture2D texture = new Texture2D(64, 16, TextureFormat.RGBA32, false);
        
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                float alpha = 1f - (x / 64f);
                texture.SetPixel(x, y, new Color(0.53f, 0.81f, 0.92f, alpha * 0.5f));
            }
        }
        
        texture.Apply();
        return texture;
    }
    
    #endregion
    
    #region 辅助方法
    
    bool IsInCircle(int x, int y, int centerX, int centerY, int radius)
    {
        return (x - centerX) * (x - centerX) + (y - centerY) * (y - centerY) <= radius * radius;
    }
    
    bool IsInEllipse(int x, int y, int centerX, int centerY, int radiusX, int radiusY)
    {
        float dx = (x - centerX) / (float)radiusX;
        float dy = (y - centerY) / (float)radiusY;
        return dx * dx + dy * dy <= 1f;
    }
    
    void DrawCircle(Texture2D texture, int centerX, int centerY, int radius, Color color)
    {
        for (int x = centerX - radius; x <= centerX + radius; x++)
        {
            for (int y = centerY - radius; y <= centerY + radius; y++)
            {
                if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                {
                    if (IsInCircle(x, y, centerX, centerY, radius))
                    {
                        texture.SetPixel(x, y, color);
                    }
                }
            }
        }
    }
    
    void DrawStar(Texture2D texture, int centerX, int centerY, int radius, Color color)
    {
        for (int i = 0; i < 5; i++)
        {
            float angle1 = (i * 72 - 90) * Mathf.Deg2Rad;
            float angle2 = ((i * 72) + 36 - 90) * Mathf.Deg2Rad;
            
            int x1 = Mathf.FloorToInt(centerX + Mathf.Cos(angle1) * radius);
            int y1 = Mathf.FloorToInt(centerY + Mathf.Sin(angle1) * radius);
            int x2 = Mathf.FloorToInt(centerX + Mathf.Cos(angle2) * radius * 0.5f);
            int y2 = Mathf.FloorToInt(centerY + Mathf.Sin(angle2) * radius * 0.5f);
            
            DrawLine(texture, x1, y1, x2, y2, color);
        }
        
        // 填充中心
        DrawCircle(texture, centerX, centerY, radius / 3, color);
    }
    
    void DrawLine(Texture2D texture, int x0, int y0, int x1, int y1, Color color)
    {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = (x0 < x1) ? 1 : -1;
        int sy = (y0 < y1) ? 1 : -1;
        int err = dx - dy;
        
        while (true)
        {
            if (x0 >= 0 && x0 < texture.width && y0 >= 0 && y0 < texture.height)
            {
                texture.SetPixel(x0, y0, color);
            }
            
            if (x0 == x1 && y0 == y1) break;
            
            int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; x0 += sx; }
            if (e2 < dx) { err += dx; y0 += sy; }
        }
    }
    
    void SaveTexture(Texture2D texture, string folder, string name)
    {
        string path = Path.Combine(folder, name + ".png");
        byte[] pngData = texture.EncodeToPNG();
        File.WriteAllBytes(path, pngData);
        AssetDatabase.Refresh();
    }
    
    #endregion
}
