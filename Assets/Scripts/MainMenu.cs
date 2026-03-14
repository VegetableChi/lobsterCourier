using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// 主菜单管理
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("面板")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    
    [Header("UI 元素")]
    public TextMeshProUGUI versionText;
    public TextMeshProUGUI highScoreText;
    
    [Header("音频")]
    public AudioClip menuBGM;
    
    void Start()
    {
        // 显示版本号
        if (versionText != null)
        {
            versionText.text = $"v{Application.version}";
        }
        
        // 加载最高分
        UpdateHighScore();
        
        // 播放菜单音乐
        if (AudioManager.Instance != null && menuBGM != null)
        {
            AudioManager.Instance.PlayBGM(menuBGM);
        }
        
        ShowMainMenu();
    }
    
    void Update()
    {
        // 按 ESC 返回主菜单
        if (Input.GetKeyDown(KeyCode.Escape) && mainMenuPanel.activeSelf)
        {
            if (settingsPanel.activeSelf || creditsPanel.activeSelf)
            {
                ShowMainMenu();
            }
        }
    }
    
    #region 菜单导航
    
    public void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }
    
    public void ShowSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }
    
    public void ShowCredits()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }
    
    #endregion
    
    #region 游戏操作
    
    public void NewGame()
    {
        // 重置存档（可选）
        // SaveSystem.Instance?.ResetSave();
        
        // 加载游戏场景
        SceneManager.LoadScene("GameScene");
    }
    
    public void ContinueGame()
    {
        if (SaveSystem.Instance != null && SaveSystem.Instance.HasSave())
        {
            SaveSystem.Instance.LoadGame();
            SceneManager.LoadScene("GameScene");
        }
    }
    
    public void LoadGame()
    {
        if (SaveSystem.Instance != null)
        {
            if (SaveSystem.Instance.LoadGame())
            {
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                Debug.LogWarning("没有可用的存档");
            }
        }
    }
    
    public void QuitGame()
    {
        Debug.Log("退出游戏");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    #endregion
    
    #region 设置
    
    public void SetMasterVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetAllVolumes(volume, volume, volume);
        }
    }
    
    public void SetBGMVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(volume);
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(volume);
        }
    }
    
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    
    public void SetResolution(int width, int height)
    {
        Screen.SetResolution(width, height, Screen.fullScreen);
    }
    
    public void ResetSaveData()
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.ResetSave();
            UpdateHighScore();
        }
    }
    
    #endregion
    
    void UpdateHighScore()
    {
        if (highScoreText != null && SaveSystem.Instance != null)
        {
            var saveData = SaveSystem.Instance.GetSaveData();
            highScoreText.text = $"最高声誉：{saveData.bestStats.highestReputation}";
        }
    }
}
