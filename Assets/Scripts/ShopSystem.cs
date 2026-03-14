using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 商店系统 - 购买升级和道具
/// </summary>
public class ShopSystem : MonoBehaviour
{
    public static ShopSystem Instance { get; private set; }
    
    [Header("商店配置")]
    public List<ShopItem> availableItems = new List<ShopItem>();
    
    [Header("玩家引用")]
    public LobsterController player;
    public GameManager gameManager;
    
    [Header("UI 引用")]
    public ShopUI shopUI;
    
    [Header("升级倍率")]
    public float speedUpgradeCostMultiplier = 1.5f;
    public float staminaUpgradeCostMultiplier = 1.3f;
    public float backpackUpgradeCostMultiplier = 2f;
    
    // 玩家购买记录
    private Dictionary<string, int> purchasedUpgrades = new Dictionary<string, int>();
    
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
        InitializeShopItems();
        LoadPurchaseHistory();
    }
    
    void InitializeShopItems()
    {
        // 速度升级
        availableItems.Add(new ShopItem
        {
            id = "speed_upgrade",
            name = "速度升级",
            description = "提升移动速度 +1",
            basePrice = 50,
            itemType = ShopItemType.Upgrade,
            icon = "⚡",
            maxLevel = 10
        });
        
        // 体力升级
        availableItems.Add(new ShopItem
        {
            id = "stamina_upgrade",
            name = "体力升级",
            description = "提升最大体力 +20",
            basePrice = 40,
            itemType = ShopItemType.Upgrade,
            icon = "💪",
            maxLevel = 10
        });
        
        // 背包升级
        availableItems.Add(new ShopItem
        {
            id = "backpack_upgrade",
            name = "背包升级",
            description = "同时携带包裹数 +1",
            basePrice = 100,
            itemType = ShopItemType.Upgrade,
            icon = "🎒",
            maxLevel = 5
        });
        
        // 磁力道具
        availableItems.Add(new ShopItem
        {
            id = "magnet_powerup",
            name = "磁力道具",
            description = "30 秒内自动吸引附近包裹",
            basePrice = 25,
            itemType = ShopItemType.Consumable,
            icon = "🧲",
            maxLevel = 999
        });
        
        // 无敌冲刺
        availableItems.Add(new ShopItem
        {
            id = "invincible_sprint",
            name = "无敌冲刺",
            description = "5 秒内冲刺不消耗体力且无敌",
            basePrice = 35,
            itemType = ShopItemType.Consumable,
            icon = "💨",
            maxLevel = 999
        });
        
        // 时间减缓
        availableItems.Add(new ShopItem
        {
            id = "time_slow",
            name = "时间减缓",
            description = "10 秒内订单时间流逝减半",
            basePrice = 40,
            itemType = ShopItemType.Consumable,
            icon = "⏰",
            maxLevel = 999
        });
    }
    
    public ShopItem GetItem(string itemId)
    {
        return availableItems.Find(item => item.id == itemId);
    }
    
    public int GetUpgradeLevel(string itemId)
    {
        if (purchasedUpgrades.ContainsKey(itemId))
        {
            return purchasedUpgrades[itemId];
        }
        return 0;
    }
    
    public int GetItemPrice(string itemId)
    {
        ShopItem item = GetItem(itemId);
        if (item == null) return 0;
        
        int level = GetUpgradeLevel(itemId);
        float multiplier = GetMultiplier(item.itemType);
        
        return Mathf.RoundToInt(item.basePrice * Mathf.Pow(multiplier, level));
    }
    
    float GetMultiplier(ShopItemType type)
    {
        switch (type)
        {
            case ShopItemType.Upgrade:
                return 1.5f;
            case ShopItemType.Consumable:
                return 1.0f;
            default:
                return 1.0f;
        }
    }
    
    public bool PurchaseItem(string itemId)
    {
        ShopItem item = GetItem(itemId);
        if (item == null) return false;
        
        int currentLevel = GetUpgradeLevel(itemId);
        if (currentLevel >= item.maxLevel)
        {
            Debug.Log("已达到最大等级！");
            return false;
        }
        
        int price = GetItemPrice(itemId);
        if (gameManager.playerMoney < price)
        {
            Debug.Log("金钱不足！");
            return false;
        }
        
        // 扣除金钱
        gameManager.playerMoney -= price;
        
        // 增加购买等级
        if (!purchasedUpgrades.ContainsKey(itemId))
        {
            purchasedUpgrades[itemId] = 0;
        }
        purchasedUpgrades[itemId]++;
        
        // 应用效果
        ApplyItemEffect(item, currentLevel + 1);
        
        // 保存
        SavePurchaseHistory();
        
        // 更新 UI
        if (shopUI != null)
        {
            shopUI.RefreshShop();
        }
        
        Debug.Log($"✅ 购买成功：{item.name} x{purchasedUpgrades[itemId]}");
        return true;
    }
    
    void ApplyItemEffect(ShopItem item, int level)
    {
        switch (item.id)
        {
            case "speed_upgrade":
                if (player != null)
                {
                    player.moveSpeed += 1f;
                }
                break;
                
            case "stamina_upgrade":
                if (player != null)
                {
                    player.maxStamina += 20f;
                }
                break;
                
            case "backpack_upgrade":
                // 背包升级逻辑（需要修改 LobsterController 支持多包裹）
                Debug.Log("背包容量 +1");
                break;
                
            case "magnet_powerup":
                StartCoroutine(MagnetPowerup(30f));
                break;
                
            case "invincible_sprint":
                StartCoroutine(InvincibleSprint(5f));
                break;
                
            case "time_slow":
                StartCoroutine(TimeSlow(10f));
                break;
        }
    }
    
    System.Collections.IEnumerator MagnetPowerup(float duration)
    {
        Debug.Log("🧲 磁力道具激活！");
        yield return new WaitForSeconds(duration);
        Debug.Log("磁力道具结束");
    }
    
    System.Collections.IEnumerator InvincibleSprint(float duration)
    {
        Debug.Log("💨 无敌冲刺激活！");
        if (player != null)
        {
            // 可以添加无敌效果
        }
        yield return new WaitForSeconds(duration);
        Debug.Log("无敌冲刺结束");
    }
    
    System.Collections.IEnumerator TimeSlow(float duration)
    {
        Debug.Log("⏰ 时间减缓激活！");
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        Debug.Log("时间减缓结束");
    }
    
    void SavePurchaseHistory()
    {
        // 简化保存，不保存复杂数据
        PlayerPrefs.Save();
    }
    
    void LoadPurchaseHistory()
    {
        // 简化加载，不加载复杂数据
    }
    
    public void ResetShop()
    {
        purchasedUpgrades.Clear();
        PlayerPrefs.DeleteKey("ShopSaveData");
        PlayerPrefs.Save();
    }
}

[System.Serializable]
public class ShopItem
{
    public string id;
    public string name;
    public string description;
    public int basePrice;
    public ShopItemType itemType;
    public string icon;
    public int maxLevel;
    
    public bool IsMaxLevel(int currentLevel)
    {
        return currentLevel >= maxLevel;
    }
}

public enum ShopItemType
{
    Upgrade,
    Consumable,
    Cosmetic
}

[System.Serializable]
public class SaveData
{
    public Dictionary<string, int> purchasedUpgrades = new Dictionary<string, int>();
}
