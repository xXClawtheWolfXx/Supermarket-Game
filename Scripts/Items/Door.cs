using Godot;
using System;

public partial class Door : Area3D, IInteractable {
    [Export] private CollisionShape3D collision;
    [Export] private Node3D mesh;
    [Export] private NavigationLink3D link;

    bool doorOpen;

    private void OpenDoor() {
        collision.Visible = false;
        collision.Disabled = true;
        mesh.Visible = false;
        link.Enabled = true;
        GameManager.Instance.OpenStore();
    }

    private void CloseDoor() {
        collision.Visible = true;
        collision.Disabled = false;
        mesh.Visible = true;
        link.Enabled = false;
        GameManager.Instance.CloseStore();
    }

    public void Interact(Node3D body) {
        if (body is Player && Input.IsActionJustPressed("interact")) {
            if (doorOpen)
                CloseDoor();
            else
                OpenDoor();
            doorOpen = !doorOpen;
        }
    }


}
