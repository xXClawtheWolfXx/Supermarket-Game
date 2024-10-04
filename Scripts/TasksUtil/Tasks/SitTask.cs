using Godot;
using System;

public partial class SitTask : Task {
    [Export] private Chair chair;

    public override bool CheckIfCanDoTask(NPC npc) {
        return !chair.IsUsed;
        //return base.CheckIfCanDoTask(npc);
    }

    public override void DoTask(NPC npc) {
        //sit on chair
        if (npc.GetTaskStep == 0)
            chair.SitInChair(npc);
        base.DoTask(npc);
    }

    public override void FinishTask(NPC npc) {
        //get off chair
        chair.GetOutofChair(npc);
        base.FinishTask(npc);

    }

}
