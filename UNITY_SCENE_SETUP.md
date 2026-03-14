# 🦞 龙虾快递员 - Unity 场景搭建指南

**版本:** v1.0  
**适用 Unity 版本:** 2022.3 LTS 或更高

---

## 📋 方法一：自动搭建（推荐）

### 步骤 1: 打开 Unity 项目

```
1. 启动 Unity Hub
2. Add → 选择 LobsterCourier 文件夹
3. 点击项目打开
```

### 步骤 2: 打开场景搭建工具

```
菜单栏 → Lobster Courier → 自动搭建场景
```

### 步骤 3: 创建主游戏场景

在场景搭建工具窗口中：

1. **点击 "🎬 创建主游戏场景"**
   - 自动创建 GameScene.unity
   - 包含所有管理器、UI、玩家、环境

2. **或点击 "🏠 创建主菜单场景"**
   - 自动创建 MainMenu.unity
   - 包含主菜单 UI

### 步骤 4: 保存场景

```
File → Save As → Assets/Scenes/GameScene.unity
```

---

## 📋 方法二：手动搭建

### 1. 创建场景目录

```
1. 在 Project 窗口右键
2. Create → Folder
3. 命名为 "Scenes"
```

### 2. 创建主场景

```
1. File → New Scene → 2D
2. File → Save As → Assets/Scenes/GameScene.unity
```

### 3. 创建玩家对象

```
1. 右键 Hierarchy → 2D Object → Sprites → Square
2. 重命名为 "Player"
3. 设置位置：(0, 0, 0)
4. 设置 Tag 为 "Player"
5. 添加组件:
   - Transform (已有)
   - SpriteRenderer (已有)
     * Color: 红色 (临时占位)
   - Rigidbody2D
     * Gravity Scale: 0
     * Collision Detection: Continuous
   - CircleCollider2D
     * Radius: 0.5
   - LobsterController (脚本)
   - Animator (可选)
6. 创建子对象 "HoldPoint"
   - 位置：(0.8, 0, 0)
```

### 4. 创建摄像机

```
1. 选择 Main Camera
2. 设置:
   - Projection: Orthographic
   - Size: 10
   - Background: RGB(26, 77, 128) 海底蓝
3. 添加组件:
   - CameraFollow (脚本)
     * Target: 拖入 Player 对象
     * Follow Speed: 5
     * Look Ahead Distance: 2
     * World Size: 100
```

### 5. 创建游戏管理器

```
1. 右键 Hierarchy → Create Empty
2. 重命名为 "GameManager"
3. 添加组件:
   - GameManager (脚本)
4. 配置引用:
   - Player: 拖入 Player 对象
   - UI 引用：创建 Canvas 后拖入
```

### 6. 创建所有管理器

创建以下空对象并添加对应脚本：

```
- OceanCurrentManager → OceanCurrentManager
- LevelGenerator → LevelGenerator
- PackageSpawner → PackageSpawner
- SaveSystem → SaveSystem
- AchievementSystem → AchievementSystem
- AudioManager → AudioManager
- ParticleManager → ParticleManager
- ComboSystem → ComboSystem
- TutorialSystem → TutorialSystem
- DifficultyManager → DifficultyManager
- GameEndSystem → GameEndSystem
- WeatherSystem → WeatherSystem
- DailyChallenge → DailyChallenge
```

### 7. 创建 UI Canvas

