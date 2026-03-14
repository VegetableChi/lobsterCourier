using UnityEngine;
using System;
using System.IO;

/// <summary>
/// 存档系统 - 保存和加载游戏进度
/// </summary>
public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    
    [Header("存档设置")]
    public string saveFileName = "savegame.json";
    public bool autoSave = true;
    public float autoSaveInterval = 60f;
    
    [Header("存档数据")]
    public GameSaveData currentSave;
    
    private float lastAutoSaveTime = 0f;
    private string savePath;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        
        savePath = Path.Combine(Application.persistentDataPath, saveFileName);
    }
    
    void Start()
    {
        InitializeSaveData();
    }
    
    void Update()
    {
        if (autoSave && Time.time - lastAutoSaveTime >= autoSaveInterval)
        {
            AutoSave();
            lastAutoSaveTime = Time.time;
        }
    }
    
    void InitializeSaveData()
    {
        currentSave = new GameSaveData
        {
            saveVersion = "1.0",
            lastSaveTime = DateTime.Now.ToString(),
            totalPlayTime = 0f,
            totalMoney = 0,
            totalReputation = 0,
            totalDeliveries = 0,
            unlockedUpgrades = new System.Collections.Generic.Dictionary<string, int>(),
            unlockedAchievements = new System.Collections.Generic.List<string>(),
            bestStats = new BestStatsData()
        };
        
        // 尝试加载现有存档
        if (File.Exists(savePath))
        {
            LoadGame();
        }
    }
    
    public void AutoSave()
    {
        Debug.Log("💾 自动保存中...");
        SaveGame();
    }
    
    public void SaveGame()
    {
        try
        {
            // 更新存档数据
            UpdateSaveData();
            
            // 序列化为 JSON
            string json = JsonUtility.ToJson(currentSave, true);
            
            // 写入文件
            File.WriteAllText(savePath, json);
            
            Debug.Log($"✅ 游戏已保存：{savePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"❌ 保存失败：{e.Message}");
        }
    }
    
    public bool LoadGame()
    {
        try
        {
            if (!File.Exists(savePath))
            {
                Debug.LogWarning("⚠️ 存档文件不存在");
                return false;
            }
            
            // 读取文件
            string json = File.ReadAllText(savePath);
            
            // 反序列化
            currentSave = JsonUtility.FromJson<GameSaveData>(json);
            
            // 应用存档数据
            ApplySaveData();
            
            Debug.Log($"✅ 游戏已加载：{savePath}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"❌ 加载失败：{e.Message}");
            return false;
        }
    }
    
    void UpdateSaveData()
    {
        if (GameManager.Instance != null)
        {
            currentSave.totalMoney = GameManager.Instance.playerMoney;
            currentSave.totalReputation = GameManager.Instance.playerReputation;
            currentSave.totalDeliveries = GameManager.Instance.totalDeliveries;
        }
        
        currentSave.lastSaveTime = DateTime.Now.ToString();
        currentSave.totalPlayTime += Time.deltaTime;
        
        // 保存商店升级
        if (ShopSystem.Instance != null)
        {
            // 需要从 ShopSystem 获取升级数据
        }
    }
    
    void ApplySaveData()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerMoney = currentSave.totalMoney;
            GameManager.Instance.playerReputation = currentSave.totalReputation;
            GameManager.Instance.totalDeliveries = currentSave.totalDeliveries;
        }
        
        // 应用成就
        if (AchievementSystem.Instance != null)
        {
            foreach (string achievementId in currentSave.unlockedAchievements)
            {
                AchievementSystem.Instance.UnlockAchievement(achievementId, false);
            }
        }
    }
    
    public void ResetSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
        
        InitializeSaveData();
        Debug.Log("🗑️ 存档已重置");
    }
    
    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("🗑️ 存档文件已删除");
        }
    }
    
    public bool HasSave()
    {
        return File.Exists(savePath);
    }
    
    public GameSaveData GetSaveData()
    {
        return currentSave;
    }
}

[System.Serializable]
public class GameSaveData
{
    public string saveVersion;
    public string lastSaveTime;
    public float totalPlayTime;
    public int totalMoney;
    public int totalReputation;
    public int totalDeliveries;
    public System.Collections.Generic.Dictionary<string, int> unlockedUpgrades;
    public System.Collections.Generic.List<string> unlockedAchievements;
    public BestStatsData bestStats;
}

[System.Serializable]
public class BestStatsData
{
    public int fastestDelivery;
    public int mostMoneyInSingleRun;
    public int highestReputation;
    public int longestCombo;
}
