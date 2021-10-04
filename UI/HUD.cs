using Godot;
using System;

public class HUD : CanvasLayer
{
    public static HUD Ref { get; private set; }

    public Label pointsText;
    public Label comboText;
    public TextureProgress comboBar;
    public TextureProgress timeBar;
    public Node2D hearts;

    private Texture heart;
    private int healthOffset = 0;
    private const int OffsetStep = 11;

    public override void _Ready()
    {
        base._Ready();
        Ref = this;

        pointsText = GetNode<Label>("Points/Label");
        comboText = GetNode<Label>("Combo/Label");
        comboBar = GetNode<TextureProgress>("Combo/TextureProgress");
        timeBar = GetNode<TextureProgress>("Time/TextureProgress");
        hearts = GetNode<Node2D>("Hearts");

        heart = ResourceLoader.Load<Texture>("res://UI/heart.png");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        comboBar.Value = Player.Ref.comboTimer.TimeLeft / Player.ComboDuration * 100;
        timeBar.Value = GM.TimeLeft / GM.GameTime * 100;
    }

    public void AddHeart()
    {
        TextureRect t = new();
        hearts.AddChild(t);
        t.Texture = heart;
        t.RectPosition = new(healthOffset, 0);
        healthOffset += OffsetStep;
    }

    public void RemoveHeart()
    {
        // remove last child
        hearts.RemoveChild(hearts.GetChild(hearts.GetChildCount() - 1));
        healthOffset -= OffsetStep;
    }
}
