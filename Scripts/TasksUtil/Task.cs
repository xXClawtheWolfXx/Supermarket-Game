using Godot;
using System;
using Godot.Collections;
using System.Threading;

[GlobalClass]

public partial class Task : Node3D {

    [Export] protected TaskR taskBase;
    [Export] protected bool isUsed = false;
    [Export] protected bool isCultureTask = false;
    [Export] protected Array<Task> adjacentTasks = new Array<Task>();

    public Need GetNeed { get { return taskBase.GetNeedSatisfied; } }
    public bool GetIsJobTask { get { return taskBase.GetIsJobTask; } }
    public int GetDuration { get { return taskBase.GetDuration; } }
    public Vector3 GetTaskPosition { get { return GlobalPosition; } }
    public bool IsBeingUsed { get { return isUsed; } }
    public bool GetIsCultureTask { get { return isCultureTask; } }
    public bool GetIsDurationTask { get { return taskBase.GetIsDurationTask; } }

    private Vector3 location;

    public float GetTaskScore(Vector3 npcPosition) {
        return location.DistanceTo(npcPosition);
    }

    public void SetBeingUsed(bool used) {
        isUsed = used;
    }

    public virtual void DoTask(NPC npc) {
        isUsed = true;
        GD.PrintS(npc.Name, "is doing", ToString());
    }

    public virtual bool CheckIfCanDoTask(NPC npc) {
        return !isUsed;
    }

    public virtual void FinishTask(NPC npc) {
        isUsed = false;
    }

    public virtual Task GetAdjacentTasks(NPC npc) {
        return null;
    }

    public override string ToString() {
        return taskBase.ToString() + " isUsed: " + isUsed;
    }
}
