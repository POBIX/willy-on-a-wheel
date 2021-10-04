using Godot;

public class GM : Node
{
    public const float Gravity = 9.8f;

    public const int GameTime = 93;
    public static float TimeLeft { get; private set; }

    public static Vector2 Size { get; set; }

    public static int highscore = 0;

    public override void _Ready()
    {
        base._Ready();

        TimeLeft = GameTime;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        TimeLeft -= delta;

        if (TimeLeft <= 0) Player.Ref.Win();
    }
}
