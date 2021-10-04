shader_type canvas_item;

uniform float percent;

void fragment()
{
    vec4 col = texture(TEXTURE, UV);
    if (col.a > 0.1)
        COLOR = mix(col, vec4(1, 0, 0, 1), percent);
    else COLOR = col;
}