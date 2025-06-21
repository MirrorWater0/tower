shader_type canvas_item;
#define iTime TIME
#define NUM_OCTAVES 5
#define iResolution (1./SCREEN_PIXEL_SIZE)
#define fragCoord ((UV * iResolution).xy)
#define fragColor COLOR

#define PI 3.14159265359

vec2 normalizeScreenSpace(vec2 frag, out float aa,vec2 iResol) {
    aa = 0.003;
    return ( 2.*frag - iResol.xy )  / iResol.y;
}

mat2 rotate(float a) {
    a *= PI * 2.;
    float s = sin(a);
    float c = cos(a);
    return mat2(vec2(c, -s), vec2(s, c));
}

// Function from Iñigo Quiles - https://iquilezles.org/articles/distfunctions2d/
float dot2(in vec2 v ) { return dot(v,v); }
float sdTrapezoid( in vec2 p, in float r1, float r2, float he )
{
    vec2 k1 = vec2(r2,he);
    vec2 k2 = vec2(r2-r1,2.0*he);
    p.x = abs(p.x);
    vec2 ca = vec2(p.x-min(p.x,(p.y<0.0)?r1:r2), abs(p.y)-he);
    vec2 cb = p - k1 + k2*clamp( dot(k1-p,k2)/dot2(k2), 0.0, 1.0 );
    float s = (cb.x<0.0 && ca.y<0.0) ? -1.0 : 1.0;
    return s*sqrt( min(dot2(ca),dot2(cb)) );
}

float slash(vec2 p, vec2 position, float rotation, float time) {

    vec3 slashColor = vec3(1.,1.,1.);
    vec3 fadedColor = vec3(0.5,0.5,0.5);
    p += position;
    
    float slashShade = smoothstep(0.001, -0.001, sdTrapezoid(p*rotate(rotation), 0.015, 0.001, 3.));
    
    // Animation t 0.0 -> 0.1 = fading in from top to bottom
    float slashTime = clamp(time, 0.,0.1)*20.;
    vec2 slideY = p*rotate(rotation);
    float slashAnimationMask = 1.-clamp(smoothstep(slashTime, slashTime, slideY.y+1.),0.,1.);
    
    // Animation t 0.1 -> 1.0 = fading in entirety to gray
    float fadeTime = 1.-clamp(time, 0.1, 0.9);
    
    if(time == 0.) {
        return 0.;
    }
    
    return slashAnimationMask * slashShade * fadeTime;
}

void fragment( )
{
    float loopingTime = fract(iTime/4.);
    float aa = 0.;
    vec2 p = normalizeScreenSpace(fragCoord, aa,iResolution);

    // Camera shake
    float shakeScale = 0.;
    shakeScale = max(shakeScale, clamp(sin((clamp(loopingTime * 8., 0.,1.)-1.)*6.),0.,1.));
    shakeScale = max(shakeScale, clamp(sin((clamp(loopingTime * 8., 1.,2.)-1.)*6.),0.,1.));
    shakeScale = max(shakeScale, clamp(sin((clamp(loopingTime * 8., 2.,3.)-2.)*6.),0.,1.));
    
    shakeScale = max(shakeScale, clamp(sin((clamp(loopingTime * 8. * 2., 8.,9.)-2.)*6.),0.,1.));
    shakeScale = max(shakeScale, clamp(sin((clamp(loopingTime * 8. * 2., 9.,10.)-2.)*6.),0.,1.));
    shakeScale = max(shakeScale, clamp(sin((clamp(loopingTime * 8. * 2., 10.,11.)-2.)*6.),0.,1.));
    shakeScale = max(shakeScale, clamp(sin((clamp(loopingTime * 8. * 2., 11.,12.)-2.)*6.),0.,1.));
    shakeScale = max(shakeScale, clamp(sin((clamp(loopingTime * 8. * 2., 12.,13.)-2.)*6.),0.,1.));
    
    p += vec2(sin(loopingTime*150.)*0.05 * shakeScale, sin((loopingTime+2.)*173.)*0.05 * shakeScale);
    
    // Slashes
    float slashValue = slash(p, vec2(0.4,0.6), 0.67, loopingTime * 8.);
    slashValue = max(slashValue, slash(p, vec2(0.4,0.2), 0.47, clamp(loopingTime * 8., 1.,2.)-1.));
    slashValue = max(slashValue, slash(p, vec2(-0.2,0.6), 0.33, clamp(loopingTime * 8., 2.,3.)-2.));
    
    slashValue = max(slashValue, slash(p, vec2(0.25,-0.3), 0.88, clamp(loopingTime * 8. * 2., 8.,9.)-8.));
    slashValue = max(slashValue, slash(p, vec2(-0.85,0.2), 0.23, clamp(loopingTime * 8. * 2., 9.,10.)-9.));
    slashValue = max(slashValue, slash(p, vec2(0.25,-0.1), 0.02, clamp(loopingTime * 8. * 2., 10.,11.)-10.));
    slashValue = max(slashValue, slash(p, vec2(0.15,0.2), 0.59, clamp(loopingTime * 8. * 2., 11.,12.)-11.));
    slashValue = max(slashValue, slash(p, vec2(-0.85,0.7), 0.29, clamp(loopingTime * 8. * 2., 12.,13.)-12.));
    
    // Output to screen
    float v = slashValue;
    fragColor = vec4(v,v,v,1.0);
	COLOR.a = (COLOR.r +COLOR.b+COLOR.g)/3.;
}