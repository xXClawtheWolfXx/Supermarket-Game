using Godot;
using System;
using Godot.Collections;
using System.Runtime.Serialization;

/// <summary>
/// Manages the store openings, item translations, etc.
/// </summary>
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

    }

    /// <summary>
    /// Opens the store
    /// </summary>
    public void OpenStore() {
        NPCSpawner.Instance.SpawnCustomer();
        EmitSignal(SignalName.OnOpenStore);
    }

    /// <summary>
    /// Returns an ItemR matching the specified name from an Item
    /// </summary>
    /// <param name="name"> the name of the ItemR</param>
    /// <returns>an ItemR with the specified name</returns>
    public ItemR GetItemR(string name) {
        foreach (ItemR itemR in allItems)
            if (name == itemR.GetName)
                return itemR;
        return null;
    }

    public void CloseStore() {

    }
}


