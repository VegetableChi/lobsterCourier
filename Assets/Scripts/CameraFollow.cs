using UnityEngine;

/// <summary>
/// 摄像机跟随 - 平滑跟随玩家
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("目标")]
    public Transform target;
    
    [Header("跟随参数")]
    public float followSpeed = 5f;
    public float lookAheadDistance = 2f;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    
    [Header("边界限制")]
    public bool limitToBounds = true;
    public float worldSize = 100f;
    
    [Header("偏移")]
    public Vector3 offset = new Vector3(0, 0, -10f);
    
    private Camera cam;
    private Vector3 velocity = Vector3.zero;
    
    void Awake()
    {
        cam = GetComponent<Camera>();
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        // 计算目标位置
        Vector3 targetPosition = target.position + offset;
        
        // 添加前瞻
        Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
        if (targetRb != null && targetRb.velocity != Vector2.zero)
        {
            Vector3 lookAhead = (Vector3)targetRb.velocity.normalized * lookAheadDistance;
            targetPosition += lookAhead;
        }
        
        // 平滑跟随
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            1f / followSpeed
        );
        
        // 限制边界
        if (limitToBounds)
        {
            LimitToBounds();
        }
    }
    
    void LimitToBounds()
    {
        float halfSize = worldSize / 2f;
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -halfSize + camWidth, halfSize - camWidth);
        pos.y = Mathf.Clamp(pos.y, -halfSize + camHeight, halfSize - camHeight);
        transform.position = pos;
    }
    
    public void SetZoom(float zoom)
    {
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = zoom;
    }
    
    public void ZoomIn()
    {
        SetZoom(cam.orthographicSize - 1f);
    }
    
    public void ZoomOut()
    {
        SetZoom(cam.orthographicSize + 1f);
    }
}
