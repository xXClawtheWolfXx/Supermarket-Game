using Godot;
using System;
using System.Net.Http.Headers;
using static Godot.GD;

public partial class Shelf : StaticBody3D, IInteractable {

    [Export] Node3D spawnPos;
    [Export] Label3D label;

    int itemAmount;
    ItemR item;
    CrateR crateR;


    public ItemR GetItemR { get { return item; } }
    public Node3D GetSpawnPos { get { return spawnPos; } }

    public bool IsEmpty() {
        return itemAmount <= 0;
    }

    //will later take an int param
    public void TakeItem(Customer cust, int howMany) {
        if (cust.GetItemLookingFor.GetName != item.GetName) return;
        if (howMany <= 0) return;


        if (IsEmpty()) {
            cust.FillShoppingBasket(null);
            return;
        }

        itemAmount--;
        TakeItem(cust, howMany - 1);
        cust.FillShoppingBasket(item);
        DisplayStock();

    }



    void StockItem(CrateR crate) {
        //if we already stock
        if (itemAmount >= crate.GetAmtToSpawn) {
            Print("No need for stock");
            Player.Instance.PickUp(crate);
            return;
        }
        //we have some stock
        if (itemAmount > 0) {
            //if the stocked item is different from potential stock items
            if (item != crate.GetItemR) {
                Print("Cannot stock different item at the moment");
                Player.Instance.PickUp(crate);
                return;
            }
            //make the amount, the amount in the crate, without overflow
            int max = crate.GetAmtToSpawn;
            crate.UpdateAmt(max - itemAmount);
            itemAmount = max;
            Player.Instance.PickUp(crate);

        }
        //if we don't have stock
        crateR = crate;
        item = crate.GetItemR;
        itemAmount = crate.GetAmtToSpawn;
        DisplayStock();
        //PrintStock();
    }

    void UnstockItem() {
        if (crateR == null || item == null)
            return;
        item = null;
        itemAmount = 0;
        crateR.UpdateAmt(itemAmount);
        Player.Instance.PickUp(crateR);
        Print(crateR);
        crateR = null;
        DisplayStock();
        //PrintStock();
    }

    void DisplayStock() {
        label.Text = item.GetName + ": " + itemAmount;
    }


    public void Interact(Node3D body) {
        if (body is Customer)
            TakeItem((Customer)body, ((Customer)body).HowManyItems(item));
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
