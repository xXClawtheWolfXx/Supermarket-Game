using Godot;
using System;

public partial class ItemR : Resource {
    [Export] PackedScene scene;
    [Export] string name;
    [Export] int baseSellPrice;
    [Export] int baseBuyPrice;
    [Export] int weight;
}
