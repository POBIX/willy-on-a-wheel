using Godot;
using System;

public class Credits : CanvasLayer
{
    public override void _Ready()
    {
        base._Ready();

        GetNode<Button>("TextureRect/Back").Connect("pressed", this, nameof(OnBack));
    }

    private void OnBack() => GetTree().ChangeScene("res://UI/MainMenu.tscn");
}
