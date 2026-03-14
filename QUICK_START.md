# 🦞 龙虾快递员 - 5 分钟快速启动指南

**目标:** 5 分钟内在 Unity 中搭建可运行的场景

---

## ⚡ 超快速启动 (3 步)

### 步骤 1: 打开 Unity 项目

```bash
# 1. 启动 Unity Hub
# 2. Add → 选择 LobsterCourier 文件夹
# 3. 点击项目打开
```

### 步骤 2: 运行自动搭建工具

```
菜单栏 → Lobster Courier → 自动搭建场景

点击 "🎬 创建主游戏场景"
等待 10 秒...
✅ 场景创建完成！
```

### 步骤 3: 运行游戏

```
1. 保存场景 (Ctrl+S)
2. 点击 ▶️ Play 按钮
3. 按 W/A/S/D 移动测试
```

**完成！** 🎉 现在你有一个可运行的游戏场景了！

---

## 📋 完整启动流程 (10 分钟)

### 1. 创建场景 (2 分钟)

```
菜单栏 → Lobster Courier → 自动搭建场景

✓ 点击 "🎬 创建主游戏场景"
✓ 点击 "🏠 创建主菜单场景"
✓ 关闭工具窗口
```

### 2. 创建预制体 (1 分钟)

```
菜单栏 → Lobster Courier → 创建预制体 → 所有基础预制体

✓ Player
✓ Package
✓ DeliveryPoint
✓ Obstacle
✓ Decoration
```

### 3. 创建游戏配置 (1 分钟)

```
1. 右键 Project 窗口 → Create → Folder
2. 命名为 "Resources"
3. 右键 Resources → Create → Lobster Courier → Game Config
4. 保存为 "GameConfig.asset"
5. 使用默认数值即可
```

### 4. 添加场景到 Build (1 分钟)

```
1. File → Build Settings
2. 点击 "Add Open Scenes" (添加 GameScene)
3. 打开 Assets/Scenes/MainMenu.unity
4. 点击 "Add Open Scenes" (添加 MainMenu)
5. 确保 GameScene 在场景 0
```

### 5. 测试运行 (5 分钟)

```
1. 打开 GameScene
2. 保存所有 (Ctrl+S)
3. 点击 ▶️ Play
4. 测试操作:
   - W/A/S/D: 移动
   - Shift: 冲刺
   - E: 拾取包裹
   - P: 暂停
   - B: 商店
   - A: 成就
```

---

## ✅ 检查清单

运行游戏前，确保：

### 场景结构
- [ ] Main Camera 存在
- [ ] Player 对象存在
- [ ] Canvas 存在
- [ ] EventSystem 存在

### 管理器
- [ ] GameManager 存在
- [ ] 其他管理器已创建（自动工具会创建）

### UI
- [ ] Canvas 存在
- [ ] MoneyText 存在
- [ ] StaminaBar 存在

### 配置
- [ ] GameConfig.asset 在 Resources 文件夹
- [ ] 场景已添加到 Build Settings

---

## 🎮 操作说明

| 按键 | 功能 |
|------|------|
| W/A/S/D | 移动 |
| Shift | 冲刺 |
| E/空格 | 拾取包裹 |
| P/ESC | 暂停 |
| B | 商店 |
| A | 成就 |
| M | 小地图 |

---

## 🐛 快速故障排除

### 问题：Play 后玩家不移动

**解决:**
```
1. 检查 Player 是否有 Rigidbody2D
2. 确保 Gravity Scale = 0
3. 检查 Input Manager 配置
```

### 问题：控制台有红色错误

**解决:**
```
1. 确保安装了 TextMeshPro
2. 检查所有脚本在 Assets/Scripts 文件夹
3. 重启 Unity
```

### 问题：UI 不显示

**解决:**
```
1. 检查 Canvas Render Mode = Screen Space Overlay
2. 确保 UI 元素在 Canvas 下
3. 检查 Canvas Scaler 配置
```

### 问题：场景是空的

**解决:**
```
1. 重新运行自动搭建工具
2. 或手动按照 UNITY_SCENE_SETUP.md 搭建
```

---

## 📦 下一步

场景运行正常后：

### 1. 替换美术资源
```
• 玩家：红色方块 → 龙虾精灵
• 包裹：棕色方块 → 包裹图标
• 背景：蓝色方块 → 海底背景
```

### 2. 添加音效
```
• BGM 文件
• 音效文件
```

### 3. 测试完整流程
```
• 教程
• 送货循环
• 商店购买
• 成就解锁
```

---

## 📞 需要帮助？

查看详细文档：

| 文档 | 内容 |
|------|------|
| `UNITY_SCENE_SETUP.md` | 详细场景搭建步骤 |
| `COMPLETE_GUIDE.md` | 完整开发指南 |
| `README.md` | 项目介绍 |

---

**5 分钟完成！开始你的海底快递之旅吧！** 🦞🌊

---

**最后更新:** 2026-03-14  
**适用版本:** Unity 2022.3 LTS+
