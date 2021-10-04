using Godot;
using System;

public class Zombie : Enemy
{
    private const int Speed = 50;

    private Vector2 velocity;

    private Sprite sprite;
    private Sprite colorSprite;
    private Timer explodeTimer;

    private ShaderMaterial colorChanger;

    private ParticleEmitter deathParticles;

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode<Sprite>("Sprite");
        explodeTimer = GetNode<Timer>("ExplodeTimer");

        explodeTimer.Connect("timeout", this, nameof(OnExplodeTimeout));

        colorSprite = GetNode<Sprite>("ColorSprite");
        colorChanger = (ShaderMaterial)colorSprite.Material.Duplicate();
        colorSprite.Material = colorChanger;

        deathParticles = GetNode<ParticleEmitter>("DeathParticles");
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        velocity.x = Mathf.Sign(Player.Ref.GlobalPosition.x - GlobalPosition.x) * Speed;
        colorSprite.FlipH = sprite.FlipH = Mathf.Sign(velocity.x) == -1;
        velocity.y += GM.Gravity;
        velocity = MoveAndSlide(velocity, Vector2.Up);

        // GD.Print((explodeTimer.WaitTime - explodeTimer.TimeLeft) / explodeTimer.WaitTime);
        colorChanger.SetShaderParam("percent", (explodeTimer.WaitTime - explodeTimer.TimeLeft) / explodeTimer.WaitTime);
    }

    private void OnExplodeTimeout()
    {
        // anim.Play("Explode");
        Explode();
    }

    private void Explode()
    {
        Spawner.Node2D("res://Enemies/Explosion.tscn", GetParent(), GlobalPosition);
        QueueFree();
    }

    private static string ToHex(Color col)
    {
        return $"#{(byte)col.r8:x2}{(byte)col.g8:x2}{(byte)col.b8:x2}";
    }

    public override void Die()
    {
        deathParticles.Emit(GetParent(), GlobalPosition);

        int combo = Player.Ref.DoCombo();
        int points = (int)(Mathf.Abs(Player.Ref.wheel.velocity.x) / 3 * combo * Player.Ref.comboMultiplier);
        string color = ToHex(Helper.GetPointsColor(points / 300f));
        string text =
            "[wave amp=20 freq=5]" +
            $"{GetText(combo)}" +
            "[/wave]\n" +
            $"[color={color}]+{points}[/color]";
        Spawner.Popup(PopupLayer.Ref, GlobalPosition, text, 0.5f, Colors.Yellow);

        Player.Ref.Points += points;

        Sound.Play("res://Enemies/splat.wav", GetParent());

        base.Die();
    }

    private string GetText(int combo) => combo switch
    {
        1 => "",
        2 => "double\nkill",
        3 => "triple\nkill",
        4 => "quadruple\nkill",
        5 => "quintuple\nkill",
        _ => GetUltraText()
    };

    private Random rnd = new();
    private string GetUltraText() => rnd.Next(0, 7) switch
    {
        0 => "domination",
        1 => "rampage",
        2 => "wicked",
        3 => "monster",
        4 => "sick",
        5 => "mega",
        6 => "unstoppable",
        _ => ""
    };
}
