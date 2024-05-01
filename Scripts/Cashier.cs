using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

public partial class Cashier : StaticBody3D, IInteractable {

    [Export] Node3D spawnPos;

    List<Customer> line = new List<Customer>();

    public List<Customer> GetLine { get { return line; } }
    public Node3D GetSpawnPos { get { return spawnPos; } }
    //removes a customer from the line
    //used when customers get too angry
    public void RemoveCustomer(Customer cust) {
        line.Remove(cust);
    }

    void AddCustomer(Customer cust) {
        cust.StartCashierTimer();
        line.Add(cust);
    }

    void Checkout() {
        //take first customer
        while (line.Count != 0) {
            Customer cust = line[0];
            Inventory custSB = cust.GetShoppingBasket;
            while (!custSB.IsEmpty()) {
                ItemR item = (ItemR)custSB.RemoveFromInventory();
                Player.Instance.SetMoney(item.BaseSellPrice);
                GD.Print("Checking out " + item.GetName);
            }

            line.RemoveAt(0);
            cust.Leave();
        }
    }

    public void Interact(Node3D body) {
        if (body is Customer)
            AddCustomer((Customer)body);
        if (body is Player)
            Checkout();
    }
}
