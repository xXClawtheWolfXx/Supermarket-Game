using Godot;
using System;
using static Godot.GD;

public partial class Shelf : StaticBody3D, IInteractable {

    int itemAmount;
    ItemR item;
    CrateR crateR;

    public ItemR GetItemR { get { return item; } }

    public bool IsEmpty() {
        return itemAmount == 0;
    }

    //will later take an int param
    public void TakeItem(Customer cust) {
        if (!IsEmpty()) {
            Print("Taking item from she;f");
            itemAmount--;
            cust.FillShoppingBasket(item);
            PrintStock();
            return;
        }
        Print("Taking no item");
        PrintStock();
        cust.FillShoppingBasket(null);
    }

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
            Print(item.ToString());
        Print(" total: " + itemAmount);
    }
    void DisplayCount() {

    }

    public void Interact(Node3D body) {
        if (body is Customer)
            TakeItem((Customer)body);
        if (body is Player) {
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
}
