shader_type canvas_item;

// 基础参数
uniform vec4 ring_color : source_color = vec4(0.0, 0.8, 1.0, 1.0); // 光环主色[1](@ref)
uniform vec4 edge_color : source_color = vec4(1.0, 0.5, 0.0, 1.0); // 流动边缘色
uniform float speed : hint_range(-5.0, 5.0) = 1.0;                 // 旋转速度
uniform float thickness : hint_range(0.01, 0.5) = 0.05;           // 环宽度[1](@ref)
uniform float noise_intensity : hint_range(0.0, 1.0) = 0.3;       // 噪声强度

// 流光参数
uniform float flow_width : hint_range(0.01, 0.3) = 0.1;          // 流光条宽度
uniform float flow_brightness : hint_range(1.0, 5.0) = 2.0;       // 流光亮度

// 噪声纹理
uniform sampler2D noise_tex : hint_default_white;                         // 噪波贴图[3](@ref)

void fragment() {
    // 中心点坐标标准化
    vec2 center = vec2(0.5);
    vec2 uv = UV - center;
    float angle = atan(uv.y, uv.x); // 当前像素的角度

    // 旋转动画（随时间变化角度）
    angle += TIME * speed;

    // 噪声扰动
    vec2 noise_uv = vec2(cos(angle), sin(angle)) * 0.5 + 0.5;
    float noise = texture(noise_tex, noise_uv + TIME * 0.2).r;
    angle += noise * noise_intensity * 0.5; // 噪声影响角度

    // 重建环形UV
    uv = vec2(cos(angle), sin(angle)) * length(uv);
    float dist = length(uv);

    // 基础圆环
    float ring = smoothstep(thickness, thickness * 0.95, dist);
    ring *= 1.0 - smoothstep(thickness * 0.95, thickness * 1.1, dist);

    // 流动边缘特效（核心）
    float flow_mask = fract(angle / (2.0 * 3.1415926) + TIME * 0.5); // 循环位置[6](@ref)
    float flow = smoothstep(0.0, flow_width, flow_mask);
    flow *= 1.0 - smoothstep(flow_width, flow_width * 1.2, flow_mask);

    // 颜色合成
    vec3 base_color = ring_color.rgb * ring;
    vec3 glow_color = edge_color.rgb * flow * flow_brightness;

    // 边缘叠加
    vec3 final_color = base_color + glow_color;
    float alpha = max(ring, flow * 0.7);

    COLOR = vec4(final_color, alpha * ring_color.a);
}