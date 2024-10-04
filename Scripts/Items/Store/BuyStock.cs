using Godot;
using System;

/// Allows the player to buy stock
public partial class BuyStock : StaticBody3D, IInteractable {

    [Export] CrateR crate;

    /// Grants it's crate to the player if the player has enough money to purchase it
    public void Interact(Node3D body) {
        if (body is Player) {
            if (Player.Instance.GetMoney > crate.GetItemR.BaseBuyPrice) {
                CrateR newCrate = new CrateR(
                    crate.GetItemR,
                    crate.GetPackedScene(),
                    crate.GetAmtToSpawn
                );
                Player.Instance.SetMoney(-crate.GetItemR.BaseBuyPrice);
                Player.Instance.PickUp(newCrate);
            } else
                GD.Print("Not enought money");

        }
    }
}
