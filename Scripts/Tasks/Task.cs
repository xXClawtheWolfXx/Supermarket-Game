using Godot;
using System;

public partial class Task : Node3D {

    [Export] private TaskR taskBase;
    [Export] private bool isUsed = false;

    private Vector3 location;

    public float GetTaskCost(Vector3 npcPosition) {
        return location.DistanceTo(npcPosition);
    }

    public void SetBeingUsed(bool used) {
        isUsed = used;
    }

    public bool IsBeingUsed() {
        return isUsed;
    }
}
