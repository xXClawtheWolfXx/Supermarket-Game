using Godot;
using System;
using Godot.Collections;

public partial class Inventory : Node3D {

    [Export] Array<ItemR> items;
    [Export] Array<Node3D> itemPositions;
    [Export] int maxItems;

    int currItemAmt = 0;
    int currItemIndex = 0;

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
        currItemIndex = 0;
        foreach (ItemR item in items) {
            //spawn, to pos, parent to itemPOS
            Node3D newItem = item.GetPackedScene.Instantiate<Node3D>();
            newItem.Position = itemPositions[currItemIndex].Position;
            itemPositions[currItemIndex].AddChild(newItem);
        }
    }


}
