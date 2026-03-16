#!/usr/bin/env python3
"""
龙虾快递员 - 游戏资产生成器
根据 ART_STYLE_GUIDE.md 生成所有必需的美术资源
"""

from PIL import Image, ImageDraw, ImageFilter
import os
import math

# 颜色定义（来自美术指南）
COLORS = {
    # 主色调
    'ocean_blue': (74, 144, 200),
    'coral_orange': (255, 140, 66),
    'seaweed_green': (88, 185, 106),
    'sand_yellow': (255, 217, 102),
    
    # 龙虾主角
    'lobster_body': (255, 107, 74),
    'lobster_belly': (255, 182, 193),
    'lobster_claw': (255, 69, 0),
    
    # NPC 颜色
    'starfish': (255, 215, 0),
    'octopus': (155, 89, 182),
    'coral_fish': (255, 105, 180),
    'crab': (220, 20, 60),
    'seahorse': (135, 206, 235),
    'jellyfish': (230, 230, 250),
    'turtle': (34, 139, 34),
    'shark': (0, 0, 139),
    
    # UI 颜色
    'ui_button': (135, 206, 235),
    'ui_button_hover': (0, 191, 255),
    'ui_text': (255, 255, 255),
    'ui_text_stroke': (0, 0, 139),
    'gold': (255, 215, 0),
    'diamond': (0, 206, 209),
    
    # 通用
    'white': (255, 255, 255),
    'black': (0, 0, 0),
    'transparent': (0, 0, 0, 0),
}

OUTPUT_DIR = os.path.join(os.path.dirname(__file__), 'Assets', 'Sprites')
UI_DIR = os.path.join(os.path.dirname(__file__), 'Assets', 'UI')

def ensure_dirs():
    """确保输出目录存在"""
    os.makedirs(OUTPUT_DIR, exist_ok=True)
    os.makedirs(UI_DIR, exist_ok=True)
    os.makedirs(os.path.join(OUTPUT_DIR, 'characters'), exist_ok=True)
    os.makedirs(os.path.join(OUTPUT_DIR, 'environment'), exist_ok=True)
    os.makedirs(os.path.join(OUTPUT_DIR, 'ui'), exist_ok=True)

def draw_lobster(size=64):
    """绘制龙虾主角"""
    img = Image.new('RGBA', (size, size), COLORS['transparent'])
    draw = ImageDraw.Draw(img)
    
    cx, cy = size // 2, size // 2
    scale = size / 64
    
    # 身体（椭圆形）
    body_bbox = [
        cx - 12 * scale, cy - 8 * scale,
        cx + 12 * scale, cy + 10 * scale
    ]
    draw.ellipse(body_bbox, fill=COLORS['lobster_body'])
    
    # 腹部
    belly_bbox = [
        cx - 8 * scale, cy + 2 * scale,
        cx + 8 * scale, cy + 10 * scale
    ]
    draw.ellipse(belly_bbox, fill=COLORS['lobster_belly'])
    
    # 眼睛（大眼睛）
    eye_offset = 6 * scale
    eye_size = 4 * scale
    # 左眼
    draw.ellipse([cx - eye_offset - eye_size, cy - 6 * scale, 
                  cx - eye_offset, cy - 2 * scale], fill=COLORS['white'])
    draw.ellipse([cx - eye_offset - eye_size + 1.5 * scale, cy - 5 * scale,
                  cx - eye_offset - eye_size + 3 * scale, cy - 3 * scale], fill=COLORS['black'])
    # 右眼
    draw.ellipse([cx + eye_offset, cy - 6 * scale,
                  cx + eye_offset + eye_size, cy - 2 * scale], fill=COLORS['white'])
    draw.ellipse([cx + eye_offset + 1 * scale, cy - 5 * scale,
                  cx + eye_offset + 3 * scale, cy - 3 * scale], fill=COLORS['black'])
    
    # 触角
    antenna_length = 10 * scale
    draw.line([cx - 4 * scale, cy - 8 * scale, 
               cx - 6 * scale, cy - 8 * scale - antenna_length], 
              fill=COLORS['lobster_body'], width=int(2 * scale))
    draw.line([cx + 4 * scale, cy - 8 * scale,
               cx + 6 * scale, cy - 8 * scale - antenna_length],
              fill=COLORS['lobster_body'], width=int(2 * scale))
    # 触角末端小球
    draw.ellipse([cx - 7 * scale, cy - 8 * scale - antenna_length - 1.5 * scale,
                  cx - 5 * scale, cy - 8 * scale - antenna_length + 0.5 * scale],
                 fill=COLORS['lobster_body'])
    draw.ellipse([cx + 5 * scale, cy - 8 * scale - antenna_length - 1.5 * scale,
                  cx + 7 * scale, cy - 8 * scale - antenna_length + 0.5 * scale],
                 fill=COLORS['lobster_body'])
    
    # 钳子（一大一小）
    # 大钳子（右侧）
    claw_bbox = [cx + 10 * scale, cy + 4 * scale,
                 cx + 20 * scale, cy + 12 * scale]
    draw.ellipse(claw_bbox, fill=COLORS['lobster_claw'])
    draw.line([cx + 8 * scale, cy + 6 * scale,
               cx + 12 * scale, cy + 8 * scale],
              fill=COLORS['lobster_claw'], width=int(4 * scale))
    
    # 小钳子（左侧）
    small_claw_bbox = [cx - 18 * scale, cy + 2 * scale,
                       cx - 10 * scale, cy + 8 * scale]
    draw.ellipse(small_claw_bbox, fill=COLORS['lobster_claw'])
    draw.line([cx - 8 * scale, cy + 4 * scale,
               cx - 10 * scale, cy + 5 * scale],
              fill=COLORS['lobster_claw'], width=int(3 * scale))
    
    # 微笑
    smile_bbox = [cx - 4 * scale, cy + 4 * scale,
                  cx + 4 * scale, cy + 7 * scale]
    draw.arc(smile_bbox, 0, 180, fill=COLORS['black'], width=int(1.5 * scale))
    
    return img

