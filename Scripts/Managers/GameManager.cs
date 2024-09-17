using Godot;
using System;
using Godot.Collections;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

/// Manages the store openings, item translations, etc.
public partial class GameManager : Node {

    //singleton
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [Export] private int startTime;
    [Export] private int endTime;
    [Export] private Array<ItemR> allItems = new Array<ItemR>();

    private bool isOpen;

    public Array<ItemR> GetAllItems { get { return allItems; } }
    public bool GetIsOpen { get => isOpen; }


    [Signal] public delegate void OnOpenStoreEventHandler();
    [Signal] public delegate void OnCloseStoreEventHandler();

    //instantiates the instance when the game runs
    public override void _Ready() {
        instance = this;
        GameTime.Instance.OnTimeIncrease += CheckTime;

    }

    /// Opens the store
    public void OpenStore() {
        NPCSpawner.Instance.CanSpawn(true);
        EmitSignal(SignalName.OnOpenStore);
        isOpen = true;
    }

    /// Returns an ItemR matching the specified name from an Item
    public ItemR GetItemR(string name) {
        foreach (ItemR itemR in allItems)
            if (name == itemR.GetName)
                return itemR;
        return null;
    }

    public void CloseStore() {
        NPCSpawner.Instance.CanSpawn(false);
        GD.Print("closing store");
        EmitSignal(SignalName.OnCloseStore);
        isOpen = false;
    }

    private void CheckTime(int time) {
        //if (gameTime.Equals(startTime))
        //  OpenStore();
        //else 
        if (time == endTime)
            CloseStore();
    }
}


