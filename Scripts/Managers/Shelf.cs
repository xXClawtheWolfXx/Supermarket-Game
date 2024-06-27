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
    ItemR item = null;
    List<CrateR> crates = new List<CrateR>(); // make a list of crates instead of just one?--for unstocking?

    public ItemR GetItemR { get { return item; } }
    public Node3D GetCustomerSpawnPos { get { return customerSpawnPos; } }
    public int GetItemAmount { get { return itemAmount; } }

    public override void _Ready() {
        //item = dummyItemR;
        Print(HasItemR());
    }

    public bool IsEmpty() {
        return itemAmount <= 0;
    }

    public bool IsFull() {
        return dynamicInventory.GetIsInventoryFull;
    }

    public bool HasItemR() { return item != null; }


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
    public void StockItem(CrateR crate, ICharacter character) {
        if (dynamicInventory.GetIsInventoryFull) {
            Print("No need for stock");
            return;
        }

        //We can't stock more than one kind of item
        if (item != crate.GetItemR && itemAmount > 0) {
            Print("Cannot stock different item at the moment");
            Player.Instance.PickUp(crate);
            return;
        }

        dynamicInventory.AddToInventory(crate, character);
        itemAmount += crate.GetAmtToSpawn;
        item = crate.GetItemR;
        crates.Add(crate);
        DisplayStockAmt();

    }

    /// <summary>
    /// 
    /// </summary>
    void UnstockItem() {
        if (IsEmpty() || Player.Instance.GetMouseOverItem == null) return;

        dynamicInventory.RemoveFromInventory();
        itemAmount = 0;
        DisplayStockAmt();

    }


    void DisplayStockAmt() {
        label.Text = item.GetName + ": " + itemAmount;
    }

    public void Interact(Node3D body) {
        if (body is Customer cust)
            TakeItem(cust, cust.HowManyItems(item));
        else if (body is Player player) {
            if (player.GetHands.IsEmpty()) {
                UnstockItem();
            } else {
                IGatherable ig = player.PutDown();
                if (ig is CrateR crate) {
                    StockItem(crate, player);
                } else
                    player.PickUp(ig);
            }
        } else if (body is Stocker stocker) {
            if (!stocker.IsEmpty()) {
                IGatherable ig = stocker.PutDown();
                if (ig is CrateR crate) {
                    StockItem(crate, stocker);
                    stocker.DealWithMoreStock();
                } else
                    stocker.PickUp(ig);
            }
        }
    }
}
