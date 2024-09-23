using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class Culture : Node3D {
    [Export] public Array<Task> tasks = new();


    public override void _Ready() {
        int numTasks = tasks.Count;

        for (int i = numTasks; i < GameTime.Instance.GetMaxTimeSlots; i++)
            tasks.Add(tasks[i % numTasks]);
    }

}
