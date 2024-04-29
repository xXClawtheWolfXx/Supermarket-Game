using Godot;
using System;
using Godot.Collections;

public partial class Item : Area3D {

    [Export] string name;
    [Export] Material whiteMat;
    [Export] MeshInstance3D mesh;

    Material currMat;

    [Signal] public delegate void OnMouseOnEventHandler(bool isOn, string name);

    public override void _Ready() {
        currMat = mesh.GetActiveMaterial(0);
        //OnMouseOn += Change;

    }

    /*
    public void Change(bool isOn, string n) {
        GD.Print(Name);

        if (name == n) {
            if (isOn) mesh.SetSurfaceOverrideMaterial(0, whiteMat);
            else mesh.SetSurfaceOverrideMaterial(0, currMat);
        }

    }
    */

    void Change(bool isOn) {
        Array<Node> items = GetTree().GetNodesInGroup(name);
        foreach (Node i in items) {
            Item item = i.GetNode<Item>(".");
            if (isOn) item.mesh.SetSurfaceOverrideMaterial(0, whiteMat);
            else item.mesh.SetSurfaceOverrideMaterial(0, currMat);
        }
    }

    public void OnMouseEntered() {
        //mesh.SetSurfaceOverrideMaterial(0, whiteMat);
        Player.Instance.GetMouseOverItem = this; //GetItemR in gameManager?
        Change(true);
        //EmitSignal(SignalName.OnMouseOn, true, name);
        //GD.Print(Name);
    }

    public void OnMouseExited() {
        //mesh.SetSurfaceOverrideMaterial(0, whiteMat);
        Change(false);

        //EmitSignal(SignalName.OnMouseOn, false, name);
    }



}
