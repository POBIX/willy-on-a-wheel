using Godot;
using System;

public class MusicPlayer : AudioStreamPlayer
{
    public static MusicPlayer Ref { get; private set; }
    public override void _Ready()
    {
        base._Ready();
        Ref = this;
    }
}
