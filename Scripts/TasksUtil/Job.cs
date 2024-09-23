using Godot;
using System;
using Godot.Collections;

[GlobalClass]

public partial class Job : Node3D {

    [Export] private Array<Task> tasks = new Array<Task>();
    //JobR soon
    [Export] private int startTime;
    [Export] private int endTime;
    [Export] private int minPay;
    [Export] private int maxPay;

    public int GetStartTime { get { return startTime; } }
    public int GetEndTime { get { return endTime; } }
    //public int GetMinPay { get { return minPay; } }
    //public int GetMaxPay { get { return maxPay; } }

    private int numTasksLookedAt = 0;

    public int GetPay() {
        return GD.RandRange(minPay, maxPay);
    }

    public Task GetJobTask(NPC npc) {
        int randNum = GD.RandRange(0, tasks.Count - 1);
        if (numTasksLookedAt >= tasks.Count)
            return null;
        if (!tasks[randNum].CheckIfCanDoTask(npc)) {
            numTasksLookedAt++;
            return GetJobTask(npc);
        }
        numTasksLookedAt = 0;
        return tasks[randNum];
    }
}
