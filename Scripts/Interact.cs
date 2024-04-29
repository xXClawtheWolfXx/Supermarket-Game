using Godot;
using System;

public partial class Interact : RayCast3D {

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
