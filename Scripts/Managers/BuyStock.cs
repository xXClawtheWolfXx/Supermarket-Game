using Godot;
using System;

/// <summary>
/// Allows the player to buy stock
/// </summary>
public partial class BuyStock : StaticBody3D, IInteractable {

    [Export] CrateR crate;

    /// <summary>
    /// Grants it's crate to the player if the player has enough money to purchase it
    /// </summary>
    /// <param name="body"> the Node3D that interacted</param>
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
