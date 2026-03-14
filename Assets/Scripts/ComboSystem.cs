using UnityEngine;
using System;

/// <summary>
/// 连击系统 - 追踪连续准时送达
/// </summary>
public class ComboSystem : MonoBehaviour
{
    public static ComboSystem Instance { get; private set; }
    
    [Header("连击配置")]
    public int currentCombo = 0;
    public int maxCombo = 0;
    public float comboTimeWindow = 120f; // 2 分钟内送达算连击
    private float comboTimer = 0f;
    
    [Header("连击奖励")]
    public float comboBonusMultiplier = 0.1f; // 每连击 +10% 奖励
    public float maxComboBonus = 2f; // 最高 +200%
    
    [Header("事件")]
    public event Action<int> OnComboChanged;
    public event Action<int> OnComboBroken;
    public event Action<int, float> OnComboReward;
    
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
    
    void Update()
    {
        if (currentCombo > 0)
        {
            comboTimer -= Time.deltaTime;
            
            if (comboTimer <= 0)
            {
                BreakCombo();
            }
        }
    }
    
    public void AddDelivery(bool isOnTime = true)
    {
        if (isOnTime)
        {
            currentCombo++;
            comboTimer = comboTimeWindow;
            
            if (currentCombo > maxCombo)
            {
                maxCombo = currentCombo;
            }
            
            // 计算连击奖励
            float bonusMultiplier = Mathf.Min(currentCombo * comboBonusMultiplier, maxComboBonus);
            float bonusPercent = bonusMultiplier * 100f;
            
            Debug.Log($"🔥 连击 +1! 当前连击：{currentCombo} (奖励 +{bonusPercent}%)");
            
            OnComboChanged?.Invoke(currentCombo);
            
            // 检查成就
            CheckComboAchievements();
        }
        else
        {
            BreakCombo();
        }
    }
    
    public void BreakCombo()
    {
        if (currentCombo > 0)
        {
            int brokenCombo = currentCombo;
            currentCombo = 0;
            comboTimer = 0f;
            
            Debug.Log($"💔 连击中断！之前连击：{brokenCombo}");
            
            OnComboBroken?.Invoke(brokenCombo);
        }
    }
    
    public float GetComboBonus()
    {
        return Mathf.Min(currentCombo * comboBonusMultiplier, maxComboBonus);
    }
    
    public int GetCombo()
    {
        return currentCombo;
    }
    
    public int GetMaxCombo()
    {
        return maxCombo;
    }
    
    public float GetComboTimeRemaining()
    {
        return comboTimer;
    }
    
    public string GetComboTimeString()
    {
        int mins = Mathf.FloorToInt(comboTimer / 60f);
        int secs = Mathf.FloorToInt(comboTimer % 60f);
        return $"{mins:D2}:{secs:D2}";
    }
    
    void CheckComboAchievements()
    {
        if (AchievementSystem.Instance == null) return;
        
        // 连击成就
        if (currentCombo >= 5)
        {
            AchievementSystem.Instance.UnlockAchievement("perfect_combo_5");
        }
        
        if (currentCombo >= 20)
        {
            AchievementSystem.Instance.UnlockAchievement("perfect_combo_20");
        }
    }
    
    public void ResetCombo()
    {
        currentCombo = 0;
        maxCombo = 0;
        comboTimer = 0f;
    }
    
    public void SaveCombo()
    {
        PlayerPrefs.SetInt("CurrentCombo", currentCombo);
        PlayerPrefs.SetInt("MaxCombo", maxCombo);
        PlayerPrefs.Save();
    }
    
    public void LoadCombo()
    {
        currentCombo = PlayerPrefs.GetInt("CurrentCombo", 0);
        maxCombo = PlayerPrefs.GetInt("MaxCombo", 0);
    }
}
