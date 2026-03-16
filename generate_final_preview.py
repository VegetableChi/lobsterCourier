#!/usr/bin/env python3
"""
龙虾快递员 - 最终游戏效果预览图
展示完整的游戏运行效果 (包含新 UI)
"""

from PIL import Image, ImageDraw, ImageFont
import math
import os

COLORS = {
    'ocean_blue': (74, 144, 200),
    'deep_blue': (0, 30, 60),
    'coral_orange': (255, 140, 66),
    'seaweed_green': (88, 185, 106),
    'lobster_body': (255, 107, 74),
    'lobster_belly': (255, 182, 193),
    'gold': (255, 215, 0),
    'white': (255, 255, 255),
    'black': (0, 0, 0),
    'pink': (255, 105, 180),
    'cyan': (100, 200, 255),
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

def draw_lobster(draw, x, y, scale=1.0, frame=0):
    """绘制龙虾主角 (带动画帧)"""
    s = scale
    
    # 呼吸动画 (缩放)
    breath = 1.0 + 0.03 * math.sin(frame * 0.5)
    s *= breath
    
    # 身体
    draw.ellipse([int(x-30*s), int(y-20*s), int(x+30*s), int(y+25*s)], fill=COLORS['lobster_body'])
    # 腹部
    draw.ellipse([int(x-20*s), int(y+10*s), int(x+20*s), int(y+25*s)], fill=COLORS['lobster_belly'])
    
    # 眼睛
    draw.ellipse([int(x-15*s), int(y-15*s), int(x-5*s), int(y-5*s)], fill=COLORS['white'])
    draw.ellipse([int(x+5*s), int(y-15*s), int(x+15*s), int(y-5*s)], fill=COLORS['white'])
    draw.ellipse([int(x-12*s), int(y-12*s), int(x-8*s), int(y-8*s)], fill=COLORS['black'])
    draw.ellipse([int(x+8*s), int(y-12*s), int(x+12*s), int(y-8*s)], fill=COLORS['black'])
    
    # 触角 (带摆动)
    wave = math.sin(frame * 0.3) * 5
    draw.line([int(x-10*s), int(y-20*s), int(x-15*s+wave), int(y-35*s)], 
              fill=COLORS['lobster_body'], width=int(3*s))
    draw.line([int(x+10*s), int(y-20*s), int(x+15*s+wave), int(y-35*s)], 
              fill=COLORS['lobster_body'], width=int(3*s))
    
    # 钳子
    draw.ellipse([int(x+25*s), int(y+15*s), int(x+45*s), int(y+30*s)], fill=COLORS['lobster_body'])
    draw.ellipse([int(x-45*s), int(y+10*s), int(x-25*s), int(y+25*s)], fill=COLORS['lobster_body'])
    
    # 微笑
    draw.arc([int(x-10*s), int(y+5*s), int(x+10*s), int(y+15*s)], 
             180, 0, fill=COLORS['black'], width=int(2*s))

def draw_npc_starfish(draw, x, y, scale=0.8):
    """绘制星星 NPC"""
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
    draw.ellipse([int(cx-8*s), int(cy-5*s), int(cx-3*s), int(cy)], fill=COLORS['black'])
    draw.ellipse([int(cx+3*s), int(cy-5*s), int(cx+8*s), int(cy)], fill=COLORS['black'])
    draw.line([int(cx-3*s), int(cy-3*s), int(cx+3*s), int(cy-3*s)], 
              fill=COLORS['black'], width=int(1*s))
    
    # 微笑
    draw.arc([int(cx-6*s), int(cy+2*s), int(cx+6*s), int(cy+10*s)], 
             180, 0, fill=COLORS['black'], width=int(2*s))
    
    # 对话气泡
    draw.ellipse([int(cx+30*s), int(cy-40*s), int(cx+90*s), int(cy-20*s)], 
                 fill=(255, 255, 255, 230))
    draw.polygon([(cx+55*s, cy-20*s), (cx+50*s, cy-15*s), (cx+60*s, cy-15*s)], 
                 fill=(255, 255, 255, 230))

def draw_npc_octopus(draw, x, y, scale=0.8):
    """绘制八爪 NPC"""
    s = scale
    
    # 头部
    draw.ellipse([int(x-25*s), int(y-30*s), int(x+25*s), int(y+10*s)], fill=(155, 89, 182))
    
    # 贝雷帽
    draw.ellipse([int(x-20*s), int(y-35*s), int(x+20*s), int(y-25*s)], fill=(100, 50, 150))
    
    # 触手 (摆动动画)
    for i in range(5):
        tx = x + (i - 2) * 10 * s
        wave = math.sin(i * 0.5) * 5
        draw.line([int(tx), int(y+10*s), int(tx + (i-2)*5*s + wave), int(y+40*s)], 
                  fill=(155, 89, 182), width=int(6*s))
    
    # 眼睛
    draw.ellipse([int(x-12*s), int(y-15*s), int(x-5*s), int(y-8*s)], fill=COLORS['white'])
    draw.ellipse([int(x+5*s), int(y-15*s), int(x+12*s), int(y-8*s)], fill=COLORS['white'])

def draw_coral(draw, x, y):
    """绘制珊瑚"""
    for i in range(5):
        cx = x + (i - 2) * 8
        draw.line([cx, y, int(cx + (i-2)*3), y-40], 
                  fill=COLORS['coral_orange'], width=6)

def draw_seaweed(draw, x, y, height=60, frame=0):
    """绘制海草 (带摇摆)"""
    wave = math.sin(frame * 0.05 + x * 0.1) * 8
    points = [(x + wave, y)]
    for i in range(5):
        px = x + (i % 2 - 0.5) * 10 + wave * (1 - i/5)
        py = y - (i + 1) * height / 5
        points.append((px, py))
    
    for i in range(len(points) - 1):
        draw.line([points[i], points[i+1]], 
                  fill=COLORS['seaweed_green'], width=8)

def draw_bubble(draw, x, y, size=5, frame=0):
    """绘制气泡 (上升动画)"""
    offset = (frame * 2) % 50
    draw.ellipse([int(x-size), int(y-size-offset), int(x+size), int(y+size-offset)], 
                 outline=(200, 240, 255, 150), width=2)
    # 高光
    draw.ellipse([int(x-size/2), int(y-size/2-offset), int(x+size/4), int(y-size/4-offset)], 
                 fill=(255, 255, 255, 100))

def draw_package(draw, x, y, scale=1.0):
    """绘制包裹"""
    s = scale
    # 盒子
    draw.rectangle([int(x-15*s), int(y-15*s), int(x+15*s), int(y+15*s)], fill=(210, 180, 140))
    # 丝带
    draw.rectangle([int(x-5*s), int(y-15*s), int(x+5*s), int(y+15*s)], fill=(255, 100, 100))
    draw.rectangle([int(x-15*s), int(y-5*s), int(x+15*s), int(y+5*s)], fill=(255, 100, 100))
    # 蝴蝶结
    draw.ellipse([int(x-10*s), int(y-20*s), int(x), int(y-10*s)], fill=(255, 100, 100))
    draw.ellipse([int(x), int(y-20*s), int(x+10*s), int(y-10*s)], fill=(255, 100, 100))

def draw_seashell_button(draw, x, y, width=160, height=50, text="按钮", state='normal'):
    """绘制贝壳按钮"""
    if state == 'normal':
        color = (87, 206, 235)
        border = (100, 180, 220)
        glow = (255, 255, 255, 100)
    elif state == 'hover':
        color = (0, 191, 255)
        border = (150, 220, 255)
        glow = (255, 255, 255, 180)
    else:
        color = (50, 100, 150)
        border = (80, 140, 180)
        glow = (255, 255, 255, 50)
    
    margin = 4
    points = []
    # 顶部波浪
    wave_count = 5
    for i in range(wave_count + 1):
        px = x + margin + (width - 2 * margin) * i / wave_count
        py = y + margin + (8 if i % 2 == 0 else 0)
        points.append((px, py))
    
    # 右侧
    points.append((x + width - margin, y + height // 2))
    
    # 底部波浪
    for i in range(wave_count, -1, -1):
        px = x + margin + (width - 2 * margin) * i / wave_count
        py = y + height - margin - (8 if i % 2 == 0 else 0)
        points.append((px, py))
    
    # 左侧
    points.append((x + margin, y + height // 2))
    
    draw.polygon(points, fill=color)
    draw.polygon(points, outline=border, width=3)
    
    # 珍珠光泽
    draw.ellipse([int(x + margin + 15), int(y + margin + 10), 
                  int(x + margin + 35), int(y + margin + 25)], fill=glow)
    
    # 文字
    try:
        font = ImageFont.truetype("/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf", 18)
    except:
        font = ImageFont.load_default()
    
    text_bbox = draw.textbbox((0, 0), text, font=font)
    text_width = text_bbox[2] - text_bbox[0]
    text_x = x + (width - text_width) // 2
    text_y = y + (height - 18) // 2
    draw.text((int(text_x), int(text_y)), text, fill=COLORS['white'], font=font)

def draw_ocean_icon(draw, x, y, icon_type, size=40):
    """绘制海洋风格图标"""
    cx, cy = x + size // 2, y + size // 2
    s = size / 64
    
    # 背景光晕
    draw.ellipse([int(cx - 28*s), int(cy - 28*s), int(cx + 28*s), int(cy + 28*s)],
                 fill=(255, 255, 255, 25))
    
    if icon_type == 'coin':
        draw.ellipse([int(cx - 22*s), int(cy - 22*s), int(cx + 22*s), int(cy + 22*s)],
                     fill=COLORS['gold'])
        for i in range(5):
            angle = i * 72 * math.pi / 180
            x1 = cx + 15*s * math.cos(angle)
            y1 = cy + 15*s * math.sin(angle)
            x2 = cx + 22*s * math.cos(angle)
            y2 = cy + 22*s * math.sin(angle)
            draw.line([(x1, y1), (x2, y2)], fill=(200, 170, 0), width=int(3*s))
        draw.ellipse([int(cx - 10*s), int(cy - 10*s), int(cx - 4*s), int(cy - 4*s)],
                     fill=(255, 255, 255, 200))
    
    elif icon_type == 'diamond':
        points = [(cx, cy - 25*s), (cx + 18*s, cy - 8*s),
                  (cx, cy + 25*s), (cx - 18*s, cy - 8*s)]
        draw.polygon(points, fill=COLORS['cyan'])
        draw.line([(cx, cy - 25*s), (cx, cy + 25*s)],
                  fill=(200, 250, 250, 150), width=int(2*s))
        draw.ellipse([int(cx - 8*s), int(cy - 15*s), int(cx - 4*s), int(cy - 11*s)],
                     fill=(255, 255, 255, 220))
    
    elif icon_type == 'heart':
        for i in range(5):
            angle = (i * 72 - 18) * math.pi / 180
            inner_angle = ((i + 0.5) * 72 - 18) * math.pi / 180
            x1 = cx + 22*s * math.cos(angle)
            y1 = cy + 22*s * math.sin(angle)
            x2 = cx + 11*s * math.cos(inner_angle)
            y2 = cy + 11*s * math.sin(inner_angle)
            x3 = cx + 22*s * math.cos(((i + 1) * 72 - 18) * math.pi / 180)
            y3 = cy + 22*s * math.sin(((i + 1) * 72 - 18) * math.pi / 180)
            draw.polygon([(x1, y1), (x2, y2), (x3, y3)], fill=COLORS['pink'])
        draw.ellipse([int(cx - 6*s), int(cy - 6*s), int(cx + 6*s), int(cy + 6*s)],
                     fill=(255, 180, 200))
    
    elif icon_type == 'star':
        points = []
        for i in range(10):
            angle = math.pi / 2 + i * math.pi / 5
            r = (22 if i % 2 == 0 else 9) * s
            x = cx + r * math.cos(angle)
            y = cy - r * math.sin(angle)
            points.append((x, y))
        draw.polygon(points, fill=COLORS['gold'])
        for i in range(5):
            angle = (i * 72) * math.pi / 180
            px = cx + 18*s * math.cos(angle)
            py = cy + 18*s * math.sin(angle)
            draw.ellipse([int(px - 4*s), int(py - 4*s), int(px + 4*s), int(py + 4*s)],
                         fill=(255, 255, 255, 180))
    
    elif icon_type == 'package':
        draw.rectangle([int(cx - 18*s), int(cy - 18*s), int(cx + 18*s), int(cy + 18*s)],
                       fill=(210, 180, 140))
        draw.rectangle([int(cx - 5*s), int(cy - 18*s), int(cx + 5*s), int(cy + 18*s)],
                       fill=(255, 100, 100))
        draw.rectangle([int(cx - 18*s), int(cy - 5*s), int(cx + 18*s), int(cy + 5*s)],
                       fill=(255, 100, 100))
        draw.ellipse([int(cx - 11*s), int(cy - 15*s), int(cx - 4*s), int(cy - 8*s)],
                     fill=(255, 80, 80))
        draw.ellipse([int(cx + 4*s), int(cy - 15*s), int(cx + 11*s), int(cy - 8*s)],
                     fill=(255, 80, 80))

def draw_wave_panel(draw, x, y, width=350, height=100):
    """绘制海浪面板 (UI 顶部栏)"""
    # 渐变背景
    for py in range(height):
        ratio = py / height
        alpha = int(180 + ratio * 50)
        draw.line([(x, y + py), (x + width, y + py)],
                  fill=(0, 30, 60, alpha))
    
    # 顶部海浪
    wave_points = [(x, y + 10)]
    for i in range(17):
        px = x + i * 20
        wave_points.append((px, y + 10 + (8 if i % 2 == 0 else 0)))
        wave_points.append((px + 10, y + 10 + (0 if i % 2 == 0 else 8)))
    wave_points.append((x + width, y + 10))
    wave_points.append((x + width, y))
    wave_points.append((x, y))
    draw.polygon(wave_points, fill=(0, 50, 100, 200))
    
    # 珍珠角
    corners = [(x + 15, y + 15), (x + width - 15, y + 15)]
    for cx, cy in corners:
        draw.ellipse([cx - 8, cy - 8, cx + 8, cy + 8], fill=(255, 255, 255, 180))
        draw.ellipse([cx - 4, cy - 4, cx + 4, cy + 4], fill=(255, 255, 255, 255))

def draw_bar(draw, x, y, bar_type='heart', fill_percent=0.75):
    """绘制状态条"""
    width, height = 180, 22
    
    # 背景
    draw.rounded_rectangle([x, y, x + width, y + height],
                           radius=10, fill=(0, 40, 80, 200))
    draw.rounded_rectangle([x + 2, y + 2, x + width - 2, y + height - 2],
                           radius=8, outline=(100, 180, 220), width=2)
    
    # 填充
    fill_width = int((width - 8) * fill_percent)
    if bar_type == 'heart':
        fill_color = COLORS['pink']
    else:
        fill_color = (100, 200, 255, 180)
    
    draw.rounded_rectangle([x + 4, y + 4, x + 4 + fill_width, y + height - 4],
                           radius=6, fill=fill_color)
    
    # 气泡效果 (体力条)
    if bar_type == 'stamina':
        for i in range(3):
            bx = x + 12 + i * 20
            draw.ellipse([int(bx - 4), int(y + 5), int(bx + 4), int(y + 15)],
                         fill=(255, 255, 255, 100))

def draw_minimap(draw, center_x, center_y, size=120):
    """绘制小地图"""
    # 背景
    draw.ellipse([center_x-size, center_y-size, center_x+size, center_y+size], 
                 fill=(0, 50, 100, 180))
    draw.ellipse([center_x-size, center_y-size, center_x+size, center_y+size], 
                 outline=COLORS['white'], width=2)
    
    # 玩家位置 (中心)
    draw.ellipse([center_x-6, center_y-6, center_x+6, center_y+6], fill=COLORS['lobster_body'])
    
    # 目标点
    draw.ellipse([center_x+35, center_y-25, center_x+43, center_y-17], fill=COLORS['gold'])
    draw.ellipse([center_x-30, center_y+20, center_x-22, center_y+28], fill=COLORS['gold'])
    
    # 障碍物
    draw.ellipse([center_x-20, center_y-15, center_x-12, center_y-7], fill=(100, 100, 100, 150))
    draw.ellipse([center_x+25, center_y+10, center_x+33, center_y+18], fill=(100, 100, 100, 150))

def create_final_game_preview(frame=0):
    """创建最终游戏预览图"""
    print("🦞 正在生成最终游戏效果预览图...")
    
    width, height = 1920, 1080
    img = create_gradient_background(width, height)
    draw = ImageDraw.Draw(img)
    
    # 绘制背景装饰
    print("  - 绘制背景...")
    for i in range(25):
        x = (i * 79) % width
        y = height - 50 - (i % 5) * 20
        draw_seaweed(draw, x, y, 40 + (i % 3) * 20, frame)
    
    for i in range(18):
        x = (i * 107 + 50) % width
        y = height - 30
        draw_coral(draw, x, y)
    
    # 气泡 (动画)
    for i in range(40):
        x = (i * 49) % width
        y = (i * 37) % (height - 200) + 100
        draw_bubble(draw, x, y, 3 + (i % 4), frame)
    
    # 绘制 NPC
    print("  - 绘制 NPC...")
    draw_npc_starfish(draw, 350, 580, 1.3)
    draw_npc_octopus(draw, 1550, 530, 1.3)
    
    # 绘制包裹 (带浮动动画)
    print("  - 绘制包裹...")
    float_y = math.sin(frame * 0.08) * 5
    draw_package(draw, 750, 480 + float_y, 1.1)
    draw_package(draw, 1180, 630 + float_y, 1.1)
    
    # 绘制主角 (中心，带动画)
    print("  - 绘制主角龙虾...")
    draw_lobster(draw, 960, 540, 1.6, frame)
    
    # 绘制 UI 顶部栏
    print("  - 绘制 UI 界面...")
    draw_wave_panel(draw, 0, 0, width, 90)
    
    # 标题
    try:
        title_font = ImageFont.truetype("/usr/share/fonts/truetype/dejavu/DejaVuSans-Bold.ttf", 36)
        text_font = ImageFont.truetype("/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf", 24)
    except:
        title_font = ImageFont.load_default()
        text_font = ImageFont.load_default()
    
    draw.text((30, 25), "🦞 龙虾快递员", fill=COLORS['white'], font=title_font)
    
    # 状态信息
    draw.text((350, 30), f"📦 分数：1250", fill=COLORS['white'], font=text_font)
    draw.text((580, 30), f"⏱️ 时间：02:35", fill=COLORS['white'], font=text_font)
    draw.text((810, 30), f"🌊 关卡：3", fill=COLORS['white'], font=text_font)
    
    # 状态条
    draw.text((1050, 32), "❤️", fill=COLORS['white'], font=text_font)
    draw_bar(draw, 1100, 35, 'heart', 0.8)
    
    draw.text((1320, 32), "⚡", fill=COLORS['cyan'], font=text_font)
    draw_bar(draw, 1370, 35, 'stamina', 0.65)
    
    # 图标栏
    print("  - 绘制图标栏...")
    icons = ['coin', 'diamond', 'heart', 'star', 'package']
    for i, icon in enumerate(icons):
        draw_ocean_icon(draw, width - 280 + i * 55, 20, icon, size=45)
    
    # 小地图
    print("  - 绘制小地图...")
    draw_minimap(draw, width - 100, height - 100)
    
    # 控制说明
    try:
        ctrl_font = ImageFont.truetype("/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf", 18)
    except:
        ctrl_font = ImageFont.load_default()
    
    ctrl_bg = Image.new('RGBA', (200, 130), (0, 0, 50, 180))
    img.paste(ctrl_bg, (width - 220, height - 150), ctrl_bg)
    
    controls = [
        "🎮 控制说明",
        "WASD - 移动",
        "Shift - 冲刺",
        "E - 抓取包裹",
        "Esc - 暂停"
    ]
    for i, text in enumerate(controls):
        draw.text((width - 210, height - 145 + i * 28), text, fill=COLORS['white'], font=ctrl_font)
    
    # 贝壳按钮 (菜单)
    print("  - 绘制按钮...")
    draw_seashell_button(draw, width//2 - 250, height - 80, text="开始游戏", state='hover')
    draw_seashell_button(draw, width//2 - 80, height - 80, text="设置", state='normal')
    
    # 成就通知 (右上角)
    notify_bg = Image.new('RGBA', (350, 80), (255, 215, 0, 200))
    img.paste(notify_bg, (width - 370, 110), notify_bg)
    draw.text((width - 355, 125), "🏆 成就解锁：新手快递员!", fill=COLORS['black'], font=text_font)
    
    # 保存
    output_path = os.path.join(OUTPUT_DIR, 'final_game_preview.png')
    img.save(output_path, 'PNG', quality=95)
    print(f"✅ 最终预览图已保存：{output_path}")
    
    # 缩略图
    thumb = img.copy()
    thumb.thumbnail((960, 540))
    thumb_path = os.path.join(OUTPUT_DIR, 'final_game_preview_thumb.png')
    thumb.save(thumb_path, 'PNG')
    print(f"✅ 缩略图已保存：{thumb_path}")
    
    # 生成多帧动画预览
    print("  - 生成动画预览帧...")
    for f in [10, 20, 30]:
        frame_img = create_gradient_background(width, height)
        frame_draw = ImageDraw.Draw(frame_img)
        
        # 简化绘制
        for i in range(25):
            x = (i * 79) % width
            y = height - 50 - (i % 5) * 20
            draw_seaweed(frame_draw, x, y, 40 + (i % 3) * 20, f)
        
        for i in range(40):
            x = (i * 49) % width
            y = (i * 37) % (height - 200) + 100
            draw_bubble(frame_draw, x, y, 3 + (i % 4), f)
        
        draw_lobster(frame_draw, 960, 540, 1.6, f)
        draw_package(frame_draw, 750, 480 + math.sin(f * 0.08) * 5, 1.1)
        
        frame_img.save(os.path.join(OUTPUT_DIR, f'frame_{f:03d}.png'), 'PNG')
    
    print(f"✅ 动画预览帧已生成 (frame_010.png, frame_020.png, frame_030.png)")
    
    return output_path

if __name__ == '__main__':
    create_final_game_preview(frame=0)
    print("\n🎮 最终游戏效果预览图生成完成!")
