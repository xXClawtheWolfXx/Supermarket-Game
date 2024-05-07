using Godot;
using System;
using Godot.Collections;

/// <summary>
/// To be changed--need to only affect items in Stock's list
/// 
/// Changes the item's material 
/// </summary>
public partial class Item : Area3D {

    [Export] string name;
    [Export] Material whiteMat;
    [Export] MeshInstance3D mesh;

    public string GetName { get { return name; } }

    Material currMat;

    public override void _Ready() {
        currMat = mesh.GetActiveMaterial(0);
    }

    /// <summary>
    /// Changes all of the items with the same name's material to white if it can, and to normal if it can't
    /// </summary>
    /// <param name="isOn"> can it change to the white material</param>
    void Change(bool isOn) {
        Array<Node> items = GetTree().GetNodesInGroup(name);
        foreach (Node i in items) {
            Item item = i.GetNode<Item>(".");
            if (isOn) item.mesh.SetSurfaceOverrideMaterial(0, whiteMat);
            else item.mesh.SetSurfaceOverrideMaterial(0, currMat);
        }
    }

    //If the mouse is over the item, change it's material to white
    public void OnMouseEntered() {
        Player.Instance.GetMouseOverItem = GameManager.Instance.GetItemR(name);
        Change(true);
    }

    //If the mouse is no longer over the item, change it's material to normal
    public void OnMouseExited() {
        Change(false);
    }



}
