using Godot;
using static Godot.GD;

public partial class Shelf : StaticBody3D, IInteractable {

    [Export] Node3D customerSpawnPos;
    [Export] Label3D label;
    [Export] DynamicInventory dynamicInventory;

    int itemAmount;
    ItemR item;
    CrateR crateR; // make a list of crates instead of just one?


    public ItemR GetItemR { get { return item; } }
    public Node3D GetCustomerSpawnPos { get { return customerSpawnPos; } }

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
        dynamicInventory.RemoveFromInventory(item, 1);
        TakeItem(cust, howMany - 1);
        cust.FillShoppingBasket(item);
        DisplayStockAmt();
    }

    void StockItem(CrateR crate) {
        if (dynamicInventory.GetIsInventoryFull) {
            Print("No need for stock");
            return;
        }

        if (itemAmount > 0 && item != crate.GetItemR) {
            Print("Cannot stock different item at the moment");
            Player.Instance.PickUp(crate);
            return;
        }
        dynamicInventory.AddToInventory(crate);
        itemAmount += crate.GetAmtToSpawn;
        item = crate.GetItemR;
        DisplayStockAmt();

        /*

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
                */
    }

    void UnstockItem() {
        /*
        if (crateR == null || item == null)
            return;
        item = null;
        itemAmount = 0;
        crateR.UpdateAmt(itemAmount);
        Player.Instance.PickUp(crateR);
        Print(crateR);
        crateR = null;
        //PrintStock();*/

        dynamicInventory.RemoveFromInventory();
        if (dynamicInventory.IsEmpty())
            itemAmount = 0;
        DisplayStockAmt();

    }

    void DisplayStockAmt() {
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
