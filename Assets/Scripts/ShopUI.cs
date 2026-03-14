using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 商店 UI 管理
/// </summary>
public class ShopUI : MonoBehaviour
{
    [Header("商店面板")]
    public GameObject shopPanel;
    public GameObject shopContent;
    public GameObject shopItemPrefab;
    
    [Header("按钮")]
    public Button openShopButton;
    public Button closeShopButton;
    
    [Header("金钱显示")]
    public TextMeshProUGUI moneyText;
    
    private bool isShopOpen = false;
    
    void Start()
    {
        if (openShopButton != null)
        {
            openShopButton.onClick.AddListener(OpenShop);
        }
        
        if (closeShopButton != null)
        {
            closeShopButton.onClick.AddListener(CloseShop);
        }
        
        RefreshShop();
    }
    
    void Update()
    {
        // 按 B 键打开/关闭商店
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isShopOpen)
            {
                CloseShop();
            }
            else
            {
                OpenShop();
            }
        }
        
        // 更新金钱显示
        if (GameManager.Instance != null && moneyText != null)
        {
            moneyText.text = $"💰 ${GameManager.Instance.playerMoney}";
        }
    }
    
    public void OpenShop()
    {
        isShopOpen = true;
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
        }
        
        // 暂停游戏
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TogglePause();
        }
        
        RefreshShop();
    }
    
    public void CloseShop()
    {
        isShopOpen = false;
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
        
        // 继续游戏
        if (GameManager.Instance != null && GameManager.Instance.currentState == GameState.Paused)
        {
            GameManager.Instance.TogglePause();
        }
    }
    
    public void RefreshShop()
    {
        if (shopContent == null || shopItemPrefab == null) return;
        
        // 清空现有物品
        foreach (Transform child in shopContent.transform)
        {
            Destroy(child.gameObject);
        }
        
        // 添加商店物品
        if (ShopSystem.Instance != null)
        {
            foreach (ShopItem item in ShopSystem.Instance.availableItems)
            {
                CreateShopItem(item);
            }
        }
    }
    
    void CreateShopItem(ShopItem item)
    {
        GameObject itemObj = Instantiate(shopItemPrefab, shopContent.transform);
        ShopItemUI itemUI = itemObj.GetComponent<ShopItemUI>();
        
        if (itemUI != null)
        {
            itemUI.Initialize(item, OnPurchaseItem);
        }
    }
    
    void OnPurchaseItem(string itemId)
    {
        if (ShopSystem.Instance != null)
        {
            bool success = ShopSystem.Instance.PurchaseItem(itemId);
            if (success)
            {
                RefreshShop();
            }
        }
    }
}
