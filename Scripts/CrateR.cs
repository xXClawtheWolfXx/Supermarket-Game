using Godot;
using System;

[GlobalClass]
public partial class CrateR : Resource, IGatherable {
    [Export] ItemR itemR;
    [Export] PackedScene crateScene;
    [Export] int amtToSpawn;

    public ItemR GetItemR { get { return itemR; } }
    public int GetAmtToSpawn { get { return amtToSpawn; } }

    public PackedScene GetPackedScene() { return crateScene; }

}
