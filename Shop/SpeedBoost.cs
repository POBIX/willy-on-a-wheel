using Godot;
using System;

public class SpeedBoost : Item
{
    public override void OnPurchase() =>
        Player.Ref.acceleration = 3;
}
