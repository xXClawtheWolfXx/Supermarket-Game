using Godot;
using System;
using System.Collections.Generic;

public partial class CashRegister : StaticBody3D, IInteractable {

    [Export] Node3D custSpawnPos;
    [Export] Node3D cashierSpawnPos;
    [Export] Node3D itemSpawnPos;
    [Export] Label3D itemPriceLabel;
    [Export] Timer itemSpawnTimer;
    [Export] float custSpawnRadius = 3f;

    bool isPlayerPresent = true; // what if the player walks away mid-checkout?

    public List<Customer> GetLine { get { return line; } }
    public Node3D GetSpawnPos { get { return custSpawnPos; } }

    [Signal] public delegate void OnCustomerAddedEventHandler(Vector3 cashSpawnPos);
    [Signal] public delegate void OnNoCustomersEventHandler();

    List<Customer> line = new List<Customer>();

    Node3D currItem;
    Vector3 originalCustSpawnPosition;
    Inventory currCustInventory;
    Customer currCustomer = null;
    Customer prevCustomer = null;

    public override void _Ready() {
        originalCustSpawnPosition = custSpawnPos.GlobalPosition;
    }


    //removes a customer from the line
    //used when customers get too angry
    public void RemoveCustomer(Customer cust) {
        line.Remove(cust);
    }


    /// Add a specified customer to the line
    public void AddCustomer(Customer cust) {
        if (line.Contains(cust)) return;

        //increase spawnPos
        custSpawnPos.Position += new Vector3(0f, 0f, -custSpawnRadius);
        EmitSignal(SignalName.OnCustomerAdded, cashierSpawnPos.GlobalPosition);
        cust.StartCashierTimer();
        line.Add(cust);
        GD.PrintS("added cust", line.Count);
    }

    ///Will process all the items in a customer's cart and spawn them at the itemSpawnPos
    void Checkout() {
        if (line.Count != 0 && currCustomer == null) {
            currCustomer = line[0];
            currCustInventory = currCustomer.GetShoppingBasket;
            SpawnItem();
        }
    }

    /// The current Customer will leave
    void DespawnCustomer() {
        prevCustomer = currCustomer;
        line.Remove(currCustomer);
        GD.PrintS("gone cust", line.Count);

        currCustomer.Leave();
        GD.PrintS("gone realyl cust", line.Count);


        MoveCustomersUpInLine();

        if (line.Count == 0)
            EmitSignal(SignalName.OnNoCustomers);

        currCustInventory = null;
        currItem = null;
        currCustomer = null;
    }

    void MoveCustomersUpInLine() {
        GD.PrintS("cust in line", line.Count);
        for (int i = 0; i < line.Count; i++) {
            GD.PrintS("moving to", originalCustSpawnPosition + new Vector3(0f, 0f, custSpawnRadius * i));
            line[i].Move(originalCustSpawnPosition + new Vector3(0f, 0f, custSpawnRadius * i));
        }
    }

    /// Spawns the first item in the current customer's inventory
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
        /* if (body is Customer customer && prevCustomer != customer && customer.IsCheckingOut)
             AddCustomer(customer);*/
        if (body is Player || body is Cashier)
            Checkout();
    }

    public void OnItemSpawnTimerTimeout() {
        currItem.QueueFree();
        itemPriceLabel.Text = "";
        SpawnItem();
    }
}
