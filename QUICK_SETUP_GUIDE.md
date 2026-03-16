# 🦞 龙虾快递员 - 快速设置指南

**版本:** v1.0  
**更新日期:** 2026-03-16  
**目标:** 5 分钟内完成场景配置

---

## 📋 前置条件

- Unity 2021.3+ (2D 项目)
- 已导入所有美术资源 (Assets/Sprites/)
- 已导入所有脚本 (Assets/Scripts/)

---

## 🚀 一键设置步骤

### 步骤 1: 创建主场景

1. 在 Unity 中创建新场景：`File → New Scene → 2D`
2. 保存场景为：`Assets/Scenes/GameScene.unity`

### 步骤 2: 添加游戏管理器

1. 在 Hierarchy 中右键 → `Create Empty`
2. 命名为 `GameManager`
3. 添加组件：`GameManager` (拖入 Assets/Scripts/GameManager.cs)
4. 添加组件：`AudioManager` (拖入 Assets/Scripts/AudioManager.cs)

### 步骤 3: 添加玩家

1. 在 Hierarchy 中右键 → `2D Object → Sprites → Square`
2. 命名为 `Player`
3. 位置设置为 `(0, 0, 0)`
4. 添加组件：
   - `LobsterController` (Assets/Scripts/LobsterController.cs)
   - `CharacterAnimatorConfig` (Assets/Scripts/Character/CharacterAnimatorConfig.cs)
   - `Rigidbody2D` (Gravity Scale = 0)
   - `CircleCollider2D`

5. 配置 CharacterAnimatorConfig:
   - Character Type: **Lobster**
   - 展开 Idle Sprites，拖入:
     - `Sprites/characters/lobster_idle_0`
     - `Sprites/characters/lobster_idle_1`
     - `Sprites/characters/lobster_idle_2`

6. 创建 HoldPoint:
   - 在 Player 下创建空对象 `HoldPoint`
   - 位置设置为 `(1, 0, 0)`
   - 拖入 LobsterController 的 Hold Point 字段

### 步骤 4: 配置相机

1. 选择 Main Camera
2. 设置:
   - Projection: **Orthographic**
   - Size: **10**
   - Background: **R:74 G:144 B:200** (海洋蓝)

3. 添加组件：`CameraFollow` (Assets/Scripts/CameraFollow.cs)
4. 配置 CameraFollow:
   - Target: 拖入 Player 对象
   - Smooth Speed: **5**

### 步骤 5: 设置场景装饰

1. 在 Hierarchy 中创建空对象 `SceneManager`
2. 添加组件：`GameSceneSetup` (Assets/Scripts/Scene/GameSceneSetup.cs)
3. 添加组件：`SceneDecorator` (Assets/Scripts/Scene/SceneDecorator.cs)

4. 配置 GameSceneSetup:
   - Scene Theme: **ShallowSea**
   - Generate Decorations: ✅
   - Enable Particles: ✅
   - Spawn Area: **(40, 20)**
   - Obstacle Count: **15**
   - Decoration Count: **20**

### 步骤 6: 配置 UI

1. 在 Hierarchy 中右键 → `UI → Canvas`
2. 设置 Canvas Scaler:
   - UI Scale Mode: **Scale With Screen Size**
   - Reference Resolution: **1920 x 1080**

3. 在 Canvas 下创建空对象 `UIManager`
4. 添加组件：`GameUIManager` (Assets/Scripts/UI/GameUIManager.cs)

5. 创建 UI 面板 (可选):
   - 右键 Canvas → `UI → Panel` 创建主菜单
   - 使用 Assets/Sprites/ui/ 中的图片装饰

### 步骤 7: 配置音频

1. 选择 GameManager 对象
2. 找到 AudioManager 组件
3. 点击 `ProceduralAudioGenerator` 脚本会自动生成音效
4. 音量设置:
   - BGM Volume: **0.5**
   - SFX Volume: **0.7**
   - Ambient Volume: **0.3**

### 步骤 8: 添加输入系统

