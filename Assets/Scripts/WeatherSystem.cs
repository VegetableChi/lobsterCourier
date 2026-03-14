using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// 天气系统 - 动态天气影响游戏
/// </summary>
public class WeatherSystem : MonoBehaviour
{
    public static WeatherSystem Instance { get; private set; }
    
    [Header("当前天气")]
    public WeatherType currentWeather;
    public float weatherDuration = 60f;
    private float weatherTimer = 0f;
    
    [Header("天气参数")]
    public float visibility = 1f; // 能见度
    public float currentModifier = 1f; // 洋流影响
    public float speedModifier = 1f; // 速度影响
    
    [Header("UI")]
    public TextMeshProUGUI weatherText;
    public GameObject weatherEffectPrefab;
    private GameObject activeWeatherEffect;
    
    [Header("天气切换")]
    public bool enableWeatherChanges = true;
    public float minWeatherDuration = 30f;
    public float maxWeatherDuration = 120f;
    
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
    
    void Start()
    {
        StartRandomWeather();
    }
    
    void Update()
    {
        if (!enableWeatherChanges) return;
        
        weatherTimer -= Time.deltaTime;
        
        if (weatherTimer <= 0)
        {
            ChangeWeather();
        }
        
        UpdateWeatherUI();
    }
    
    void StartRandomWeather()
    {
        currentWeather = (WeatherType)Random.Range(0, System.Enum.GetNames(typeof(WeatherType)).Length);
        ApplyWeather(currentWeather);
        weatherTimer = Random.Range(minWeatherDuration, maxWeatherDuration);
    }
    
    public void ChangeWeather()
    {
        WeatherType newWeather = (WeatherType)Random.Range(0, System.Enum.GetNames(typeof(WeatherType)).Length);
        
        // 避免连续相同天气
        while (newWeather == currentWeather)
        {
            newWeather = (WeatherType)Random.Range(0, System.Enum.GetNames(typeof(WeatherType)).Length);
        }
        
        currentWeather = newWeather;
        ApplyWeather(currentWeather);
        weatherTimer = Random.Range(minWeatherDuration, maxWeatherDuration);
        
        Debug.Log($"🌤️ 天气变化：{GetWeatherName(currentWeather)}");
    }
    
    void ApplyWeather(WeatherType weather)
    {
        // 清除旧效果
        if (activeWeatherEffect != null)
        {
            Destroy(activeWeatherEffect);
        }
        
        switch (weather)
        {
            case WeatherType.Sunny:
                // 晴天 - 无影响
                visibility = 1f;
                currentModifier = 1f;
                speedModifier = 1f;
                break;
                
            case WeatherType.Cloudy:
                // 多云 - 轻微影响
                visibility = 0.9f;
                currentModifier = 1.1f;
                speedModifier = 0.95f;
                break;
                
            case WeatherType.Rainy:
                // 雨天 - 中等影响
                visibility = 0.7f;
                currentModifier = 1.2f;
                speedModifier = 0.9f;
                SpawnWeatherEffect("Rain");
                break;
                
            case WeatherType.Stormy:
                // 暴风雨 - 严重影响
                visibility = 0.5f;
                currentModifier = 1.5f;
                speedModifier = 0.8f;
                SpawnWeatherEffect("Storm");
                break;
                
            case WeatherType.Foggy:
                // 雾天 - 能见度低
                visibility = 0.4f;
                currentModifier = 1f;
                speedModifier = 0.85f;
                SpawnWeatherEffect("Fog");
                break;
                
            case WeatherType.Current:
                // 强洋流 - 洋流增强
                visibility = 1f;
                currentModifier = 2f;
                speedModifier = 0.9f;
                SpawnWeatherEffect("Current");
                break;
        }
    }
    
    void SpawnWeatherEffect(string effectName)
    {
        // 这里可以实例化天气效果预制体
        Debug.Log($"生成天气效果：{effectName}");
    }
    
    void UpdateWeatherUI()
    {
        if (weatherText != null)
        {
            weatherText.text = $"{GetWeatherIcon(currentWeather)} {GetWeatherName(currentWeather)}";
        }
    }
    
    string GetWeatherName(WeatherType weather)
    {
        switch (weather)
        {
            case WeatherType.Sunny: return "晴朗";
            case WeatherType.Cloudy: return "多云";
            case WeatherType.Rainy: return "雨天";
            case WeatherType.Stormy: return "暴风雨";
            case WeatherType.Foggy: return "雾天";
            case WeatherType.Current: return "强洋流";
            default: return "未知";
        }
    }
    
    string GetWeatherIcon(WeatherType weather)
    {
        switch (weather)
        {
            case WeatherType.Sunny: return "☀️";
            case WeatherType.Cloudy: return "☁️";
            case WeatherType.Rainy: return "🌧️";
            case WeatherType.Stormy: return "⛈️";
            case WeatherType.Foggy: return "🌫️";
            case WeatherType.Current: return "🌊";
            default: return "❓";
        }
    }
    
    public float GetVisibility()
    {
        return visibility;
    }
    
    public float GetCurrentModifier()
    {
        return currentModifier;
    }
    
    public float GetSpeedModifier()
    {
        return speedModifier;
    }
    
    public bool IsStormy()
    {
        return currentWeather == WeatherType.Stormy;
    }
    
    public bool IsFoggy()
    {
        return currentWeather == WeatherType.Foggy;
    }
}

public enum WeatherType
{
    Sunny,      // 晴朗
    Cloudy,     // 多云
    Rainy,      // 雨天
    Stormy,     // 暴风雨
    Foggy,      // 雾天
    Current     // 强洋流
}
