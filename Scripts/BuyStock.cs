using Godot;
using System;

public partial class BuyStock : StaticBody3D, IInteractable {

    [Export] CrateR crate;

    public void Interact(Node3D body) {
        Player.Instance.PickUp(crate);
    }
}
