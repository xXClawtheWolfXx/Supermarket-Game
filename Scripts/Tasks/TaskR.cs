using Godot;
using System;

[GlobalClass]
public partial class TaskR : Resource {

    [Export] private string taskName;
    [Export] private int duration;
    [Export] private bool isDurationTask;
    [Export] private Need needSatisfied;
    [Export] private bool isJobTask;

    public override string ToString() {
        return taskName + " takes " + duration + " and " + isDurationTask + " and satisfies " + needSatisfied.ToString();
    }
}
