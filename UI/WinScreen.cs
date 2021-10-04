using Godot;
using System;

public class WinScreen : CanvasLayer
{
    public static int points;

    public override void _Ready()
    {
        base._Ready();

        Connect("Restart", nameof(OnRestart));
        Connect("MainMenu", nameof(OnMainMenu));
        Connect("Leaderboard", nameof(OnLeaderboard));

        GetNode<Label>("TextureRect/Score").Text = points.ToString();
        GetNode<Label>("TextureRect/Highscore").Text = GM.highscore.ToString();
    }

    private void Connect(string name, string func) =>
        GetNode<Button>($"TextureRect/{name}").Connect("pressed", this, func);

    private void OnRestart() => GetTree().ChangeScene("res://Level/Level.tscn");
    private void OnMainMenu() => GetTree().ChangeScene("res://UI/MainMenu.tscn");
    private void OnLeaderboard() => GetTree().ChangeScene("res://UI/Leaderboard.tscn");
}
