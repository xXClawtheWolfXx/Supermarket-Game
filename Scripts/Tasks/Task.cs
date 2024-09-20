using Godot;
using System;

public partial class Task : Node3D {

    [Export] private TaskR taskBase;
    [Export] private bool isUsed = false;

    public Need GetNeed { get { return taskBase.GetNeedSatisfied; } }
    public bool GetIsJobTask { get { return taskBase.GetIsJobTask; } }
    public int GetDuration { get { return taskBase.GetDuration; } }
    public Vector3 GetTaskPosition { get { return Position; } }

    private Vector3 location;

    public float GetTaskScore(Vector3 npcPosition) {
        return location.DistanceTo(npcPosition);
    }

    public void SetBeingUsed(bool used) {
        isUsed = used;
    }

    public bool IsBeingUsed() {
        return isUsed;
    }

    public override string ToString() {
        return taskBase.ToString() + " isUsed: " + isUsed;
    }
}
