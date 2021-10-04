using Godot;
using System;

public class Popup : RichTextLabel
{
    public float duration;

    private const Tween.TransitionType Transition = Tween.TransitionType.Sine;

    public override void _Ready()
    {
        base._Ready();
        Tween t = new();
        AddChild(t);

        t.InterpolateProperty(this, "modulate:a", Modulate.a, 0, duration, Transition);
        t.InterpolateProperty(
            this, "rect_global_position:y", RectGlobalPosition.y, RectGlobalPosition.y - 16, duration, Transition
        );
        t.Start();

        t.Connect("tween_all_completed", this, "queue_free");
    }
}
