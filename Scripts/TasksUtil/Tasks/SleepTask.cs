using Godot;
using System;

public partial class SleepTask : Task {

    [Export] private Node3D topOfBedPos;
    [Export] private Node3D sideOfBedPos;

    public override bool CheckIfCanDoTask(NPC npc) {
        return base.CheckIfCanDoTask(npc);
    }

    public override void DoTask(NPC npc) {
        if (npc.GetTaskStep == 0)
            npc.GlobalPosition = topOfBedPos.GlobalPosition;
        base.DoTask(npc);
    }

    public override void FinishTask(NPC npc) {
        //npc.IncreaseANeed(Need.REST, 20);
        npc.GlobalPosition = sideOfBedPos.GlobalPosition;
        base.FinishTask(npc);
    }
}
