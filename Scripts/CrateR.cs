using Godot;
using System;

/// <summary>
/// Holds an itemR, the amount of items to spawn, and scene to be instantiated into the world
/// </summary>
[GlobalClass]// to intialize in the inspector
public partial class CrateR : Resource, IGatherable {
    [Export] ItemR itemR;
    [Export] PackedScene crateScene;
    [Export] int amtToSpawn;

    public ItemR GetItemR { get { return itemR; } }
    public int GetAmtToSpawn { get { return amtToSpawn; } }

    public PackedScene GetPackedScene() { return crateScene; }

    public CrateR() : this(null, null, 0) { }

    public CrateR(ItemR i, PackedScene s, int amt) {
        itemR = i;
        crateScene = s;
        amtToSpawn = amt;
    }

    public CrateR(CrateR c, int amt) {
        itemR = c.itemR;
        crateScene = c.crateScene;
        amtToSpawn = amt;
    }

    public void UpdateAmt(int amt) {
        amtToSpawn = amt;
    }

    public override string ToString() {
        return "Crate of: " + amtToSpawn + " " + itemR.GetName;
    }

}
