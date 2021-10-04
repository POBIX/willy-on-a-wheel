using Godot;
using System;

public class ComboUp : Item
{
    public override void OnPurchase() =>
        Player.Ref.comboMultiplier = 1;
}
