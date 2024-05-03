using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

public partial class Cashier : StaticBody3D, IInteractable {

    [Export] Node3D spawnPos;
    [Export] Node3D itemSpawnPos;
    [Export] Label3D itemPriceLabel;
    [Export] Timer itemSpawnTimer;

    bool isPlayerPresent = true;

    public List<Customer> GetLine { get { return line; } }
    public Node3D GetSpawnPos { get { return spawnPos; } }

    List<Customer> line = new List<Customer>();

    Node3D currItem;
    Inventory currCustInventory;
    Customer currCustomer = null;
    Customer prevCustomer = null;

    //removes a customer from the line
    //used when customers get too angry
    public void RemoveCustomer(Customer cust) {
        line.Remove(cust);
    }

    void AddCustomer(Customer cust) {
        if (line.Contains(cust)) return;

        cust.StartCashierTimer();
        line.Add(cust);
    }

    void Checkout() {
        //take first customer
        //

        if (line.Count != 0 && currCustomer == null) {
            currCustomer = line[0];
            currCustInventory = currCustomer.GetShoppingBasket;
            SpawnItem();
        }
    }

    void DespawnCustomer() {
        prevCustomer = currCustomer;
        line.Remove(currCustomer);
        currCustomer.Leave();

        currCustInventory = null;
        currItem = null;
        currCustomer = null;
    }

    void SpawnItem() {
        if (currCustInventory.IsEmpty()) {
            DespawnCustomer();
            return;
        }

        ItemR item = (ItemR)currCustInventory.RemoveFromInventory();
        Node3D newItem = item.GetPackedScene().Instantiate<Node3D>();
        //newItem.Position = itemSpawnPos.Position;
        itemSpawnPos.AddChild(newItem);
        currItem = newItem;

        itemPriceLabel.Text = "+ " + item.BaseSellPrice;

        Player.Instance.SetMoney(item.BaseSellPrice);
        GD.Print("Checking out " + item.GetName);

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
