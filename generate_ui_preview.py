#!/usr/bin/env python3
"""
龙虾快递员 - UI 预览图生成器
展示抽象海洋风格的 UI 设计
"""

from PIL import Image, ImageDraw, ImageFont
import os

COLORS = {
    'ocean_blue': (74, 144, 200),
    'deep_blue': (0, 30, 60),
    'coral_orange': (255, 140, 66),
    'gold': (255, 215, 0),
    'white': (255, 255, 255),
    'pink': (255, 105, 180),
    'cyan': (100, 200, 255),
}

OUTPUT_DIR = os.path.dirname(__file__)

def create_seashell_button(draw, x, y, width=180, height=55, text="按钮", state='normal'):
    """绘制贝壳按钮"""
    if state == 'normal':
        color = (87, 206, 235)
        border = (100, 180, 220)
    elif state == 'hover':
        color = (0, 191, 255)
        border = (150, 220, 255)
    else:
        color = (50, 100, 150)
        border = (80, 140, 180)
    
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
    draw.ellipse([x + margin + 15, y + margin + 10, 
                  x + margin + 35, y + margin + 25],
                 fill=(255, 255, 255, 120))
    
    # 文字
    try:
        font = ImageFont.truetype("/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf", 20)
    except:
        font = ImageFont.load_default()
    
    text_bbox = draw.textbbox((0, 0), text, font=font)
    text_width = text_bbox[2] - text_bbox[0]
    text_x = x + (width - text_width) // 2
    text_y = y + (height - 20) // 2
    draw.text((text_x, text_y), text, fill=COLORS['white'], font=font)

def create_ocean_icon(draw, x, y, icon_type, size=60):
    """绘制海洋风格图标"""
    cx, cy = x + size // 2, y + size // 2
    s = size / 64
    
    # 背景光晕
    draw.ellipse([cx - 28*s, cy - 28*s, cx + 28*s, cy + 28*s],
                 fill=(255, 255, 255, 25))
    
    if icon_type == 'coin':
        # 金币 - 珍珠贝壳
        draw.ellipse([cx - 22*s, cy - 22*s, cx + 22*s, cy + 22*s],
                     fill=COLORS['gold'])
        # 贝壳纹理
        import math
        for i in range(5):
            angle = i * 72 * math.pi / 180
            x1 = cx + 15*s * math.cos(angle)
            y1 = cy + 15*s * math.sin(angle)
            x2 = cx + 22*s * math.cos(angle)
            y2 = cy + 22*s * math.sin(angle)
            draw.line([(x1, y1), (x2, y2)], fill=(200, 170, 0), width=int(3*s))
        # 珍珠光泽
        draw.ellipse([cx - 10*s, cy - 10*s, cx - 4*s, cy - 4*s],
                     fill=(255, 255, 255, 200))
    
    elif icon_type == 'diamond':
        # 钻石 - 水晶气泡
        points = [(cx, cy - 25*s), (cx + 18*s, cy - 8*s),
                  (cx, cy + 25*s), (cx - 18*s, cy - 8*s)]
        draw.polygon(points, fill=COLORS['cyan'])
        # 切面
        draw.line([(cx, cy - 25*s), (cx, cy + 25*s)],
                  fill=(200, 250, 250, 150), width=int(2*s))
        draw.line([(cx - 18*s, cy - 8*s), (cx + 18*s, cy - 8*s)],
                  fill=(200, 250, 250, 150), width=int(2*s))
        # 高光
        draw.ellipse([cx - 8*s, cy - 15*s, cx - 4*s, cy - 11*s],
                     fill=(255, 255, 255, 220))
    
    elif icon_type == 'heart':
        # 爱心 - 海星形状
        import math
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
        # 中心
        draw.ellipse([cx - 6*s, cy - 6*s, cx + 6*s, cy + 6*s],
                     fill=(255, 180, 200))
    
    elif icon_type == 'star':
        # 星星 - 发光海星
        import math
        points = []
        for i in range(10):
            angle = math.pi / 2 + i * math.pi / 5
            r = (22 if i % 2 == 0 else 9) * s
            x = cx + r * math.cos(angle)
            y = cy - r * math.sin(angle)
            points.append((x, y))
        draw.polygon(points, fill=COLORS['gold'])
        # 发光点
        for i in range(5):
            angle = (i * 72) * math.pi / 180
            px = cx + 18*s * math.cos(angle)
            py = cy + 18*s * math.sin(angle)
            draw.ellipse([px - 4*s, py - 4*s, px + 4*s, py + 4*s],
                         fill=(255, 255, 255, 180))
    
    elif icon_type == 'package':
        # 包裹 - 礼品盒
        draw.rectangle([cx - 18*s, cy - 18*s, cx + 18*s, cy + 18*s],
                       fill=(210, 180, 140))
        # 丝带
        draw.rectangle([cx - 5*s, cy - 18*s, cx + 5*s, cy + 18*s],
                       fill=(255, 100, 100))
        draw.rectangle([cx - 18*s, cy - 5*s, cx + 18*s, cy + 5*s],
                       fill=(255, 100, 100))
        # 蝴蝶结
        draw.ellipse([cx - 11*s, cy - 15*s, cx - 4*s, cy - 8*s],
                     fill=(255, 80, 80))
        draw.ellipse([cx + 4*s, cy - 15*s, cx + 11*s, cy - 8*s],
                     fill=(255, 80, 80))

