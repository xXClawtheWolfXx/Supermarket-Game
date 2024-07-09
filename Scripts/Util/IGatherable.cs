using Godot;
using System;

/// An interface for objects that can be gathered by the Player, Customers, or Staff
public partial interface IGatherable {
    public PackedScene GetPackedScene();
}
