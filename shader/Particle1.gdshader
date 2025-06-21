
shader_type canvas_item;
render_mode blend_add; // 加色混合增强发光效果

// 可调参数
uniform float speed = 1.0;          // 动画速度
uniform vec3 base_color = vec3(0.1, 0.3, 1.0); // 基础颜色（蓝）
uniform float intensity = 2.0;      // 光强
uniform sampler2D noise_tex;         // 噪声纹理（需外部传入）

void fragment() {
    vec2 uv = 1.5*UV;
    float time = TIME * speed;

    // 噪声采样（动态扰动）
    vec2 noise_uv = uv * 3.0 + vec2(time * 0.2, time * 0.3);
    float noise = texture(noise_tex, noise_uv).r;

    // 中心向外的径向渐变
    vec2 center = vec2(0.75);
    float dist = distance(uv, center);
    float glow = 1.0 - smoothstep(0.3, 0.7, dist); // 中心亮、边缘淡

    // 动态波纹（基于距离+噪声）
    float ripple = sin(dist * 15.0 - time * 4.0) * 0.1 * noise;
    float alpha = glow * (0.5 + ripple); // 叠加扰动透明度

    // 边缘锐化（外发光）
    float edge = pow(1.0 - dist, 4.0) * intensity;

    // 颜色混合（蓝→青渐变）
    vec3 color = mix(base_color, base_color.grb, dist);
    color *= edge + alpha;

    COLOR = vec4(color, alpha);
}