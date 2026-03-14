using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// 暂停菜单管理
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [Header("面板")]
    public GameObject pausePanel;
    
    [Header("按钮")]
    public GameObject resumeButton;
    public GameObject settingsButton;
    public GameObject mainMenuButton;
    public GameObject restartButton;
    
    [Header("文本")]
    public TextMeshProUGUI pauseTitleText;
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI currentMoneyText;
    public TextMeshProUGUI currentReputationText;
    
    private bool isPaused = false;
    
    void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }
    
    void Update()
    {
        // 按 P 或 ESC 暂停
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance != null && GameManager.Instance.currentState == GameState.Playing)
            {
                PauseGame();
            }
            else if (GameManager.Instance != null && GameManager.Instance.currentState == GameState.Paused)
            {
                ResumeGame();
            }
        }
        
        // 更新统计信息
        if (isPaused)
        {
            UpdateStats();
        }
    }
    
    public void PauseGame()
    {
        if (GameManager.Instance == null) return;
        
        GameManager.Instance.currentState = GameState.Paused;
        Time.timeScale = 0f;
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        
        isPaused = true;
        
        Debug.Log("⏸️ 游戏暂停");
    }
    
    public void ResumeGame()
    {
        if (GameManager.Instance == null) return;
        
        GameManager.Instance.currentState = GameState.Playing;
        Time.timeScale = GameManager.Instance.gameSpeed;
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        
        isPaused = false;
        
        Debug.Log("▶️ 游戏继续");
    }
    
    public void RestartGame()
    {
        // 重置时间缩放
        Time.timeScale = 1f;
        
        // 重新加载场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void GoToMainMenu()
    {
        // 重置时间缩放
        Time.timeScale = 1f;
        
        // 保存游戏（可选）
        SaveSystem.Instance?.SaveGame();
        
        // 加载主菜单场景
        SceneManager.LoadScene("MainMenu");
    }
    
    public void SaveAndQuit()
    {
        // 保存游戏
        SaveSystem.Instance?.SaveGame();
        
        // 退出
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    void UpdateStats()
    {
        if (GameManager.Instance != null)
        {
            if (currentTimeText != null)
            {
                float playTime = Time.timeSinceLevelLoad;
                int mins = Mathf.FloorToInt(playTime / 60f);
                int secs = Mathf.FloorToInt(playTime % 60f);
                currentTimeText.text = $"游戏时间：{mins:D2}:{secs:D2}";
            }
            
            if (currentMoneyText != null)
            {
                currentMoneyText.text = $"💰 ${GameManager.Instance.playerMoney}";
            }
            
            if (currentReputationText != null)
            {
                currentReputationText.text = $"⭐ {GameManager.Instance.playerReputation}";
            }
        }
    }
    
    void OnDisable()
    {
        // 确保时间缩放恢复正常
        if (!isPaused)
        {
            Time.timeScale = 1f;
        }
    }
    
    void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
