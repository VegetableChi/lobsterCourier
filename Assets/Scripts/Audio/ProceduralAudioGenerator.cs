using UnityEngine;
using System.Collections;

/// <summary>
/// 程序化音效生成器 - 使用代码生成简单音效
/// 无需外部音频文件即可产生游戏音效
/// </summary>
public class ProceduralAudioGenerator : MonoBehaviour
{
    public static ProceduralAudioGenerator Instance { get; private set; }
    
    [Header("音频设置")]
    public int sampleRate = 44100;
    public AudioConfiguration audioConfig;
    
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
    
    #region 音效生成
    
    /// <summary>
    /// 生成拾取音效
    /// </summary>
    public AudioClip GeneratePickupSound()
    {
        return GenerateToneSound(0.1f, 880f, 1760f, AudioWaveType.Sine);
    }
    
    /// <summary>
    /// 生成交付成功音效
    /// </summary>
    public AudioClip GenerateDeliverySound()
    {
        return GenerateMultiToneSound(
            new float[] { 523.25f, 659.25f, 783.99f, 1046.50f }, // C5 E5 G5 C6
            new float[] { 0.1f, 0.1f, 0.1f, 0.2f },
            AudioWaveType.Sine
        );
    }
    
    /// <summary>
    /// 生成金币音效
    /// </summary>
    public AudioClip GenerateCoinSound()
    {
        return GenerateToneSound(0.15f, 1200f, 1800f, AudioWaveType.Sine);
    }
    
    /// <summary>
    /// 生成钻石音效
    /// </summary>
    public AudioClip GenerateDiamondSound()
    {
        return GenerateMultiToneSound(
            new float[] { 1046.50f, 1318.51f, 1567.98f, 2093.00f },
            new float[] { 0.08f, 0.08f, 0.08f, 0.15f },
            AudioWaveType.Sine
        );
    }
    
    /// <summary>
    /// 生成冲刺音效
    /// </summary>
    public AudioClip GenerateSprintSound()
    {
        return GenerateSweepSound(0.2f, 200f, 800f, AudioWaveType.Sine);
    }
    
    /// <summary>
    /// 生成体力耗尽音效
    /// </summary>
    public AudioClip GenerateExhaustedSound()
    {
        return GenerateSweepSound(0.3f, 400f, 150f, AudioWaveType.Sawtooth);
    }
    
    /// <summary>
    /// 生成碰撞音效
    /// </summary>
    public AudioClip GenerateCollisionSound()
    {
        return GenerateNoiseSound(0.1f, AudioNoiseType.White);
    }
    
    /// <summary>
    /// 生成气泡音效
    /// </summary>
    public AudioClip GenerateBubbleSound()
    {
        return GenerateToneSound(0.08f, 600f, 1200f, AudioWaveType.Sine);
    }
    
    /// <summary>
    /// 生成菜单点击音效
    /// </summary>
    public AudioClip GenerateMenuClickSound()
    {
        return GenerateToneSound(0.05f, 800f, 800f, AudioWaveType.Square);
    }
    
    /// <summary>
    /// 生成成就解锁音效
    /// </summary>
    public AudioClip GenerateAchievementSound()
    {
        return GenerateFanfareSound();
    }
    
    #endregion
    
    #region 音频生成方法
    
    /// <summary>
    /// 生成简单音调
    /// </summary>
    AudioClip GenerateToneSound(float duration, float startFreq, float endFreq, AudioWaveType waveType)
    {
        int samples = Mathf.FloorToInt(sampleRate * duration);
        float[] data = new float[samples];
        
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float freq = Mathf.Lerp(startFreq, endFreq, t);
            data[i] = GenerateWave(waveType, freq * t * 2 * Mathf.PI);
        }
        
