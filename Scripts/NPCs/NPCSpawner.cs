using Godot;
using System;
using System.Collections.Generic;

/// Spawns many Customers and destroys them when they are finished
public partial class NPCSpawner : Node3D {

    //singleton
    private static NPCSpawner instance;
    public static NPCSpawner Instance { get { return instance; } }

    [Export] PackedScene customerScene;
    [Export] int maxNumInShoppingCart = 4;
    [Export] int maxNumNPCs = 1;
    [Export] Timer timer;
    [Export] float timeUntilNextCust = 10;

    int numNPCs = 0;

    public override void _Ready() {
        instance = this;
        timer.WaitTime = timeUntilNextCust;
    }

    /// Spawns a new customer every timeUntilNextCust seconds
    public void SpawnCustomer() {
        if (numNPCs >= maxNumNPCs) {
            GD.Print("Can spawn no more customers");
            return;
        }

        numNPCs++;
        Customer newCust = customerScene.Instantiate<Customer>();
        AddChild(newCust);
        newCust.ShoppingList = GetShoppingList();
        newCust.Browse();
    }

    /// Fills a Customer's shoppingList with random ItemRs
    public List<ItemR> GetShoppingList() {
        List<ItemR> shoppingList = new List<ItemR>();
        for (int i = 0; i < maxNumInShoppingCart; i++) {
            int randNum = GD.RandRange(0, GameManager.Instance.GetAllItems.Count - 1);
            ItemR item = GameManager.Instance.GetAllItems[randNum];
            GD.Print(i + ": " + item.GetName);
            shoppingList.Add(item);
        }
        return shoppingList;
    }

    /// Destroys a specified customer from the world
    public void DestroyCustomer(Customer cust) {
        cust.QueueFree();
        numNPCs--;
    }

    public void OnTimerTimeOut() {
        SpawnCustomer();
    }

    public void CanSpawn(bool canSpawn) {
        if (canSpawn) {
            SpawnCustomer();
            timer.Start();
        } else
            timer.Stop();
    }

}
