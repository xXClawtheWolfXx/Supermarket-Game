using Godot;
using System;


/// Not used-- to be deleted
public partial class NPCInteract : RayCast3D {

    public override void _Process(double delta) {
        if (IsColliding() && GetCollider() is IInteractable) {
            ((IInteractable)GetCollider()).Interact(GetParent<Customer>());
        }
    }
}
