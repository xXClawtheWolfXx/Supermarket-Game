using Godot;
using System;

[GlobalClass]
public partial class TaskR : Resource {

    [Export] private string taskName;
    [Export] private int duration;
    [Export] private bool isDurationTask;

    public override string ToString() {
        return taskName + " takes " + duration + " and " + isDurationTask;
    }
}
