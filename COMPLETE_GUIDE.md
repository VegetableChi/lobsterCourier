# 🦞 龙虾快递员 - 完整开发指南

**版本:** v0.5.0 Alpha  
**最后更新:** 2026-03-14

---

## 📁 项目结构

```
LobsterCourier/
├── Assets/
│   ├── Scripts/              # C# 脚本 (22 个文件)
│   │   ├── Core/             # 核心系统
│   │   │   ├── LobsterController.cs    # 玩家控制
│   │   │   ├── GameManager.cs          # 游戏管理
│   │   │   ├── ComboSystem.cs          # 连击系统
│   │   │   └── GameConfig.cs           # 游戏配置
│   │   ├── World/            # 世界系统
│   │   │   ├── OceanCurrentManager.cs  # 洋流系统
│   │   │   ├── LevelGenerator.cs       # 关卡生成
│   │   │   ├── PackageSpawner.cs       # 包裹生成
│   │   │   └── CameraFollow.cs         # 摄像机
│   │   ├── Gameplay/         # 玩法系统
│   │   │   ├── Package.cs              # 包裹系统
│   │   │   ├── DeliveryPoint.cs        # 送货点
│   │   │   └── ShopSystem.cs           # 商店系统
│   │   ├── UI/               # UI 系统
│   │   │   ├── StaminaBar.cs           # 体力条
│   │   │   ├── ShopUI.cs               # 商店 UI
│   │   │   ├── ShopItemUI.cs           # 商品 UI
│   │   │   ├── OrderUI.cs              # 订单 UI
│   │   │   ├── OrderItemUI.cs          # 订单物品
│   │   │   ├── AchievementUI.cs        # 成就 UI
│   │   │   ├── AchievementItemUI.cs    # 成就物品
│   │   │   ├── MainMenu.cs             # 主菜单
│   │   │   └── PauseMenu.cs            # 暂停菜单
│   │   └── Systems/          # 辅助系统
│   │       ├── SaveSystem.cs           # 存档系统
│   │       ├── AchievementSystem.cs    # 成就管理
│   │       ├── AudioManager.cs         # 音频管理
│   │       └── ParticleManager.cs      # 粒子效果
│   ├── Scenes/               # Unity 场景
│   ├── Prefabs/              # 预制体
│   ├── Sprites/              # 美术资源
│   ├── Audio/                # 音效资源
│   └── Materials/            # 材质资源
├── ProjectSettings/          # Unity 项目设置
├── README.md                 # 项目介绍
├── SETUP_GUIDE.md            # 安装指南
├── PROJECT_STATUS.md         # 开发状态
└── COMPLETE_GUIDE.md         # 本文件
```

---

## 🚀 快速开始 (5 分钟)

### 步骤 1: 打开 Unity
```
1. 启动 Unity Hub
2. Add → 选择 LobsterCourier 文件夹
3. 点击项目打开
```

### 步骤 2: 创建场景
```
1. File → New Scene → 2D
2. 保存为 Assets/Scenes/GameScene.unity
```

### 步骤 3: 创建玩家
```
1. 右键 Hierarchy → 2D Object → Sprites → Square
2. 重命名为 "Player"
3. 添加组件:
   - Rigidbody2D (Gravity Scale = 0)
   - CircleCollider2D (Radius = 0.5)
   - LobsterController
   - Tag 设为 "Player"
```

### 步骤 4: 创建管理器
```
创建以下空对象并添加对应脚本:
- GameManager → GameManager
- OceanCurrentManager → OceanCurrentManager
- LevelGenerator → LevelGenerator
- SaveSystem → SaveSystem
- AchievementSystem → AchievementSystem
- AudioManager → AudioManager
- ParticleManager → ParticleManager
- ComboSystem → ComboSystem
```

