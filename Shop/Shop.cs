using Godot;
using System;
using System.Collections.Generic;

public class Shop : Sprite
{
    [Export] private PackedScene[] items = Array.Empty<PackedScene>();

    private List<int> chosen = new();

    private Random rnd = new();

    private const int ItemStep = 92;

    private Vector2 pos = new(-ItemStep, -6);

    public float discount = 1;

    private PackedScene Choose()
    {
        int item;
        do item = rnd.Next(0, items.Length - 1);
        while (chosen.Contains(item));
        chosen.Add(item);
        return items[item];
    }

    public override void _Ready()
    {
        base._Ready();
        for (int i = 0; i < 3; i++)
        {
            PackedScene s = Choose();
            Item t = s.Instance<Item>();
            t.Position = pos;
            pos.x += ItemStep;
            AddChild(t);
        }
    }

    public void ChooseNew(Vector2 pos)
    {
        PackedScene s = Choose();
        Item t = s.Instance<Item>();
        t.Position = pos;
        AddChild(t);
    }
}
