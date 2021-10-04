using Godot;
using System;

public class PostProcess : ViewportContainer
{
    public static ShaderMaterial material;

    public override void _Ready()
    {
        base._Ready();
        material = (ShaderMaterial)Material;
    }
}