        ApplyEnvelope(data, 0.01f, 0.01f);
        return CreateAudioClip(data);
    }
    
    /// <summary>
    /// 生成多音调
    /// </summary>
    AudioClip GenerateMultiToneSound(float[] frequencies, float[] durations, AudioWaveType waveType)
    {
        float totalDuration = 0;
        foreach (float d in durations) totalDuration += d;
        
        int samples = Mathf.FloorToInt(sampleRate * totalDuration);
        float[] data = new float[samples];
        
        int currentSample = 0;
        for (int i = 0; i < frequencies.Length; i++)
        {
            int noteSamples = Mathf.FloorToInt(sampleRate * durations[i]);
            for (int j = 0; j < noteSamples && currentSample + j < samples; j++)
            {
                float t = (float)(currentSample + j) / sampleRate;
                data[currentSample + j] = GenerateWave(waveType, frequencies[i] * t * 2 * Mathf.PI) * 0.5f;
            }
            currentSample += noteSamples;
        }
        
        ApplyEnvelope(data, 0.01f, 0.02f);
        return CreateAudioClip(data);
    }
    
    /// <summary>
    /// 生成扫频音效
    /// </summary>
    AudioClip GenerateSweepSound(float duration, float startFreq, float endFreq, AudioWaveType waveType)
    {
        int samples = Mathf.FloorToInt(sampleRate * duration);
        float[] data = new float[samples];
        
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float progress = t / duration;
            float freq = Mathf.Lerp(startFreq, endFreq, progress);
            data[i] = GenerateWave(waveType, freq * t * 2 * Mathf.PI);
        }
        
        ApplyEnvelope(data, 0.01f, 0.02f);
        return CreateAudioClip(data);
    }
    
    /// <summary>
    /// 生成噪音音效
    /// </summary>
    AudioClip GenerateNoiseSound(float duration, AudioNoiseType noiseType)
    {
        int samples = Mathf.FloorToInt(sampleRate * duration);
        float[] data = new float[samples];
        
        for (int i = 0; i < samples; i++)
        {
            data[i] = GenerateNoise(noiseType);
        }
        
        ApplyEnvelope(data, 0.005f, 0.02f);
        return CreateAudioClip(data);
    }
    
    /// <summary>
    /// 生成欢呼音效
    /// </summary>
    AudioClip GenerateFanfareSound()
    {
        return GenerateMultiToneSound(
            new float[] { 523.25f, 659.25f, 783.99f, 1046.50f, 783.99f, 1046.50f },
            new float[] { 0.15f, 0.15f, 0.15f, 0.2f, 0.1f, 0.3f },
            AudioWaveType.Sine
        );
    }
    
    #endregion
    
    #region 波形生成
    
    float GenerateWave(AudioWaveType waveType, float phase)
    {
        switch (waveType)
        {
            case AudioWaveType.Sine:
                return Mathf.Sin(phase);
            case AudioWaveType.Square:
                return Mathf.Sin(phase) > 0 ? 1 : -1;
            case AudioWaveType.Sawtooth:
                return 2 * (phase / (2 * Mathf.PI) - Mathf.Floor(phase / (2 * Mathf.PI) + 0.5f));
            case AudioWaveType.Triangle:
                return 2 * Mathf.Abs(2 * (phase / (2 * Mathf.PI) - Mathf.Floor(phase / (2 * Mathf.PI) + 0.5f))) - 1;
            default:
                return Mathf.Sin(phase);
        }
    }
    
    float GenerateNoise(AudioNoiseType noiseType)
    {
        switch (noiseType)
        {
            case AudioNoiseType.White:
                return Random.Range(-1f, 1f);
            case AudioNoiseType.Pink:
                // 简化版粉红噪音
                float white = Random.Range(-1f, 1f);
                return (white + Random.Range(-1f, 1f)) / 2;
            default:
                return Random.Range(-1f, 1f);
        }
    }
    
    void ApplyEnvelope(float[] data, float attackTime, float releaseTime)
    {
        int attackSamples = Mathf.FloorToInt(sampleRate * attackTime);
        int releaseSamples = Mathf.FloorToInt(sampleRate * releaseTime);
        
        // Attack
        for (int i = 0; i < attackSamples && i < data.Length; i++)
        {
            data[i] *= (float)i / attackSamples;
        }
        
        // Release
        for (int i = 0; i < releaseSamples && i < data.Length; i++)
        {
            int index = data.Length - 1 - i;
            data[index] *= (float)i / releaseSamples;
        }
    }
    
    AudioClip CreateAudioClip(float[] data)
    {
        AudioClip clip = AudioClip.Create("ProceduralSFX", data.Length, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }
    
    #endregion
    
    #region 缓存音效
    
    private AudioClip[] cachedClips;
    
    void Start()
    {
        CacheAllSounds();
    }
    
    void CacheAllSounds()
    {
        cachedClips = new AudioClip[10];
        cachedClips[0] = GeneratePickupSound();
        cachedClips[1] = GenerateDeliverySound();
        cachedClips[2] = GenerateCoinSound();
        cachedClips[3] = GenerateDiamondSound();
        cachedClips[4] = GenerateSprintSound();
        cachedClips[5] = GenerateExhaustedSound();
        cachedClips[6] = GenerateCollisionSound();
        cachedClips[7] = GenerateBubbleSound();
        cachedClips[8] = GenerateMenuClickSound();
        cachedClips[9] = GenerateAchievementSound();
        
        Debug.Log("[ProceduralAudio] 已缓存所有程序化音效");
    }
    
    public AudioClip GetCachedSound(SoundType type)
    {
        if (cachedClips == null) CacheAllSounds();
        
        int index = (int)type;
        if (index >= 0 && index < cachedClips.Length)
        {
            return cachedClips[index];
        }
        return null;
    }
    
    #endregion
    
    #region 便捷播放
    
    public void PlaySound(SoundType type, AudioSource source = null)
    {
        AudioClip clip = GetCachedSound(type);
        if (clip == null) return;
        
        if (source != null)
        {
            source.PlayOneShot(clip);
        }
        else
        {
            // 创建临时音频源播放
            GameObject tempObj = new GameObject("TempAudio");
            tempObj.transform.position = Camera.main.transform.position;
            AudioSource tempSource = tempObj.AddComponent<AudioSource>();
            tempSource.PlayOneShot(clip);
            Destroy(tempObj, clip.length);
        }
    }
    
    #endregion
}

/// <summary>
/// 音效类型枚举
/// </summary>
public enum SoundType
{
    Pickup,
    Delivery,
    Coin,
    Diamond,
    Sprint,
    Exhausted,
    Collision,
    Bubble,
    MenuClick,
    Achievement
}

/// <summary>
/// 波形类型
/// </summary>
public enum AudioWaveType
{
    Sine,
    Square,
    Sawtooth,
    Triangle
}

/// <summary>
/// 噪音类型
/// </summary>
public enum AudioNoiseType
{
    White,
    Pink
}
