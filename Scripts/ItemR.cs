using Godot;
using System;

public partial class ItemR : Resource {
    [Export] PackedScene scene;
    [Export] string name;
    [Export] int baseSellPrice;
    [Export] int baseBuyPrice;
    [Export] int weight;

    public PackedScene GetPackedScene { get { return scene; } }
    public string GetName { get { return name; } }
    public int BaseSellPrice { get { return baseSellPrice; } }
    public int BaseBuyPrice { get { return baseBuyPrice; } }
    public int Weight { get { return weight; } }
}
