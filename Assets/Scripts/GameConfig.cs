using UnityEngine;

/// <summary>
/// 游戏配置 - ScriptableObject 集中管理游戏数值
/// 在 Unity 中右键 Create → Game Config 创建配置资产
/// </summary>
[CreateAssetMenu(fileName = "GameConfig", menuName = "Lobster Courier/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("玩家基础数值")]
    [Tooltip("基础移动速度")]
    public float baseMoveSpeed = 5f;
    
    [Tooltip("冲刺速度倍率")]
    public float sprintMultiplier = 2f;
    
    [Tooltip("最大体力")]
    public float baseMaxStamina = 100f;
    
    [Tooltip("体力消耗速率")]
    public float staminaDrainRate = 20f;
    
    [Tooltip("体力恢复速率")]
    public float staminaRegenRate = 10f;
    
    [Header("经济系统")]
    [Tooltip("初始金钱")]
    public int startingMoney = 100;
    
    [Tooltip("基础包裹价值")]
    public float basePackageValue = 10f;
    
    [Tooltip("易碎品价值倍率")]
    public float fragileValueMultiplier = 1.5f;
    
    [Tooltip("紧急订单价值倍率")]
    public float urgentValueMultiplier = 2f;
    
    [Tooltip("超时声誉惩罚")]
    public int timeoutReputationPenalty = 5;
    
    [Header("升级数值")]
    [Tooltip("速度升级成本倍率")]
    public float speedUpgradeCostMultiplier = 1.5f;
    
    [Tooltip("体力升级成本倍率")]
    public float staminaUpgradeCostMultiplier = 1.3f;
    
    [Tooltip("背包升级成本倍率")]
    public float backpackUpgradeCostMultiplier = 2f;
    
    [Header("生成配置")]
    [Tooltip("世界大小")]
    public float worldSize = 100f;
    
    [Tooltip("障碍物数量")]
    public int obstacleCount = 20;
    
    [Tooltip("洋流区域数量")]
    public int currentZoneCount = 5;
    
    [Tooltip("送货点数量")]
    public int deliveryPointCount = 8;
    
    [Tooltip("包裹生成间隔")]
    public float packageSpawnInterval = 30f;
    
    [Tooltip("最大活跃包裹数")]
    public int maxActivePackages = 5;
    
    [Header("时间限制")]
    [Tooltip("基础订单时间")]
    public float baseOrderTime = 60f;
    
    [Tooltip("紧急订单时间倍率")]
    public float urgentTimeMultiplier = 0.5f;
    
    [Header("难度曲线")]
    [Tooltip("每 10 单难度提升")]
    public float difficultyIncreaseInterval = 10f;
    
    [Tooltip("难度提升后速度增加")]
    public float difficultySpeedIncrease = 0.2f;
    
    [Tooltip("难度提升后洋流增强")]
    public float difficultyCurrentIncrease = 0.1f;
    
    [Header("成就阈值")]
    [Tooltip("送货达人成就要求")]
    public int hundredDeliveriesThreshold = 100;
    
    [Tooltip("传奇快递员成就要求")]
    public int thousandDeliveriesThreshold = 1000;
    
    [Tooltip("富有的龙虾成就要求")]
    public int richLobsterThreshold = 10000;
    
    [Tooltip("百万富翁成就要求")]
    public int millionaireThreshold = 1000000;
    
    [Header("UI 设置")]
    [Tooltip("成就通知显示时长")]
    public float achievementNotificationDuration = 3f;
    
    [Tooltip("成就通知淡入淡出时长")]
    public float achievementNotificationFadeDuration = 0.5f;
    
    [Header("音频设置")]
    [Range(0f, 1f)]
    public float defaultBGMVolume = 0.5f;
    
    [Range(0f, 1f)]
    public float defaultSFXVolume = 0.7f;
    
    [Range(0f, 1f)]
    public float defaultAmbientVolume = 0.3f;
    
    [Header("存档设置")]
    [Tooltip("自动保存间隔 (秒)")]
    public float autoSaveInterval = 60f;
    
    [Tooltip("存档文件名")]
    public string saveFileName = "savegame.json";
}
