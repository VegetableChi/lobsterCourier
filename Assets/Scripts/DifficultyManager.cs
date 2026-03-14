using UnityEngine;

/// <summary>
/// 难度管理器 - 动态调整游戏难度
/// </summary>
public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }
    
    [Header("难度等级")]
    public int currentDifficulty = 1;
    public float difficultyTimer = 0f;
    public float difficultyIncreaseInterval = 120f; // 每 2 分钟提升难度
    
    [Header("难度参数")]
    [Range(1f, 5f)] public float speedMultiplier = 1f;
    [Range(1f, 3f)] public float currentStrengthMultiplier = 1f;
    [Range(0.5f, 2f)] public float packageValueMultiplier = 1f;
    [Range(0.5f, 2f)] public float orderTimeMultiplier = 1f;
    
    [Header("难度上限")]
    public int maxDifficulty = 10;
    
    // 事件
    public delegate void DifficultyChangedEvent(int newDifficulty);
    public static event DifficultyChangedEvent OnDifficultyIncreased;
    
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
        ResetDifficulty();
    }
    
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentState == GameState.Playing)
        {
            difficultyTimer += Time.deltaTime;
            
            if (difficultyTimer >= difficultyIncreaseInterval)
            {
                IncreaseDifficulty();
                difficultyTimer = 0f;
            }
        }
    }
    
    public void IncreaseDifficulty()
    {
        if (currentDifficulty >= maxDifficulty) return;
        
        currentDifficulty++;
        
        // 更新难度参数
        UpdateDifficultyParameters();
        
        // 触发事件
        OnDifficultyIncreased?.Invoke(currentDifficulty);
        
        Debug.Log($"📈 难度提升！当前难度：{currentDifficulty}");
    }
    
    void UpdateDifficultyParameters()
    {
        // 速度 multiplier (1.0 -> 2.0)
        speedMultiplier = 1f + (currentDifficulty - 1) * 0.1f;
        
        // 洋流强度 (1.0 -> 2.0)
        currentStrengthMultiplier = 1f + (currentDifficulty - 1) * 0.1f;
        
        // 包裹价值 (1.0 -> 1.5)
        packageValueMultiplier = 1f + (currentDifficulty - 1) * 0.05f;
        
        // 订单时间 (1.0 -> 0.7)
        orderTimeMultiplier = 1f - (currentDifficulty - 1) * 0.03f;
        orderTimeMultiplier = Mathf.Max(0.7f, orderTimeMultiplier);
    }
    
    public void ResetDifficulty()
    {
        currentDifficulty = 1;
        difficultyTimer = 0f;
        speedMultiplier = 1f;
        currentStrengthMultiplier = 1f;
        packageValueMultiplier = 1f;
        orderTimeMultiplier = 1f;
    }
    
    public float GetPackageValue(float baseValue)
    {
        return baseValue * packageValueMultiplier;
    }
    
    public float GetOrderTime(float baseTime)
    {
        return baseTime * orderTimeMultiplier;
    }
    
    public float GetCurrentSpeedMultiplier()
    {
        return speedMultiplier;
    }
    
    public float GetCurrentStrengthMultiplier()
    {
        return currentStrengthMultiplier;
    }
    
    public string GetDifficultyName()
    {
        switch (currentDifficulty)
        {
            case 1: return "新手";
            case 2: return "学徒";
            case 3: return "熟练";
            case 4: return "专业";
            case 5: return "专家";
            case 6: return "大师";
            case 7: return "宗师";
            case 8: return "传奇";
            case 9: return "史诗";
            case 10: return "神话";
            default: return "未知";
        }
    }
    
    public int GetDifficultyStars()
    {
        return Mathf.CeilToInt(currentDifficulty / 2f);
    }
}
