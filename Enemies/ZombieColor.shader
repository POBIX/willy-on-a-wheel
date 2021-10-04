shader_type canvas_item;

uniform float percent;
uniform vec4 color : hint_color;

void fragment()
{
    vec4 col = texture(TEXTURE, UV);
    if (col.a > 0.1)
        COLOR = vec4(color.rgb, percent);
    else
        COLOR = vec4(0);
}