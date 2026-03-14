using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// 程序化音效生成器 - 生成占位音效资源
/// 使用方法：菜单栏 → Lobster Courier → 生成音效资源
/// </summary>
public class AudioGenerator : EditorWindow
{
    private Vector2 scrollPosition;
    
    [MenuItem("Lobster Courier/生成音效资源")]
    public static void ShowWindow()
    {
        var window = GetWindow<AudioGenerator>("音效生成");
        window.minSize = new Vector2(400, 600);
        window.Show();
    }
    
    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        GUILayout.Label("🦞 龙虾快递员 - 程序化音效生成", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "本工具将生成程序化占位音效，\n" +
            "使用音频合成技术生成 WAV 文件。\n" +
            "可以在 Unity 中立即使用，后续可替换为精美音效。",
            MessageType.Info);
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("🎵 生成所有音效", GUILayout.Height(40)))
        {
            GenerateAllAudio();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("🎼 生成背景音乐 (BGM)", GUILayout.Height(35)))
        {
            GenerateBGM();
        }
        
        if (GUILayout.Button("🔊 生成游戏音效 (SFX)", GUILayout.Height(35)))
        {
            GenerateSFX();
        }
        
        if (GUILayout.Button("🎹 生成 UI 音效", GUILayout.Height(35)))
        {
            GenerateUISFX();
        }
        
        EditorGUILayout.EndScrollView();
    }
    
    void GenerateAllAudio()
    {
        try
        {
            EditorUtility.DisplayProgressBar("生成音效", "创建文件夹...", 0f);
            CreateFolders();
            
            EditorUtility.DisplayProgressBar("生成音效", "生成背景音乐...", 0.2f);
            GenerateBGM();
            
            EditorUtility.DisplayProgressBar("生成音效", "生成游戏音效...", 0.5f);
            GenerateSFX();
            
            EditorUtility.DisplayProgressBar("生成音效", "生成 UI 音效...", 0.8f);
            GenerateUISFX();
            
            EditorUtility.ClearProgressBar();
            
            AssetDatabase.Refresh();
            
            Debug.Log("✅ 所有音效生成完成！");
            EditorUtility.DisplayDialog("生成完成", 
                "所有程序化音效已生成！\n\n" +
                "包含:\n" +
                "• BGM × 2 (主菜单 + 游戏内)\n" +
                "• 游戏 SFX × 6\n" +
                "• UI 音效 × 4\n\n" +
                "现在可以在 Unity 中使用音效了！", "好的");
        }
        catch (System.Exception e)
        {
            EditorUtility.ClearProgressBar();
            Debug.LogError($"❌ 生成失败：{e.Message}");
            EditorUtility.DisplayDialog("错误", $"生成失败:\n{e.Message}", "确定");
        }
    }
    
    void CreateFolders()
    {
        CreateFolder("Assets/Audio");
        CreateFolder("Assets/Audio/BGM");
        CreateFolder("Assets/Audio/SFX");
    }
    
    void CreateFolder(string path)
    {
        if (!AssetDatabase.IsValidFolder(path.Replace("Assets/", "")))
        {
            string[] parts = path.Split('/');
            string currentPath = "Assets";
            
            for (int i = 1; i < parts.Length; i++)
            {
                string newPath = Path.Combine(currentPath, parts[i]);
                if (!Directory.Exists(newPath))
                {
                    AssetDatabase.CreateFolder(currentPath, parts[i]);
                }
                currentPath = newPath;
            }
        }
    }
    
    #region 生成 BGM
    
    void GenerateBGM()
    {
        string folder = "Assets/Audio/BGM";
        
        // BGM 1: 主菜单 - 轻松愉快
        AudioClip menuBGM = SynthesizeBGM("MenuBGM", 120f, 0.5f, new float[] { 261.63f, 329.63f, 392.00f, 523.25f });
        SaveAudioClip(menuBGM, folder, "BGM_Menu");
        
        // BGM 2: 游戏内 - 轻快冒险
        AudioClip gameBGM = SynthesizeBGM("GameBGM", 140f, 0.6f, new float[] { 329.63f, 392.00f, 523.25f, 659.25f });
        SaveAudioClip(gameBGM, folder, "BGM_Game");
        
        Debug.Log("✅ BGM 生成完成 (2 个)");
    }
    
    AudioClip SynthesizeBGM(string name, float tempo, float volume, float[] notes)
    {
        int sampleRate = 44100;
        float duration = 120f; // 2 分钟循环
        int samples = (int)(sampleRate * duration);
        
        float[] data = new float[samples];
        
        // 简单的琶音合成
        int noteIndex = 0;
        float noteDuration = 60f / tempo; // 每拍秒数
        int noteSamples = (int)(sampleRate * noteDuration);
        
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            
            // 和弦进行
            float freq = notes[noteIndex % notes.Length];
            if (i % noteSamples == 0)
            {
                noteIndex++;
            }
            
            // 包络 (ADSR)
            float envelope = 1f;
            int sampleInNote = i % noteSamples;
            if (sampleInNote < sampleRate * 0.01f) // Attack
            {
                envelope = sampleInNote / (sampleRate * 0.01f);
            }
            else if (sampleInNote > noteSamples - sampleRate * 0.1f) // Release
            {
                envelope = 1f - (sampleInNote - (noteSamples - sampleRate * 0.1f)) / (sampleRate * 0.1f);
            }
            
            // 合成波形 (正弦波 + 泛音)
            float sample = 0f;
            sample += Mathf.Sin(2f * Mathf.PI * freq * t) * 0.6f;
            sample += Mathf.Sin(2f * Mathf.PI * freq * 2f * t) * 0.2f;
            sample += Mathf.Sin(2f * Mathf.PI * freq * 3f * t) * 0.1f;
            
            // 低音伴奏
            float bassFreq = notes[0] / 2f;
            if ((i / noteSamples) % 4 == 0)
            {
                sample += Mathf.Sin(2f * Mathf.PI * bassFreq * t) * 0.3f;
            }
            
            data[i] = sample * envelope * volume;
        }
        
        return CreateAudioClip(name, data, sampleRate);
    }
    
    #endregion
    
    #region 生成游戏 SFX
    
    void GenerateSFX()
    {
        string folder = "Assets/Audio/SFX";
        
        // 移动音效 - 轻柔的水声
        AudioClip moveSFX = SynthesizeSFX("Move", 0.2f, new float[] { 400f, 500f }, 0.1f, SFXType.Noise);
        SaveAudioClip(moveSFX, folder, "SFX_Move");
        
        // 冲刺音效 - 快速上升的音调
        AudioClip sprintSFX = SynthesizeSFX("Sprint", 0.5f, new float[] { 300f, 800f }, 0.3f, SFXType.Slide);
        SaveAudioClip(sprintSFX, folder, "SFX_Sprint");
        
        // 拾取音效 - 清脆的叮声
        AudioClip pickupSFX = SynthesizeSFX("Pickup", 0.3f, new float[] { 800f, 1200f }, 0.4f, SFXType.Chime);
        SaveAudioClip(pickupSFX, folder, "SFX_Pickup");
        
        // 交付音效 - 成功的和弦
        AudioClip deliverSFX = SynthesizeSFX("Deliver", 0.8f, new float[] { 523.25f, 659.25f, 783.99f }, 0.5f, SFXType.Chord);
        SaveAudioClip(deliverSFX, folder, "SFX_Deliver");
        
        // 金币音效 - 闪亮的声音
        AudioClip coinSFX = SynthesizeSFX("Coin", 0.4f, new float[] { 1000f, 1500f, 2000f }, 0.3f, SFXType.Sparkle);
        SaveAudioClip(coinSFX, folder, "SFX_Coin");
        
        // 成就音效 - 胜利的旋律
        AudioClip achievementSFX = SynthesizeSFX("Achievement", 1.5f, new float[] { 523.25f, 659.25f, 783.99f, 1046.50f }, 0.6f, SFXType.Fanfare);
        SaveAudioClip(achievementSFX, folder, "SFX_Achievement");
        
        Debug.Log("✅ 游戏 SFX 生成完成 (6 个)");
    }
    
    AudioClip SynthesizeSFX(string name, float duration, float[] frequencies, float volume, SFXType type)
    {
        int sampleRate = 44100;
        int samples = (int)(sampleRate * duration);
        
        float[] data = new float[samples];
        
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float normalizedTime = i / (float)samples;
            float sample = 0f;
            
            switch (type)
            {
                case SFXType.Noise:
                    // 白噪声 (水声)
                    sample = (Random.value - 0.5f) * 0.3f;
                    // 添加音调
                    foreach (float freq in frequencies)
                    {
                        sample += Mathf.Sin(2f * Mathf.PI * freq * t) * 0.1f;
                    }
                    break;
                    
                case SFXType.Slide:
                    // 滑音 (冲刺)
                    float slideFreq = Mathf.Lerp(frequencies[0], frequencies[1], normalizedTime);
                    sample = Mathf.Sin(2f * Mathf.PI * slideFreq * t);
                    break;
                    
                case SFXType.Chime:
                    // 叮声 (拾取)
                    foreach (float freq in frequencies)
                    {
                        sample += Mathf.Sin(2f * Mathf.PI * freq * t) * 0.3f;
                    }
                    // 包络
                    sample *= Mathf.Exp(-normalizedTime * 5f);
                    break;
                    
                case SFXType.Chord:
                    // 和弦 (交付)
                    foreach (float freq in frequencies)
                    {
                        sample += Mathf.Sin(2f * Mathf.PI * freq * t) * 0.2f;
                    }
                    sample *= Mathf.Exp(-normalizedTime * 3f);
                    break;
                    
                case SFXType.Sparkle:
                    // 闪亮声 (金币)
                    foreach (float freq in frequencies)
                    {
                        sample += Mathf.Sin(2f * Mathf.PI * freq * t) * 0.15f;
                    }
                    // 添加颤音
                    sample *= Mathf.Sin(2f * Mathf.PI * 10f * t) * 0.5f + 0.5f;
                    sample *= Mathf.Exp(-normalizedTime * 4f);
                    break;
                    
                case SFXType.Fanfare:
                    // 胜利旋律 (成就)
                    float melodyFreq = frequencies[(i / 10000) % frequencies.Length];
                    sample = Mathf.Sin(2f * Mathf.PI * melodyFreq * t) * 0.4f;
                    sample += Mathf.Sin(2f * Mathf.PI * (melodyFreq / 2f) * t) * 0.2f;
                    break;
            }
            
            data[i] = sample * volume;
        }
        
        return CreateAudioClip(name, data, sampleRate);
    }
    
    #endregion
    
    #region 生成 UI 音效
    
    void GenerateUISFX()
    {
        string folder = "Assets/Audio/SFX";
        
        // UI 点击音效
        AudioClip clickSFX = SynthesizeSFX("Click", 0.1f, new float[] { 800f }, 0.3f, SFXType.Noise);
        SaveAudioClip(clickSFX, folder, "SFX_UI_Click");
        
        // UI 悬停音效
        AudioClip hoverSFX = SynthesizeSFX("Hover", 0.05f, new float[] { 600f }, 0.2f, SFXType.Noise);
        SaveAudioClip(hoverSFX, folder, "SFX_UI_Hover");
        
        // 购买音效
        AudioClip purchaseSFX = SynthesizeSFX("Purchase", 0.5f, new float[] { 600f, 900f }, 0.4f, SFXType.Chime);
        SaveAudioClip(purchaseSFX, folder, "SFX_UI_Purchase");
        
        // 错误音效
        AudioClip errorSFX = SynthesizeSFX("Error", 0.4f, new float[] { 200f, 150f }, 0.4f, SFXType.Slide);
        SaveAudioClip(errorSFX, folder, "SFX_UI_Error");
        
        Debug.Log("✅ UI 音效生成完成 (4 个)");
    }
    
    #endregion
    
    #region 音频工具方法
    
    AudioClip CreateAudioClip(string name, float[] data, int sampleRate)
    {
        AudioClip clip = AudioClip.Create(name, data.Length, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }
    
    void SaveAudioClip(AudioClip clip, string folder, string filename)
    {
        string path = Path.Combine(folder, filename + ".wav");
        
        // 转换为 WAV 格式
        byte[] wavData = ConvertToWAV(clip);
        File.WriteAllBytes(path, wavData);
        
        Debug.Log($"保存：{path}");
    }
    
    byte[] ConvertToWAV(AudioClip clip)
    {
        int sampleRate = clip.frequency;
        int channels = clip.channels;
        int samples = clip.samples;
        
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                // RIFF header
                bw.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
                bw.Write(36 + samples * channels * 2); // File size - 8
                bw.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));
                
                // fmt subchunk
                bw.Write(System.Text.Encoding.ASCII.GetBytes("fmt "));
                bw.Write(16); // Subchunk1Size
                bw.Write((short)1); // AudioFormat (PCM)
                bw.Write((short)channels);
                bw.Write(sampleRate);
                bw.Write(sampleRate * channels * 2); // ByteRate
                bw.Write((short)(channels * 2)); // BlockAlign
                bw.Write((short)16); // BitsPerSample
                
                // data subchunk
                bw.Write(System.Text.Encoding.ASCII.GetBytes("data"));
                bw.Write(samples * channels * 2);
                
                // 写入音频数据
                float[] floatData = new float[samples * channels];
                clip.GetData(floatData, 0);
                
                foreach (float sample in floatData)
                {
                    short shortSample = (short)(sample * short.MaxValue);
                    bw.Write(shortSample);
                }
            }
            
            return ms.ToArray();
        }
    }
    
    #endregion
    
    enum SFXType
    {
        Noise,      // 噪声
        Slide,      // 滑音
        Chime,      // 叮声
        Chord,      // 和弦
        Sparkle,    // 闪亮
        Fanfare     // 胜利
    }
}