def draw_starfish(size=64):
    """绘制星星海星"""
    img = Image.new('RGBA', (size, size), COLORS['transparent'])
    draw = ImageDraw.Draw(img)
    
    cx, cy = size // 2, size // 2
    scale = size / 64
    
    # 五角星形状
    import math
    points = []
    for i in range(10):
        angle = math.pi / 2 + i * math.pi / 5
        r = (15 if i % 2 == 0 else 7) * scale
        x = cx + r * math.cos(angle)
        y = cy - r * math.sin(angle)
        points.append((x, y))
    
    draw.polygon(points, fill=COLORS['starfish'])
    
    # 眼镜
    draw.ellipse([cx - 5 * scale, cy - 3 * scale,
                  cx - 2 * scale, cy], fill=COLORS['black'])
    draw.ellipse([cx + 2 * scale, cy - 3 * scale,
                  cx + 5 * scale, cy], fill=COLORS['black'])
    draw.line([cx - 2 * scale, cy - 1.5 * scale,
               cx + 2 * scale, cy - 1.5 * scale],
              fill=COLORS['black'], width=int(1 * scale))
    
    # 微笑
    draw.arc([cx - 3 * scale, cy + 2 * scale,
              cx + 3 * scale, cy + 6 * scale],
             0, 180, fill=COLORS['black'], width=int(1 * scale))
    
    return img

def draw_octopus(size=64):
    """绘制八爪章鱼"""
    img = Image.new('RGBA', (size, size), COLORS['transparent'])
    draw = ImageDraw.Draw(img)
    
    cx, cy = size // 2, size // 2
    scale = size / 64
    
    # 头部（圆形）
    head_bbox = [cx - 12 * scale, cy - 12 * scale,
                 cx + 12 * scale, cy + 4 * scale]
    draw.ellipse(head_bbox, fill=COLORS['octopus'])
    
    # 贝雷帽
    beret_bbox = [cx - 10 * scale, cy - 14 * scale,
                  cx + 10 * scale, cy - 8 * scale]
    draw.ellipse(beret_bbox, fill=(100, 50, 150))
    
    # 触手（8 条简化为 4 条可见）
    for i in range(4):
        x_offset = (i - 1.5) * 6 * scale
        draw.line([cx + x_offset, cy + 4 * scale,
                   cx + x_offset - 3 * scale, cy + 18 * scale],
                  fill=COLORS['octopus'], width=int(4 * scale))
    
    # 眼睛
    draw.ellipse([cx - 5 * scale, cy - 4 * scale,
                  cx - 2 * scale, cy - 1 * scale], fill=COLORS['white'])
    draw.ellipse([cx + 2 * scale, cy - 4 * scale,
                  cx + 5 * scale, cy - 1 * scale], fill=COLORS['white'])
    draw.ellipse([cx - 4 * scale, cy - 3.5 * scale,
                  cx - 3 * scale, cy - 2 * scale], fill=COLORS['black'])
    draw.ellipse([cx + 3 * scale, cy - 3.5 * scale,
                  cx + 4 * scale, cy - 2 * scale], fill=COLORS['black'])
    
    return img

