using Godot;
using System;
using Godot.Collections;
using System.Collections;
using System.Collections.Generic;

/// A soon-to-be deprecated class
/// DynamicInventory is to be used for inventorys who need graphics
/// List will suffice to inventories without graphics
/// 
/// Holds a list of items and might show them, though this was never tested
public partial class Inventory : Node3D {

    List<IGatherable> items = new List<IGatherable>();
    [Export] Array<Node3D> itemPositions = new Array<Node3D>();
    [Export] int maxItems;

    int currItemAmt = 0;
    int currItemIndex = 0;

    public bool IsEmpty() {
        return currItemAmt == 0;
    }

    /// Adds the specified item to the inventory
    public bool AddToInventory(IGatherable item) {
        if (currItemAmt <= maxItems) {
            items.Add(item);
            currItemAmt++;
            return true;
        }
        GD.PrintErr("No more space in Inventory");
        return false;
    }

    /// Removes the specified item from the inventory
    public void RemoveFromInventory(IGatherable item) {
        if (items.Contains(item)) {
            items.Remove(item);
            currItemAmt--;
            //HideItems();
        }
    }
    /// Always removes the first item from the inventory and returns it
    public IGatherable RemoveFromInventory() {
        if (!IsEmpty()) {
            IGatherable item = items[0];
            items.RemoveAt(0);
            currItemAmt--;
            return item;
        }
        return null;
    }

    public bool HasItem(IGatherable item) {
        return items.Contains(item);
    }
    /*
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
    */
}
