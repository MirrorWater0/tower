shader_type canvas_item;

#define t TIME
#define R SCREEN_PIXEL_SIZE
void fragment() {
	vec4 tex = texture(TEXTURE,UV);
    // Position & Aspect ratio
    float aspect = R.x / R.y;
    vec2 uv = UV;
    uv.x *= aspect;
    vec2 center = vec2(0.5 * aspect, 0.5);

    float dist = length(uv - center);

    // Modulate UV with distance from center & time
    uv += cos(dist * 10. - t) * (cos(t) + .5);

    // Brightness inversely proportional to distance from center
    vec3 c = vec3(.01 / length(uv - center)) / dist;

    COLOR = vec4(c, 1.);
	//COLOR.a = (COLOR.r +COLOR.b+COLOR.g)/3.;
	COLOR = COLOR*tex;
}