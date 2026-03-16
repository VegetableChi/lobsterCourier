using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 游戏 UI 管理器 - 统一管理所有 UI 元素
/// 基于 UI_DESIGN_SYSTEM.md v2.0
/// </summary>
public class GameUIManager : MonoBehaviour
{
    [Header("UI 预设件")]
    public GameObject mainMenuPanel;
    public GameObject gameHUDPanel;
    public GameObject scorePanel;
    public GameObject pausePanel;
    public GameObject settingsPanel;
    
    [Header("UI 元素引用")]
    public Text scoreText;
    public Text timerText;
    public Text levelText;
    public Image progressBar;
    public Button pauseButton;
    public Button settingsButton;
    
    [Header("颜色配置")]
    public Color oceanBlue = new Color(74f/255, 144f/255, 200f/255);
    public Color coralOrange = new Color(255f/255, 140f/255, 66f/255);
    public Color seaweedGreen = new Color(88f/255, 185f/255, 106f/255);
    public Color gold = new Color(255f/255, 215f/255, 0f/255);
    
    [Header("动画配置")]
    public float fadeDuration = 0.3f;
    public float slideDuration = 0.2f;
    
    private static GameUIManager instance;
    private CanvasGroup activePanel;
    
    public static GameUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameUIManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameUIManager");
                    instance = go.AddComponent<GameUIManager>();
                }
            }
            return instance;
        }
    }
    
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeUI();
    }
    
    /// <summary>
    /// 初始化 UI 系统
    /// </summary>
    void InitializeUI()
    {
        // 设置所有面板的初始状态
        SetAllPanelsActive(false);
        
        // 显示主菜单
        ShowMainMenu();
        
        Debug.Log("[GameUIManager] UI 系统初始化完成");
    }
    
    /// <summary>
    /// 设置所有面板的激活状态
    /// </summary>
    void SetAllPanelsActive(bool active)
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(active);
        if (gameHUDPanel != null) gameHUDPanel.SetActive(active);
        if (scorePanel != null) scorePanel.SetActive(active);
        if (pausePanel != null) pausePanel.SetActive(active);
        if (settingsPanel != null) settingsPanel.SetActive(active);
    }
    
    #region 菜单控制
    
    public void ShowMainMenu()
    {
        SetAllPanelsActive(false);
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
            FadeIn(mainMenuPanel);
        }
        Time.timeScale = 0;
    }
    
    public void HideMainMenu()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }
    }
    
    public void StartGame()
    {
        HideMainMenu();
        ShowGameHUD();
        Time.timeScale = 1;
        GameManager.Instance?.StartGame();
    }
    
    #endregion
    
    #region 游戏 HUD
    
    public void ShowGameHUD()
    {
        SetAllPanelsActive(false);
        if (gameHUDPanel != null)
        {
            gameHUDPanel.SetActive(true);
            FadeIn(gameHUDPanel);
        }
    }
    
    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"📦 {score}";
            // 分数变化动画
            StartCoroutine(ScorePopAnimation());
        }
    }
    
    public void UpdateTimer(float time)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
    
    public void UpdateLevel(int level)
    {
        if (levelText != null)
        {
            levelText.text = $"🌊 关卡 {level}";
        }
    }
    
    public void UpdateProgress(float progress)
    {
        if (progressBar != null)
        {
            progressBar.fillAmount = Mathf.Clamp01(progress);
        }
    }
    
    #endregion
    
    #region 暂停菜单
    
    public void ShowPauseMenu()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
            FadeIn(pausePanel);
            Time.timeScale = 0;
        }
    }
    
    public void HidePauseMenu()
    {
        if (pausePanel != null)
        {
            FadeOut(pausePanel, () => {
                pausePanel.SetActive(false);
            });
            Time.timeScale = 1;
        }
    }
    
    public void TogglePause()
    {
        if (Time.timeScale == 0)
        {
            HidePauseMenu();
        }
        else
        {
            ShowPauseMenu();
        }
    }
    
    #endregion
    
    #region 设置
    
    public void ShowSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            FadeIn(settingsPanel);
        }
    }
    
    public void HideSettings()
    {
        if (settingsPanel != null)
        {
            FadeOut(settingsPanel, () => {
                settingsPanel.SetActive(false);
            });
        }
    }
    
    #endregion
    
    #region 动画效果
    
    void FadeIn(GameObject panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null) cg = panel.AddComponent<CanvasGroup>();
        
        StartCoroutine(FadeCoroutine(cg, 0, 1, fadeDuration));
    }
    
    void FadeOut(GameObject panel, System.Action onComplete = null)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null) cg = panel.AddComponent<CanvasGroup>();
        
        StartCoroutine(FadeCoroutine(cg, 1, 0, fadeDuration, onComplete));
    }
    
    IEnumerator FadeCoroutine(CanvasGroup cg, float from, float to, float duration, System.Action onComplete = null)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            cg.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        cg.alpha = to;
        onComplete?.Invoke();
    }
    
    IEnumerator ScorePopAnimation()
    {
        if (scoreText == null) yield break;
        
        Vector3 originalScale = scoreText.transform.localScale;
        float elapsed = 0;
        float duration = 0.2f;
        
        while (elapsed < duration / 2)
        {
            float t = elapsed / (duration / 2);
            scoreText.transform.localScale = Vector3.Lerp(originalScale, originalScale * 1.3f, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        elapsed = 0;
        while (elapsed < duration / 2)
        {
            float t = elapsed / (duration / 2);
            scoreText.transform.localScale = Vector3.Lerp(originalScale * 1.3f, originalScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        scoreText.transform.localScale = originalScale;
    }
    
    #endregion
    
    #region 工具方法
    
    /// <summary>
    /// 创建按钮点击音效
    /// </summary>
    public void PlayButtonClickSound()
    {
        // TODO: 添加按钮点击音效
        Debug.Log("[UI] 按钮点击");
    }
    
    /// <summary>
    /// 显示浮动提示
    /// </summary>
    public void ShowFloatingText(string text, Vector3 position, Color color)
    {
        // TODO: 实现浮动文字效果
        Debug.Log($"[UI] 浮动文字：{text}");
    }
    
    #endregion
}