def create_wave_panel(draw, x, y, width=380, height=280):
    """绘制海浪面板"""
    # 渐变背景
    for py in range(height):
        ratio = py / height
        alpha = int(180 + ratio * 50)
        draw.line([(x, y + py), (x + width, y + py)],
                  fill=(0, 30, 60, alpha))
    
    # 顶部海浪
    wave_points = [(x, y + 10)]
    for i in range(19):
        px = x + i * 20
        wave_points.append((px, y + 10 + (8 if i % 2 == 0 else 0)))
        wave_points.append((px + 10, y + 10 + (0 if i % 2 == 0 else 8)))
    wave_points.append((x + width, y + 10))
    wave_points.append((x + width, y))
    wave_points.append((x, y))
    draw.polygon(wave_points, fill=(0, 50, 100, 200))
    
    # 底部海浪
    wave_points = [(x, y + height - 10)]
    for i in range(19):
        px = x + i * 20
        wave_points.append((px, y + height - 10 + (8 if i % 2 == 0 else 0)))
        wave_points.append((px + 10, y + height - 10 + (0 if i % 2 == 0 else 8)))
    wave_points.append((x + width, y + height - 10))
    wave_points.append((x + width, y + height))
    wave_points.append((x, y + height))
    draw.polygon(wave_points, fill=(0, 50, 100, 200))
    
    # 珍珠角
    corners = [(x + 15, y + 15), (x + width - 15, y + 15),
               (x + 15, y + height - 15), (x + width - 15, y + height - 15)]
    for cx, cy in corners:
        draw.ellipse([cx - 8, cy - 8, cx + 8, cy + 8], fill=(255, 255, 255, 180))
        draw.ellipse([cx - 4, cy - 4, cx + 4, cy + 4], fill=(255, 255, 255, 255))
    
    # 边框
    draw.rectangle([x + 5, y + 5, x + width - 5, y + height - 5],
                   outline=COLORS['ocean_blue'], width=2)

def create_bar(draw, x, y, bar_type='heart', fill_percent=0.75):
    """绘制状态条"""
    width, height = 200, 25
    
    # 背景
    draw.rounded_rectangle([x, y, x + width, y + height],
                           radius=12, fill=(0, 40, 80, 200))
    draw.rounded_rectangle([x + 2, y + 2, x + width - 2, y + height - 2],
                           radius=10, outline=(100, 180, 220), width=2)
    
    # 填充
    fill_width = int((width - 8) * fill_percent)
    if bar_type == 'heart':
        fill_color = COLORS['pink']
    else:
        fill_color = (100, 200, 255, 180)
    
    draw.rounded_rectangle([x + 4, y + 4, x + 4 + fill_width, y + height - 4],
                           radius=8, fill=fill_color)
    
    # 气泡效果 (体力条)
    if bar_type == 'stamina':
        for i in range(4):
            bx = x + 15 + i * 22
            draw.ellipse([bx - 5, y + 7, bx + 5, y + 17],
                         fill=(255, 255, 255, 100))