1. 打开 `Edit → Project Settings → Input Manager`
2. 确认以下轴存在:
   - Horizontal (A/D 或←/→)
   - Vertical (W/S 或↑/↓)

3. 添加新按钮:
   - Name: **Sprint**
   - Positive Button: **left shift**
   
   - Name: **Grab**
   - Positive Button: **e**

### 步骤 9: 测试运行

1. 确保 Player 在场景中心 (0, 0)
2. 点击 Unity 的 Play 按钮
3. 测试:
   - WASD 移动龙虾
   - Shift 冲刺
   - E 抓取包裹

---

## 🎨 美术资源配置

### 角色精灵导入设置

选择所有 `Assets/Sprites/characters/*.png`:

```
Texture Type: Sprite (2D and UI)
Sprite Mode: Single
Pixels Per Unit: 64
Filter Mode: Point (no filter)  ← 像素风格
Compression: None
```

### 环境资产导入设置

选择所有 `Assets/Sprites/environment/*.png`:

```
Texture Type: Sprite (2D and UI)
Sprite Mode: Single
Pixels Per Unit: 64
Filter Mode: Bilinear
Compression: Normal Quality
```

### UI 元素导入设置

选择所有 `Assets/Sprites/ui/*.png`:

```
Texture Type: Sprite (2D and UI)
Sprite Mode: Single
Pixels Per Unit: 100
Filter Mode: Bilinear
Compression: High Quality
```

---

## 🔧 常见问题

### Q: 角色不显示？
**A:** 检查:
1. SpriteRenderer 是否添加
2. Sprite 是否赋值
3. Sorting Layer 是否为 Default
4. 相机是否正确配置

### Q: 移动不正常？
**A:** 检查:
1. Rigidbody2D 的 Gravity Scale 是否为 0
2. Input Manager 中轴是否正确
3. LobsterController 的 Move Speed 是否合理 (默认 5)

### Q: 动画不播放？
**A:** 检查:
1. CharacterAnimatorConfig 是否正确配置
2. Idle Sprites 数组是否有 3 个精灵
3. 脚本是否在运行

### Q: 场景是黑色的？
**A:** 检查:
1. Main Camera 的 Background 颜色
2. 是否有光源
3. GameSceneSetup 是否正确配置

---

## 📁 推荐场景层级结构

```
Hierarchy:
├── GameManager
│   ├── GameManager (script)
│   └── AudioManager (script)
├── SceneManager
│   ├── GameSceneSetup (script)
│   └── SceneDecorator (script)
├── Player
│   ├── SpriteRenderer
│   ├── Rigidbody2D
│   ├── CircleCollider2D
│   ├── LobsterController (script)
│   ├── CharacterAnimatorConfig (script)
│   └── HoldPoint
├── Main Camera
│   └── CameraFollow (script)
├── UI Canvas
│   └── UIManager
│       └── GameUIManager (script)
└── Directional Light
```

---

## ⚙️ 性能优化建议

### 1. 对象池
对于频繁生成的物体 (如气泡、包裹)，使用对象池：

```csharp
// 参考 Assets/Scripts/Utilities/ObjectPool.cs
```

### 2. 图层管理
设置合理的 Sorting Layer:
- Background: -10
- Environment: 0
- Player: 10
- UI: 100

### 3. 碰撞优化
- 使用 Layer Collision Matrix 禁用不必要的碰撞检测
- 简单障碍物使用 Trigger 而非物理碰撞

---

## 🎮 游戏控制说明

| 按键 | 功能 |
|------|------|
| W/A/S/D | 移动 |
| Left Shift | 冲刺 (消耗体力) |
| E | 抓取/放下包裹 |
| Esc | 暂停菜单 |
| 鼠标点击 | UI 交互 |

---

## 📞 获取帮助

如遇问题，查看以下文档:
- `README.md` - 项目总览
- `QUICK_START.md` - 快速开始
- `ART_STYLE_GUIDE.md` - 美术规范
- `UI_DESIGN_SYSTEM.md` - UI 设计
- `PROJECT_COMPLETION_SUMMARY.md` - 完成报告

---

**祝开发顺利！** 🦞🍤
