using Godot;
using System;

public class PopupLayer : CanvasLayer
{
    public static PopupLayer Ref { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        Ref = this;
    }
}