def draw_crab(size=64):
    """绘制钳子蟹"""
    img = Image.new('RGBA', (size, size), COLORS['transparent'])
    draw = ImageDraw.Draw(img)
    
    cx, cy = size // 2, size // 2
    scale = size / 64
    
    # 身体（扁圆）
    body_bbox = [cx - 14 * scale, cy - 6 * scale,
                 cx + 14 * scale, cy + 8 * scale]
    draw.ellipse(body_bbox, fill=COLORS['crab'])
    
    # 商帽
    hat_bbox = [cx - 8 * scale, cy - 10 * scale,
                 cx + 8 * scale, cy - 6 * scale]
    draw.rectangle(hat_bbox, fill=(50, 50, 50))
    
    # 大钳子
    draw.ellipse([cx - 22 * scale, cy - 2 * scale,
                  cx - 14 * scale, cy + 6 * scale],
                 fill=COLORS['gold'])
    draw.ellipse([cx + 14 * scale, cy - 2 * scale,
                  cx + 22 * scale, cy + 6 * scale],
                 fill=COLORS['gold'])
    
    # 眼睛
    draw.ellipse([cx - 6 * scale, cy - 2 * scale,
                  cx - 3 * scale, cy + 1 * scale], fill=COLORS['white'])
    draw.ellipse([cx + 3 * scale, cy - 2 * scale,
                  cx + 6 * scale, cy + 1 * scale], fill=COLORS['white'])
    draw.ellipse([cx - 5 * scale, cy - 1 * scale,
                  cx - 4 * scale, cy], fill=COLORS['black'])
    draw.ellipse([cx + 4 * scale, cy - 1 * scale,
                  cx + 5 * scale, cy], fill=COLORS['black'])
    
    return img

def draw_seahorse(size=64):
    """绘制卷卷海马"""
    img = Image.new('RGBA', (size, size), COLORS['transparent'])
    draw = ImageDraw.Draw(img)
    
    cx, cy = size // 2, size // 2
    scale = size / 64
    
    # 身体（S 形简化）
    points = [
        (cx - 2 * scale, cy - 12 * scale),
        (cx + 4 * scale, cy - 6 * scale),
        (cx - 4 * scale, cy + 2 * scale),
        (cx + 2 * scale, cy + 10 * scale),
    ]
    draw.line(points, fill=COLORS['seahorse'], width=int(8 * scale))
    
    # 头部
    draw.ellipse([cx - 4 * scale, cy - 16 * scale,
                  cx + 4 * scale, cy - 8 * scale],
                 fill=COLORS['seahorse'])
    
    # 老花镜
    draw.ellipse([cx - 3 * scale, cy - 12 * scale,
                  cx - 1 * scale, cy - 10 * scale], outline=COLORS['black'], width=int(1 * scale))
    draw.ellipse([cx + 1 * scale, cy - 12 * scale,
                  cx + 3 * scale, cy - 10 * scale], outline=COLORS['black'], width=int(1 * scale))
    
    return img

def draw_jellyfish(size=64):
    """绘制飘飘水母"""
    img = Image.new('RGBA', (size, size), COLORS['transparent'])
    draw = ImageDraw.Draw(img)
    
    cx, cy = size // 2, size // 2
    scale = size / 64
    
    # 半透明伞状身体
    jelly_color = (*COLORS['jellyfish'][:3], 180)
    draw.ellipse([cx - 12 * scale, cy - 12 * scale,
                  cx + 12 * scale, cy + 4 * scale],
                 fill=jelly_color)
    
    # 飘逸触手
    for i in range(5):
        x = cx + (i - 2) * 4 * scale
        draw.line([x, cy + 4 * scale,
                   x + (i - 2) * scale, cy + 16 * scale],
                  fill=jelly_color, width=int(2 * scale))
    
    # 发光效果
    glow = Image.new('RGBA', (size, size), COLORS['transparent'])
    glow_draw = ImageDraw.Draw(glow)
    glow_draw.ellipse([cx - 14 * scale, cy - 14 * scale,
                       cx + 14 * scale, cy + 6 * scale],
                      fill=(255, 255, 255, 50))
    img = Image.alpha_composite(glow, img)
    
    return img

def draw_turtle(size=64):
    """绘制老海海龟"""
    img = Image.new('RGBA', (size, size), COLORS['transparent'])
    draw = ImageDraw.Draw(img)
    
    cx, cy = size // 2, size // 2
    scale = size / 64
    
    # 龟壳
    shell_bbox = [cx - 14 * scale, cy - 8 * scale,
                  cx + 14 * scale, cy + 8 * scale]
    draw.ellipse(shell_bbox, fill=COLORS['turtle'])
    
    # 壳上花纹
    draw.ellipse([cx - 6 * scale, cy - 4 * scale,
                  cx + 6 * scale, cy + 4 * scale],
                 fill=(50, 100, 50))
    
    # 头部
    draw.ellipse([cx - 6 * scale, cy - 14 * scale,
                  cx + 6 * scale, cy - 6 * scale],
                 fill=COLORS['turtle'])
    
    # 老花镜
    draw.ellipse([cx - 5 * scale, cy - 12 * scale,
                  cx - 2 * scale, cy - 9 * scale], outline=COLORS['black'], width=int(1 * scale))
    draw.ellipse([cx + 2 * scale, cy - 12 * scale,
                  cx + 5 * scale, cy - 9 * scale], outline=COLORS['black'], width=int(1 * scale))
    
    # 胡子
    draw.line([cx - 4 * scale, cy - 6 * scale,
               cx - 6 * scale, cy - 2 * scale],
              fill=COLORS['black'], width=int(1 * scale))
    draw.line([cx + 4 * scale, cy - 6 * scale,
               cx + 6 * scale, cy - 2 * scale],
              fill=COLORS['black'], width=int(1 * scale))
    
    return img

