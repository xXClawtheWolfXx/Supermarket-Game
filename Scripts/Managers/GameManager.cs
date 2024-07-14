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

    [Export] Clock startTime;
    [Export] Clock endTime;
    [Export] Array<ItemR> allItems = new Array<ItemR>();

    public Array<ItemR> GetAllItems { get { return allItems; } }

    [Signal] public delegate void OnOpenStoreEventHandler();

    //instantiates the instance when the game runs
    public override void _Ready() {
        instance = this;
        GameTime.Instance.OnTimeIncrease += CheckTime;

    }

    /// Opens the store
    public void OpenStore() {
        NPCSpawner.Instance.CanSpawn(true);
        EmitSignal(SignalName.OnOpenStore);
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
    }

    private void CheckTime(Clock gameTime) {
        if (gameTime.Equals(startTime))
            OpenStore();
        else if (gameTime.Equals(endTime))
            CloseStore();
    }
}


