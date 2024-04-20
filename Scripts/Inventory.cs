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

    public bool AddToInventory(IGatherable item) {
        if (currItemAmt <= maxItems) {
            items.Add(item);
            currItemAmt++;
            return true;
        }
        return false;
        //else print that basket is full
    }

    public void RemoveFromInventory(IGatherable item) {
        if (items.Contains(item)) {
            items.Remove(item);
            currItemAmt--;

        }
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
            Item child = node3D.GetChild<Item>(0);
            child.Visible = false;
        }
    }

}
