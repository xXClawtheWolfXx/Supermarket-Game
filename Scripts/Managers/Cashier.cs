using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Cashier checks out customers when interacted by Customers and Players
/// </summary>
public partial class Cashier : StaticBody3D, IInteractable {

    [Export] Node3D spawnPos;
    [Export] Node3D itemSpawnPos;
    [Export] Label3D itemPriceLabel;
    [Export] Timer itemSpawnTimer;

    bool isPlayerPresent = true; // what if the player walks away mid-checkout?

    public List<Customer> GetLine { get { return line; } }
    public Node3D GetSpawnPos { get { return spawnPos; } }

    List<Customer> line = new List<Customer>();

    Node3D currItem;
    Inventory currCustInventory;
    Customer currCustomer = null;
    Customer prevCustomer = null;


    /// <summary>
    /// Remove a specified customer from the line
    /// </summary>
    /// <param name="cust">the customer to be removed</param>
    //removes a customer from the line
    //used when customers get too angry
    public void RemoveCustomer(Customer cust) {
        line.Remove(cust);
    }

    /// <summary>
    /// Add a specified customer to the line
    /// </summary>
    /// <param name="cust">the customer to add</param>
    void AddCustomer(Customer cust) {
        if (line.Contains(cust)) return;

        cust.StartCashierTimer();
        line.Add(cust);
    }

    /// <summary>
    ///Will process all the items in a customer's cart and spawn them at the itemSpawnPos
    /// </summary>
    void Checkout() {
        if (line.Count != 0 && currCustomer == null) {
            currCustomer = line[0];
            currCustInventory = currCustomer.GetShoppingBasket;
            SpawnItem();
        }
    }

    /// <summary>
    /// The current Customer will leave
    /// </summary>
    void DespawnCustomer() {
        prevCustomer = currCustomer;
        line.Remove(currCustomer);
        currCustomer.Leave();

        currCustInventory = null;
        currItem = null;
        currCustomer = null;
    }

    /// <summary>
    /// Spawns the first item in the current customer's inventory
    /// </summary>
    void SpawnItem() {
        if (currCustInventory.IsEmpty()) {
            DespawnCustomer();
            return;
        }

        ItemR item = (ItemR)currCustInventory.RemoveFromInventory();
        Node3D newItem = item.GetPackedScene().Instantiate<Node3D>();
        itemSpawnPos.AddChild(newItem);
        currItem = newItem;

        itemPriceLabel.Text = "+ " + item.BaseSellPrice;

        Player.Instance.SetMoney(item.BaseSellPrice);
        itemSpawnTimer.Start();
    }


    public void Interact(Node3D body) {
        if (body is Customer && prevCustomer != (Customer)body)
            AddCustomer((Customer)body);
        if (body is Player)
            Checkout();
    }

    public void OnItemSpawnTimerTimeout() {
        currItem.QueueFree();
        itemPriceLabel.Text = "";
        SpawnItem();
    }
}
