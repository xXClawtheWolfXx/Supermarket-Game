using Godot;
using System;
using Godot.Collections;
public partial class GameManager : Node {
    //singleton
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [Export] Array<ItemR> allItems;

    public Array<ItemR> GetAllItems { get { return allItems; } }

    public override void _Ready() {
        instance = this;
    }

    public static void OpenStore() {
        NPCSpawner.Instance.SpawnCustomer();
    }

    public void CloseStore() {

    }
}


