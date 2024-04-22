using Godot;
using System;

public partial class BuyStock : StaticBody3D, IInteractable {

    [Export] CrateR crate;

    public void Interact(Node3D body) {
        GD.Print(crate.ToString());
        Player.Instance.PickUp(crate);
    }
}
