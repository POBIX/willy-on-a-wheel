using Godot;
using System;

public class Artillery : KinematicBody2D
{
    private float velocity = 0;

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        velocity += GM.Gravity / 3;
        velocity = MoveAndSlide(new(0, velocity), Vector2.Up).y;

        if (IsOnFloor())
            OnCollide();
    }

    private void OnCollide()
    {
        Spawner.Node2DDeferred("res://Enemies/Explosion.tscn", GetParent(), GlobalPosition);
        QueueFree();
    }
}
