using UnityEngine;
using System.Collections;

/// <summary>
/// 音频管理器 - 统一管理游戏音效和音乐
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("音频剪辑")]
    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;
    
    [Header("音频源")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource ambientSource;
    
    [Header("音量设置")]
    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 0.7f;
    [Range(0f, 1f)] public float ambientVolume = 0.3f;
    
    [Header("设置")]
    public bool playOnStart = true;
    public bool loopBgm = true;
    
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
        
        InitializeAudioSources();
    }
    
    void Start()
    {
        if (playOnStart && bgmClips.Length > 0)
        {
            PlayBGM(0);
        }
    }
    
    void InitializeAudioSources()
    {
        // 创建或配置音频源
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
        }
        bgmSource.loop = loopBgm;
        bgmSource.volume = bgmVolume;
        bgmSource.playOnAwake = false;
        
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        sfxSource.volume = sfxVolume;
        sfxSource.playOnAwake = false;
        sfxSource.spatialBlend = 0f; // 2D 音效
        
        if (ambientSource == null)
        {
            ambientSource = gameObject.AddComponent<AudioSource>();
        }
        ambientSource.loop = true;
        ambientSource.volume = ambientVolume;
        ambientSource.playOnAwake = false;
    }
    
    #region 背景音乐
    
    public void PlayBGM(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= bgmClips.Length) return;
        
        bgmSource.clip = bgmClips[clipIndex];
        bgmSource.Play();
    }
    
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        
        bgmSource.clip = clip;
        bgmSource.Play();
    }
    
    public void StopBGM()
    {
        bgmSource.Stop();
    }
    
    public void PauseBGM()
    {
        bgmSource.Pause();
    }
    
    public void ResumeBGM()
    {
        bgmSource.UnPause();
    }
    
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume;
    }
    
    public void FadeInBGM(float duration)
    {
        StartCoroutine(FadeInCoroutine(bgmSource, duration));
    }
    
    public void FadeOutBGM(float duration)
    {
        StartCoroutine(FadeOutCoroutine(bgmSource, duration));
    }
    
    #endregion
    
    #region 音效
    
    public void PlaySFX(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= sfxClips.Length) return;
        
        sfxSource.PlayOneShot(sfxClips[clipIndex]);
    }
    
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        
        sfxSource.PlayOneShot(clip);
    }
    
    public void PlaySFX(AudioClip clip, float volumeScale)
    {
        if (clip == null) return;
        
        sfxSource.PlayOneShot(clip, volumeScale);
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }
    
    #endregion
    
    #region 环境音
    
    public void PlayAmbient(AudioClip clip)
    {
        if (clip == null) return;
        
        ambientSource.clip = clip;
        ambientSource.Play();
    }
    
    public void StopAmbient()
    {
        ambientSource.Stop();
    }
    
    public void SetAmbientVolume(float volume)
    {
        ambientVolume = Mathf.Clamp01(volume);
        ambientSource.volume = ambientVolume;
    }
    
    #endregion
    
    #region 预设音效
    
    public void PlayPickupSound()
    {
        // 可以添加具体的拾取音效
        Debug.Log("🔊 播放拾取音效");
    }
    
    public void PlayDeliverySound()
    {
        // 可以添加具体的交付音效
        Debug.Log("🔊 播放交付音效");
    }
    
    public void PlayPurchaseSound()
    {
        // 可以添加具体的购买音效
        Debug.Log("🔊 播放购买音效");
    }
    
    public void PlayAchievementSound()
    {
        // 可以添加具体的成就解锁音效
        Debug.Log("🔊 播放成就音效");
    }
    
    public void PlayExhaustedSound()
    {
        // 可以添加具体的体力耗尽音效
        Debug.Log("🔊 播放体力耗尽音效");
    }
    
    #endregion
    
    #region 协程
    
    IEnumerator FadeInCoroutine(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float elapsed = 0f;
        
        source.volume = 0f;
        source.Play();
        
        while (elapsed < duration)
        {
            source.volume = Mathf.Lerp(0f, startVolume, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        
        source.volume = startVolume;
    }
    
    IEnumerator FadeOutCoroutine(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            source.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        
        source.Stop();
        source.volume = startVolume;
    }
    
    #endregion
    
    #region 工具方法
    
    public void SetAllVolumes(float bgm, float sfx, float ambient)
    {
        SetBGMVolume(bgm);
        SetSFXVolume(sfx);
        SetAmbientVolume(ambient);
    }
    
    public void MuteAll()
    {
        bgmSource.mute = true;
        sfxSource.mute = true;
        ambientSource.mute = true;
    }
    
    public void UnmuteAll()
    {
        bgmSource.mute = false;
        sfxSource.mute = false;
        ambientSource.mute = false;
    }
    
    #endregion
}
