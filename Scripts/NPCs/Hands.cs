using Godot;
using System;
/// <summary>
/// An inventory that only holds 1 IGatherable and spawns it into the world
/// </summary>
public partial class Hands : Node3D {

    [Export] Node3D handsPos;
    IGatherable item = null;

    /// <summary>
    /// Checks whether the inventory is empty
    /// </summary>
    /// <returns> if the inventory is empty</returns>
    public bool IsEmpty() {
        return item == null;
    }

    /// <summary>
    /// Adds the specified Igatherable to inventory
    /// </summary>
    /// <param name="ig">the gatherable to be added</param>
    /// <returns> true if it succeeds</returns>
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

    /// <summary>
    /// Returns an IGatherable and empties the inventory
    /// </summary>
    /// <returns> the item removed </returns>
    public IGatherable PutDown() {
        if (IsEmpty()) return null;
        IGatherable itemDown = item;
        HideItem();
        item = null;
        return itemDown;
    }

    /// <summary>
    /// Spawns items into the world at the handPos
    /// </summary>
    public void ShowItem() {
        Node3D spawnedItem = item.GetPackedScene().Instantiate<Node3D>();
        spawnedItem.Position = handsPos.Position;
        handsPos.AddChild(spawnedItem);
    }

    /// <summary>
    /// Destroys the object from the world
    /// </summary>
    public void HideItem() {
        handsPos.GetChild<Node3D>(0).QueueFree();
    }
}
