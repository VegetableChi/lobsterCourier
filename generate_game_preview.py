#!/usr/bin/env python3
"""
龙虾快递员 - 游戏效果预览图生成器
生成游戏运行时的模拟效果图
"""

from PIL import Image, ImageDraw, ImageFont, ImageFilter
import os

# 颜色定义
COLORS = {
    'ocean_blue': (74, 144, 200),
    'deep_blue': (0, 50, 100),
    'coral_orange': (255, 140, 66),
    'seaweed_green': (88, 185, 106),
    'lobster_body': (255, 107, 74),
    'lobster_belly': (255, 182, 193),
    'gold': (255, 215, 0),
    'white': (255, 255, 255),
    'black': (0, 0, 0),
}

OUTPUT_DIR = os.path.dirname(__file__)

def create_gradient_background(width, height):
    """创建渐变海底背景"""
    img = Image.new('RGB', (width, height), COLORS['deep_blue'])
    draw = ImageDraw.Draw(img)
    
    for y in range(height):
        ratio = y / height
        r = int(0 + ratio * 30)
        g = int(20 + ratio * 60)
        b = int(50 + ratio * 120)
        draw.line([(0, y), (width, y)], fill=(r, g, b))
    
    return img

def draw_lobster(draw, x, y, scale=1.0):
    """绘制龙虾主角"""
    s = scale
    
    # 身体
    draw.ellipse([x-30*s, y-20*s, x+30*s, y+25*s], fill=COLORS['lobster_body'])
    # 腹部
    draw.ellipse([x-20*s, y+10*s, x+20*s, y+25*s], fill=COLORS['lobster_belly'])
    
    # 眼睛
    draw.ellipse([x-15*s, y-15*s, x-5*s, y-5*s], fill=COLORS['white'])
    draw.ellipse([x+5*s, y-15*s, x+15*s, y-5*s], fill=COLORS['white'])
    draw.ellipse([x-12*s, y-12*s, x-8*s, y-8*s], fill=COLORS['black'])
    draw.ellipse([x+8*s, y-12*s, x+12*s, y-8*s], fill=COLORS['black'])
    
    # 触角
    draw.line([x-10*s, y-20*s, x-15*s, y-35*s], fill=COLORS['lobster_body'], width=int(3*s))
    draw.line([x+10*s, y-20*s, x+15*s, y-35*s], fill=COLORS['lobster_body'], width=int(3*s))
    
    # 钳子
    draw.ellipse([x+25*s, y+15*s, x+45*s, y+30*s], fill=COLORS['lobster_body'])
    draw.ellipse([x-45*s, y+10*s, x-25*s, y+25*s], fill=COLORS['lobster_body'])
    
    # 微笑
    draw.arc([x-10*s, y+5*s, x+10*s, y+15*s], 180, 0, fill=COLORS['black'], width=int(2*s))

def draw_npc_starfish(draw, x, y, scale=0.8):
    """绘制星星 NPC"""
    import math
    s = scale
    cx, cy = x, y
    
    # 五角星
    points = []
    for i in range(10):
        angle = math.pi / 2 + i * math.pi / 5
        r = (25 if i % 2 == 0 else 12) * s
        px = cx + r * math.cos(angle)
        py = cy - r * math.sin(angle)
        points.append((px, py))
    
    draw.polygon(points, fill=COLORS['gold'])
    
    # 眼镜
    draw.ellipse([cx-8*s, cy-5*s, cx-3*s, cy], fill=COLORS['black'])
    draw.ellipse([cx+3*s, cy-5*s, cx+8*s, cy], fill=COLORS['black'])
    draw.line([cx-3*s, cy-3*s, cx+3*s, cy-3*s], fill=COLORS['black'], width=int(1*s))
    
    # 微笑
    draw.arc([cx-6*s, cy+2*s, cx+6*s, cy+10*s], 180, 0, fill=COLORS['black'], width=int(2*s))

def draw_npc_octopus(draw, x, y, scale=0.8):
    """绘制八爪 NPC"""
    s = scale
    
    # 头部
    draw.ellipse([x-25*s, y-30*s, x+25*s, y+10*s], fill=(155, 89, 182))
    
    # 贝雷帽
    draw.ellipse([x-20*s, y-35*s, x+20*s, y-25*s], fill=(100, 50, 150))
    
    # 触手
    for i in range(5):
        tx = x + (i - 2) * 10 * s
        draw.line([tx, y+10*s, tx + (i-2)*5*s, y+40*s], fill=(155, 89, 182), width=int(6*s))
    
    # 眼睛
    draw.ellipse([x-12*s, y-15*s, x-5*s, y-8*s], fill=COLORS['white'])
    draw.ellipse([x+5*s, y-15*s, x+12*s, y-8*s], fill=COLORS['white'])

def draw_coral(draw, x, y):
    """绘制珊瑚"""
    for i in range(5):
        cx = x + (i - 2) * 8
        draw.line([cx, y, cx + (i-2)*3, y-40], fill=COLORS['coral_orange'], width=6)

def draw_seaweed(draw, x, y, height=60):
    """绘制海草"""
    points = [(x, y)]
    for i in range(5):
        px = x + (i % 2 - 0.5) * 10
        py = y - (i + 1) * height / 5
        points.append((px, py))
    
    for i in range(len(points) - 1):
        draw.line([points[i], points[i+1]], fill=COLORS['seaweed_green'], width=8)

