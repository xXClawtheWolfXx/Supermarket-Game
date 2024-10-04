using Godot;
using System;
using System.Xml.Schema;

//later make playerinteractwith chair
public partial class Chair : StaticBody3D {
    [Export] private bool isUsed;
    [Export] private Node3D topOfChair;
    [Export] private Node3D sideOfChair;

    public bool IsUsed { get { return isUsed; } set { isUsed = value; } }

    public void SitInChair(Node3D body) {
        if (isUsed) return;
        isUsed = true;
        body.GlobalPosition = topOfChair.GlobalPosition;
    }

    public void GetOutofChair(Node3D body) {
        isUsed = false;
        body.GlobalPosition = sideOfChair.GlobalPosition;
    }
}
