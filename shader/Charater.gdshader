shader_type canvas_item;

// 模糊参数
uniform float blur_amount : hint_range(0.0, 10.0) = 2.0;
uniform float time_scale : hint_range(0.0, 5.0) = 1.0;
uniform float pulse_speed : hint_range(0.0, 5.0) = 1.0;
uniform vec2 center = vec2(0.5, 0.5);
uniform bool radial_blur = true;
uniform bool motion_blur = true;
