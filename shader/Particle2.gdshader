shader_type canvas_item;
#define iTime TIME
#define NUM_OCTAVES 5
#define iResolution (1./SCREEN_PIXEL_SIZE)
#define fragCoord ((UV * iResolution).xy)
#define fragColor COLOR

float circle(vec2 uv,float r,vec2 p){
	return 1. - smoothstep(r,r+0.0008,length(uv-p));
}

float trail(vec2 uv,float r,vec2 p,vec2 v){
	float col = 0.;
	for(int j = 0;j <80;j++){
		float newr = r/100.*float(j);
		vec2 newp = p+ .00005*v*float(j);
		col += circle(uv,newr,newp)/100.*float(j);
	}
	return col;
}
void fragment()
{
	vec2 uv = UV;
	vec2 v = 10.*vec2(1.,2.);
	vec2 p = vec2(0.5);
	float speed = 0.1;
	p.y = v.y*fract(p.y+TIME/2.);
	p.x = v.x*fract(p.x + TIME/2.);
	p*= speed;
	COLOR = vec4(vec3(1.),trail(uv,0.005,p,v));
}