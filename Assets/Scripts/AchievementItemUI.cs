using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;

/// <summary>
/// 单个成就物品 UI
/// </summary>
public class AchievementItemUI : MonoBehaviour
{
    [Header("UI 元素")]
    public TextMeshProUGUI iconText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI progressText;
    public Image backgroundImage;
    public Image progressBar;
    
    [Header("颜色")]
    public Color unlockedColor = new Color(1f, 0.84f, 0f); // 金色
    public Color lockedColor = Color.gray;
    public Color lockedBackground = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    public Color unlockedBackground = new Color(0.4f, 0.3f, 0.1f, 0.9f);
    
    public void Initialize(Achievement achievement, bool isUnlocked)
    {
        if (iconText != null)
        {
            iconText.text = achievement.icon;
            iconText.color = isUnlocked ? unlockedColor : lockedColor;
        }
        
        if (nameText != null)
        {
            nameText.text = achievement.name;
            nameText.color = isUnlocked ? unlockedColor : lockedColor;
        }
        
        if (descriptionText != null)
        {
            descriptionText.text = achievement.description;
            descriptionText.color = isUnlocked ? Color.white : Color.gray;
        }
        
        if (rewardText != null)
        {
            rewardText.text = isUnlocked ? $"✅ +${achievement.reward}" : $"🔒 +${achievement.reward}";
        }
        
        if (backgroundImage != null)
        {
            backgroundImage.color = isUnlocked ? unlockedBackground : lockedBackground;
        }
        
        // 进度条
        UpdateProgress(achievement, isUnlocked);
    }
    
    void UpdateProgress(Achievement achievement, bool isUnlocked)
    {
        if (isUnlocked)
        {
            if (progressBar != null)
            {
                progressBar.fillAmount = 1f;
            }
            if (progressText != null)
            {
                progressText.text = "已完成";
            }
        }
        else
        {
            // 获取当前进度
            float currentValue = GetCurrentValue(achievement.conditionType);
            float progress = achievement.GetProgress(currentValue);
            
            if (progressBar != null)
            {
                progressBar.fillAmount = progress;
            }
            
            if (progressText != null)
            {
                progressText.text = $"{Mathf.FloorToInt(currentValue)} / {Mathf.FloorToInt(achievement.targetValue)}";
            }
        }
    }
    
    float GetCurrentValue(AchievementConditionType type)
    {
        switch (type)
        {
            case AchievementConditionType.Count:
                return GameManager.Instance != null ? GameManager.Instance.totalDeliveries : 0;
            case AchievementConditionType.TotalMoney:
                return GameManager.Instance != null ? GameManager.Instance.playerMoney : 0;
            case AchievementConditionType.Reputation:
                return GameManager.Instance != null ? GameManager.Instance.playerReputation : 0;
            default:
                return 0;
        }
    }
}
