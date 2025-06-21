shader_type canvas_item;
varying vec4 modulate;
void vertex(){
	modulate = COLOR;
}
vec2 roate(vec2 v,float theta,vec2 p){
	return mat2(vec2(sin(theta),cos(theta)),
	            vec2(-cos(theta),sin(theta)))*(v-p);
}
float circle(vec2 uv,float r,vec2 p){
	return 1. - smoothstep(r,r+0.008,length(uv-p));
}

float rect(vec2 st,vec2 size,vec2 p){
	st -= p;                     // 中心点归零
    st = abs(st) - size * 0.5;        // 计算轴向距离分量
    float sdf = length(max(st, 0.0))       // 外部距离
         + min(max(st.x, st.y), 0.0); // 内部距离修正
	return smoothstep(0.001,0.,sdf);
}

void fragment() {
	COLOR = vec4(1.);
	vec2 uv = UV;
	float col = rect(roate(uv,TIME,vec2(0.5)),vec2(0.15,0.15),vec2(0.1,0.35));
	col += rect(roate(uv,TIME/1.3,vec2(0.5)),vec2(0.2,0.2),vec2(0.1,0.2));
	col += rect(roate(uv,TIME/1.1,vec2(0.5)),vec2(0.12,0.12),vec2(0.22,0.22));
	col += rect(roate(uv,TIME/1.1,vec2(0.5)),vec2(0.08,0.08),vec2(0.12,0.25));
	COLOR.rgb = vec3(0.5,0.6,0.7+sin(TIME)/10.)+vec3(0.2);
	COLOR.a = col;
	COLOR *=modulate;
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
