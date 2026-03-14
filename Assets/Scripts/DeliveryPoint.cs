using UnityEngine;

/// <summary>
/// 送货点 - 海洋生物的收货位置
/// </summary>
public class DeliveryPoint : MonoBehaviour
{
    [Header("送货点信息")]
    public string customerName;
    public CustomerType customerType;
    public Sprite customerSprite;
    
    [Header("视觉效果")]
    public SpriteRenderer indicatorRenderer;
    public GameObject highlightEffect;
    public GameObject completedEffect;
    
    [Header("配置")]
    public float detectionRadius = 2f;
    public Color availableColor = Color.green;
    public Color completedColor = Color.gray;
    
    private bool isCompleted = false;
    
    void Start()
    {
        UpdateVisuals();
    }
    
    public void CompleteDelivery()
    {
        if (isCompleted) return;
        
        isCompleted = true;
        
        // 播放完成特效
        if (completedEffect != null)
        {
            Instantiate(completedEffect, transform.position, Quaternion.identity);
        }
        
        // 显示对话气泡
        ShowCustomerDialogue();
        
        UpdateVisuals();
    }
    
    void ShowCustomerDialogue()
    {
        string dialogue = GetRandomDialogue();
        Debug.Log($"💬 {customerName}: \"{dialogue}\"");
        // 可以在这里显示 UI 对话框
    }
    
    string GetRandomDialogue()
    {
        switch (customerType)
        {
            case CustomerType.Starfish:
                return GetStarfishDialogue();
            case CustomerType.Octopus:
                return GetOctopusDialogue();
            case CustomerType.ReefFish:
                return GetReefFishDialogue();
            case CustomerType.Crab:
                return GetCrabDialogue();
            case CustomerType.Seahorse:
                return GetSeahorseDialogue();
            default:
                return "谢谢！";
        }
    }
    
    string GetStarfishDialogue()
    {
        string[] dialogues = new string[]
        {
            "我的海鲜外卖！终于到了！",
            "星星饿了很久了！",
            "这味道...是海藻沙拉吗？",
            "我要再点一份！"
        };
        return dialogues[Random.Range(0, dialogues.Length)];
    }
    
    string GetOctopusDialogue()
    {
        string[] dialogues = new string[]
        {
            "我的咖啡，希望没凉。",
            "放桌上吧，我在画画。",
            "艺术创作时需要咖啡因。",
            "八只手才能拿稳这杯咖啡。"
        };
        return dialogues[Random.Range(0, dialogues.Length)];
    }
    
    string GetReefFishDialogue()
    {
        string[] dialogues = new string[]
        {
            "谢谢！我要拿给家人。",
            "珊瑚区的外卖终于到了！",
            "孩子们一定会喜欢的。",
            "游了这么远，辛苦你了！"
        };
        return dialogues[Random.Range(0, dialogues.Length)];
    }
    
    string GetCrabDialogue()
    {
        string[] dialogues = new string[]
        {
            "贝壳贝壳！...哦这是我的包裹。",
            "海底集市的外卖竞争真激烈。",
            "让我看看里面是什么。",
            "不错不错，给个好评！"
        };
        return dialogues[Random.Range(0, dialogues.Length)];
    }
    
    string GetSeahorseDialogue()
    {
        string[] dialogues = new string[]
        {
            "慢慢游，不急。",
            "我的尾巴都等酸了。",
            "终于到了，谢谢！",
            "下次还找你送。"
        };
        return dialogues[Random.Range(0, dialogues.Length)];
    }
    
    void UpdateVisuals()
    {
        if (indicatorRenderer != null)
        {
            indicatorRenderer.color = isCompleted ? completedColor : availableColor;
        }
        
        if (highlightEffect != null)
        {
            highlightEffect.SetActive(!isCompleted);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lobster"))
        {
            LobsterController lobster = other.GetComponent<LobsterController>();
            if (lobster != null && lobster.isHoldingPackage)
            {
                // 高亮提示可以交付
                if (highlightEffect != null)
                {
                    highlightEffect.SetActive(true);
                }
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Lobster"))
        {
            if (highlightEffect != null && !isCompleted)
            {
                highlightEffect.SetActive(false);
            }
        }
    }
}

public enum CustomerType
{
    Starfish,       // 星星海星
    Octopus,        // 八爪章鱼
    ReefFish,       // 珊瑚鱼
    Crab,           // 钳子蟹
    Seahorse,       // 卷尾海马
    Jellyfish,      // 漂浮水母
    Turtle,         // 长寿海龟
    Shark           // 深海鲨 (VIP 客户)
}