```
1. 右键 Hierarchy → UI → Canvas
2. 设置 Canvas Scaler:
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920 x 1080
3. 创建子对象:

HUD:
├── MoneyText (TextMeshPro)
│   - 位置：(-800, 500)
│   - 文本："💰 $0"
│   - 字号：36
│   - 颜色：黄色
├── ReputationText (TextMeshPro)
│   - 位置：(-800, 460)
│   - 文本："⭐ 0"
│   - 字号：36
├── OrderCountText (TextMeshPro)
│   - 位置：(800, 500)
│   - 文本："📦 0"
│   - 字号：36
├── ComboText (TextMeshPro)
│   - 位置：(0, 500)
│   - 文本：""
│   - 字号：48
│   - 颜色：橙色
└── StaminaBar
    ├── Background (Image)
    │   - 位置：(0, -500)
    │   - 大小：(300, 30)
    │   - 颜色：灰色
    └── Fill (Image)
        - 类型：Filled
        - 填充方法：Horizontal
        - 颜色：绿色

PausePanel:
├── PauseTitle (TextMeshPro)
│   - 文本："⏸️ 游戏暂停"
│   - 字号：64
├── ResumeButton (Button)
│   - 文本："继续游戏"
│   - 大小：(200, 50)
├── RestartButton (Button)
│   - 文本："重新开始"
│   - 大小：(200, 50)
└── MainMenuButton (Button)
    - 文本："返回主菜单"
    - 大小：(200, 50)

ShopPanel:
├── ShopTitle (TextMeshPro)
│   - 文本："🏪 商店"
│   - 字号：64
├── MoneyText (TextMeshPro)
│   - 文本："💰 $0"
│   - 位置：(700, 500)
├── Content (Vertical Layout Group)
│   - 商品列表容器
└── CloseButton (Button)
    - 文本："关闭"

AchievementPanel:
├── AchievementTitle (TextMeshPro)
│   - 文本："🏆 成就"
│   - 字号：64
├── Content (Vertical Layout Group)
│   - 成就列表容器
└── CloseButton (Button)
    - 文本："关闭"

TutorialPanel:
├── TutorialTitle (TextMeshPro)
│   - 文本："教程标题"
│   - 字号：48
├── TutorialContent (TextMeshPro)
│   - 文本："教程内容"
│   - 字号：32
├── TutorialTip (TextMeshPro)
│   - 文本："提示"
│   - 字号：24
├── NextButton (Button)
│   - 文本："下一步"
│   - 大小：(150, 50)
└── CompleteButton (Button)
    - 文本："完成"
    - 大小：(150, 50)
```

### 8. 创建事件系统

```
1. 右键 Hierarchy → UI → Event System
2. 确保包含:
   - EventSystem (组件)
   - Standalone Input Module (组件)
```

### 9. 创建环境

```
1. 创建背景:
   - 右键 Hierarchy → 2D Object → Sprites → Square
   - 重命名为 "Background"
   - 缩放：(200, 200, 1)
   - SpriteRenderer Color: RGB(26, 77, 128)
   - Sorting Order: -10

2. 创建光源:
   - 右键 Hierarchy → Light → Directional Light
   - 颜色：淡蓝色 RGB(204, 230, 255)
   - 强度：0.8
   - 旋转：(50, -30, 0)

3. 设置环境光:
   - Window → Rendering → Lighting
   - Environment → Ambient Color: RGB(51, 102, 153)
   - 启用 Fog:
     * Color: RGB(26, 77, 128)
     * Mode: Linear
     * Start: 20
     * End: 80
```

### 10. 创建包裹预制体

```
1. 右键 Hierarchy → 2D Object → Sprites → Square
2. 重命名为 "Package"
3. 设置 Tag 为 "Package"
4. 添加组件:
   - SpriteRenderer
     * Color: 棕色 (临时)
   - Rigidbody2D
     * Gravity Scale: 0
   - CircleCollider2D
     * Is Trigger: ✓
   - Package (脚本)
5. 拖入 Prefabs 文件夹创建预制体
6. 在场景中生成 5 个测试包裹
```

### 11. 创建送货点预制体

```
1. 右键 Hierarchy → 2D Object → Sprites → Circle
2. 重命名为 "DeliveryPoint"
3. 设置 Tag 为 "DeliveryPoint"
4. 添加组件:
   - CircleCollider2D
     * Is Trigger: ✓
   - DeliveryPoint (脚本)
5. 拖入 Prefabs 文件夹创建预制体
6. 在场景中生成 8 个送货点
```

### 12. 创建游戏配置资产

```
1. 右键 Project 窗口 → Create → Folder
2. 命名为 "Resources"
3. 右键 Resources → Create → Lobster Courier → Game Config
4. 保存为 "GameConfig.asset"
5. 配置数值:
   - Base Move Speed: 5
   - Sprint Multiplier: 2
   - Base Max Stamina: 100
   - Starting Money: 100
   - World Size: 100
   - Obstacle Count: 20
   - Delivery Point Count: 8
```

### 13. 添加场景到 Build Settings

```
1. File → Build Settings
2. 点击 "Add Open Scenes"
3. 确保包含:
   - GameScene
   - MainMenu (如果已创建)
4. 设置 GameScene 为场景 0
```

---

## ✅ 场景检查清单

### 玩家对象
- [ ] Player 对象已创建
- [ ] Tag 设置为 "Player"
- [ ] Rigidbody2D (Gravity Scale = 0)
- [ ] CircleCollider2D
- [ ] LobsterController 脚本
- [ ] HoldPoint 子对象

