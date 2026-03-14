using UnityEngine;

/// <summary>
/// 包裹系统 - 可拾取和交付的物品
/// </summary>
public class Package : MonoBehaviour
{
    [Header("包裹属性")]
    public PackageType packageType;
    public float value = 10f;
    public float tipMultiplier = 1f;
    public bool isFragile = false;
    public bool isUrgent = false;
    
    [Header("时间限制")]
    public bool hasTimeLimit = false;
    public float timeLimit = 60f;
    private float remainingTime;
    
    // 状态
    public bool IsGrabable => !isHeld && !isDelivered;
    public bool IsDelivered => isDelivered;
    
    [Header("视觉")]
    public SpriteRenderer spriteRenderer;
    public GameObject glowEffect;
    
    private bool isHeld = false;
    private bool isDelivered = false;
    private Transform holdParent;
    private Vector2 holdOffset;
    
    void Start()
    {
        if (hasTimeLimit)
        {
            remainingTime = timeLimit;
        }
    }
    
    void Update()
    {
        if (isHeld && holdParent != null)
        {
            transform.position = holdParent.position + (Vector3)holdOffset;
        }
        
        if (hasTimeLimit && !isDelivered)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                OnTimeExpired();
            }
        }
    }
    
    public void AttachToLobster(Transform holdPoint)
    {
        isHeld = true;
        holdParent = holdPoint;
        holdOffset = transform.position - holdPoint.position;
        
        // 禁用物理
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false;
        }
        
        // 显示高亮
        if (glowEffect != null)
        {
            glowEffect.SetActive(true);
        }
    }
    
    public void Release()
    {
        isHeld = false;
        holdParent = null;
        
        // 启用物理
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
        }
    }
    
    public void Deliver()
    {
        isDelivered = true;
        
        // 计算最终奖励
        float finalValue = value;
        if (isUrgent && remainingTime > timeLimit * 0.5f)
        {
            finalValue *= tipMultiplier * 1.5f; // 紧急订单提前送达
        }
        else if (isFragile)
        {
            finalValue *= tipMultiplier;
        }
        
        GameManager.Instance.OnPackageDelivered(this, finalValue);
        
        // 播放特效
        PlayDeliveryEffect();
        
        // 销毁包裹
        Destroy(gameObject, 0.5f);
    }
    
    void OnTimeExpired()
    {
        GameManager.Instance.OnPackageExpired(this);
        isDelivered = true;
        Destroy(gameObject);
    }
    
    void PlayDeliveryEffect()
    {
        // 可以添加粒子特效、音效等
        Debug.Log($"📦 包裹送达！奖励：${finalValue}");
    }
}

public enum PackageType
{
    Food,           // 食物外卖
    Express,        // 快递
    Fragile,        // 易碎品
    Urgent,         // 紧急物品
    Valuable,       // 贵重物品
    Mystery         // 神秘包裹
}
