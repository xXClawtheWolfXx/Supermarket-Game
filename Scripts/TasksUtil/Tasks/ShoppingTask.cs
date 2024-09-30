using Godot;
using System;

public partial class ShoppingTask : Task {

    public override bool CheckIfCanDoTask(NPC npc) {
        return base.CheckIfCanDoTask(npc);
    }

    public override void DoTask(NPC npc) {
        base.DoTask(npc);
    }
}
