using Godot;

public class Enemy : KinematicBody2D
{
    public virtual void Die() => QueueFree();
}
