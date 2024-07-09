using Godot;
using System;
/// An inventory that only holds 1 IGatherable and spawns it into the world
public partial class Hands : Node3D {

    [Export] Node3D handsPos;
    IGatherable item = null;

    /// Checks whether the inventory is empty
    public bool IsEmpty() {
        return item == null;
    }

    /// Adds the specified Igatherable to inventory
    public bool PickUp(IGatherable ig) {
        GD.PrintS("item", ig);
        if (IsEmpty()) {
            item = ig;
            ShowItem();
            return true;
        }
        GD.Print("Hands Full");
        return false;
    }

    /// Returns an IGatherable and empties the inventory
    public IGatherable PutDown() {
        if (IsEmpty()) return null;
        IGatherable itemDown = item;
        HideItem();
        item = null;
        return itemDown;
    }

    /// Spawns items into the world at the handPos
    public void ShowItem() {
        Node3D spawnedItem = item.GetPackedScene().Instantiate<Node3D>();
        spawnedItem.Position = handsPos.Position;
        handsPos.AddChild(spawnedItem);
    }

    /// Destroys the object from the world
    public void HideItem() {
        handsPos.GetChild<Node3D>(0).QueueFree();
    }
}
