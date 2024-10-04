using Godot;
using System;
using Godot.Collections;


public partial class Item : Area3D {

    [Export] string name;
    [Export] Material whiteMat;
    [Export] MeshInstance3D mesh;

    public string GetName { get { return name; } }

    Material currMat;

    public override void _Ready() {
        currMat = mesh.GetActiveMaterial(0);
    }


    //Changes all of the items with the same name's material to white if it can, and to normal if it can't
    public void Change(bool isOn) {
        Array<Node> items = GetParent().GetChildren();
        foreach (Node i in items) {
            if (i.IsInGroup(name)) {
                Item item = i.GetNode<Item>(".");
                if (isOn) item.mesh.SetSurfaceOverrideMaterial(0, whiteMat);
                else item.mesh.SetSurfaceOverrideMaterial(0, currMat);
            }
        }
    }

}
