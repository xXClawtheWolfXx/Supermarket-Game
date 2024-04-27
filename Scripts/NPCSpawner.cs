using Godot;
using System;
using System.Collections.Generic;

public partial class NPCSpawner : Node3D {

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

    public void SpawnCustomer() {
        timer.Start();
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

    public void DestroyCustomer(Customer cust) {
        cust.QueueFree();
        numNPCs--;
    }

    public void OnTimerTimeOut() {
        SpawnCustomer();
    }

}
