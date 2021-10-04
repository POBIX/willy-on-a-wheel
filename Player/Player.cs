using Godot;
using System;

public class Player : Node2D
{
    public enum State
    {
        Normal,
        Jump,
        Die,
        Pause
    }

    public float acceleration = 2f;
    private const float Friction = 0.35f;
    private const int MaxSpeed = 100;
    public float jumpForce = 125;

    public float velocity;

    public static Player Ref { get; private set; }

    public Wheel wheel;
    private float handle;

    private AnimationPlayer anim;
    private Sprite sprite;

    public int health = 5;

    public State state = State.Normal;

    private float jumpVel;

    private int points = 0;

    private Vector2 cameraPausePos;

    public int Points
    {
        get => points;
        set
        {
            points = value;
            HUD.Ref.pointsText.Text = points.ToString();
            HUD.Ref.pointsText.AddColorOverride("font_color", Helper.GetPointsColor(points / 3000f));
        }
    }

    private int combo = 0;
    public Timer comboTimer;
    public const int ComboDuration = 2;

    private bool dying = false;

    public float comboMultiplier = 0.5f;

    public override void _Ready()
    {
        base._Ready();
        Ref = this;
        wheel = GetParent<Wheel>();

        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        sprite = GetNode<Sprite>("Sprite");

        comboTimer = new()
        {
            WaitTime = ComboDuration,
            OneShot = true,
            Autostart = false
        };
        AddChild(comboTimer);
        comboTimer.Connect("timeout", this, nameof(OnComboTimeout));

        for (int i = 0; i < health; i++)
            HUD.Ref.AddHeart();

        MusicPlayer.Ref.Stop();
    }

    private void ProcessInput(float delta)
    {
        if (Input.IsActionPressed("MoveLeft"))
        {
            velocity -= acceleration;
            sprite.FlipH = true;
        }
        else if (Input.IsActionPressed("MoveRight"))
        {
            velocity += acceleration;
            sprite.FlipH = false;
        }
        else velocity = (int)Mathf.Lerp(velocity, 0, Friction);

        velocity = Mathf.Clamp(velocity, -MaxSpeed, MaxSpeed);

        handle += velocity * delta / Mathf.Tau;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        // if (!jumping)
        // {
        //     offset += velocity * delta;
        //
        //     float totalOffset = offset + wheel.angle;
        //     anim.PlaybackSpeed = wheel.velocity / 100;
        //
        //     float y = Mathf.Sqrt(Wheel.Radius * Wheel.Radius - totalOffset * totalOffset);
        //     if (Mathf.IsNaN(y))
        //     {
        //         Die();
        //         return;
        //     }
        //
        //     GlobalPosition = new(wheel.GlobalPosition.x + totalOffset, wheel.GlobalPosition.y - y - 8);
        // }
        switch (state)
        {
            case State.Normal:
                ProcessInput(delta);

                if (Input.IsActionJustPressed("Jump"))
                {
                    state = State.Jump;
                    sprite.Rotation = 0;
                    jumpVel = -jumpForce;
                    break;
                }

                handle = MathF.IEEERemainder(handle, Mathf.Tau);
                float totalHandle = handle + wheel.angle - Mathf.Pi / 2;

                float sin = Wheel.Radius * Mathf.Sin(totalHandle);
                Vector2 offset = new Vector2(velocity == 0 ? 0 : 2, 7).Rotated(totalHandle - Mathf.Pi / 2);
                GlobalPosition = wheel.GlobalPosition + new Vector2(Wheel.Radius * Mathf.Cos(totalHandle), sin) + offset;
                anim.PlaybackSpeed = MathF.IEEERemainder(totalHandle, Mathf.Pi);
                // anim.Play(MathF.IEEERemainder(wheel.angle, Mathf.Pi) < Mathf.Pi / 8 ? "Idle" : "Run");
                sprite.Rotation = totalHandle + Mathf.Pi / 2;
                if (sin >= 0) Die();

                break;

            case State.Jump:
                ProcessInput(delta);

                jumpVel += GM.Gravity;
                if (jumpVel >= 0 && GlobalPosition.y + 10 >= wheel.GlobalPosition.y - Wheel.Radius)
                {
                    state = State.Normal;
                    float f = (wheel.GlobalPosition.x - GlobalPosition.x) / Wheel.Radius;
                    if (f is < -1 or > 1) Die();
                    float t = Mathf.Acos(f);
                    handle = t - wheel.angle - Mathf.Pi / 2;
                }
                else GlobalPosition += new Vector2(velocity, jumpVel) * delta;

                break;

            case State.Die:
                Vector2 posPrime = GlobalPosition + new Vector2(velocity, jumpVel) * delta;
                if (posPrime.y >= 65) // hardcoded floor height
                {
                    jumpVel = 0;
                    velocity = 0;
                }
                else jumpVel += GM.Gravity;

                GlobalPosition += new Vector2(velocity, jumpVel) * delta;

                Camera.Ref.GlobalPosition = GlobalPosition;

                break;

            case State.Pause:
                Camera.Ref.GlobalPosition = cameraPausePos;
                break;
        }
    }

    public void Die() => DieAnim("Die");
    public void WheelDie()
    {
        if (dying) return;
        dying = true;
        Spawner.Node2DDeferred("res://Enemies/Explosion.tscn", wheel.GetParent(), wheel.GlobalPosition);

        wheel.RemoveChild(Camera.Ref);
        AddChild(Camera.Ref);

        cameraPausePos = wheel.GlobalPosition;

        wheel.RemoveChild(this);
        wheel.GetParent().AddChild(this);

        wheel.QueueFree();

        sprite.Hide();

        state = State.Pause;

        GetTree().CreateTimer(3).Connect("timeout", this, nameof(DoDeath));
        Sound.Play("Player/death.wav", Raycast.Ref);
    }

    private void DieAnim(string name)
    {
        if (dying) return;
        dying = true;
        anim.Play(name);
        state = State.Die;

        // make the player not attached to the wheel
        Vector2 p = GlobalPosition;
        wheel.RemoveChild(this);
        wheel.GetParent().AddChild(this);
        GlobalPosition = p;

        sprite.Rotation = 0;

        jumpVel = 0;

        anim.PlaybackSpeed = 1;
    }

    // ReSharper disable once UnusedMember.Local (signal)
    public void DoDeath()
    {
        MusicPlayer.Ref.Play();
        UpdateHighscore();
        DeathScreen.points = points;
        GetTree().ChangeScene("res://UI/DeathScreen.tscn");
    }

    public void Damage()
    {
        health--;
        wheel.DamageAnimation();
        if (health <= 0) WheelDie();
        if (health >= 0) HUD.Ref.RemoveHeart();
    }

    public void AddHealth()
    {
        health++;
        HUD.Ref.AddHeart();
    }

    public int DoCombo()
    {
        combo++;
        comboTimer.Start();
        HUD.Ref.comboText.Text = combo.ToString();
        return combo;
    }

    private void OnComboTimeout()
    {
        combo = 0;
        HUD.Ref.comboText.Text = "0";
    }

    public void UpdateHighscore() =>
        GM.highscore = Mathf.Max(GM.highscore, points);

    public void Win()
    {
        MusicPlayer.Ref.Play();
        UpdateHighscore();
        WinScreen.points = points;
        GetTree().ChangeScene("res://UI/WinScreen.tscn");
    }
}
