using Godot;
using System;

/// <summary>
/// An interface for objects that can be gathered by the Player, Customers, or Staff
/// </summary>
public partial interface IGatherable {
    public PackedScene GetPackedScene();
}
