using Godot;
using System;

public partial class Item : Area3D {

    [Export] string name;
    [Export] Material whiteMat;
    [Export] MeshInstance3D mesh;

    Material currMat;

    [Signal] public delegate void OnMouseOnEventHandler(bool isOn, string name);

    public override void _Ready() {
        currMat = mesh.GetActiveMaterial(0);
        OnMouseOn += Change;
        GD.Print("Jungkook");

    }

    public void Change(bool isOn, string n) {
        if (name == n) {
            if (isOn) mesh.SetSurfaceOverrideMaterial(0, whiteMat);
            else mesh.SetSurfaceOverrideMaterial(0, currMat);
        }

    }

    public void OnMouseEntered() {
        GD.Print("Jimin");
        mesh.SetSurfaceOverrideMaterial(0, whiteMat);
        EmitSignal(SignalName.OnMouseOn, true, name);
    }

    public void OnMouseExited() {
        GD.Print("Yoongi");

        mesh.SetSurfaceOverrideMaterial(0, whiteMat);
        EmitSignal(SignalName.OnMouseOn, false, name);

    }



}
