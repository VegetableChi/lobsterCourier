using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 单个商店物品 UI
/// </summary>
public class ShopItemUI : MonoBehaviour
{
    [Header("UI 元素")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI iconText;
    public Button purchaseButton;
    public Image backgroundImage;
    
    [Header("颜色")]
    public Color affordableColor = Color.green;
    public Color expensiveColor = Color.red;
    public Color maxLevelColor = Color.gray;
    
    private ShopItem item;
    private System.Action<string> onPurchase;
    
    public void Initialize(ShopItem item, System.Action<string> onPurchase)
    {
        this.item = item;
        this.onPurchase = onPurchase;
        
        UpdateDisplay();
        
        if (purchaseButton != null)
        {
            purchaseButton.onClick.AddListener(OnPurchaseClicked);
        }
    }
    
    void UpdateDisplay()
    {
        if (item == null) return;
        
        int currentLevel = ShopSystem.Instance.GetUpgradeLevel(item.id);
        int price = ShopSystem.Instance.GetItemPrice(item.id);
        
        // 更新文本
        if (nameText != null)
        {
            nameText.text = $"{item.icon} {item.name}";
        }
        
        if (descriptionText != null)
        {
            descriptionText.text = item.description;
        }
        
        if (priceText != null)
        {
            priceText.text = $"${price}";
        }
        
        if (levelText != null)
        {
            if (item.itemType == ShopItemType.Upgrade)
            {
                levelText.text = $"Lv.{currentLevel}/{item.maxLevel}";
            }
            else
            {
                levelText.text = $"拥有：{currentLevel}";
            }
        }
        
        if (iconText != null)
        {
            iconText.text = item.icon;
        }
        
        // 更新按钮状态
        UpdateButtonState(currentLevel, price);
    }
    
    void UpdateButtonState(int currentLevel, int price)
    {
        if (purchaseButton == null) return;
        
        // 检查是否满级
        if (currentLevel >= item.maxLevel)
        {
            purchaseButton.interactable = false;
            if (backgroundImage != null)
            {
                backgroundImage.color = maxLevelColor;
            }
            if (priceText != null)
            {
                priceText.text = "MAX";
            }
            return;
        }
        
        // 检查是否买得起
        bool canAfford = GameManager.Instance != null && GameManager.Instance.playerMoney >= price;
        purchaseButton.interactable = canAfford;
        
        if (backgroundImage != null)
        {
            backgroundImage.color = canAfford ? affordableColor : expensiveColor;
        }
    }
    
    void OnPurchaseClicked()
    {
        if (onPurchase != null && item != null)
        {
            onPurchase.Invoke(item.id);
        }
    }
    
    void Update()
    {
        // 实时更新按钮状态（金钱变化时）
        if (item != null && purchaseButton != null && purchaseButton.interactable)
        {
            int currentLevel = ShopSystem.Instance.GetUpgradeLevel(item.id);
            int price = ShopSystem.Instance.GetItemPrice(item.id);
            UpdateButtonState(currentLevel, price);
        }
    }
}
