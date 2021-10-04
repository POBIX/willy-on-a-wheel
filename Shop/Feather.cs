using Godot;
using System;

public class Feather : Item
{
    public override void OnPurchase() =>
        Player.Ref.wheel.maxAcceleration = 0.85f;
}
