using Godot;
using System;

public class Explosion : Area2D
{
    private const int InitialRadius = 8;
    private const int FinalRadius = 48;

    private const float Duration = 0.75f;
    private const Tween.TransitionType Transition = Tween.TransitionType.Linear;
    private const Tween.EaseType Ease = Tween.EaseType.In;

    private static float GetShakeAmount(float max, float dist, float width) => -max * dist / width + max;

    public override void _Ready()
    {
        base._Ready();

        Connect("body_entered", this, nameof(OnBodyEntered));

        // only shake camera if we're on screen
        float dist = Mathf.Abs(GlobalPosition.x - Player.Ref.GlobalPosition.x);
        if (dist < GM.Size.x)
        {
            Camera.Ref.Shake(GetShakeAmount(5, dist, GM.Size.x), 2, 0.5f);
            var player = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
            player.VolumeDb = -dist / 20;
            player.Play();
        }

        Tween tween = new();
        AddChild(tween);

        var collider = GetNode<CollisionShape2D>("CollisionShape2D");
        var shape = (CircleShape2D)collider.Shape;

        tween.InterpolateProperty(shape, "radius", InitialRadius, FinalRadius, Duration, Transition, Ease);

        tween.Start();

        GetTree().CreateTimer(Duration + 0.1f).Connect("timeout", this, "queue_free");
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Wheel)
            Player.Ref.Damage();
    }
}
