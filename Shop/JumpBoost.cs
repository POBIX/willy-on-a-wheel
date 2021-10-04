using Godot;
using System;

public class JumpBoost : Item
{
    public override void OnPurchase()
    {
        Player.Ref.jumpForce = 200;
    }
}
