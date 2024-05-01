using Godot;
using System;
using System.Runtime.CompilerServices;

[GlobalClass]
public partial class ItemR : Resource, IGatherable {
    [Export] PackedScene scene;
    [Export] string name;
    [Export] int baseSellPrice;
    [Export] int baseBuyPrice;
    [Export] int weight;

    public string GetName { get { return name; } }
    public int BaseSellPrice { get { return baseSellPrice; } }
    public int BaseBuyPrice { get { return baseBuyPrice; } }
    public int Weight { get { return weight; } }

    public PackedScene GetPackedScene() { return scene; }

    public ItemR() : this("", 0, 0, 0, null) { }

    public ItemR(string n, int bsp, int bbp, int w, PackedScene s) {
        name = n;
        baseSellPrice = bsp;
        baseBuyPrice = bbp;
        weight = w;
        scene = s;
    }

    public override string ToString() {
        return "ItemR: " + name + " sell:" + baseSellPrice + " buy: "
        + baseBuyPrice + " weight: " + weight;
    }
}
