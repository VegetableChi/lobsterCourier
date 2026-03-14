using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏结束系统 - 处理胜利/失败条件
/// </summary>
public class GameEndSystem : MonoBehaviour
{
    public static GameEndSystem Instance { get; private set; }
    
    [Header("结束面板")]
    public GameObject gameEndPanel;
    public TextMeshProUGUI endTitle;
    public TextMeshProUGUI endReason;
    public TextMeshProUGUI endStats;
    public GameObject restartButton;
    public GameObject mainMenuButton;
    
    [Header("游戏结束条件")]
    public bool enableReputationFail = true;
    public int minReputation = -50; // 声誉低于此值失败
    
    [Header("胜利条件")]
    public bool enableGoalVictory = true;
    public int goalReputation = 1000; // 声誉达到此值胜利
    public int goalMoney = 100000; // 或金钱达到此值
    
    private bool isGameEnded = false;
    
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
        if (isGameEnded) return;
        
        if (GameManager.Instance != null)
        {
            CheckEndConditions();
        }
    }
    
    void CheckEndConditions()
    {
        // 检查失败条件
        if (enableReputationFail && GameManager.Instance.playerReputation <= minReputation)
        {
            EndGame(false, "声誉过低", "你的服务质量太差，客户都不再信任你了...");
            return;
        }
        
        // 检查胜利条件
        if (enableGoalVictory)
        {
            if (GameManager.Instance.playerReputation >= goalReputation)
            {
                EndGame(true, "传奇快递员", "你成为了蓝海大陆最著名的快递员！");
                return;
            }
            
            if (GameManager.Instance.playerMoney >= goalMoney)
            {
                EndGame(true, "商业大亨", "你积累了巨额财富，可以退休了！");
                return;
            }
        }
    }
    
    public void EndGame(bool isVictory, string title, string reason)
    {
        if (isGameEnded) return;
        
        isGameEnded = true;
        
        // 暂停游戏
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentState = GameState.GameOver;
        }
        Time.timeScale = 0f;
        
        // 显示结束面板
        if (gameEndPanel != null)
        {
            gameEndPanel.SetActive(true);
        }
        
        // 设置标题
        if (endTitle != null)
        {
            endTitle.text = isVictory ? "🏆 胜利！" : "💔 游戏结束";
            endTitle.color = isVictory ? Color.yellow : Color.red;
        }
        
        // 设置原因
        if (endReason != null)
        {
            endReason.text = $"{title}\n\n{reason}";
        }
        
        // 显示统计
        if (endStats != null)
        {
            ShowStats();
        }
        
        // 保存游戏
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.SaveGame();
        }
        
        Debug.Log(isVictory ? "✅ 游戏胜利！" : "❌ 游戏结束");
    }
    
    void ShowStats()
    {
        if (GameManager.Instance == null) return;
        
        int deliveries = GameManager.Instance.totalDeliveries;
        int money = GameManager.Instance.playerMoney;
        int reputation = GameManager.Instance.playerReputation;
        float playTime = Time.timeSinceLevelLoad;
        
        int combo = ComboSystem.Instance != null ? ComboSystem.Instance.GetMaxCombo() : 0;
        
        string stats = $@"
📦 送货次数：{deliveries}
💰 最终金钱：${money}
⭐ 最终声誉：{reputation}
🔥 最高连击：{combo}
⏱️ 游戏时间：{FormatTime(playTime)}
📈 最高难度：{DifficultyManager.Instance?.currentDifficulty ?? 1}
";
        
        if (endStats != null)
        {
            endStats.text = stats;
        }
    }
    
    string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{mins:D2}:{secs:D2}";
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
