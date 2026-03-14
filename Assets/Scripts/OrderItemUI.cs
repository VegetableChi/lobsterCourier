using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;

/// <summary>
/// 单个订单物品 UI
/// </summary>
public class OrderItemUI : MonoBehaviour
{
    [Header("UI 元素")]
    public TextMeshProUGUI orderTypeText;
    public TextMeshProUGUI pickupLocationText;
    public TextMeshProUGUI deliveryLocationText;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI timerText;
    public Image timerBackground;
    public Image priorityIndicator;
    
    [Header("颜色")]
    public Color normalColor = Color.white;
    public Color urgentColor = Color.red;
    public Color fragileColor = Color.cyan;
    public Color valuableColor = Color.yellow;
    public Color timerWarningColor = Color.yellow;
    public Color timerDangerColor = Color.red;
    
    private DeliveryOrder order;
    private float timeRemaining = 0f;
    
    public void Initialize(DeliveryOrder order)
    {
        this.order = order;
        
        if (order == null || order.package == null) return;
        
        UpdateDisplay();
    }
    
    void UpdateDisplay()
    {
        if (order.package == null) return;
        
        // 包裹类型
        if (orderTypeText != null)
        {
            orderTypeText.text = GetPackageTypeIcon(order.package.packageType);
            orderTypeText.color = GetPackageTypeColor(order.package.packageType);
        }
        
        // 取货点
        if (pickupLocationText != null && order.pickupLocation != null)
        {
            pickupLocationText.text = $"📍 {order.pickupLocation.name}";
        }
        
        // 送货点
        if (deliveryLocationText != null && order.deliveryLocation != null)
        {
            DeliveryPoint point = order.deliveryLocation;
            deliveryLocationText.text = $"🏠 {point.customerName} ({GetCustomerIcon(point.customerType)})";
        }
        
        // 奖励
        if (rewardText != null)
        {
            rewardText.text = $"💰 ${order.package.value}";
        }
        
        // 优先级指示器
        if (priorityIndicator != null)
        {
            priorityIndicator.color = order.package.isUrgent ? urgentColor : normalColor;
        }
        
        // 时间限制
        if (order.package.hasTimeLimit)
        {
            timeRemaining = order.package.timeLimit;
            if (timerText != null)
            {
                timerText.text = FormatTime(timeRemaining);
            }
        }
        else
        {
            if (timerText != null)
            {
                timerText.text = "∞";
            }
        }
    }
    
    void Update()
    {
        if (order != null && order.package != null && order.package.hasTimeLimit)
        {
            timeRemaining -= Time.deltaTime;
            
            if (timerText != null)
            {
                timerText.text = FormatTime(Mathf.Max(0, timeRemaining));
                
                // 更新颜色
                if (timeRemaining <= 10f)
                {
                    timerBackground.color = timerDangerColor;
                }
                else if (timeRemaining <= 30f)
                {
                    timerBackground.color = timerWarningColor;
                }
            }
        }
    }
    
    string GetPackageTypeIcon(PackageType type)
    {
        switch (type)
        {
            case PackageType.Food: return "🍔";
            case PackageType.Express: return "📦";
            case PackageType.Fragile: return "🥚";
            case PackageType.Urgent: return "⚡";
            case PackageType.Valuable: return "💎";
            case PackageType.Mystery: return "❓";
            default: return "📦";
        }
    }
    
    Color GetPackageTypeColor(PackageType type)
    {
        switch (type)
        {
            case PackageType.Urgent: return urgentColor;
            case PackageType.Fragile: return fragileColor;
            case PackageType.Valuable: return valuableColor;
            default: return normalColor;
        }
    }
    
    string GetCustomerIcon(CustomerType type)
    {
        switch (type)
        {
            case CustomerType.Starfish: return "⭐";
            case CustomerType.Octopus: return "🐙";
            case CustomerType.Clownfish: return "🐠";
            case CustomerType.Crab: return "🦀";
            case CustomerType.Seahorse: return "🐴";
            case CustomerType.Jellyfish: return "🪼";
            case CustomerType.Turtle: return "🐢";
            case CustomerType.Shark: return "🦈";
            default: return "🐟";
        }
    }
    
    string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{mins:D2}:{secs:D2}";
    }
}
