using UnityEngine;

/// <summary>
/// 龙虾玩家控制器 - 核心移动和交互
/// </summary>
public class LobsterController : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;
    public float rotationSpeed = 10f;
    
    [Header("体力系统")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 10f;
    private float currentStamina;
    private bool isSprinting = false;
    
    [Header("钳子状态")]
    public bool isHoldingPackage = false;
    public Transform holdPoint;
    
    [Header("组件引用")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    // 状态
    public bool IsExhausted => currentStamina <= 0;
    public float StaminaPercent => currentStamina / maxStamina;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Start()
    {
        currentStamina = maxStamina;
    }
    
    void Update()
    {
        HandleInput();
        HandleStamina();
        UpdateAnimation();
    }
    
    void FixedUpdate()
    {
        HandleMovement();
    }
    
    void HandleInput()
    {
        // 移动输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // 冲刺输入
        isSprinting = Input.GetButton("Sprint") && currentStamina > 0;
        
        // 抓取输入
        if (Input.GetButtonDown("Grab"))
        {
            TryGrabPackage();
        }
        
        // 存储输入用于 FixedUpdate
        inputVector = new Vector2(horizontal, vertical).normalized;
    }
    
    private Vector2 inputVector;
    
    void HandleMovement()
    {
        if (inputVector.magnitude == 0) return;
        
        // 计算实际速度（考虑洋流影响）
        float currentSpeed = moveSpeed;
        if (isSprinting && !IsExhausted)
        {
            currentSpeed *= sprintMultiplier;
        }
        
        // 应用洋流影响
        Vector2 currentFlow = OceanCurrentManager.Instance?.GetCurrentFlow(transform.position) ?? Vector2.zero;
        Vector2 finalVelocity = (inputVector * currentSpeed) + currentFlow;
        
        // 移动
        rb.velocity = finalVelocity;
        
        // 翻转朝向
        if (inputVector.x != 0)
        {
            float targetScaleX = inputVector.x > 0 ? 1 : -1;
            transform.localScale = new Vector3(
                Mathf.Lerp(transform.localScale.x, targetScaleX, rotationSpeed * Time.fixedDeltaTime),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }
    
    void HandleStamina()
    {
        if (isSprinting && !IsExhausted)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Max(0, currentStamina);
        }
        else if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(maxStamina, currentStamina);
        }
    }
    
    void UpdateAnimation()
    {
        if (animator == null) return;
        
        animator.SetFloat("Speed", inputVector.magnitude);
        animator.SetBool("IsSprinting", isSprinting);
        animator.SetBool("IsExhausted", IsExhausted);
        animator.SetBool("IsHoldingPackage", isHoldingPackage);
    }
    
    void TryGrabPackage()
    {
        if (isHoldingPackage) return; // 已经拿着包裹
        
        // 检测附近的包裹
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        foreach (var col in nearby)
        {
            if (col.CompareTag("Package"))
            {
                Package package = col.GetComponent<Package>();
                if (package != null && package.IsGrabable)
                {
                    GrabPackage(package);
                    break;
                }
            }
        }
    }
    
    void GrabPackage(Package package)
    {
        isHoldingPackage = true;
        package.AttachToLobster(holdPoint);
        GameManager.Instance.OnPackagePickedUp(package);
    }
    
    public void DropPackage()
    {
        if (!isHoldingPackage) return;
        
        isHoldingPackage = false;
        // 包裹会从 holdPoint 释放
        GameManager.Instance.OnPackageDropped();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeliveryPoint") && isHoldingPackage)
        {
            DeliveryPoint point = other.GetComponent<DeliveryPoint>();
            if (point != null)
            {
                point.CompleteDelivery();
                DropPackage();
            }
        }
    }
}
