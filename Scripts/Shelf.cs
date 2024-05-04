using System.Collections.Generic;
using Godot;
using static Godot.GD;

/// <summary>
/// Holds the buyable ItemR's that Customers can put into their shopping baskets and leave with
/// </summary>
public partial class Shelf : StaticBody3D, IInteractable {

    [Export] Node3D customerSpawnPos;
    [Export] Label3D label;
    [Export] DynamicInventory dynamicInventory;
    [Export] ItemR dummyItemR;

    int itemAmount;
    ItemR item;
    List<CrateR> crates = new List<CrateR>(); // make a list of crates instead of just one?--for unstocking?

    public ItemR GetItemR { get { return item; } }
    public Node3D GetCustomerSpawnPos { get { return customerSpawnPos; } }

    public override void _Ready() {
        item = dummyItemR;
    }

    public bool IsEmpty() {
        return itemAmount <= 0;
    }

    /// <summary>
    /// For each of the same item in the customer's shopping basket, will give that item to the customer 
    /// </summary>
    /// <param name="cust">The customer interacting</param>
    /// <param name="howMany">How much of the item the customer wants</param>
    public void TakeItem(Customer cust, int howMany) {
        //if this isn't the item
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

    /// <summary>
    /// Stocks the specified crate's items onto the Shelf
    /// </summary>
    /// <param name="crate">the crate who's items will be stocked</param>
    void StockItem(CrateR crate) {
        if (dynamicInventory.GetIsInventoryFull) {
            Print("No need for stock");
            return;
        }

        //We can't stock more than one kind of item
        if (itemAmount > 0 && item != crate.GetItemR) {
            Print("Cannot stock different item at the moment");
            Player.Instance.PickUp(crate);
            return;
        }

        dynamicInventory.AddToInventory(crate);
        itemAmount += crate.GetAmtToSpawn;
        item = crate.GetItemR;
        crates.Add(crate);
        DisplayStockAmt();

    }

    /// <summary>
    /// 
    /// </summary>
    void UnstockItem() {
        if (IsEmpty()) return;

        dynamicInventory.RemoveFromInventory();
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
