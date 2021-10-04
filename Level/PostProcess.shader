shader_type canvas_item;

uniform float falloff;
uniform float amount;

uniform float strength;

uniform vec2 offset;

uniform float percent;

vec4 vignette(vec4 color, vec2 uv)
{
    float dist = distance(uv + offset, vec2(0.5));
    color.rgb *= smoothstep(strength, falloff * strength, dist * (amount + falloff)) + 
        vec3(0.5 * percent, 0, 0);
    return color;
}

void fragment()
{
    vec4 color = texture(TEXTURE, UV);

    color = vignette(color, UV);

    COLOR = color;
}
