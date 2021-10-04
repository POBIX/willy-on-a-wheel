using Godot;
using System;

public class HealthRefill : Item
{
    public override void OnPurchase()
    {
        for (int i = Player.Ref.health; i < 5; i++)
            Player.Ref.AddHealth();
    }
}
