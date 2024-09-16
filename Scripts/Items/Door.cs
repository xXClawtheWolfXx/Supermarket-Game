using Godot;
using System;

public partial class Door : StaticBody3D, IInteractable {
    [Export] private CollisionShape3D collision;
    [Export] private Node3D mesh;

    bool doorOpen;
    public override void _Ready() {

    }

    private void OpenDoor() {
        collision.Visible = false;
        collision.Disabled = true;
        mesh.Visible = false;
        GameManager.Instance.OpenStore();
    }

    private void CloseDoor() {
        collision.Visible = true;
        collision.Disabled = false;
        mesh.Visible = true;
        GameManager.Instance.CloseStore();

    }

    public void Interact(Node3D body) {
        if (body is Player && Input.IsActionJustPressed("interact")) {
            if (doorOpen)
                CloseDoor();
            else
                OpenDoor();
        }
    }


}
