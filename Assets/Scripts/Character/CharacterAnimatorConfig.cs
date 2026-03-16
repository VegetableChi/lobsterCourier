using UnityEngine;

/// <summary>
/// 角色动画配置器 - 自动配置 Animator 和动画剪辑
/// 基于生成的美术资源自动设置动画
/// </summary>
public class CharacterAnimatorConfig : MonoBehaviour
{
    [Header("角色类型")]
    public CharacterType characterType = CharacterType.Lobster;
    
    [Header("精灵引用")]
    public Sprite[] idleSprites;  // 3 帧 idle 动画
    public Sprite[] runSprites;   // 4 帧跑步动画（可选）
    public Sprite[] grabSprites;  // 2 帧抓取动画（可选）
    
    [Header("Animator 配置")]
    public Animator animator;
    public RuntimeAnimatorController animatorController;
    
    [Header("SpriteRenderer")]
    public SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 自动加载精灵
        AutoLoadSprites();
        
        // 配置 Animator
        SetupAnimator();
    }
    
    void AutoLoadSprites()
    {
        string characterName = GetCharacterName();
        string basePath = "Sprites/characters/";
        
        // 加载 idle 动画帧
        idleSprites = new Sprite[3];
        for (int i = 0; i < 3; i++)
        {
            idleSprites[i] = Resources.Load<Sprite>($"{basePath}{characterName}_idle_{i}");
        }
        
        // 设置初始精灵
        if (idleSprites.Length > 0 && idleSprites[0] != null)
        {
            spriteRenderer.sprite = idleSprites[0];
        }
    }
    
    string GetCharacterName()
    {
        switch (characterType)
        {
            case CharacterType.Lobster: return "lobster";
            case CharacterType.Starfish: return "starfish";
            case CharacterType.Octopus: return "octopus";
            case CharacterType.Crab: return "crab";
            case CharacterType.CoralFish: return "coral_fish";
            case CharacterType.Seahorse: return "seahorse";
            case CharacterType.Jellyfish: return "jellyfish";
            case CharacterType.Turtle: return "turtle";
            case CharacterType.Shark: return "shark";
            default: return "lobster";
        }
    }
    
    void SetupAnimator()
    {
        if (animator == null) return;
        
        // 如果没有 Animator Controller，创建一个简单的
        if (animator.runtimeAnimatorController == null)
        {
            CreateSimpleAnimator();
        }
    }
    
    void CreateSimpleAnimator()
    {
        // 创建 Animator Override Controller
        AnimatorOverrideController overrideController = new AnimatorOverrideController();
        
        // 设置基础控制器（需要有一个基础的 Animator Controller）
        // 这里我们使用代码方式设置动画
        
        // 简单方案：直接使用代码控制精灵切换
        StartCoroutine(IdleAnimationCoroutine());
    }
    
    System.Collections.IEnumerator IdleAnimationCoroutine()
    {
        if (idleSprites == null || idleSprites.Length == 0) yield break;
        
        float frameRate = 0.1f; // 10 FPS
        
        while (true)
        {
            for (int i = 0; i < idleSprites.Length; i++)
            {
                if (idleSprites[i] != null)
                {
                    spriteRenderer.sprite = idleSprites[i];
                }
                yield return new WaitForSeconds(frameRate);
            }
        }
    }
    
    /// <summary>
    /// 播放跑步动画
    /// </summary>
    public void PlayRunAnimation(bool isRunning)
    {
        if (!isRunning) return;
        
        if (runSprites != null && runSprites.Length > 0)
        {
            StartCoroutine(RunAnimationCoroutine());
        }
    }
    
    System.Collections.IEnumerator RunAnimationCoroutine()
    {
        if (runSprites == null || runSprites.Length == 0) yield break;
        
        float frameRate = 0.08f; // 12.5 FPS
        
        while (true)
        {
            for (int i = 0; i < runSprites.Length; i++)
            {
                if (runSprites[i] != null)
                {
                    spriteRenderer.sprite = runSprites[i];
                }
                yield return new WaitForSeconds(frameRate);
            }
        }
    }
    
    /// <summary>
    /// 设置角色朝向
    /// </summary>
    public void SetFacingDirection(Vector2 direction)
    {
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}

/// <summary>
/// 角色类型枚举
/// </summary>
public enum CharacterType
{
    Lobster,
    Starfish,
    Octopus,
    Crab,
    CoralFish,
    Seahorse,
    Jellyfish,
    Turtle,
    Shark
}
