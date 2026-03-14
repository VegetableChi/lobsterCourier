using UnityEngine;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// 订单 UI - 显示当前订单信息
/// </summary>
public class OrderUI : MonoBehaviour
{
    [Header("订单面板")]
    public GameObject orderPanel;
    public GameObject orderListContent;
    public GameObject orderItemPrefab;
    
    [Header("小地图")]
    public GameObject minimapPanel;
    public RectTransform minimapPlayerMarker;
    public RectTransform minimapSize;
    
    [Header("计时器")]
    public TextMeshProUGUI timerText;
    public Image timerBackground;
    public Color timerNormalColor = Color.green;
    public Color timerWarningColor = Color.yellow;
    public Color timerDangerColor = Color.red;
    
    private List<OrderItemUI> orderItems = new List<OrderItemUI>();
    private float worldSize = 100f;
    
    void Start()
    {
        RefreshOrders();
    }
    
    void Update()
    {
        UpdateTimers();
        UpdateMinimap();
    }
    
    public void RefreshOrders()
    {
        if (orderListContent == null || orderItemPrefab == null) return;
        
        // 清空现有订单
        foreach (var item in orderItems)
        {
            if (item != null && item.gameObject != null)
            {
                Destroy(item.gameObject);
            }
        }
        orderItems.Clear();
        
        // 添加新订单
        if (GameManager.Instance != null)
        {
            foreach (var order in GameManager.Instance.activeOrders)
            {
                CreateOrderItem(order);
            }
        }
    }
    
    void CreateOrderItem(DeliveryOrder order)
    {
        if (orderListContent == null) return;
        
        GameObject itemObj = Instantiate(orderItemPrefab, orderListContent.transform);
        OrderItemUI itemUI = itemObj.GetComponent<OrderItemUI>();
        
        if (itemUI != null)
        {
            itemUI.Initialize(order);
            orderItems.Add(itemUI);
        }
    }
    
    void UpdateTimers()
    {
        if (GameManager.Instance == null || GameManager.Instance.activeOrders.Count == 0)
        {
            if (timerText != null)
            {
                timerText.text = "";
            }
            return;
        }
        
        // 找到最紧急的订单
        float minTime = float.MaxValue;
        foreach (var order in GameManager.Instance.activeOrders)
        {
            if (order.isPickedUp && order.package != null)
            {
                // 这里需要从 Package 获取剩余时间
                // 简化处理，暂时不显示
            }
        }
    }
    
    void UpdateMinimap()
    {
        if (minimapPanel == null || !minimapPanel.activeSelf) return;
        
        // 更新玩家位置
        if (minimapPlayerMarker != null && GameManager.Instance?.player != null)
        {
            Vector2 playerPos = GameManager.Instance.player.transform.position;
            minimapPlayerMarker.anchoredPosition = WorldToMinimap(playerPos);
        }
        
        // 更新订单目标标记
        UpdateOrderMarkers();
    }
    
    void UpdateOrderMarkers()
    {
        // 可以在这里添加订单目标在小地图上的标记
    }
    
    Vector2 WorldToMinimap(Vector2 worldPos)
    {
        float halfWorldSize = worldSize / 2f;
        float normalizedX = (worldPos.x + halfWorldSize) / worldSize;
        float normalizedY = (worldPos.y + halfWorldSize) / worldSize;
        
        float minimapWidth = minimapSize.rect.width;
        float minimapHeight = minimapSize.rect.height;
        
        return new Vector2(
            (normalizedX - 0.5f) * minimapWidth,
            (normalizedY - 0.5f) * minimapHeight
        );
    }
    
    public void ToggleMinimap()
    {
        if (minimapPanel != null)
        {
            minimapPanel.SetActive(!minimapPanel.activeSelf);
        }
    }
    
    public void ShowMinimap()
    {
        if (minimapPanel != null)
        {
            minimapPanel.SetActive(true);
        }
    }
    
    public void HideMinimap()
    {
        if (minimapPanel != null)
        {
            minimapPanel.SetActive(false);
        }
    }
}
