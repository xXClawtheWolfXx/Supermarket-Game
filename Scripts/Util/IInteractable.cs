using Godot;
using System;

/// An interface for objects can be interacted with by Customers, Players,and Staff
public partial interface IInteractable {

    public void Interact(Node3D body);
}
