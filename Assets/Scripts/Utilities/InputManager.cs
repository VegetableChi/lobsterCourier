using UnityEngine;

/// <summary>
/// 输入管理器 - 统一输入处理，支持键盘和手柄
/// 优化：输入缓冲、连击检测、输入记录
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    [Header("输入缓冲")]
    public float inputBufferTime = 0.1f; // 输入缓冲时间（秒）
    
    [Header("连击检测")]
    public float comboTime = 0.3f; // 连击时间窗口
    public int maxCombo = 10;
    
    [Header("调试")]
    public bool showInputDebug = false;
    
    // 输入状态
    private Vector2 moveInput;
    private bool sprintHeld;
    private bool grabPressed;
    
    // 输入缓冲
    private BufferedInput bufferedGrab;
    
    // 连击计数
    private int currentCombo = 0;
    private float lastInputTime = 0f;
    
    // 事件
    public delegate void InputEvent(Vector2 input);
    public static event InputEvent OnMoveInput;
    
    public static event System.Action OnGrabInput;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        ReadInput();
        UpdateBuffers();
        DetectCombo();
    }
    
    void ReadInput()
    {
        // 移动输入
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical).normalized;
        
        // 冲刺
        sprintHeld = Input.GetButton("Sprint");
        
        // 抓取（带缓冲）
        if (Input.GetButtonDown("Grab"))
        {
            grabPressed = true;
            bufferedGrab.Press();
        }
    }
    
    void UpdateBuffers()
    {
        bufferedGrab.Update(Time.deltaTime, inputBufferTime);
    }
    
    void DetectCombo()
    {
        if (grabPressed && Time.time - lastInputTime < comboTime)
        {
            currentCombo = Mathf.Min(currentCombo + 1, maxCombo);
        }
        else if (grabPressed)
        {
            currentCombo = 1;
        }
        
        if (grabPressed)
        {
            lastInputTime = Time.time;
        }
        
        grabPressed = false; // 重置
    }
    
    #region 公共接口
    
    public Vector2 GetMoveInput()
    {
        return moveInput;
    }
    
    public bool IsSprinting()
    {
        return sprintHeld;
    }
    
    public bool GetGrabInput()
    {
        return grabPressed || bufferedGrab.IsPressed;
    }
    
    public int GetCurrentCombo()
    {
        return currentCombo;
    }
    
    public void ResetCombo()
    {
        currentCombo = 0;
    }
    
    #endregion
    
    #region 输入缓冲
    
    struct BufferedInput
    {
        public bool IsPressed;
        public float Timer;
        
        public void Press()
        {
            IsPressed = true;
            Timer = 0f;
        }
        
        public void Update(float deltaTime, float bufferTime)
        {
            if (IsPressed)
            {
                Timer += deltaTime;
                if (Timer >= bufferTime)
                {
                    IsPressed = false;
                    Timer = 0f;
                }
            }
        }
    }
    
    #endregion
    
    #region 调试
    
    void OnGUI()
    {
        if (!showInputDebug) return;
        
        GUILayout.BeginArea(new Rect(10, 250, 300, 200));
        GUILayout.Label("=== 输入调试 ===");
        GUILayout.Label($"移动：{moveInput}");
        GUILayout.Label($"冲刺：{sprintHeld}");
        GUILayout.Label($"抓取：{grabPressed}");
        GUILayout.Label($"缓冲抓取：{bufferedGrab.IsPressed}");
        GUILayout.Label($"连击：{currentCombo}");
        GUILayout.EndArea();
    }
    
    #endregion
}
