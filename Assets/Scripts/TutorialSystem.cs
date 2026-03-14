using UnityEngine;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// 教程系统 - 新手引导
/// </summary>
public class TutorialSystem : MonoBehaviour
{
    public static TutorialSystem Instance { get; private set; }
    
    [Header("教程面板")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialTitle;
    public TextMeshProUGUI tutorialContent;
    public TextMeshProUGUI tutorialTip;
    public GameObject nextButton;
    public GameObject completeButton;
    
    [Header("高亮显示")]
    public GameObject highlightPrefab;
    private List<GameObject> activeHighlights = new List<GameObject>();
    
    [Header("教程步骤")]
    public List<TutorialStep> tutorialSteps = new List<TutorialStep>();
    private int currentStepIndex = 0;
    
    [Header("设置")]
    public bool showTutorial = true;
    public bool skipOnEscape = true;
    
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
        InitializeTutorialSteps();
        
        // 检查是否已看过教程
        if (PlayerPrefs.HasKey("TutorialCompleted"))
        {
            showTutorial = false;
        }
        
        if (showTutorial)
        {
            StartTutorial();
        }
    }
    
    void InitializeTutorialSteps()
    {
        tutorialSteps = new List<TutorialStep>
        {
            new TutorialStep
            {
                title = "欢迎来到蓝海大陆！",
                content = "你是一只龙虾快递员，你的任务是为海洋生物们送达外卖和包裹。",
                tip = "点击【下一步】开始你的快递生涯！",
                highlightTarget = null,
                onComplete = null
            },
            new TutorialStep
            {
                title = "移动控制",
                content = "使用【W/A/S/D】或【方向键】来控制龙虾的移动方向。",
                tip = "试着移动一下！",
                highlightTarget = null,
                onComplete = () => {
                    // 等待玩家移动
                }
            },
            new TutorialStep
            {
                title = "冲刺技能",
                content = "按住【Shift】可以冲刺，快速到达目的地！\n\n但冲刺会消耗体力，体力耗尽后需要休息恢复。",
                tip = "按【Shift】试试冲刺！",
                highlightTarget = null,
                onComplete = null
            },
            new TutorialStep
            {
                title = "收取包裹",
                content = "靠近闪烁的包裹，按【E】或【空格】可以拾取包裹。",
                tip = "靠近包裹并按【E】拾取",
                highlightTarget = null,
                onComplete = null
            },
            new TutorialStep
            {
                title = "送达包裹",
                content = "将包裹送到标记的送货点，客户会在那里等待。",
                tip = "送到绿色标记区域",
                highlightTarget = null,
                onComplete = null
            },
            new TutorialStep
            {
                title = "连击系统",
                content = "连续准时送达可以获得连击奖励！\n\n连击数越高，奖励越多（最高 +200%）。",
                tip = "尽量保持连击！",
                highlightTarget = null,
                onComplete = null
            },
            new TutorialStep
            {
                title = "商店系统",
                content = "按【B】打开商店，可以购买升级和道具。\n\n升级可以提升你的能力，道具可以在关键时刻帮助你。",
                tip = "按【B】打开商店",
                highlightTarget = null,
                onComplete = null
            },
            new TutorialStep
            {
                title = "成就系统",
                content = "按【A】查看成就列表。\n\n完成成就可获得大量金币奖励！",
                tip = "按【A】查看成就",
                highlightTarget = null,
                onComplete = null
            },
            new TutorialStep
            {
                title = "小地图",
                content = "按【M】可以打开/关闭小地图。\n\n小地图会显示你的位置和目标方向。",
                tip = "按【M】开关小地图",
                highlightTarget = null,
                onComplete = null
            },
            new TutorialStep
            {
                title = "准备好了吗？",
                content = "你已经学会了所有基础知识！\n\n现在，开始你的快递生涯吧！",
                tip = "点击【完成】开始游戏",
                highlightTarget = null,
                onComplete = () => {
                    CompleteTutorial();
                }
            }
        };
    }
    
    public void StartTutorial()
    {
        showTutorial = true;
        currentStepIndex = 0;
        ShowStep(currentStepIndex);
        
        // 暂停游戏
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentState = GameState.Paused;
            Time.timeScale = 0f;
        }
    }
    
    void ShowStep(int index)
    {
        if (index >= tutorialSteps.Count) return;
        
        TutorialStep step = tutorialSteps[index];
        
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }
        
        if (tutorialTitle != null)
        {
            tutorialTitle.text = step.title;
        }
        
        if (tutorialContent != null)
        {
            tutorialContent.text = step.content;
        }
        
        if (tutorialTip != null)
        {
            tutorialTip.text = step.tip;
        }
        
        // 显示高亮
        if (step.highlightTarget != null)
        {
            ShowHighlight(step.highlightTarget);
        }
        
        // 更新按钮
        if (nextButton != null)
        {
            nextButton.SetActive(index < tutorialSteps.Count - 1);
        }
        
        if (completeButton != null)
        {
            completeButton.SetActive(index == tutorialSteps.Count - 1);
        }
    }
    
    public void NextStep()
    {
        if (currentStepIndex < tutorialSteps.Count - 1)
        {
            currentStepIndex++;
            ShowStep(currentStepIndex);
        }
    }
    
    public void CompleteTutorial()
    {
        PlayerPrefs.SetInt("TutorialCompleted", 1);
        PlayerPrefs.Save();
        
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
        
        ClearHighlights();
        
        // 继续游戏
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentState = GameState.Playing;
            Time.timeScale = GameManager.Instance.gameSpeed;
        }
        
        Debug.Log("✅ 教程完成！");
    }
    
    public void SkipTutorial()
    {
        if (skipOnEscape)
        {
            PlayerPrefs.SetInt("TutorialCompleted", 1);
            PlayerPrefs.Save();
            
            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(false);
            }
            
            ClearHighlights();
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.currentState = GameState.Playing;
                Time.timeScale = GameManager.Instance.gameSpeed;
            }
        }
    }
    
    void ShowHighlight(GameObject target)
    {
        if (target == null || highlightPrefab == null) return;
        
        GameObject highlight = Instantiate(highlightPrefab, target.transform.position, Quaternion.identity);
        highlight.transform.SetParent(target.transform);
        activeHighlights.Add(highlight);
    }
    
    void ClearHighlights()
    {
        foreach (var highlight in activeHighlights)
        {
            if (highlight != null)
            {
                Destroy(highlight);
            }
        }
        activeHighlights.Clear();
    }
    
    void Update()
    {
        if (!showTutorial) return;
        
        // 按 ESC 跳过教程
        if (skipOnEscape && Input.GetKeyDown(KeyCode.Escape))
        {
            SkipTutorial();
        }
        
        // 检查当前步骤的完成条件
        CheckStepCompletion();
    }
    
    void CheckStepCompletion()
    {
        if (currentStepIndex >= tutorialSteps.Count) return;
        
        TutorialStep step = tutorialSteps[currentStepIndex];
        
        // 根据步骤类型检查完成条件
        switch (currentStepIndex)
        {
            case 1: // 移动控制
                if (GameManager.Instance?.player != null)
                {
                    // 检测玩家是否移动过
                }
                break;
            case 2: // 冲刺
                // 检测是否使用过冲刺
                break;
            case 3: // 拾取包裹
                // 检测是否拾取过包裹
                break;
        }
    }
}

[System.Serializable]
public class TutorialStep
{
    public string title;
    public string content;
    public string tip;
    public GameObject highlightTarget;
    public System.Action onComplete;
}
