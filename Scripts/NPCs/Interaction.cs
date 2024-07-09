using Godot;
using System;
using Godot.Collections;

/// A soon to be obselete class
/// Used in early testing to get interaction from Area3Ds that were attached to every interactable object
/// If the player entered and pressed the interact key, they could interact with the world
/// If a customer enters, it would interact with the interactable
public partial class Interaction : Area3D {

    [Export] Node3D interactable;

    bool isPlayerInteracting = false;

    public override void _Ready() {
        //this code was surprisingly never used since I always assigned it in the inspector
        interactable = GetParent<Node3D>();
    }

    //every frame, checks if player is here and pressed interact
    public override void _Process(double delta) {
        if (isPlayerInteracting && Input.IsActionJustPressed("interact"))
            ((IInteractable)interactable).Interact(Player.Instance);
    }

    //When the player enters the body, it CAN interact
    //When teh customer enteres, it DOES interact
    public void OnBodyEntered(Node3D body) {
        if (body is Player)
            isPlayerInteracting = true;
        else if (body is Customer)
            ((IInteractable)interactable).Interact(body);
    }
    //When the player exits the body, it CANNOT interact
    public void OnBodyExited(Node3D body) {
        if (body is Player)
            isPlayerInteracting = false;

    }
}
