using Godot;
using System;
using Godot.Collections;

public partial class Inventory : Node3D {

    [Export] Array<ItemR> items;
    [Export] Array<Node3D> itemPositions;
    [Export] int maxItems;

    int currItemAmt;

    public void AddToInventory(ItemR item) {
        if (currItemAmt < maxItems) {
            items.Add(item);
            currItemAmt++;
        }
        //else print that basket is full
    }

    public void RemoveFromInventory(ItemR item) {
        if (items.Contains(item)) {
            items.Remove(item);
            currItemAmt--;
        }
    }

    public void ShowItems() {
        //spawn in itemPos, 1st to 1st. 
    }


}
