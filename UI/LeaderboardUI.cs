#define HTML5
using Godot;
using System.Threading.Tasks;


public class LeaderboardUI : CanvasLayer
{
    private ItemList table;
    private LineEdit name;
    private LineEdit score;

    public override async void _Ready()
    {
        base._Ready();

        #if HTML5
        foreach (Node child in GetChildren())
        {
            if (child.Name != "Back" && child.Name != "WebMessage")
                child.QueueFree();
            else (child as Control)!.Show();
        }
        #else

        table = GetNode<ItemList>("ItemList");
        name = GetNode<LineEdit>("Name");
        score = GetNode<LineEdit>("Score");
        score.Text = GM.highscore.ToString();

        await UpdateScores();

        GetNode<Button>("Submit").Connect("pressed", this, nameof(OnSubmit));
        #endif

        GetNode<Button>("Back/Back").Connect("pressed", this, nameof(OnBack));
    }

    private async Task UpdateScores()
    {
        PlayerScore[] board = await Leaderboard.GetBoard();
        table.Clear();
        for (int i = 0; i < board.Length; i++)
        {
            table.AddItem($" {i + 1}.{board[i].name}", selectable: false);
            table.AddItem(board[i].score.ToString(), selectable: false);
        }
    }

    private async void OnSubmit()
    {
        await Leaderboard.AddScore(name.Text, GM.highscore);
        await UpdateScores();
    }

    private void OnBack() => GetTree().ChangeScene("res://UI/MainMenu.tscn");
}

public struct PlayerScore
{
    public string name;
    public int score;
    public string date;
}
