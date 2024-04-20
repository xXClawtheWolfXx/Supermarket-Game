using Godot;
using System;

public partial class Shelf : StaticBody3D, IInteractable {

    [Export] Inventory inventory;
    [Export] int itemAmount;
    [Export] ItemR item;

    void StockItem() {

    }

    void DisplayCount() {

    }

    public void Interact(Node3D body) {
        GD.Print("Jimin is a cat");
    }
}