def draw_shark(size=64):
    """绘制深深海鲨"""
    img = Image.new('RGBA', (size, size), COLORS['transparent'])
    draw = ImageDraw.Draw(img)
    
    cx, cy = size // 2, size // 2
    scale = size / 64
    
    # 流线型身体
    body_points = [
        (cx - 20 * scale, cy),
        (cx - 8 * scale, cy - 8 * scale),
        (cx + 8 * scale, cy - 8 * scale),
        (cx + 14 * scale, cy),
        (cx + 8 * scale, cy + 8 * scale),
        (cx - 8 * scale, cy + 8 * scale),
    ]
    draw.polygon(body_points, fill=COLORS['shark'])
    
    # 背鳍
    fin_points = [
        (cx - 4 * scale, cy - 8 * scale),
        (cx, cy - 16 * scale),
        (cx + 4 * scale, cy - 8 * scale),
    ]
    draw.polygon(fin_points, fill=COLORS['shark'])
    
    # 西装领子
    draw.line([cx - 4 * scale, cy + 8 * scale,
               cx, cy + 12 * scale],
              fill=(50, 50, 50), width=int(3 * scale))
    draw.line([cx + 4 * scale, cy + 8 * scale,
               cx, cy + 12 * scale],
              fill=(50, 50, 50), width=int(3 * scale))
    
    # 眼睛（友好但专业）
    draw.ellipse([cx + 6 * scale, cy - 4 * scale,
                  cx + 10 * scale, cy - 1 * scale], fill=COLORS['white'])
    draw.ellipse([cx + 7 * scale, cy - 3 * scale,
                  cx + 9 * scale, cy - 2 * scale], fill=COLORS['black'])
    
    return img

def draw_coral_fish(size=64):
    """绘制珊瑚鱼"""
    img = Image.new('RGBA', (size, size), COLORS['transparent'])
    draw = ImageDraw.Draw(img)
    
    cx, cy = size // 2, size // 2
    scale = size / 64
    
    # 椭圆身体
    body_bbox = [cx - 12 * scale, cy - 8 * scale,
                 cx + 12 * scale, cy + 8 * scale]
    draw.ellipse(body_bbox, fill=COLORS['coral_fish'])
    
    # 彩色条纹
    for i in range(3):
        stripe_x = cx - 6 * scale + i * 6 * scale
        draw.line([stripe_x, cy - 8 * scale,
                   stripe_x, cy + 8 * scale],
                  fill=COLORS['white'], width=int(2 * scale))
    
    # 鱼鳍
    draw.ellipse([cx - 4 * scale, cy - 12 * scale,
                  cx + 4 * scale, cy - 8 * scale],
                 fill=COLORS['coral_fish'])
    
    # 尾巴
    tail_points = [
        (cx + 12 * scale, cy),
        (cx + 18 * scale, cy - 6 * scale),
        (cx + 18 * scale, cy + 6 * scale),
    ]
    draw.polygon(tail_points, fill=COLORS['coral_fish'])
    
    # 眼睛
    draw.ellipse([cx - 8 * scale, cy - 3 * scale,
                  cx - 5 * scale, cy], fill=COLORS['white'])
    draw.ellipse([cx - 7 * scale, cy - 2 * scale,
                  cx - 6 * scale, cy - 1 * scale], fill=COLORS['black'])
    
    return img

def draw_background(size=512):
    """绘制海底背景（可平铺）"""
    img = Image.new('RGBA', (size, size), (0, 0, 51, 255))
    draw = ImageDraw.Draw(img)
    
    # 渐变背景
    for y in range(size):
        ratio = y / size
        r = int(0 + ratio * 30)
        g = int(0 + ratio * 50)
        b = int(51 + ratio * 100)
        draw.line([(0, y), (size, y)], fill=(r, g, b))
    
    # 海草
    for i in range(8):
        x = i * 64 + 32
        for j in range(3):
            grass_x = x + j * 8 - 8
            draw.line([grass_x, size - 20,
                       grass_x + 10, size - 60 - j * 20],
                      fill=COLORS['seaweed_green'], width=4)
    
    # 珊瑚礁
    coral_positions = [(100, size - 40), (300, size - 50), (450, size - 35)]
    for cx, cy in coral_positions:
        draw.ellipse([cx - 20, cy - 20, cx + 20, cy + 20],
                     fill=COLORS['coral_orange'])
    
    # 气泡
    for i in range(15):
        bx = (i * 37) % size
        by = (i * 23) % (size - 100) + 50
        draw.ellipse([bx - 3, by - 3, bx + 3, by + 3],
                     outline=(200, 240, 255, 150), width=1)
    
    return img

