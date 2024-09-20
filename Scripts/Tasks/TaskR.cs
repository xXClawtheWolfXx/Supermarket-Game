using Godot;
using System;

[GlobalClass]
public partial class TaskR : Resource {

    [Export] private string taskName;
    [Export] private int duration;
    [Export] private bool isDurationTask;
    [Export] private Need needSatisfied;
    [Export] private bool isJobTask;

    public string GetTaskName { get { return taskName; } }
    public int GetDuration { get { return duration; } }
    public bool GetIsDurationTask { get { return isDurationTask; } }
    public Need GetNeedSatisfied { get { return needSatisfied; } }
    public bool GetIsJobTask { get { return isJobTask; } }


    public override string ToString() {
        return taskName + " takes: " + duration + " time and isDurationTask: " + isDurationTask + " and satisfies: " + needSatisfied.ToString();
    }
}
