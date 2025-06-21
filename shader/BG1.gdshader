shader_type canvas_item;
#define iTime TIME
#define NUM_OCTAVES 5
#define iResolution (1./SCREEN_PIXEL_SIZE)
#define fragCoord ((UV * iResolution).xy)
#define fragColor COLOR

uniform bool isADDtex = false;
uniform vec4 light = vec4(0);
void fragment()
{
vec4 tex = texture(TEXTURE,UV);
float t = iTime + 19.0;
    vec2 uv = 1. - (3.0 * fragCoord - iResolution.xy) / iResolution.y;

    vec3 ro = vec3(0.0, 0.0, -6.0);        // Ray origin (camera)
    vec3 rd = normalize(vec3(uv, 1.5));    // Ray direction

    float d = 0.0;  // Distance accumulator
    float i = 0.0;  // Iteration index
    vec3 col = vec3(0.0);

    // Raymarch loop — simulate volumetric smoke field
    for (int j = 0; j < 64; ++j) {
        vec3 p = ro + d * rd;
        vec3 q = p;

        // Turbulence - recursive offset
        for (float k = 1.0; k < 5.0; k += 1.0) {
            q += cos(q.yzx * k + vec3(t * 0.2, t * 0.3, t * 0.1)) / k;
        }

        float density = 0.2 / length(q.xy) * 0.5;  // Radial density falloff
        density *= exp(-0.2 * p.z);                // Depth fade
        col += vec3(0.7, 0.8, 1.0) * density * 0.03;

        d += 0.05 + 0.01 * sin(t + float(j));       // Dynamic step size
    }

    // Tonemap & finish
    col = tanh(col * 1.5);                          // Gentle tone mapping
    fragColor =light + vec4(col, 1.0)/1.5;

	if(isADDtex){
		fragColor.a = tex.a;
	}
}
