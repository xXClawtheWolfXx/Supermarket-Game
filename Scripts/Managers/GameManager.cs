using Godot;
using System;
using Godot.Collections;

/// <summary>
/// Manages the store openings, item translations, etc.
/// </summary>
public partial class GameManager : Node {

    //singleton
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [Export] Array<ItemR> allItems = new Array<ItemR>();

    public Array<ItemR> GetAllItems { get { return allItems; } }

    //instantiates the instance when the game runs
    public override void _Ready() {
        instance = this;
    }

    /// <summary>
    /// Opens the store
    /// </summary>
    public static void OpenStore() {
        NPCSpawner.Instance.SpawnCustomer();
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


