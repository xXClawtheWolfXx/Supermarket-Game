using Godot;
using System;

public partial class CustomerCheckout : ShoppingTask {

    public override bool CheckIfCanDoTask(NPC npc) {

        return base.CheckIfCanDoTask(npc);
    }

    public override void DoTask(NPC npc) {
        if (npc.GetTaskStep == 0) {
            //move to line
        }

        base.DoTask(npc);
    }

    public override void FinishTask(NPC npc) {
        base.FinishTask(npc);
        //so the npcs dont shop indefinetly
    }
}

