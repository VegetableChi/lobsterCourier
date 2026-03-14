using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// 成就 UI 显示
/// </summary>
public class AchievementUI : MonoBehaviour
{
    [Header("通知面板")]
    public GameObject notificationPanel;
    public TextMeshProUGUI achievementIcon;
    public TextMeshProUGUI achievementName;
    public TextMeshProUGUI achievementDescription;
    public TextMeshProUGUI rewardText;
    public Image notificationBackground;
    
    [Header("动画设置")]
    public float showDuration = 3f;
    public float fadeDuration = 0.5f;
    
    [Header("成就面板")]
    public GameObject achievementPanel;
    public GameObject achievementListContent;
    public GameObject achievementItemPrefab;
    
    private bool isNotificationShowing = false;
    
    public void ShowAchievementNotification(Achievement achievement)
    {
        if (notificationPanel == null) return;
        
        // 设置内容
        if (achievementIcon != null)
        {
            achievementIcon.text = achievement.icon;
        }
        
        if (achievementName != null)
        {
            achievementName.text = achievement.name;
        }
        
        if (achievementDescription != null)
        {
            achievementDescription.text = achievement.description;
        }
        
        if (rewardText != null)
        {
            rewardText.text = $"+${achievement.reward}";
        }
        
        // 显示通知
        notificationPanel.SetActive(true);
        
        // 淡入
        StartCoroutine(FadeInNotification());
        
        // 自动隐藏
        StartCoroutine(HideNotificationAfterDelay());
    }
    
    IEnumerator FadeInNotification()
    {
        CanvasGroup canvasGroup = notificationPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = notificationPanel.AddComponent<CanvasGroup>();
        }
        
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
    
    IEnumerator HideNotificationAfterDelay()
    {
        yield return new WaitForSecondsRealtime(showDuration);
        
        CanvasGroup canvasGroup = notificationPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = notificationPanel.AddComponent<CanvasGroup>();
        }
        
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        
        notificationPanel.SetActive(false);
        canvasGroup.alpha = 1f;
    }
    
    public void OpenAchievementPanel()
    {
        if (achievementPanel == null) return;
        
        achievementPanel.SetActive(true);
        RefreshAchievementList();
        
        // 暂停游戏
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TogglePause();
        }
    }
    
    public void CloseAchievementPanel()
    {
        if (achievementPanel == null) return;
        
        achievementPanel.SetActive(false);
        
        // 继续游戏
        if (GameManager.Instance != null && GameManager.Instance.currentState == GameState.Paused)
        {
            GameManager.Instance.TogglePause();
        }
    }
    
    public void RefreshAchievementList()
    {
        if (achievementListContent == null || achievementItemPrefab == null) return;
        
        // 清空现有
        foreach (Transform child in achievementListContent.transform)
        {
            Destroy(child.gameObject);
        }
        
        // 添加成就
        if (AchievementSystem.Instance != null)
        {
            foreach (var achievement in AchievementSystem.Instance.achievements)
            {
                CreateAchievementItem(achievement);
            }
        }
    }
    
    void CreateAchievementItem(Achievement achievement)
    {
        GameObject itemObj = Instantiate(achievementItemPrefab, achievementListContent.transform);
        AchievementItemUI itemUI = itemObj.GetComponent<AchievementItemUI>();
        
        if (itemUI != null)
        {
            bool isUnlocked = AchievementSystem.Instance.IsUnlocked(achievement.id);
            itemUI.Initialize(achievement, isUnlocked);
        }
    }
}