def draw_seashell_button(width=200, height=60, state='normal'):
    """绘制贝壳风格按钮 - 抽象有特色"""
    if state == 'normal':
        color = COLORS['ui_button']
        border_color = (100, 180, 220)
        glow = (255, 255, 255, 100)
    elif state == 'hover':
        color = COLORS['ui_button_hover']
        border_color = (150, 220, 255)
        glow = (255, 255, 255, 180)
    else:  # pressed / disabled
        color = (50, 100, 150)
        border_color = (80, 140, 180)
        glow = (255, 255, 255, 50)
    
    img = Image.new('RGBA', (width, height), COLORS['transparent'])
    draw = ImageDraw.Draw(img)
    
    # 贝壳形状 - 上边缘波浪形
    margin = 4
    points = []
    # 顶部波浪
    wave_count = 5
    for i in range(wave_count + 1):
        x = margin + (width - 2 * margin) * i / wave_count
        y = margin + (8 if i % 2 == 0 else 0)
        points.append((x, y))
    
    # 右侧圆弧
    points.append((width - margin, height // 2))
    
    # 底部波浪 (反向)
    for i in range(wave_count, -1, -1):
        x = margin + (width - 2 * margin) * i / wave_count
        y = height - margin - (8 if i % 2 == 0 else 0)
        points.append((x, y))
    
    # 左侧圆弧
    points.append((margin, height // 2))
    
    draw.polygon(points, fill=color)
    
    # 发光边框
    draw.polygon(points, outline=border_color, width=3)
    
    # 内部高光 (珍珠光泽)
    highlight = Image.new('RGBA', (width, height), COLORS['transparent'])
    hdraw = ImageDraw.Draw(highlight)
    hdraw.ellipse([margin + 10, margin + 10, margin + 40, margin + 30], 
                  fill=glow)
    img = Image.alpha_composite(img, highlight)
    
    # 珍珠装饰点
    pearl_positions = [(margin + 5, height // 2 - 10), 
                       (width - margin - 5, height // 2 + 10)]
    for px, py in pearl_positions:
        draw.ellipse([px - 3, py - 3, px + 3, py + 3], fill=(255, 255, 255, 200))
    
    return img

def draw_icon(icon_type, size=32):
    """绘制 UI 图标 - 抽象海洋风格"""
    img = Image.new('RGBA', (size, size), COLORS['transparent'])
    draw = ImageDraw.Draw(img)
    
    cx, cy = size // 2, size // 2
    scale = size / 32
    
    # 添加背景光晕
    glow = Image.new('RGBA', (size, size), COLORS['transparent'])
    gdraw = ImageDraw.Draw(glow)
    gdraw.ellipse([cx - 14 * scale, cy - 14 * scale,
                   cx + 14 * scale, cy + 14 * scale],
                  fill=(255, 255, 255, 30))
    img = Image.alpha_composite(img, glow)
    
    if icon_type == 'coin':
        # 金币 - 贝壳中的珍珠
        draw.ellipse([cx - 12 * scale, cy - 12 * scale,
                      cx + 12 * scale, cy + 12 * scale],
                     fill=COLORS['gold'])
        # 贝壳纹理
        for i in range(5):
            angle = i * 72 * 3.14159 / 180
            x1 = cx + 8 * scale * math.cos(angle)
            y1 = cy + 8 * scale * math.sin(angle)
            x2 = cx + 12 * scale * math.cos(angle)
            y2 = cy + 12 * scale * math.sin(angle)
            draw.line([(x1, y1), (x2, y2)], fill=(200, 170, 0), width=int(2*scale))
        # 珍珠光泽
        draw.ellipse([cx - 5 * scale, cy - 5 * scale,
                      cx - 2 * scale, cy - 2 * scale],
                     fill=(255, 255, 255, 200))
    
    elif icon_type == 'diamond':
        # 钻石 - 水晶气泡
        diamond_points = [
            (cx, cy - 14 * scale),
            (cx + 10 * scale, cy - 4 * scale),
            (cx, cy + 14 * scale),
            (cx - 10 * scale, cy - 4 * scale),
        ]
        draw.polygon(diamond_points, fill=COLORS['diamond'])
        # 水晶切面
        draw.line([(cx, cy - 14 * scale), (cx, cy + 14 * scale)], 
                  fill=(200, 250, 250, 150), width=int(1*scale))
        draw.line([(cx - 10 * scale, cy - 4 * scale), 
                   (cx + 10 * scale, cy - 4 * scale)],
                  fill=(200, 250, 250, 150), width=int(1*scale))
        # 高光
        draw.ellipse([cx - 4 * scale, cy - 8 * scale,
                      cx - 2 * scale, cy - 6 * scale],
                     fill=(255, 255, 255, 220))
    
    elif icon_type == 'heart':
        # 爱心 - 海星形状
        for i in range(5):
            angle = (i * 72 - 18) * math.pi / 180
            inner_angle = ((i + 0.5) * 72 - 18) * math.pi / 180
            x1 = cx + 12 * scale * math.cos(angle)
            y1 = cy + 12 * scale * math.sin(angle)
            x2 = cx + 6 * scale * math.cos(inner_angle)
            y2 = cy + 6 * scale * math.sin(inner_angle)
            x3 = cx + 12 * scale * math.cos((i + 1) * 72 * math.pi / 180 - 18 * math.pi / 180)
            y3 = cy + 12 * scale * math.sin((i + 1) * 72 * math.pi / 180 - 18 * math.pi / 180)
            draw.polygon([(x1, y1), (x2, y2), (x3, y3)], fill=(255, 105, 180))
        # 中心光泽
        draw.ellipse([cx - 3 * scale, cy - 3 * scale,
                      cx + 3 * scale, cy + 3 * scale],
                     fill=(255, 180, 200))
    
    elif icon_type == 'star':
        # 星星 - 发光海星
        points = []
        for i in range(10):
            angle = math.pi / 2 + i * math.pi / 5
            r = (12 if i % 2 == 0 else 5) * scale
            x = cx + r * math.cos(angle)
            y = cy - r * math.sin(angle)
            points.append((x, y))
        draw.polygon(points, fill=COLORS['gold'])
        # 发光效果
        for i in range(5):
            angle = (i * 72) * math.pi / 180
            x = cx + 10 * scale * math.cos(angle)
            y = cy + 10 * scale * math.sin(angle)
            draw.ellipse([x - 2 * scale, y - 2 * scale,
                          x + 2 * scale, y + 2 * scale],
                         fill=(255, 255, 255, 180))
    
    elif icon_type == 'play':
        # 播放 - 游动的鱼
        fish_body = [
            (cx - 8 * scale, cy),
            (cx + 12 * scale, cy - 6 * scale),
            (cx + 12 * scale, cy + 6 * scale),
        ]
        draw.polygon(fish_body, fill=COLORS['white'])
        # 鱼尾
        draw.line([(cx - 8 * scale, cy), (cx - 14 * scale, cy - 5 * scale)],
                  fill=COLORS['white'], width=int(3*scale))
        draw.line([(cx - 8 * scale, cy), (cx - 14 * scale, cy + 5 * scale)],
                  fill=COLORS['white'], width=int(3*scale))
        # 眼睛
        draw.ellipse([cx + 6 * scale, cy - 2 * scale,
                      cx + 8 * scale, cy], fill=(50, 50, 50))
    
    elif icon_type == 'pause':
        # 暂停 - 两块珊瑚
        # 左珊瑚
        draw.rounded_rectangle([cx - 12 * scale, cy - 12 * scale,
                                cx - 4 * scale, cy + 12 * scale],
                               radius=int(3*scale), fill=COLORS['coral_orange'])
        # 右珊瑚
        draw.rounded_rectangle([cx + 4 * scale, cy - 12 * scale,
                                cx + 12 * scale, cy + 12 * scale],
                               radius=int(3*scale), fill=COLORS['coral_orange'])
        # 珊瑚纹理
        for i in range(3):
            y = cy - 8 * scale + i * 8 * scale
            draw.line([(cx - 10 * scale, y), (cx - 6 * scale, y)],
                      fill=(200, 100, 50), width=int(2*scale))
            draw.line([(cx + 6 * scale, y), (cx + 10 * scale, y)],
                      fill=(200, 100, 50), width=int(2*scale))
    
    elif icon_type == 'settings':
        # 设置 - 海螺螺旋
        spiral_points = []
        for i in range(20):
            angle = i * 36 * math.pi / 180
            r = (2 + i * 0.6) * scale
            x = cx + r * math.cos(angle)
            y = cy + r * math.sin(angle)
            spiral_points.append((x, y))
        
        if len(spiral_points) > 1:
            draw.line(spiral_points, fill=(180, 140, 100), width=int(3*scale))
        
        # 中心装饰
        draw.ellipse([cx - 3 * scale, cy - 3 * scale,
                      cx + 3 * scale, cy + 3 * scale],
                     fill=(200, 160, 120))
    
    elif icon_type == 'package':
        # 包裹 - 礼品盒
        draw.rectangle([cx - 10 * scale, cy - 10 * scale,
                        cx + 10 * scale, cy + 10 * scale],
                       fill=(210, 180, 140))
        # 丝带
        draw.rectangle([cx - 3 * scale, cy - 10 * scale,
                        cx + 3 * scale, cy + 10 * scale],
                       fill=(255, 100, 100))
        draw.rectangle([cx - 10 * scale, cy - 3 * scale,
                        cx + 10 * scale, cy + 3 * scale],
                       fill=(255, 100, 100))
        # 蝴蝶结
        draw.ellipse([cx - 6 * scale, cy - 8 * scale,
                      cx - 2 * scale, cy - 4 * scale],
                     fill=(255, 80, 80))
        draw.ellipse([cx + 2 * scale, cy - 8 * scale,
                      cx + 6 * scale, cy - 4 * scale],
                     fill=(255, 80, 80))
    
    elif icon_type == 'map':
        # 地图 - 藏宝图
        draw.rectangle([cx - 12 * scale, cy - 9 * scale,
                        cx + 12 * scale, cy + 9 * scale],
                       fill=(210, 180, 140))
        # 卷边效果
        draw.arc([cx + 6 * scale, cy + 3 * scale,
                  cx + 14 * scale, cy + 11 * scale],
                 180, 270, fill=(180, 150, 110), width=int(2*scale))
        # X 标记
        draw.line([(cx - 4 * scale, cy - 4 * scale),
                   (cx, cy)], fill=(255, 100, 100), width=int(2*scale))
        draw.line([(cx, cy - 4 * scale),
                   (cx - 4 * scale, cy)], fill=(255, 100, 100), width=int(2*scale))
    
    return img

def generate_character_sprites():
    """生成所有角色精灵"""
    print("生成角色精灵...")
    
    characters = {
        'lobster': draw_lobster,
        'starfish': draw_starfish,
        'octopus': draw_octopus,
        'crab': draw_crab,
        'seahorse': draw_seahorse,
        'jellyfish': draw_jellyfish,
        'turtle': draw_turtle,
        'shark': draw_shark,
        'coral_fish': draw_coral_fish,
    }
    
    for name, draw_func in characters.items():
        # 生成 4 个方向的精灵（简化为相同，后续可扩展）
        for direction in ['down', 'up', 'left', 'right']:
            sprite = draw_func(64)
            sprite.save(os.path.join(OUTPUT_DIR, 'characters', f'{name}_{direction}.png'))
        
        # 生成 idle 动画帧（3 帧轻微变化）
        for frame in range(3):
            sprite = draw_func(64)
            # 轻微呼吸效果
            if frame == 1:
                sprite = sprite.resize((66, 66), Image.Resampling.LANCZOS).resize((64, 64), Image.Resampling.LANCZOS)
            elif frame == 2:
                sprite = sprite.resize((62, 62), Image.Resampling.LANCZOS).resize((64, 64), Image.Resampling.LANCZOS)
            sprite.save(os.path.join(OUTPUT_DIR, 'characters', f'{name}_idle_{frame}.png'))
        
        print(f"  ✓ {name}")

def generate_environment_assets():
    """生成环境资产"""
    print("生成环境资产...")
    
    # 背景
    bg = draw_background(512)
    bg.save(os.path.join(OUTPUT_DIR, 'environment', 'seabed_background.png'))
    print("  ✓ 背景")
    
    # 障碍物
    obstacles = {
        'coral_reef': (COLORS['coral_orange'], 64, 64),
        'seaweed': (COLORS['seaweed_green'], 32, 96),
        'rock': ((105, 105, 105), 48, 48),
        'shipwreck': ((139, 69, 19), 128, 64),
        'shell': (COLORS['gold'], 32, 32),
    }
    
    for name, (color, w, h) in obstacles.items():
        img = Image.new('RGBA', (w, h), COLORS['transparent'])
        draw = ImageDraw.Draw(img)
        
        if name == 'coral_reef':
            draw.ellipse([4, 4, w - 4, h - 4], fill=color)
        elif name == 'seaweed':
            draw.line([w // 2, h, w // 2 + 8, 0], fill=color, width=8)
        elif name == 'rock':
            draw.ellipse([2, 2, w - 2, h - 2], fill=color)
        elif name == 'shipwreck':
            draw.rectangle([4, h // 2, w - 4, h - 4], fill=color)
            draw.polygon([(w // 2, 4), (4, h // 2), (w - 4, h // 2)], fill=color)
        elif name == 'shell':
            draw.ellipse([2, 2, w - 2, h - 2], fill=color)
        
        img.save(os.path.join(OUTPUT_DIR, 'environment', f'{name}.png'))
        print(f"  ✓ {name}")

def generate_ui_assets():
    """生成 UI 资产 - 抽象海洋风格"""
    print("生成 UI 资产...")
    
    # 按钮（3 状态 × 2 尺寸）- 使用贝壳风格
    sizes = [(200, 60), (150, 50)]
    states = ['normal', 'hover', 'pressed']
    
    for w, h in sizes:
        for state in states:
            btn = draw_seashell_button(w, h, state)
            btn.save(os.path.join(OUTPUT_DIR, 'ui', f'button_shell_{w}x{h}_{state}.png'))
    print("  ✓ 贝壳按钮")
    
    # 图标 - 新增 package 和 map
    icons = ['coin', 'diamond', 'heart', 'star', 'play', 'pause', 'settings', 'package', 'map']
    sizes = [32, 64]
    
    for icon in icons:
        for size in sizes:
            icon_img = draw_icon(icon, size)
            icon_img.save(os.path.join(OUTPUT_DIR, 'ui', f'icon_{icon}_{size}x{size}.png'))
    print("  ✓ 图标 (海洋风格)")
    
    # 面板背景 - 海浪风格
    panel = Image.new('RGBA', (400, 300), COLORS['transparent'])
    draw = ImageDraw.Draw(panel)
    
    # 渐变背景
    for y in range(300):
        ratio = y / 300
        alpha = int(200 + ratio * 55)
        draw.line([(0, y), (400, y)], fill=(0, 30, 60, alpha))
    
    # 顶部海浪边框
    wave_points = [(0, 10)]
    for i in range(20):
        x = i * 20
        y = 10 + (8 if i % 2 == 0 else 0)
        wave_points.append((x, y))
        wave_points.append((x + 10, 10 + (0 if i % 2 == 0 else 8)))
    wave_points.append((400, 10))
    wave_points.append((400, 0))
    wave_points.append((0, 0))
    draw.polygon(wave_points, fill=(0, 50, 100, 200))
    
    # 底部海浪边框 (镜像)
    wave_points_bottom = [(0, 290)]
    for i in range(20):
        x = i * 20
        y = 290 + (8 if i % 2 == 0 else 0)
        wave_points_bottom.append((x, min(y, 300)))
        wave_points_bottom.append((x + 10, 290 + (0 if i % 2 == 0 else 8)))
    wave_points_bottom.append((400, 290))
    wave_points_bottom.append((400, 300))
    wave_points_bottom.append((0, 300))
    draw.polygon(wave_points_bottom, fill=(0, 50, 100, 200))
    
    # 珍珠装饰角
    corners = [(15, 15), (385, 15), (15, 285), (385, 285)]
    for cx, cy in corners:
        draw.ellipse([cx - 8, cy - 8, cx + 8, cy + 8],
                     fill=(255, 255, 255, 180))
        draw.ellipse([cx - 4, cy - 4, cx + 4, cy + 4],
                     fill=(255, 255, 255, 255))
    
    # 边框
    draw.rectangle([5, 5, 395, 295], outline=COLORS['ocean_blue'], width=2)
    
    panel.save(os.path.join(OUTPUT_DIR, 'ui', 'panel_waves.png'))
    print("  ✓ 海浪面板")
    
    # 状态条背景 (生命/体力)
    bar_bg = Image.new('RGBA', (200, 25), COLORS['transparent'])
    bar_draw = ImageDraw.Draw(bar_bg)
    bar_draw.rounded_rectangle([0, 0, 200, 25], radius=12, fill=(0, 40, 80, 200))
    bar_draw.rounded_rectangle([2, 2, 198, 23], radius=10, outline=(100, 180, 220), width=2)
    bar_bg.save(os.path.join(OUTPUT_DIR, 'ui', 'bar_bg.png'))
    
    # 状态条填充 (生命 - 心形)
    bar_fill_heart = Image.new('RGBA', (200, 25), COLORS['transparent'])
    fill_draw = ImageDraw.Draw(bar_fill_heart)
    fill_draw.rounded_rectangle([2, 2, 150, 23], radius=10, fill=(255, 105, 180))
    bar_fill_heart.save(os.path.join(OUTPUT_DIR, 'ui', 'bar_fill_heart.png'))
    
    # 状态条填充 (体力 - 气泡)
    bar_fill_stamina = Image.new('RGBA', (200, 25), COLORS['transparent'])
    fill_draw = ImageDraw.Draw(bar_fill_stamina)
    fill_draw.rounded_rectangle([2, 2, 150, 23], radius=10, 
                                fill=(100, 200, 255, 180))
    # 气泡效果
    for i in range(5):
        bx = 20 + i * 25
        fill_draw.ellipse([bx - 6, 5, bx + 6, 17],
                         fill=(255, 255, 255, 100))
    bar_fill_stamina.save(os.path.join(OUTPUT_DIR, 'ui', 'bar_fill_stamina.png'))
    print("  ✓ 状态条")
    
    # 成就徽章
    badge = Image.new('RGBA', (80, 80), COLORS['transparent'])
    badge_draw = ImageDraw.Draw(badge)
    # 金色外圈
    badge_draw.ellipse([2, 2, 78, 78], fill=COLORS['gold'])
    # 银色内圈
    badge_draw.ellipse([8, 8, 72, 72], fill=(200, 200, 200))
    # 星星
    import math
    star_points = []
    for i in range(10):
        angle = math.pi / 2 + i * math.pi / 5
        r = 20 if i % 2 == 0 else 10
        x = 40 + r * math.cos(angle)
        y = 40 - r * math.sin(angle)
        star_points.append((x, y))
    badge_draw.polygon(star_points, fill=COLORS['gold'])
    badge.save(os.path.join(OUTPUT_DIR, 'ui', 'badge_achievement.png'))
    print("  ✓ 成就徽章")

def main():
    print("=" * 50)
    print("🦞 龙虾快递员 - 美术资源生成器")
    print("=" * 50)
    
    ensure_dirs()
    
    generate_character_sprites()
    generate_environment_assets()
    generate_ui_assets()
    
    print("=" * 50)
    print("✅ 所有资源生成完成!")
    print(f"📁 输出目录：{OUTPUT_DIR}")
    print("=" * 50)

if __name__ == '__main__':
    main()
