# 🦞 龙虾快递员 - Unity 设置指南

---

## 📋 第一步：创建 Unity 项目

### 1.1 打开 Unity Hub
```
1. 启动 Unity Hub
2. 点击 "New Project"
3. 选择 "2D Core" 模板
4. 项目名称：LobsterCourier
5. 选择保存位置
6. 点击 "Create Project"
```

### 1.2 导入脚本
将 `Assets/Scripts/` 文件夹中的所有 `.cs` 文件复制到 Unity 项目的对应目录。

---

## 🎮 第二步：设置场景

### 2.1 创建主场景
```
1. File → New Scene → 2D
2. 保存为 Assets/Scenes/MainScene.unity
```

### 2.2 创建玩家对象

**创建龙虾玩家：**
```
1. 右键 Hierarchy → 2D Object → Sprites → Square
2. 重命名为 "Player"
3. 添加组件：
   - Rigidbody2D (Gravity Scale = 0)
   - CircleCollider2D (Radius = 0.5)
   - LobsterController (脚本)
   - Animator (可选)
4. 设置 Tag 为 "Player"
```

**配置 LobsterController：**
```
Move Speed: 5
Sprint Multiplier: 2
Max Stamina: 100
Stamina Drain Rate: 20
Stamina Regen Rate: 10
Hold Point: 创建一个空子对象，放在玩家前方
```

### 2.3 创建摄像机

**设置主摄像机：**
```
1. 选择 Main Camera
2. 添加组件：CameraFollow
3. 配置：
   - Target: 拖入 Player 对象
   - Follow Speed: 5
   - Look Ahead Distance: 2
   - World Size: 100
   - Offset: (0, 0, -10)
```

### 2.4 创建游戏管理器

**创建 GameManager 对象：**
```
1. 右键 Hierarchy → Create Empty
2. 重命名为 "GameManager"
3. 添加组件：GameManager (脚本)
4. 配置引用：
   - Player: 拖入 Player 对象
   - UI 引用：创建 Canvas 后拖入对应 UI 元素
```

### 2.5 创建洋流管理器

**创建 OceanCurrentManager 对象：**
```
1. 右键 Hierarchy → Create Empty
2. 重命名为 "OceanCurrentManager"
3. 添加组件：OceanCurrentManager
```

### 2.6 创建关卡生成器

**创建 LevelGenerator 对象：**
```
1. 右键 Hierarchy → Create Empty
2. 重命名为 "LevelGenerator"
3. 添加组件：LevelGenerator
4. 配置：
   - World Size: 100
   - Obstacle Count: 20
   - Delivery Point Count: 8
```

---

## 🎨 第三步：创建 UI

### 3.1 创建 Canvas
```
1. 右键 Hierarchy → UI → Canvas
2. Canvas Scaler: Scale With Screen Size
   - Reference Resolution: 1920 x 1080
```

### 3.2 创建 UI 元素

**体力条：**
```
1. 右键 Canvas → UI → Image
2. 重命名为 "StaminaBar"
3. 添加子对象 "Fill" (Image)
4. 添加组件：StaminaBar
5. 配置：
   - Fill Image: 拖入 Fill 子对象
   - Full Color: 绿色
   - Low Color: 黄色
   - Empty Color: 红色
```

**金钱显示：**
```
1. 右键 Canvas → UI → Text - TextMeshPro
2. 重命名为 "MoneyText"
3. 位置：右上角
4. 文本："$0"
```

**声誉显示：**
```
1. 右键 Canvas → UI → Text - TextMeshPro
2. 重命名为 "ReputationText"
3. 位置：右上角，金钱下方
4. 文本："⭐ 0"
```

**订单计数：**
```
1. 右键 Canvas → UI → Text - TextMeshPro
2. 重命名为 "OrderCountText"
3. 位置：左上角
4. 文本："📦 0"
```

---

## 🎯 第四步：创建游戏对象

### 4.1 创建包裹预制体

```
1. 右键 Hierarchy → 2D Object → Sprites → Square
2. 重命名为 "Package"
3. 添加组件：
   - Rigidbody2D (Gravity Scale = 0)
   - CircleCollider2D
   - Package (脚本)
4. 设置 Tag 为 "Package"
5. 拖入 Prefabs 文件夹创建预制体
6. 删除场景中的对象
```

### 4.2 创建送货点预制体

```
1. 右键 Hierarchy → 2D Object → Sprites → Circle
2. 重命名为 "DeliveryPoint"
3. 添加组件：
   - CircleCollider2D (Is Trigger = true)
   - DeliveryPoint (脚本)
4. 设置 Tag 为 "DeliveryPoint"
5. 拖入 Prefabs 文件夹创建预制体
6. 删除场景中的对象
```

### 4.3 创建障碍物预制体

```
1. 创建多个障碍物变体（珊瑚、岩石、沉船等）
2. 添加 CircleCollider2D 或 PolygonCollider2D
3. 拖入 Prefabs 文件夹
```

---

## ⚙️ 第五步：输入设置

### 5.1 创建 Input Manager 配置

打开 `Edit → Project Settings → Input Manager`

**添加新轴：**
```
Axis 1: Horizontal
- Positive Button: right
- Negative Button: left
- Type: Key or Mouse Button

Axis 2: Vertical
- Positive Button: up
- Negative Button: down
- Type: Key or Mouse Button

Button 1: Sprint
- Positive Button: left shift
- Type: Key or Mouse Button

Button 2: Grab
- Positive Button: e
- Type: Key or Mouse Button
```

### 5.2 或使用新 Input System

如果 Unity 版本支持，可以安装 Input System 包并创建 Input Action Asset。

---

## 🧪 第六步：测试游戏

### 6.1 运行测试
```
1. 保存所有场景和预制体
2. 点击 Unity 编辑器的 Play 按钮
3. 使用 WASD 或方向键移动
4. 按 Shift 冲刺
5. 按 E 抓取包裹
```

### 6.2 调试
```
- 打开 Console 窗口查看日志
- 使用 Gizmos 查看洋流区域和生成点
- 检查碰撞体是否正确配置
```

---

## 🐛 常见问题

### Q: 玩家不移动？
A: 检查 Rigidbody2D 的 Gravity Scale 是否为 0，检查输入轴配置。

### Q: 摄像机不跟随？
A: 确保 CameraFollow 的 Target 已正确赋值。

### Q: 包裹无法拾取？
A: 检查 Package 的 Tag 是否为 "Package"，检查碰撞体配置。

### Q: 体力条不更新？
A: 确保 GameManager 的 staminaBar 引用已正确赋值。

---

## 📦 下一步

1. 添加美术资源（精灵图、动画）
2. 添加音效和背景音乐
3. 完善订单系统
4. 添加商店和升级系统
5. 创建更多关卡和生物
6. 测试并发布

---

**祝开发顺利！** 🦞🌊
