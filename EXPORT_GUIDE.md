# 🦞 龙虾快递员 - 项目导出指南

**导出日期:** 2026-03-14  
**项目版本:** v0.9.0

---

## ✅ 已完成的导出

### 导出文件
```
文件名：LobsterCourier_20260314_204702.tar.gz
大小：69KB（压缩后）
位置：/root/.openclaw/workspace/
格式：tar.gz
```

---

## 📦 导出方式

### 方式一：压缩包导出（推荐）

**已完成:**
```bash
cd /root/.openclaw/workspace
tar -czf LobsterCourier_YYYYMMDD_HHMMSS.tar.gz LobsterCourier/
```

**文件位置:**
```
/root/.openclaw/workspace/LobsterCourier_20260314_204702.tar.gz
```

---

### 方式二：直接复制

**项目位置:**
```
/root/.openclaw/workspace/LobsterCourier/
```

**复制命令:**
```bash
# 复制到其他目录
cp -r /root/.openclaw/workspace/LobsterCourier/ /your/target/path/

# 或使用 rsync
rsync -av /root/.openclaw/workspace/LobsterCourier/ /your/target/path/
```

---

### 方式三：Git 仓库

**初始化为 Git 仓库:**
```bash
cd /root/.openclaw/workspace/LobsterCourier/
git init
git add .
git commit -m "Initial commit - Lobster Courier v0.9.0"

# 添加远程仓库（可选）
git remote add origin <your-repo-url>
git push -u origin main
```

---

## 📁 导出内容

### 包含的文件
```
LobsterCourier/
├── Assets/
│   └── Scripts/              # 所有 C# 脚本 (35 个文件)
│       ├── Core/             # 核心系统
│       ├── Editor/           # 编辑器工具
│       ├── Extensions/       # 扩展功能
│       ├── Utilities/        # 工具类
│       └── ...
├── README.md                 # 项目介绍
├── QUICK_START.md            # 快速启动指南
├── UNITY_SCENE_SETUP.md      # 场景搭建指南
├── COMPLETE_GUIDE.md         # 完整开发指南
├── IP_GUIDE.md               # 原创 IP 设定
├── OPTIMIZATION_REPORT.md    # 优化报告
├── 深度优化报告.md           # 深度优化报告
├── 开发完成报告.md           # 开发总结
└── PROJECT_STATUS.md         # 项目状态
```

### 不包含的文件
```
❌ Library/          # Unity 库文件（可重新生成）
❌ Temp/             # 临时文件
❌ Obj/              # 编译对象
❌ .vs/              # Visual Studio 配置
❌ .idea/            # Rider 配置
```

---

## 🚀 导入到 Unity

### 步骤 1: 解压文件

```bash
# 解压 tar.gz
tar -xzf LobsterCourier_20260314_204702.tar.gz

# 或解压 zip
unzip LobsterCourier.zip
```

### 步骤 2: 打开 Unity Hub

```
1. 启动 Unity Hub
2. 点击 "Add" 或 "添加"
3. 选择 LobsterCourier 文件夹
4. 点击 "Add Project" 或 "添加项目"
```

### 步骤 3: 打开项目

```
1. 在 Unity Hub 中找到 LobsterCourier
2. 点击项目打开
3. 等待 Unity 导入资源（首次打开需要几分钟）
```

### 步骤 4: 验证导入

```
1. 检查 Console 窗口是否有错误
2. 打开 Assets/Scenes/GameScene.unity（如果已创建）
3. 或运行自动搭建工具创建场景
```

---

## 💾 备份建议

### 定期备份
```bash
# 创建带日期的备份
cd /root/.openclaw/workspace
tar -czf LobsterCourier_Backup_$(date +%Y%m%d).tar.gz LobsterCourier/
```

### 备份到外部存储
```bash
# 复制到外部硬盘
cp LobsterCourier_*.tar.gz /mnt/external-drive/backups/

# 或使用 rsync 同步
rsync -av LobsterCourier/ /mnt/external-drive/backups/LobsterCourier/
```

### 云备份
```bash
# 使用 rclone 备份到云存储
rclone copy LobsterCourier_*.tar.gz remote:backups/

# 或使用云同步工具（Dropbox、Google Drive 等）
```

---

## 📊 导出统计

| 项目 | 数值 |
|------|------|
| **总文件数** | ~40 个 |
| **C# 脚本** | 35 个 |
| **文档** | 10 个 |
| **压缩前大小** | ~200KB |
| **压缩后大小** | ~69KB |
| **压缩率** | ~65% |

---

## 🔧 导出脚本

### 一键导出脚本

创建 `export.sh`:
```bash
#!/bin/bash

# 龙虾快递员项目导出脚本

PROJECT_DIR="/root/.openclaw/workspace/LobsterCourier"
EXPORT_DIR="/root/.openclaw/workspace"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
EXPORT_FILE="LobsterCourier_${TIMESTAMP}.tar.gz"

echo "🦞 开始导出龙虾快递员项目..."
echo "项目目录：$PROJECT_DIR"
echo "导出文件：$EXPORT_FILE"

# 创建压缩包
cd $EXPORT_DIR
tar -czf $EXPORT_FILE LobsterCourier/

# 显示结果
if [ -f "$EXPORT_FILE" ]; then
    FILE_SIZE=$(ls -lh $EXPORT_FILE | awk '{print $5}')
    echo "✅ 导出完成！"
    echo "文件：$EXPORT_FILE"
    echo "大小：$FILE_SIZE"
else
    echo "❌ 导出失败！"
    exit 1
fi
```

使用：
```bash
chmod +x export.sh
./export.sh
```

---

## 📝 导出检查清单

导出前检查：
- [ ] 所有脚本已保存
- [ ] 没有编译错误
- [ ] 场景已保存
- [ ] 预制体已保存
- [ ] 文档已更新

导出后验证：
- [ ] 压缩包完整
- [ ] 文件大小合理
- [ ] 可以成功解压
- [ ] 可以在 Unity 中打开

---

## 🐛 常见问题

### Q: 导出后在 Unity 中打不开？
**A:** 确保：
1. Unity 版本是 2022.3 LTS 或更高
2. 解压完整，没有文件丢失
3. 首次打开需要等待资源导入完成

### Q: 导出的文件太大？
**A:** 检查是否包含了：
- Library/ 文件夹（应该排除）
- Temp/ 文件夹（应该排除）
- 大型资源文件（可以单独压缩）

### Q: 如何在不同电脑间同步？
**A:** 推荐：
1. 使用 Git 进行版本控制
2. 使用云同步（Dropbox、Google Drive）
3. 定期打包备份

---

## 📞 当前导出文件

**已生成的导出文件:**
```
/root/.openclaw/workspace/LobsterCourier_20260314_204702.tar.gz
大小：69KB
时间：2026-03-14 20:47:02
```

**复制到其他位置:**
```bash
cp /root/.openclaw/workspace/LobsterCourier_20260314_204702.tar.gz /your/target/path/
```

---

**导出完成！项目已安全打包！** 🦞📦

---

**最后更新:** 2026-03-14  
**项目版本:** v0.9.0