### 步骤 5: 创建 UI
```
1. 右键 Hierarchy → UI → Canvas
2. 创建子对象:
   - MoneyText (TextMeshPro)
   - ReputationText (TextMeshPro)
   - StaminaBar (Image + Fill)
   - ComboText (TextMeshPro)
```

### 步骤 6: 运行
```
点击 Play 按钮开始游戏！
```

---

## 🎮 游戏操作

### 键盘控制
| 按键 | 功能 |
|------|------|
| W/A/S/D | 移动 |
| Shift | 冲刺 |
| E/空格 | 抓取包裹 |
| P/ESC | 暂停 |
| B | 打开商店 |
| A | 打开成就 |
| M | 小地图开关 |
| R | 重新开始 (游戏结束时) |

### 手柄支持 (待实现)
| 按钮 | 功能 |
|------|------|
| 左摇杆 | 移动 |
| A | 冲刺 |
| B | 抓取 |
| Start | 暂停 |

---

## 📋 核心系统说明

### 1. 玩家系统 (LobsterController)

**属性:**
- `moveSpeed`: 移动速度 (默认 5)
- `maxStamina`: 最大体力 (默认 100)
- `staminaDrainRate`: 体力消耗 (默认 20/秒)
- `staminaRegenRate`: 体力恢复 (默认 10/秒)

**机制:**
- 冲刺消耗体力，体力耗尽无法冲刺
- 可以抓取并携带包裹
- 送到指定位置获得奖励

### 2. 洋流系统 (OceanCurrentManager)

**功能:**
- 动态洋流影响玩家移动
- 顺流加速，逆流减速
- 程序化生成洋流区域

**使用:**
```csharp
Vector2 flow = OceanCurrentManager.Instance.GetCurrentFlow(position);
```

### 3. 经济系统 (GameManager)

**货币:**
- 💰 金币：购买升级和道具
- ⭐ 声誉：解锁新区域和客户

**获取方式:**
- 准时送达包裹
- 连击奖励
- 成就奖励

### 4. 商店系统 (ShopSystem)

**升级类:**
| 升级 | 效果 | 满级 |
|------|------|------|
| 速度升级 | +1 移动速度 | Lv.10 |
| 体力升级 | +20 最大体力 | Lv.10 |
| 背包升级 | +1 携带数量 | Lv.5 |

**消耗品:**
| 道具 | 效果 | 价格 |
|------|------|------|
| 磁力道具 | 30 秒自动吸引包裹 | $25 |
| 无敌冲刺 | 5 秒无敌不耗体力 | $35 |
| 时间减缓 | 10 秒时间流逝减半 | $40 |

### 5. 成就系统 (AchievementSystem)

**12 项成就:**
- 📦 第一单 (完成 1 次送货)
- 🎯 送货达人 (100 次)
- 🏆 传奇快递员 (1000 次)
- 💰 富有的龙虾 (累计$10,000)
- 💎 百万富翁 (累计$1,000,000)
- ⭐ 五星好评 (声誉 100)
- 🔥 完美连击 (5 连击)
- ⚡ 连击大师 (20 连击)
- 💨 速度恶魔 (10 秒内送达)
- 🛡️ 完美无缺 (10 次无损坏)
- 🤝 人脉广泛 (8 种客户)

### 6. 连击系统 (ComboSystem)

**机制:**
- 连续准时送达增加连击数
- 每个连击 +10% 奖励 (最高 +200%)
- 2 分钟内送达算连击
- 超时打断连击

### 7. 存档系统 (SaveSystem)

**保存内容:**
- 金钱和声誉
- 送货统计
- 解锁成就
- 商店升级
- 最高记录

**自动保存:** 每 60 秒

---

## 🎨 美术资源需求

### 必需资源
| 类型 | 数量 | 规格 |
|------|------|------|
| 玩家精灵 | 4 方向 × 3 帧 | 64x64 |
| 包裹图标 | 6 种 | 32x32 |
| NPC 精灵 | 8 种 | 64x64 |
| 背景 | 1 张 (可平铺) | 512x512 |
| 障碍物 | 5 种 | 64x64 |
| UI 图标 | 20+ | 32x32 |

