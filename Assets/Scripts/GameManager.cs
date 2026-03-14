using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// 游戏管理器 - 核心游戏逻辑和状态管理 (优化版)
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("游戏状态")]
    public GameState currentState;
    public float gameSpeed = 1f;
    
    [Header("玩家引用")]
    public LobsterController player;
    
    [Header("经济系统")]
    public int playerMoney = 0;
    public int playerReputation = 0;
    public int totalDeliveries = 0;
    public int totalMoneyEarned = 0;
    
    [Header("任务系统")]
    public List<DeliveryOrder> activeOrders = new List<DeliveryOrder>();
    public List<DeliveryOrder> completedOrders = new List<DeliveryOrder>();
    
    [Header("UI 引用")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI reputationText;
    public TextMeshProUGUI orderCountText;
    public TextMeshProUGUI comboText;
    public StaminaBar staminaBar;
    public OrderUI orderUI;
    public AchievementUI achievementUI;
    
    [Header("生成设置")]
    public Package[] packagePrefabs;
    public DeliveryPoint[] deliveryPoints;
    public Transform[] spawnPoints;
    
    [Header("配置")]
    public GameConfig gameConfig;
    
    // 事件
    public delegate void GameEvent();
    public static event GameEvent OnGameStart;
    public static event GameEvent OnGamePause;
    public static event GameEvent OnGameResume;
    public static event GameEvent OnGameOver;
    
    public delegate void MoneyChangedEvent(int amount);
    public static event MoneyChangedEvent OnMoneyChanged;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 只在 Play 模式下使用 DontDestroyOnLoad
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // 加载配置
        if (gameConfig == null)
        {
            // 尝试从 Resources 加载
            gameConfig = Resources.Load<GameConfig>("GameConfig");
        }
        
        StartNewGame();
        
        // 加载存档
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.LoadGame();
        }
        
        // 加载连击
        if (ComboSystem.Instance != null)
        {
            ComboSystem.Instance.LoadCombo();
        }
    }
    
    void Update()
    {
        HandleInput();
        UpdateUI();
    }
    
    void HandleInput()
    {
        // 暂停/继续
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
        
        // 重新开始
        if (Input.GetKeyDown(KeyCode.R) && currentState == GameState.GameOver)
        {
            StartNewGame();
        }
        
        // 打开商店
        if (Input.GetKeyDown(KeyCode.B))
        {
            // ShopUI 会处理
        }
        
        // 打开成就
        if (Input.GetKeyDown(KeyCode.A))
        {
            achievementUI?.OpenAchievementPanel();
        }
        
        // 小地图切换
        if (Input.GetKeyDown(KeyCode.M))
        {
            orderUI?.ToggleMinimap();
        }
    }
    
    public void StartNewGame()
    {
        currentState = GameState.Playing;
        
        // 从配置加载初始值
        if (gameConfig != null)
        {
            playerMoney = gameConfig.startingMoney;
        }
        else
        {
            playerMoney = 100;
        }
        
        playerReputation = 0;
        totalDeliveries = 0;
        totalMoneyEarned = 0;
        activeOrders.Clear();
        completedOrders.Clear();
        
        Time.timeScale = gameSpeed;
        
        OnGameStart?.Invoke();
        
        // 重置系统
        ComboSystem.Instance?.ResetCombo();
        
        Debug.Log("🎮 游戏开始！开始送货吧！");
        
        // 生成初始订单
        for (int i = 0; i < 3; i++)
        {
            SpawnNewOrder();
        }
    }
    
    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0f;
            OnGamePause?.Invoke();
            Debug.Log("⏸️ 游戏暂停");
        }
        else if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = gameSpeed;
            OnGameResume?.Invoke();
            Debug.Log("▶️ 游戏继续");
        }
    }
    
    #region 包裹管理
    
    public void OnPackagePickedUp(Package package)
    {
        // 查找对应的订单
        var order = activeOrders.Find(o => o.package == package);
        if (order != null)
        {
            order.isPickedUp = true;
        }
        
        // 播放音效
        AudioManager.Instance?.PlayPickupSound();
        
        // 播放粒子
        ParticleManager.Instance?.PlayPickup(package.transform.position);
    }
    
    public void OnPackageDropped()
    {
        // 可以添加惩罚逻辑
    }
    
    public void OnPackageDelivered(Package package, float value)
    {
        totalDeliveries++;
        
        // 计算连击奖励
        float comboBonus = 0f;
        if (ComboSystem.Instance != null)
        {
            comboBonus = ComboSystem.Instance.GetComboBonus();
            ComboSystem.Instance.AddDelivery(true);
        }
        
        // 计算最终奖励
        int reward = Mathf.RoundToInt(value * (1f + comboBonus));
        int reputationGain = 1;
        
        if (package.isUrgent) reputationGain += 2;
        if (package.isFragile) reputationGain += 1;
        
        playerMoney += reward;
        totalMoneyEarned += reward;
        playerReputation += reputationGain;
        
        // 移除订单
        var order = activeOrders.Find(o => o.package == package);
        if (order != null)
        {
            activeOrders.Remove(order);
            completedOrders.Add(order);
        }
        
        OnMoneyChanged?.Invoke(reward);
        
        // 播放效果
        AudioManager.Instance?.PlayDeliverySound();
        ParticleManager.Instance?.PlayDeliverySuccess(package.transform.position);
        ParticleManager.Instance?.PlayCoin(package.transform.position);
        
        Debug.Log($"✅ 送达！+${reward} (连击 +{comboBonus * 100}%) | 声誉 +{reputationGain}");
        
        // 检查成就
        CheckDeliveryAchievements();
        
        // 生成新订单
        if (activeOrders.Count < 3)
        {
            SpawnNewOrder();
        }
        
        // 自动保存
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.AutoSave();
        }
    }
    
    public void OnPackageExpired(Package package)
    {
        playerReputation -= (gameConfig != null) ? gameConfig.timeoutReputationPenalty : 5;
        
        // 打断连击
        if (ComboSystem.Instance != null)
        {
            ComboSystem.Instance.AddDelivery(false);
        }
        
        var order = activeOrders.Find(o => o.package == package);
        if (order != null)
        {
            activeOrders.Remove(order);
        }
        
        // 播放效果
        AudioManager.Instance?.PlayExhaustedSound();
        ParticleManager.Instance?.PlayDeliveryFail(package.transform.position);
        
        Debug.Log($"❌ 订单超时！声誉 -5");
        
        // 生成新订单
        SpawnNewOrder();
    }
    
    #endregion
    
    #region 订单生成
    
    public void SpawnNewOrder()
    {
        if (spawnPoints.Length == 0 || packagePrefabs.Length == 0 || deliveryPoints.Length == 0) return;
        
        // 随机选择生成点
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        DeliveryPoint deliveryPoint = deliveryPoints[Random.Range(0, deliveryPoints.Length)];
        
        // 随机选择包裹类型
        PackageType type = (PackageType)Random.Range(0, System.Enum.GetNames(typeof(PackageType)).Length);
        
        // 创建订单
        DeliveryOrder order = new DeliveryOrder
        {
            package = null,
            pickupLocation = spawnPoint,
            deliveryLocation = deliveryPoint,
            isPickedUp = false,
            isCompleted = false
        };
        
        activeOrders.Add(order);
        
        Debug.Log($"📋 新订单！从 {spawnPoint.name} 到 {deliveryPoint.name}");
    }
    
    #endregion
    
    #region UI 更新
    
    void UpdateUI()
    {
        if (moneyText != null)
        {
            moneyText.text = $"💰 ${playerMoney}";
        }
        
        if (reputationText != null)
        {
            reputationText.text = $"⭐ {playerReputation}";
        }
        
        if (orderCountText != null)
        {
            orderCountText.text = $"📦 {activeOrders.Count}";
        }
        
        if (comboText != null && ComboSystem.Instance != null)
        {
            int combo = ComboSystem.Instance.GetCombo();
            if (combo > 0)
            {
                comboText.text = $"🔥 连击 x{combo}";
            }
            else
            {
                comboText.text = "";
            }
        }
        
        if (staminaBar != null && player != null)
        {
            staminaBar.SetStamina(player.StaminaPercent);
        }
    }
    
    #endregion
    
    #region 成就检查
    
    void CheckDeliveryAchievements()
    {
        if (AchievementSystem.Instance == null) return;
        
        // 送货数量成就
        AchievementSystem.Instance.CheckAchievement(AchievementConditionType.Count, totalDeliveries);
        
        // 金钱成就
        AchievementSystem.Instance.CheckAchievement(AchievementConditionType.TotalMoney, totalMoneyEarned);
        
        // 声誉成就
        AchievementSystem.Instance.CheckAchievement(AchievementConditionType.Reputation, playerReputation);
    }
    
    #endregion
    
    void OnDestroy()
    {
        // 保存连击
        if (ComboSystem.Instance != null)
        {
            ComboSystem.Instance.SaveCombo();
        }
        
        Time.timeScale = 1f;
    }
}

public enum GameState
{
    Menu,
    Playing,
    Paused,
    GameOver
}

[System.Serializable]
public class DeliveryOrder
{
    public Package package;
    public Transform pickupLocation;
    public DeliveryPoint deliveryLocation;
    public bool isPickedUp;
    public bool isCompleted;
}
