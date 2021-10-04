using Godot;

public static class Spawner
{
    public static T Instance<T>(NodePath path) where T : class
    {
        var p = ResourceLoader.Load<PackedScene>(path);
        return p.Instance<T>();
    }

    public static T Spawn<T>(NodePath path, Node parent) where T : Node
    {
        T t = Instance<T>(path);
        parent.AddChild(t);
        return t;
    }

    public static Node2D Node2D(string path, Node parent, Vector2 pos)
    {
        Node2D n = Instance<Node2D>(path);
        n.GlobalPosition = pos;
        parent.AddChild(n);
        return n;
    }

    public static T SpawnDeferred<T>(NodePath path, Node parent) where T : Node
    {
        T t = Instance<T>(path);
        parent.CallDeferred("add_child", t);
        return t;
    }

    public static Node2D Node2DDeferred(string path, Node parent, Vector2 pos)
    {
        Node2D n = Instance<Node2D>(path);
        n.GlobalPosition = pos;
        parent.CallDeferred("add_child", n);
        return n;
    }

    public static Popup Popup(Node parent, Vector2 position, string text, float scale = 1, Color? color = null,
                              float duration = 2)
    {
        var p = Instance<Popup>("res://UI/Popup.tscn");
        p.BbcodeText = text;
        p.duration = duration;
        p.RectGlobalPosition = position;
        p.RectScale = new(scale, scale);
        p.Modulate = color ?? Colors.White;
        parent.AddChild(p);
        return p;
    }
}