def draw_bubble(draw, x, y, size=5):
    """绘制气泡"""
    draw.ellipse([x-size, y-size, x+size, y+size], outline=(200, 240, 255, 150), width=2)

def draw_package(draw, x, y, scale=1.0):
    """绘制包裹"""
    s = scale
    # 盒子
    draw.rectangle([x-15*s, y-15*s, x+15*s, y+15*s], fill=(210, 180, 140))
    # 丝带
    draw.rectangle([x-5*s, y-15*s, x+5*s, y+15*s], fill=(255, 100, 100))
    draw.rectangle([x-15*s, y-5*s, x+15*s, y+5*s], fill=(255, 100, 100))
    # 蝴蝶结
    draw.ellipse([x-10*s, y-20*s, x, y-10*s], fill=(255, 100, 100))
    draw.ellipse([x, y-20*s, x+10*s, y-10*s], fill=(255, 100, 100))

def draw_ui_panel(draw, width, height):
    """绘制 UI 面板"""
    # 半透明背景
    overlay = Image.new('RGBA', (width, 100), (0, 0, 50, 180))
    draw_overlay = ImageDraw.Draw(overlay)
    
    # 分数栏
    draw_overlay.text((20, 30), "📦 分数：1250", fill=COLORS['white'])
    draw_overlay.text((200, 30), "⏱️ 时间：02:35", fill=COLORS['white'])
    draw_overlay.text((400, 30), "🌊 关卡：3", fill=COLORS['white'])
    draw_overlay.text((600, 30), "❤️ 生命：3", fill=COLORS['white'])
    
    return overlay

def draw_minimap(draw, center_x, center_y, size=150):
    """绘制小地图"""
    # 背景
    draw.ellipse([center_x-size, center_y-size, center_x+size, center_y+size], 
                 fill=(0, 50, 100, 180))
    draw.ellipse([center_x-size, center_y-size, center_x+size, center_y+size], 
                 outline=COLORS['white'], width=2)
    
    # 玩家位置（中心）
    draw.ellipse([center_x-5, center_y-5, center_x+5, center_y+5], fill=COLORS['lobster_body'])
    
    # 目标点
    draw.ellipse([center_x+40, center_y-30, center_x+50, center_y-20], fill=COLORS['gold'])
    draw.ellipse([center_x-35, center_y+25, center_x-25, center_y+35], fill=COLORS['gold'])

def create_game_preview():
    """创建游戏预览图"""
    print("🦞 正在生成游戏预览图...")
    
    # 创建 1920x1080 画布
    width, height = 1920, 1080
    img = create_gradient_background(width, height)
    draw = ImageDraw.Draw(img)
    
    # 绘制背景装饰
    print("  - 绘制背景...")
    for i in range(20):
        x = (i * 97) % width
        y = height - 50 - (i % 5) * 20
        draw_seaweed(draw, x, y, 40 + (i % 3) * 20)
    
    for i in range(15):
        x = (i * 127 + 50) % width
        y = height - 30
        draw_coral(draw, x, y)
    
    # 绘制气泡
    for i in range(30):
        x = (i * 67) % width
        y = (i * 37) % (height - 200) + 100
        draw_bubble(draw, x, y, 3 + (i % 4))
    
    # 绘制 NPC
    print("  - 绘制 NPC...")
    draw_npc_starfish(draw, 400, 600, 1.2)
    draw_npc_octopus(draw, 1500, 550, 1.2)
    
    # 绘制包裹
    print("  - 绘制包裹...")
    draw_package(draw, 800, 500, 1.0)
    draw_package(draw, 1200, 650, 1.0)
    
    # 绘制主角（中心位置）
    print("  - 绘制主角龙虾...")
    draw_lobster(draw, 960, 540, 1.5)
    
    # 绘制 UI
    print("  - 绘制 UI 界面...")
    ui_overlay = draw_ui_panel(draw, width, height)
    img.paste(ui_overlay, (0, 0), ui_overlay)
    
    # 绘制小地图
    print("  - 绘制小地图...")
    draw_minimap(draw, width - 100, height - 100)
    
    # 添加标题
    print("  - 添加标题...")
    title_text = "🦞 龙虾快递员 - Lobster Express"
    try:
        font = ImageFont.truetype("/usr/share/fonts/truetype/dejavu/DejaVuSans-Bold.ttf", 48)
    except:
        font = ImageFont.load_default()
    
    # 标题背景
    draw.rectangle([50, 30, 700, 100], fill=(0, 0, 50, 200))
    draw.text((70, 45), title_text, fill=COLORS['white'], font=font)
    
    # 添加控制说明
    controls = [
        "WASD - 移动",
        "Shift - 冲刺",
        "E - 抓取包裹",
        "Esc - 暂停"
    ]
    for i, text in enumerate(controls):
        draw.text((width - 250, 800 + i * 40), text, fill=COLORS['white'])
    
    # 保存
    output_path = os.path.join(OUTPUT_DIR, 'game_preview.png')
    img.save(output_path, 'PNG', quality=95)
    print(f"✅ 预览图已保存：{output_path}")
    
    # 同时生成一个缩略图
    thumbnail = img.copy()
    thumbnail.thumbnail((640, 360))
    thumbnail_path = os.path.join(OUTPUT_DIR, 'game_preview_thumb.png')
    thumbnail.save(thumbnail_path, 'PNG')
    print(f"✅ 缩略图已保存：{thumbnail_path}")
    
    return output_path

if __name__ == '__main__':
    create_game_preview()
    print("\n🎮 游戏预览图生成完成！")
