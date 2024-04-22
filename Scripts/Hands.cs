using Godot;
using System;

public partial class Hands : Node3D {

    [Export] Node3D handsPos;
    IGatherable item = null;

    public bool IsEmpty() {
        return item == null;
    }

    public bool PickUp(IGatherable ig) {
        if (IsEmpty()) {
            item = ig;
            return true;
        }
        GD.Print("Hands Full");
        return false;
    }

    public IGatherable PutDown() {
        if (IsEmpty()) return null;
        IGatherable itemDown = item;
        HideItem();
        item = null;
        return itemDown;
    }

    public void ShowItem() {
        Node3D spawnedItem = item.GetPackedScene().Instantiate<Node3D>();
        spawnedItem.Position = handsPos.Position;
        handsPos.AddChild(spawnedItem);
    }

    public void HideItem() {
        handsPos.GetChild<Node3D>(0).QueueFree();
    }
}