### 推荐资源
| 类型 | 数量 |
|------|------|
| 粒子效果 | 8 种 |
| 动画剪辑 | 10+ |
| UI 背景 | 5+ |

---

## 🔊 音频资源需求

### 背景音乐
| 场景 | 时长 | 风格 |
|------|------|------|
| 主菜单 | 2:00 | 轻松愉快 |
| 游戏内 | 3:00 | 轻快冒险 |
| 紧张时刻 | 1:30 | 快节奏 |

### 音效
| 事件 | 时长 |
|------|------|
| 移动 | 0.1s |
| 冲刺 | 0.3s |
| 拾取 | 0.2s |
| 交付 | 0.5s |
| 购买 | 0.3s |
| 成就 | 1.0s |
| UI 点击 | 0.1s |

---

## ⚙️ 配置说明 (GameConfig)

### 创建配置资产
```
右键 Project 窗口 → Create → Lobster Courier → Game Config
保存为 Assets/Resources/GameConfig.asset
```

### 关键配置项
```yaml
玩家基础数值:
  baseMoveSpeed: 5
  sprintMultiplier: 2
  baseMaxStamina: 100
  
经济系统:
  startingMoney: 100
  basePackageValue: 10
  fragileValueMultiplier: 1.5
  
生成配置:
  worldSize: 100
  obstacleCount: 20
  deliveryPointCount: 8
```

---

## 🐛 调试技巧

### 控制台命令 (开发用)
```csharp
// 添加金钱
GameManager.Instance.playerMoney += 1000;

// 解锁所有成就
AchievementSystem.Instance.UnlockAchievement("all");

// 重置连击
ComboSystem.Instance.ResetCombo();

// 强制保存
SaveSystem.Instance.SaveGame();
```

### 常用调试
```csharp
// 显示 FPS
Debug.Log($"FPS: {1f/Time.unscaledDeltaTime}");

// 检查碰撞体
Debug.DrawRay(transform.position, Vector2.up * 2, Color.red);

// 性能分析
Profiler.BeginSample("MethodName");
// ... 代码 ...
Profiler.EndSample();
```

---

## 📦 发布流程

### 1. PC 发布
```
File → Build Settings
→ 添加 GameScene 和 MainMenu
→ Platform: Windows/Mac/Linux
→ Build
```

### 2. 移动端 (待实现)
```
Platform: iOS/Android
→ 适配触摸控制
→ 优化 UI 布局
```

### 3. 主机 (待实现)
```
Platform: Switch/PS/Xbox
→ 适配手柄
→ 性能优化
```

---

## 📈 性能优化建议

### 已实现
- ✅ 粒子对象池
- ✅ ScriptableObject 配置
- ✅ 事件系统

### 推荐
- [ ] 包裹对象池
- [ ] 异步场景加载
- [ ] 纹理压缩
- [ ] 音频压缩

---

## 🎯 后续开发计划

### v0.5.1 (Bug 修复)
- [ ] 修复已知问题
- [ ] 平衡数值
- [ ] 添加教程

### v0.6 (内容扩展)
- [ ] 新海洋生物
- [ ] 新包裹类型
- [ ] 天气系统

### v0.8 (Beta)
- [ ] 公开测试
- [ ] 收集反馈
- [ ] 性能优化

### v1.0 (发布)
- [ ] 多平台发布
- [ ] 营销材料
- [ ] 商店上线

---

## 🙏 致谢

感谢所有参与开发的贡献者！

---

## 📞 联系方式

- 项目发起：坨坨虾 🍤
- 版本：v0.5.0 Alpha
- 许可证：MIT

---

**Slogan:** *钳子在手，说走就走——海底最快外卖员！* 🦞🌊
