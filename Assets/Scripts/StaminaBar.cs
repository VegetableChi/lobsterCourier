using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 体力条 UI
/// </summary>
public class StaminaBar : MonoBehaviour
{
    [Header("UI 引用")]
    public Image fillImage;
    public Image background;
    
    [Header("颜色设置")]
    public Color fullColor = Color.green;
    public Color lowColor = Color.yellow;
    public Color emptyColor = Color.red;
    
    [Header("动画")]
    public float colorLerpSpeed = 5f;
    public float fillLerpSpeed = 10f;
    
    private float currentFill = 1f;
    private Color targetColor;
    
    void Start()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = 1f;
            targetColor = fullColor;
        }
    }
    
    public void SetStamina(float percent)
    {
        currentFill = Mathf.Clamp01(percent);
        
        // 根据体力值设置目标颜色
        if (currentFill > 0.6f)
        {
            targetColor = fullColor;
        }
        else if (currentFill > 0.3f)
        {
            targetColor = lowColor;
        }
        else
        {
            targetColor = emptyColor;
        }
    }
    
    void Update()
    {
        if (fillImage == null) return;
        
        // 平滑填充
        fillImage.fillAmount = Mathf.Lerp(
            fillImage.fillAmount, 
            currentFill, 
            fillLerpSpeed * Time.deltaTime
        );
        
        // 平滑颜色
        fillImage.color = Color.Lerp(
            fillImage.color, 
            targetColor, 
            colorLerpSpeed * Time.deltaTime
        );
        
        // 体力耗尽时闪烁
        if (currentFill <= 0)
        {
            FlashExhausted();
        }
    }
    
    void FlashExhausted()
    {
        float flashSpeed = 5f;
        float alpha = (Mathf.Sin(Time.time * flashSpeed) + 1) / 2f;
        Color flashColor = fillImage.color;
        flashColor.a = 0.5f + alpha * 0.5f;
        fillImage.color = flashColor;
    }
}
