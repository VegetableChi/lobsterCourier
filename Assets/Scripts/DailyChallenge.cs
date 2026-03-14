using UnityEngine;
using System;

/// <summary>
/// 每日挑战系统 - 每日特殊任务
/// </summary>
public class DailyChallenge : MonoBehaviour
{
    public static DailyChallenge Instance { get; private set; }
    
    [Header("当前挑战")]
    public DailyChallengeData currentChallenge;
    public DateTime challengeDate;
    public bool isCompleted = false;
    
    [Header("挑战进度")]
    public int currentProgress = 0;
    
    [Header("挑战池")]
    public ChallengeTemplate[] challengeTemplates;
    
    [Header("奖励")]
    public int baseMoneyReward = 500;
    public int baseReputationReward = 50;
    
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
    
    void Start()
    {
        LoadOrGenerateChallenge();
    }
    
    void LoadOrGenerateChallenge()
    {
        string lastDate = PlayerPrefs.GetString("LastChallengeDate", "");
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        
        if (lastDate == today)
        {
            // 加载已保存的挑战
            LoadChallenge();
        }
        else
        {
            // 生成新挑战
            GenerateChallenge();
        }
    }
    
    void GenerateChallenge()
    {
        if (challengeTemplates.Length == 0) return;
        
        // 随机选择挑战
        ChallengeTemplate template = challengeTemplates[UnityEngine.Random.Range(0, challengeTemplates.Length)];
        
        currentChallenge = new DailyChallengeData
        {
            id = template.id,
            name = template.name,
            description = template.description,
            targetType = template.targetType,
            targetValue = template.targetValue,
            difficulty = template.difficulty
        };
        
        challengeDate = DateTime.Now;
        currentProgress = 0;
        isCompleted = false;
        
        SaveChallenge();
        
        Debug.Log($"📅 今日挑战：{currentChallenge.name}");
    }
    
    void LoadChallenge()
    {
        string data = PlayerPrefs.GetString("DailyChallengeData", "");
        if (string.IsNullOrEmpty(data))
        {
            GenerateChallenge();
            return;
        }
        
        // 简单解析（实际应该用 JSON）
        string[] parts = data.Split('|');
        if (parts.Length >= 5)
        {
            currentChallenge = new DailyChallengeData
            {
                id = parts[0],
                name = parts[1],
                description = parts[2],
                targetType = (ChallengeTargetType)Enum.Parse(typeof(ChallengeTargetType), parts[3]),
                targetValue = int.Parse(parts[4])
            };
            
            currentProgress = PlayerPrefs.GetInt("DailyChallengeProgress", 0);
            isCompleted = PlayerPrefs.GetInt("DailyChallengeCompleted", 0) == 1;
        }
        else
        {
            GenerateChallenge();
        }
    }
    
    void SaveChallenge()
    {
        string data = $"{currentChallenge.id}|{currentChallenge.name}|{currentChallenge.description}|{currentChallenge.targetType}|{currentChallenge.targetValue}";
        PlayerPrefs.SetString("DailyChallengeData", data);
        PlayerPrefs.SetString("LastChallengeDate", DateTime.Now.ToString("yyyy-MM-dd"));
        PlayerPrefs.SetInt("DailyChallengeProgress", currentProgress);
        PlayerPrefs.SetInt("DailyChallengeCompleted", isCompleted ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    public void AddProgress(int amount = 1)
    {
        if (isCompleted) return;
        
        currentProgress += amount;
        
        if (currentProgress >= currentChallenge.targetValue)
        {
            CompleteChallenge();
        }
        
        SaveChallenge();
    }
    
    void CompleteChallenge()
    {
        isCompleted = true;
        
        // 计算奖励
        int moneyReward = baseMoneyReward * GetDifficultyMultiplier(currentChallenge.difficulty);
        int reputationReward = baseReputationReward * GetDifficultyMultiplier(currentChallenge.difficulty);
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerMoney += moneyReward;
            GameManager.Instance.playerReputation += reputationReward;
        }
        
        Debug.Log($"✅ 完成每日挑战！+${moneyReward} | +{reputationReward}声誉");
        
        SaveChallenge();
    }
    
    int GetDifficultyMultiplier(ChallengeDifficulty difficulty)
    {
        switch (difficulty)
        {
            case ChallengeDifficulty.Easy: return 1;
            case ChallengeDifficulty.Medium: return 2;
            case ChallengeDifficulty.Hard: return 3;
            default: return 1;
        }
    }
    
    public void ResetDailyChallenge()
    {
        PlayerPrefs.DeleteKey("DailyChallengeData");
        PlayerPrefs.DeleteKey("LastChallengeDate");
        PlayerPrefs.DeleteKey("DailyChallengeProgress");
        PlayerPrefs.DeleteKey("DailyChallengeCompleted");
        PlayerPrefs.Save();
        
        GenerateChallenge();
    }
}

[System.Serializable]
public class DailyChallengeData
{
    public string id;
    public string name;
    public string description;
    public ChallengeTargetType targetType;
    public int targetValue;
    public ChallengeDifficulty difficulty;
}

[System.Serializable]
public class ChallengeTemplate
{
    public string id;
    public string name;
    public string description;
    public ChallengeTargetType targetType;
    public int targetValue;
    public ChallengeDifficulty difficulty;
}

public enum ChallengeTargetType
{
    Deliveries,       // 送达次数
    Money,           // 赚取金钱
    Combo,           // 连击数
    PerfectDelivery, // 完美送达
    UrgentDelivery   // 紧急订单
}

public enum ChallengeDifficulty
{
    Easy,
    Medium,
    Hard
}
