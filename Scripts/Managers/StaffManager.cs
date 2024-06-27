using Godot;
using System;

public partial class StaffManager : Node3D {

    private static StaffManager instance;
    public static StaffManager Instance { get { return instance; } }

    public override void _Ready() {
        instance = this;
    }




}
