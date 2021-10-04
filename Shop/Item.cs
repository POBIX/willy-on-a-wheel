using Godot;
using System;

public abstract class Item : Area2D
{
    private bool inside = false;

    [Export] private string name;
    [Export] private string description;
    [Export] private int price;

    private Label nameLabel;
    private Label descLabel;
    private Label priceLabel;
    private Sprite buyPrompt;

    private Shop shop;

    public abstract void OnPurchase();

    public override void _Ready()
    {
        base._Ready();
        Connect("body_entered", this, nameof(OnEntered));
        Connect("body_exited", this, nameof(OnExited));

        shop = GetParent<Shop>();

        nameLabel = GetNode<Label>("Name");
        nameLabel.Text = name;
        descLabel = GetNode<Label>("Description");
        descLabel.Text = description;
        priceLabel = GetNode<Label>("Price");
        priceLabel.Text = $"{price * shop.discount} points";

        buyPrompt = GetNode<Sprite>("BuyPrompt");
    }

    private void OnEntered(Node body)
    {
        if (body == Player.Ref.wheel)
        {
            inside = true;
            nameLabel.Show();
            descLabel.Show();
            buyPrompt.Show();
            priceLabel.Show();
            priceLabel.Text = $"{price * shop.discount} points";
        }
    }

    private void OnExited(Node body)
    {
        if (body == Player.Ref.wheel)
        {
            inside = false;
            nameLabel.Hide();
            descLabel.Hide();
            buyPrompt.Hide();
            priceLabel.Hide();
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (!inside) return;

        if (Player.Ref.Points < price)
        {
            buyPrompt.Frame = 1;
            return;
        }

        buyPrompt.Frame = 0;

        if (Input.IsActionJustPressed("Purchase"))
        {
            Player.Ref.Points -= price;
            OnPurchase();
            GetParent<Shop>().ChooseNew(Position);
            QueueFree();
        }
    }
}
