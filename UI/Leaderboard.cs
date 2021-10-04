using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Godot;
using Newtonsoft.Json;

public class Leaderboard : Node
{
    public static Leaderboard Ref { get; private set; }

    private static string requestResult;

    public override void _Ready()
    {
        base._Ready();
        Ref = this;
        AddChild(Client);
        Client.Connect("request_completed", this, nameof(OnRequestCompleted));
    }

    private const string Site = "http://dreamlo.com/lb/";
    private const string Code = "nZ66LHnd1UqNZrw6h0nURg5MAMIockhEC0uFwzsPHHrQ";
    private static readonly HTTPRequest Client = new();
    private new static async Task<string> Get(string url)
    {
        Client.Request(url);
        await Ref.ToSignal(Client, "request_completed");
        return requestResult;
    }

    private void OnRequestCompleted(int result, int response, string[] headers, byte[] body)
    {
        requestResult = "";
        foreach (byte b in body)
            requestResult += (char)b;
    }

    public static async Task AddScore(string username, int score)
    {
        string result = await Get(Site + Code + $"/add/{username}/{score}");
        GD.Print($"add score result: {result}");
    }

    public static async Task<PlayerScore[]> GetBoard()
    {
        try
        {
            string board = await Get(Site + Code + "/json");
            var jsonArg = new JsonTextReader(new StringReader(board));
            var serializer = new JsonSerializer();
            var json = serializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>>>>(jsonArg);
            List<Dictionary<string, string>> entries = json!["dreamlo"]["leaderboard"]["entry"];
            var result = new PlayerScore[entries.Count];
            for (var i = 0; i < entries.Count; i++)
            {
                var e = entries[i];
                result[i] = new() {name = e["name"], score = int.Parse(e["score"]), date=e["date"]};
            }
            return result;
        }
        catch (Exception e)
        {
            GD.Print(e);
            return Array.Empty<PlayerScore>();
        }
    }
}
