using Godot;
using System;
using Godot.Collections;

public partial class Interaction : Area3D {

    [Export] Node3D interactable;

    bool isPlayerInteracting = false;

    public override void _Ready() {
        interactable = GetParent<Node3D>();
    }

    public override void _Process(double delta) {
        if (isPlayerInteracting && Input.IsActionJustPressed("interact"))
            ((IInteractable)interactable).Interact(Player.Instance);
    }

    public void OnBodyEntered(Node3D body) {
        if (body is Player) // || body is NPC
            isPlayerInteracting = true;
        //elseif body is NPC, call NPC.interact() from interact
    }

    public void OnBodyExited(Node3D body) {
        if (body is Player)
            isPlayerInteracting = false;
    }
}
