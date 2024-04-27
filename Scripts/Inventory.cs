using Godot;
using System;
using Godot.Collections;
using System.Collections;
using System.Collections.Generic;

public partial class Inventory : Node3D {

    List<IGatherable> items = new List<IGatherable>();
    [Export] Array<Node3D> itemPositions = new Array<Node3D>();
    [Export] int maxItems;

    int currItemAmt = 0;
    int currItemIndex = 0;

    public bool IsEmpty() {
        return currItemAmt == 0;
    }

    //if we have enough space, add the item to inventory
    public bool AddToInventory(IGatherable item) {
        if (currItemAmt <= maxItems) {
            items.Add(item);
            currItemAmt++;
            return true;
        }
        GD.PrintErr("No more space in Inventory");
        return false;
        //else print that basket is full
    }

    //removes item from inventory
    public void RemoveFromInventory(IGatherable item) {
        if (items.Contains(item)) {
            items.Remove(item);
            currItemAmt--;
            HideItems();
        }
    }

    //always removes first item from inventory and returns it
    public IGatherable RemoveFromInventory() {
        if (!IsEmpty()) {
            IGatherable item = items[0];
            items.RemoveAt(0);
            currItemAmt--;
            return item;
        }
        return null;
    }

    //checks if an item exists in the inventory
    public bool HasItem(IGatherable item) {
        return items.Contains(item);
    }

    public void ShowItems() {
        currItemIndex = 0;
        foreach (IGatherable item in items) {
            //spawn, to pos, parent to itemPOS
            Node3D newItem = item.GetPackedScene().Instantiate<Node3D>();
            newItem.Position = itemPositions[currItemIndex].Position;
            itemPositions[currItemIndex].AddChild(newItem);
            currItemIndex++;
        }
    }

    public void HideItems() {
        currItemIndex = 0;
        foreach (Node3D node3D in itemPositions) {
            node3D.GetChild<Item>(0).QueueFree();
        }
    }

}
