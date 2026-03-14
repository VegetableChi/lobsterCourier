using UnityEngine;

/// <summary>
/// 游戏设置 - 集中管理所有游戏设置
/// 支持保存/加载、画质预设、音量控制
/// </summary>
public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }
    
    [Header("画质设置")]
    public QualityLevel currentQuality = QualityLevel.Medium;
    
    [Header("音量设置")]
    [Range(0f, 1f)] public float masterVolume = 0.8f;
    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 0.7f;
    
    [Header("游戏设置")]
    public bool showTutorial = true;
    public bool autoSave = true;
    public float autoSaveInterval = 60f;
    
    [Header("控制设置")]
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode sprint = KeyCode.LeftShift;
    public KeyCode grab = KeyCode.E;
    public KeyCode pause = KeyCode.P;
    public KeyCode shop = KeyCode.B;
    public KeyCode achievement = KeyCode.A;
    public KeyCode minimap = KeyCode.M;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        LoadSettings();
        ApplySettings();
    }
    
    #region 画质设置
    
    public void SetQuality(QualityLevel quality)
    {
        currentQuality = quality;
        ApplyQuality();
        SaveSettings();
    }
    
    public void ApplyQuality()
    {
        switch (currentQuality)
        {
            case QualityLevel.Low:
                QualitySettings.SetQualityLevel(0);
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
                
            case QualityLevel.Medium:
                QualitySettings.SetQualityLevel(3);
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
                
            case QualityLevel.High:
                QualitySettings.SetQualityLevel(5);
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
                
            case QualityLevel.Ultra:
                QualitySettings.SetQualityLevel(6);
                Screen.SetResolution(2560, 1440, Screen.fullScreen);
                break;
        }
        
        Debug.Log($"画质已设置为：{currentQuality}");
    }
    
    #endregion
    
    #region 音量设置
    
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        ApplyVolume();
        SaveSettings();
    }
    
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        ApplyVolume();
        SaveSettings();
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        ApplyVolume();
        SaveSettings();
    }
    
    public void ApplyVolume()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetAllVolumes(
                bgmVolume * masterVolume,
                sfxVolume * masterVolume,
                0.3f * masterVolume
            );
        }
    }
    
    #endregion
    
    #region 设置保存/加载
    
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Quality", (int)currentQuality);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetInt("ShowTutorial", showTutorial ? 1 : 0);
        PlayerPrefs.SetInt("AutoSave", autoSave ? 1 : 0);
        PlayerPrefs.SetFloat("AutoSaveInterval", autoSaveInterval);
        PlayerPrefs.Save();
        
        Debug.Log("设置已保存");
    }
    
    public void LoadSettings()
    {
        currentQuality = (QualityLevel)PlayerPrefs.GetInt("Quality", (int)QualityLevel.Medium);
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
        showTutorial = PlayerPrefs.GetInt("ShowTutorial", 1) == 1;
        autoSave = PlayerPrefs.GetInt("AutoSave", 1) == 1;
        autoSaveInterval = PlayerPrefs.GetFloat("AutoSaveInterval", 60f);
    }
    
    public void ResetSettings()
    {
        PlayerPrefs.DeleteAll();
        currentQuality = QualityLevel.Medium;
        masterVolume = 0.8f;
        bgmVolume = 0.5f;
        sfxVolume = 0.7f;
        showTutorial = true;
        autoSave = true;
        autoSaveInterval = 60f;
        
        ApplySettings();
        Debug.Log("设置已重置");
    }
    
    #endregion
    
    #region 应用设置
    
    public void ApplySettings()
    {
        ApplyQuality();
        ApplyVolume();
    }
    
    #endregion
    
    #region 快捷键
    
    void Update()
    {
        // F2 快速保存设置
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SaveSettings();
        }
        
        // F3 快速加载设置
        if (Input.GetKeyDown(KeyCode.F3))
        {
            LoadSettings();
            ApplySettings();
        }
        
        // F4 重置设置
        if (Input.GetKeyDown(KeyCode.F4))
        {
            ResetSettings();
        }
    }
    
    #endregion
}

public enum QualityLevel
{
    Low,
    Medium,
    High,
    Ultra
}
