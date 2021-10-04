#define HTML5
using Godot;
using System;

public class MainMenu : CanvasLayer
{
    public override void _Ready()
    {
        base._Ready();

        SetSize(1920, 1080);

        Connect("NewGame", nameof(OnNewGame));
        Connect("Leaderboard", nameof(OnLeaderboard));
        Connect("Credits", nameof(OnCredits));
        #if !HTML5
        Connect("LeaveExit", nameof(OnLeaveExit));
        #endif
    }

    private void Connect(string name, string func) =>
        GetNode<Button>($"TextureRect/{name}").Connect("pressed", this, func);

    private void OnNewGame() => ChangeScene("res://Level/Level.tscn");
    private void OnLeaderboard() => ChangeScene("res://UI/Leaderboard.tscn");
    private void OnCredits() => ChangeScene("res://UI/Credits.tscn");
    private void OnLeaveExit() => GetTree().Quit();

    private void SetSize(int w, int h) =>
        GetTree().SetScreenStretch(SceneTree.StretchMode.Viewport, SceneTree.StretchAspect.KeepWidth, new(w, h));

    private void ChangeScene(NodePath path)
    {
        SetSize(512, 288);
        GetTree().ChangeScene(path);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        OS.WindowFullscreen = true;
    }
}