def create_ui_showcase():
    """生成 UI 展示图"""
    print("🎨 正在生成 UI 预览图...")
    
    width, height = 1600, 1200
    img = Image.new('RGBA', (width, height), (10, 20, 40, 255))
    draw = ImageDraw.Draw(img)
    
    # 标题
    try:
        title_font = ImageFont.truetype("/usr/share/fonts/truetype/dejavu/DejaVuSans-Bold.ttf", 48)
        desc_font = ImageFont.truetype("/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf", 24)
    except:
        title_font = ImageFont.load_default()
        desc_font = ImageFont.load_default()
    
    title = "🦞 龙虾快递员 - UI 设计系统 v2.0"
    subtitle = "抽象海洋风格 · 贝壳按钮 · 海浪面板 · 珍珠装饰"
    
    draw.text((50, 40), title, fill=COLORS['white'], font=title_font)
    draw.text((50, 100), subtitle, fill=(150, 180, 220), font=desc_font)
    
    # 1. 贝壳按钮展示
    print("  - 绘制贝壳按钮...")
    btn_y = 180
    create_seashell_button(draw, 50, btn_y, text="正常", state='normal')
    create_seashell_button(draw, 260, btn_y, text="悬停", state='hover')
    create_seashell_button(draw, 470, btn_y, text="按下", state='pressed')
    create_seashell_button(draw, 50, btn_y + 80, text="开始游戏", state='normal')
    create_seashell_button(draw, 260, btn_y + 80, text="设置", state='normal')
    create_seashell_button(draw, 470, btn_y + 80, text="退出", state='normal')
    
    # 按钮标签
    draw.text((50, btn_y - 30), "贝壳按钮 (三种状态)", fill=(150, 180, 220), font=desc_font)
    
    # 2. 图标展示
    print("  - 绘制海洋图标...")
    icons = ['coin', 'diamond', 'heart', 'star', 'package']
    icon_y = 380
    draw.text((50, icon_y - 30), "海洋风格图标", fill=(150, 180, 220), font=desc_font)
    for i, icon in enumerate(icons):
        create_ocean_icon(draw, 50 + i * 100, icon_y, icon, size=70)
    
    # 3. 海浪面板
    print("  - 绘制海浪面板...")
    panel_y = 520
    draw.text((50, panel_y - 30), "海浪面板 (珍珠角装饰)", fill=(150, 180, 220), font=desc_font)
    create_wave_panel(draw, 50, panel_y)
    
    # 面板内文字
    try:
        panel_text_font = ImageFont.truetype("/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf", 20)
    except:
        panel_text_font = ImageFont.load_default()
    
    draw.text((70, panel_y + 60), "游戏信息面板", fill=COLORS['white'], font=panel_text_font)
    draw.text((70, panel_y + 100), "分数：1250", fill=(150, 180, 220), font=panel_text_font)
    draw.text((70, panel_y + 140), "关卡：3", fill=(150, 180, 220), font=panel_text_font)
    draw.text((70, panel_y + 180), "时间：02:35", fill=(150, 180, 220), font=panel_text_font)
    
    # 4. 状态条
    print("  - 绘制状态条...")
    bar_y = 850
    draw.text((50, bar_y - 30), "状态条 (生命/体力)", fill=(150, 180, 220), font=desc_font)
    draw.text((50, bar_y + 15), "生命", fill=COLORS['white'], font=panel_text_font)
    create_bar(draw, 150, bar_y + 10, 'heart', 0.8)
    draw.text((50, bar_y + 60), "体力", fill=COLORS['white'], font=panel_text_font)
    create_bar(draw, 150, bar_y + 55, 'stamina', 0.6)
    
    # 5. 成就徽章
    print("  - 绘制成就徽章...")
    badge_y = 980
    draw.text((50, badge_y - 30), "成就徽章", fill=(150, 180, 220), font=desc_font)
    
    # 绘制徽章
    for i in range(3):
        bx = 50 + i * 120
        # 外圈
        draw.ellipse([bx + 2, badge_y + 2, bx + 78, badge_y + 78], fill=COLORS['gold'])
        # 内圈
        draw.ellipse([bx + 8, badge_y + 8, bx + 72, badge_y + 72], fill=(200, 200, 200))
        # 星星
        import math
        star_points = []
        for j in range(10):
            angle = math.pi / 2 + j * math.pi / 5
            r = 20 if j % 2 == 0 else 10
            x = bx + 40 + r * math.cos(angle)
            y = badge_y + 40 - r * math.sin(angle)
            star_points.append((x, y))
        draw.polygon(star_points, fill=COLORS['gold'])
        
        # 标签
        labels = ["新手", "达人", "大师"]
        draw.text((bx + 15, badge_y + 85), labels[i], fill=COLORS['white'], font=panel_text_font)
    
    # 6. 设计说明
    print("  - 添加设计说明...")
    note_y = 1100
    draw.text((50, note_y), "设计理念:", fill=COLORS['coral_orange'], font=panel_text_font)
    draw.text((50, note_y + 30), "• 贝壳形状按钮 - 波浪边缘，珍珠光泽", fill=(180, 200, 220), font=panel_text_font)
    draw.text((50, note_y + 60), "• 海洋风格图标 - 珍珠贝壳金币，水晶气泡钻石，海星爱心", fill=(180, 200, 220), font=panel_text_font)
    draw.text((50, note_y + 90), "• 海浪面板 - 上下波浪边框，珍珠角装饰", fill=(180, 200, 220), font=panel_text_font)
    draw.text((50, note_y + 120), "• 气泡状态条 - 体力条中的气泡效果", fill=(180, 200, 220), font=panel_text_font)
    
    # 保存
    output_path = os.path.join(OUTPUT_DIR, 'ui_design_showcase.png')
    img.save(output_path, 'PNG', quality=95)
    print(f"✅ UI 预览图已保存：{output_path}")
    
    # 缩略图
    thumb = img.copy()
    thumb.thumbnail((800, 600))
    thumb_path = os.path.join(OUTPUT_DIR, 'ui_design_showcase_thumb.png')
    thumb.save(thumb_path, 'PNG')
    print(f"✅ 缩略图已保存：{thumb_path}")
    
    return output_path

if __name__ == '__main__':
    create_ui_showcase()
    print("\n🎨 UI 预览图生成完成！")
