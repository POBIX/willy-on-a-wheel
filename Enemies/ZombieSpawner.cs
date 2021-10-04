using Godot;
using System;

public class ZombieSpawner : Node
{
    private const float ZombieTimer = 0.75f;
    private const float ArtilleryTimer = 6;

    private Random rnd = new();

    private Timer CreateTimer(float duration)
    {
        Timer t = new()
        {
            WaitTime = duration,
            OneShot = false,
            Autostart = true
        };
        AddChild(t);
        return t;
    }

    public override void _Ready()
    {
        base._Ready();

        CreateTimer(ZombieTimer).Connect("timeout", this, nameof(OnZombieTimeout));
        CreateTimer(ArtilleryTimer).Connect("timeout", this, nameof(OnArtilleryTimeout));
    }

    private void Spawn(int range, int margin, int height, NodePath scene)
    {
        int pos = (int)(Player.Ref.GlobalPosition.x + Player.Ref.wheel.velocity.x);

        // we don't want to spawn anything within inside of the wheel.
        float targetLeft = rnd.Next(pos - range, pos - margin);
        float targetRight = rnd.Next(pos + margin, pos + range);
        float target = rnd.Next(0, 2) == 1 ? targetLeft : targetRight; // choose a side at random

        Spawner.Node2D(scene, GetParent(), new(target, -height));
    }

    private void OnZombieTimeout() => Spawn(
        range: 300,
        margin: 4 * Wheel.Radius,
        height: -8,
        scene: "res://Enemies/Zombie.tscn"
    );

    private void OnArtilleryTimeout()
    {
        // hardcoded shop coordinates. if player inside shop.
        if (Player.Ref.GlobalPosition.x is > -1024 and < -784) return;

        Spawn(
            range: 0,
            margin: 0,
            height: 8,
            scene: "res://Enemies/RocketWarning.tscn"
        );
    }
}
