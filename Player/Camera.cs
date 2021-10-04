using Godot;
using System;

public class Camera : Camera2D
{
    public static Camera Ref { get; private set; }

    private float shake = 0;
    private float duration = 1;
    private float elapsed = 0;
    private float decay = 0;

    public override void _Ready()
    {
        base._Ready();
        Ref = this;

        GM.Size = GetViewport().Size * Zoom;
    }

    public void Shake(float amount, float duration, float decay)
    {
        shake = amount;
        this.duration = duration;
        elapsed = 0;
        this.decay = decay;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        elapsed += delta;
        if (elapsed >= duration)
            shake = 0;

        if (shake == 0) return;
        float power = shake * ((duration - elapsed) / duration * (1 - decay));
        Offset = new(
            (float)GD.RandRange(-1, 1) * power,
            (float)GD.RandRange(-1, 1) * power
        );
    }
}
