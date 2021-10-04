using Godot;
using System;

public class ParticleEmitter : CPUParticles2D
{
    public void Emit(Node parent, Vector2 position)
    {
        GetParent().RemoveChild(this);
        parent.AddChild(this);
        Emitting = true;
        GlobalPosition = position;

        // wait until it stops emitting
        GetTree().CreateTimer(Lifetime).Connect("timeout", this, "queue_free");
    }
}
