using UnityEngine;
using UnityEditor;

/// <summary>
/// Input 设置工具 - 自动配置 Input Manager
/// </summary>
public class InputSetup : EditorWindow
{
    [MenuItem("Lobster Courier/配置 Input 设置")]
    public static void SetupInput()
    {
        Debug.Log("=== 配置 Input 设置 ===");
        Debug.Log("请手动配置 Input Manager:");
        Debug.Log("1. Edit → Project Settings → Input Manager");
        Debug.Log("2. 展开 Axes 数组");
        Debug.Log("3. 确保有以下输入轴:");
        Debug.Log("   - Horizontal (A/D)");
        Debug.Log("   - Vertical (W/S)");
        Debug.Log("   - Sprint (Left Shift)");
        Debug.Log("   - Grab (E)");
        
        EditorUtility.DisplayDialog(
            "Input 配置",
            "请手动配置 Input Manager:\n\n" +
            "1. Edit → Project Settings → Input Manager\n" +
            "2. 展开 Axes 数组\n" +
            "3. 添加以下输入轴:\n" +
            "   - Horizontal (A/D)\n" +
            "   - Vertical (W/S)\n" +
            "   - Sprint (Left Shift)\n" +
            "   - Grab (E 或 Space)",
            "确定");
    }
}
