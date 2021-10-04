using Godot;
using System;
using System.Collections.Generic;

public class Wheel : KinematicBody2D
{
    public const int Radius = 32;
    public float maxAcceleration = 0.65f;

    private Sprite sprite;
    public AnimationPlayer anim;

    public Vector2 velocity;
    public float angle;

    private ShaderMaterial material;

    // unassigned field, but actually it is because tween!!1
#pragma warning disable 649
    private float percent;
#pragma warning restore 649

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode<Sprite>("Sprite");
        material = (ShaderMaterial)sprite.Material;
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if (Player.Ref.state != Player.State.Jump)
        {
            float acceleration = Mathf.Lerp(-maxAcceleration, maxAcceleration,
                (Player.Ref.GlobalPosition.x - GlobalPosition.x) / Radius + 0.5f);

            velocity.x += acceleration;
        }

        velocity.y += GM.Gravity;

        MoveAndSlide(velocity, Vector2.Up);

        if (IsOnFloor()) velocity.y = 0;

        for (int i = 0; i < GetSlideCount(); i++)
        {
            KinematicCollision2D col = GetSlideCollision(i);
            if (col.Collider is Enemy e && !e.IsQueuedForDeletion()) // if we hit an enemy
            {
                Camera.Ref.Shake(1.5f, 0.2f, 0);
                e.Die();
            }
        }

        angle += velocity.x * delta / Radius;
        sprite.Rotation = angle;

        // if outside arena + margin
        if (Mathf.Abs(GlobalPosition.x) > 4096 + 48)
        {
            Camera.Ref.LimitBottom = 100000000; // don't stop the camera at the floor
            GetTree().CreateTimer(3).Connect("timeout", Player.Ref, nameof(Player.DoDeath)); // die after a few seconds
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        material.SetShaderParam("percent", percent);
        // PostProcess.material.SetShaderParam("percent", percent);
    }

    public void DamageAnimation()
    {
        Tween t = new();
        AddChild(t);

        t.InterpolateProperty(this, nameof(percent), 0, 1, 0.75f, Tween.TransitionType.Cubic, Tween.EaseType.In);
        t.InterpolateProperty(this, nameof(percent), 1, 0, 0.75f, Tween.TransitionType.Cubic, Tween.EaseType.Out, delay: 0.75f);
        t.Start();
        t.Connect("tween_all_completed", t, "queue_free");
    }
}
