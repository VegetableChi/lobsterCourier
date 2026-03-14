using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 成就系统 - 追踪和解锁成就
/// </summary>
public class AchievementSystem : MonoBehaviour
{
    public static AchievementSystem Instance { get; private set; }
    
    [Header("成就列表")]
    public List<Achievement> achievements = new List<Achievement>();
    
    [Header("UI 引用")]
    public AchievementUI achievementUI;
    
    private Dictionary<string, Achievement> achievementDict;
    private HashSet<string> unlockedAchievements;
    
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
        InitializeAchievements();
        LoadUnlockedAchievements();
    }
    
    void InitializeAchievements()
    {
        achievementDict = new Dictionary<string, Achievement>();
        unlockedAchievements = new HashSet<string>();
        
        // 送货相关成就
        achievements.Add(new Achievement
        {
            id = "first_delivery",
            name = "第一单",
            description = "完成第一次送货",
            icon = "📦",
            category = AchievementCategory.Delivery,
            conditionType = AchievementConditionType.Count,
            targetValue = 1,
            reward = 50
        });
        
        achievements.Add(new Achievement
        {
            id = "hundred_deliveries",
            name = "送货达人",
            description = "完成 100 次送货",
            icon = "🎯",
            category = AchievementCategory.Delivery,
            conditionType = AchievementConditionType.Count,
            targetValue = 100,
            reward = 500
        });
        
        achievements.Add(new Achievement
        {
            id = "thousand_deliveries",
            name = "传奇快递员",
            description = "完成 1000 次送货",
            icon = "🏆",
            category = AchievementCategory.Delivery,
            conditionType = AchievementConditionType.Count,
            targetValue = 1000,
            reward = 2000
        });
        
        // 金钱相关成就
        achievements.Add(new Achievement
        {
            id = "rich_lobster",
            name = "富有的龙虾",
            description = "累计赚取 10000 金币",
            icon = "💰",
            category = AchievementCategory.Money,
            conditionType = AchievementConditionType.TotalMoney,
            targetValue = 10000,
            reward = 1000
        });
        
        achievements.Add(new Achievement
        {
            id = "millionaire",
            name = "百万富翁",
            description = "累计赚取 100 万金币",
            icon = "💎",
            category = AchievementCategory.Money,
            conditionType = AchievementConditionType.TotalMoney,
            targetValue = 1000000,
            reward = 10000
        });
        
        // 声誉相关成就
        achievements.Add(new Achievement
        {
            id = "five_stars",
            name = "五星好评",
            description = "声誉达到 100",
            icon = "⭐",
            category = AchievementCategory.Reputation,
            conditionType = AchievementConditionType.Reputation,
            targetValue = 100,
            reward = 300
        });
        
        // 连击成就
        achievements.Add(new Achievement
        {
            id = "perfect_combo_5",
            name = "完美连击",
            description = "连续准时送达 5 次",
            icon = "🔥",
            category = AchievementCategory.Combo,
            conditionType = AchievementConditionType.Combo,
            targetValue = 5,
            reward = 200
        });
        
        achievements.Add(new Achievement
        {
            id = "perfect_combo_20",
            name = "连击大师",
            description = "连续准时送达 20 次",
            icon = "⚡",
            category = AchievementCategory.Combo,
            conditionType = AchievementConditionType.Combo,
            targetValue = 20,
            reward = 1000
        });
        
        // 特殊成就
        achievements.Add(new Achievement
        {
            id = "speed_demon",
            name = "速度恶魔",
            description = "在 10 秒内完成一次送货",
            icon = "💨",
            category = AchievementCategory.Special,
            conditionType = AchievementConditionType.FastDelivery,
            targetValue = 10,
            reward = 500
        });
        
        achievements.Add(new Achievement
        {
            id = "no_damage",
            name = "完美无缺",
            description = "不损坏任何包裹完成 10 次送货",
            icon = "🛡️",
            category = AchievementCategory.Special,
            conditionType = AchievementConditionType.NoDamage,
            targetValue = 10,
            reward = 800
        });
        
        // 收集成就
        achievements.Add(new Achievement
        {
            id = "all_customers",
            name = "人脉广泛",
            description = "为所有类型的客户送过货",
            icon = "🤝",
            category = AchievementCategory.Collection,
            conditionType = AchievementConditionType.UniqueCustomers,
            targetValue = 8,
            reward = 600
        });
        
        // 建立字典
        foreach (var achievement in achievements)
        {
            achievementDict[achievement.id] = achievement;
        }
    }
    
    public void CheckAchievement(AchievementConditionType type, float value)
    {
        foreach (var achievement in achievements)
        {
            if (unlockedAchievements.Contains(achievement.id)) continue;
            
            if (achievement.conditionType == type)
            {
                if (value >= achievement.targetValue)
                {
                    UnlockAchievement(achievement.id);
                }
            }
        }
    }
    
    public void UnlockAchievement(string achievementId, bool showNotification = true)
    {
        if (unlockedAchievements.Contains(achievementId)) return;
        
        if (achievementDict.ContainsKey(achievementId))
        {
            unlockedAchievements.Add(achievementId);
            
            Achievement achievement = achievementDict[achievementId];
            
            // 给予奖励
            if (GameManager.Instance != null)
            {
                GameManager.Instance.playerMoney += achievement.reward;
            }
            
            // 显示通知
            if (showNotification && achievementUI != null)
            {
                achievementUI.ShowAchievementNotification(achievement);
            }
            
            // 保存
            SaveUnlockedAchievements();
            
            Debug.Log($"🏆 成就解锁：{achievement.name} - +${achievement.reward}");
        }
    }
    
    public bool IsUnlocked(string achievementId)
    {
        return unlockedAchievements.Contains(achievementId);
    }
    
    public int GetUnlockedCount()
    {
        return unlockedAchievements.Count;
    }
    
    public int GetTotalCount()
    {
        return achievements.Count;
    }
    
    void SaveUnlockedAchievements()
    {
        string data = string.Join(",", unlockedAchievements);
        PlayerPrefs.SetString("UnlockedAchievements", data);
        PlayerPrefs.Save();
    }
    
    void LoadUnlockedAchievements()
    {
        if (PlayerPrefs.HasKey("UnlockedAchievements"))
        {
            string data = PlayerPrefs.GetString("UnlockedAchievements");
            string[] ids = data.Split(',');
            foreach (string id in ids)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    unlockedAchievements.Add(id);
                }
            }
        }
    }
    
    public List<Achievement> GetUnlockedAchievements()
    {
        List<Achievement> unlocked = new List<Achievement>();
        foreach (var achievement in achievements)
        {
            if (unlockedAchievements.Contains(achievement.id))
            {
                unlocked.Add(achievement);
            }
        }
        return unlocked;
    }
    
    public List<Achievement> GetLockedAchievements()
    {
        List<Achievement> locked = new List<Achievement>();
        foreach (var achievement in achievements)
        {
            if (!unlockedAchievements.Contains(achievement.id))
            {
                locked.Add(achievement);
            }
        }
        return locked;
    }
}

[System.Serializable]
public class Achievement
{
    public string id;
    public string name;
    public string description;
    public string icon;
    public AchievementCategory category;
    public AchievementConditionType conditionType;
    public float targetValue;
    public int reward;
    
    public float GetProgress(float currentValue)
    {
        return Mathf.Min(currentValue / targetValue, 1f);
    }
}

public enum AchievementCategory
{
    Delivery,
    Money,
    Reputation,
    Combo,
    Special,
    Collection
}

public enum AchievementConditionType
{
    Count,
    TotalMoney,
    Reputation,
    Combo,
    FastDelivery,
    NoDamage,
    UniqueCustomers
}
