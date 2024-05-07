using Godot;
using System;

/// <summary>
/// An interface for objects can be interacted with by Customers, Players,and Staff
/// </summary>
public partial interface IInteractable {
    public void Interact(Node3D body);
}
