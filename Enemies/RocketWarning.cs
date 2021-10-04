using Godot;
using System;

public class RocketWarning : Sprite
{
    private const float BounceTimes = 3.5f;

    public override void _Ready()
    {
        base._Ready();

        var anim = GetNode<AnimationPlayer>("AnimationPlayer");

        GetTree().CreateTimer(anim.GetAnimation("Warn").Length * BounceTimes - 1)
            .Connect("timeout", this, nameof(OnFollowDone));
        GetTree().CreateTimer(anim.GetAnimation("Warn").Length * BounceTimes)
            .Connect("timeout", this, nameof(OnWarnDone));
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        float x = Player.Ref.wheel.GlobalPosition.x - 16 * Mathf.Sign(Player.Ref.wheel.velocity.x);
        GlobalPosition = new(x, GlobalPosition.y);
    }

    private void OnWarnDone()
    {
        Spawner.Node2D("res://Enemies/Artillery.tscn", GetParent(), GlobalPosition);
        QueueFree();
    }

    private void OnFollowDone() => SetProcess(false);
}