### 摄像机
- [ ] Main Camera 存在
- [ ] Orthographic 模式
- [ ] Size = 10
- [ ] CameraFollow 脚本
- [ ] Target 指向 Player

### 管理器
- [ ] GameManager
- [ ] OceanCurrentManager
- [ ] LevelGenerator
- [ ] PackageSpawner
- [ ] SaveSystem
- [ ] AchievementSystem
- [ ] AudioManager
- [ ] ParticleManager
- [ ] ComboSystem
- [ ] TutorialSystem
- [ ] DifficultyManager
- [ ] GameEndSystem
- [ ] WeatherSystem
- [ ] DailyChallenge

### UI
- [ ] Canvas (Screen Space Overlay)
- [ ] Canvas Scaler (1920x1080)
- [ ] Event System
- [ ] HUD (金钱/声誉/订单/连击/体力条)
- [ ] PausePanel
- [ ] ShopPanel
- [ ] AchievementPanel
- [ ] TutorialPanel

### 环境
- [ ] Background (海底蓝色)
- [ ] Directional Light
- [ ] Fog 设置

### 预制体
- [ ] Package 预制体
- [ ] DeliveryPoint 预制体
- [ ] Player 预制体 (可选)

### 配置
- [ ] GameConfig.asset (Resources 文件夹)
- [ ] 场景已添加到 Build Settings

---

## 🎮 测试运行

### 步骤 1: 保存所有
```
File → Save (Ctrl+S)
File → Save Project
```

### 步骤 2: 进入 Play 模式
```
点击 Unity 编辑器顶部的 ▶️ Play 按钮
```

### 步骤 3: 测试操作
```
- W/A/S/D 或方向键：移动
- Shift：冲刺
- E 或空格：拾取包裹
- P 或 ESC：暂停
- B：打开商店
- A：打开成就
- M：小地图开关
```

### 步骤 4: 检查控制台
```
Window → General → Console
确保没有红色错误
```

---

## 🐛 常见问题

### Q: 玩家不移动？
**A:** 检查:
- Rigidbody2D 的 Gravity Scale 是否为 0
- Input Manager 中的轴配置
- LobsterController 脚本是否附加

### Q: 摄像机不跟随？
**A:** 检查:
- CameraFollow 的 Target 是否指向 Player
- Player 的 Tag 是否为 "Player"

### Q: UI 不显示？
**A:** 检查:
- Canvas 的 Render Mode 是否为 Screen Space Overlay
- UI 元素是否在 Canvas 下
- Canvas Scaler 配置是否正确

### Q: 包裹无法拾取？
**A:** 检查:
- Package 的 Tag 是否为 "Package"
- CircleCollider2D 的 Is Trigger 是否勾选
- 玩家和包裹的 Layer 设置

### Q: 脚本编译错误？
**A:** 检查:
- 是否安装了 TextMeshPro
- 所有脚本是否在 Assets/Scripts 文件夹
- Unity 版本是否为 2022.3 LTS 或更高

---

## 📦 下一步

场景搭建完成后：

1. **添加美术资源**
   - 替换玩家精灵（红色方块 → 龙虾）
   - 替换包裹图标
   - 添加海洋生物 NPC
   - 添加海底背景

2. **添加音效**
   - BGM
   - 音效文件

3. **创建粒子效果**
   - 交付特效
   - 拾取特效
   - 金币特效

4. **测试游戏流程**
   - 教程
   - 送货循环
   - 商店
   - 成就

---

## 🎯 场景预览

```
GameScene.unity
├── Main Camera (CameraFollow)
├── Directional Light
├── Background
├── Player (LobsterController)
│   └── HoldPoint
├── Canvas
│   ├── HUD
│   ├── PausePanel
│   ├── ShopPanel
│   ├── AchievementPanel
│   └── TutorialPanel
├── EventSystem
├── GameManager
├── OceanCurrentManager
├── LevelGenerator
├── PackageSpawner
├── SaveSystem
├── AchievementSystem
├── AudioManager
├── ParticleManager
├── ComboSystem
├── TutorialSystem
├── DifficultyManager
├── GameEndSystem
├── WeatherSystem
└── DailyChallenge
```

---

**场景搭建完成！现在可以开始测试游戏了！** 🦞🌊

---

**最后更新:** 2026-03-14  
**适用版本:** Unity 2022.3 LTS+
