using Godot;
using System;

public class SpikeWheel : Item
{
    public override void OnPurchase() =>
        Player.Ref.wheel.anim.Play("TurnIntoSpikes");
}
