using Godot;
using System;

/// <summary>
/// The Interaction class for the player
/// The Player probes the world with a raycast every frame, looking for interactables
/// </summary>
public partial class Interact : RayCast3D {

    //checks every frame for an interact keypress and an interactable to interact with
    public override void _Process(double delta) {
        if (IsColliding()) {
            GodotObject collision = GetCollider();
            if (collision is IInteractable && Input.IsActionJustPressed("interact")) {
                Player.Instance.GetCurrInteractable = (IInteractable)collision;
                GD.Print("interacted with: " + ((Node3D)collision).Name);
                ((IInteractable)collision).Interact(Player.Instance);
            }
        }
    }
}
