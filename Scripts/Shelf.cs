using Godot;
using System;
using static Godot.GD;

public partial class Shelf : StaticBody3D, IInteractable {

    int itemAmount;
    ItemR item;
    CrateR crateR;

    void StockItem(CrateR crate) {
        crateR = crate;
        item = crate.GetItemR;
        itemAmount = crate.GetAmtToSpawn;
        PrintStock();
    }

    void UnstockItem() {
        item = null;
        itemAmount = 0;
        crateR.UpdateAmt(itemAmount);
        Player.Instance.PickUp(crateR);
        Print(crateR);
        crateR = null;
        PrintStock();
    }

    void PrintStock() {
        Print("__________Shelf Stock________");
        if (item == null)
            Print("NO stock");
        else
            Print(item.ToString() + " total: " + itemAmount);
    }
    void DisplayCount() {

    }

    public void Interact(Node3D body) {
        if (Player.Instance.GetHands.IsEmpty()) {
            UnstockItem();
        } else {
            IGatherable ig = Player.Instance.PutDown();
            if (ig is CrateR) {
                StockItem((CrateR)ig);
            } else
                Player.Instance.PickUp(ig);
        }
    }
}
