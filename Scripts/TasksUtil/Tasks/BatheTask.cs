using Godot;
using System;

public partial class BatheTask : Task {

    public override bool CheckIfCanDoTask(NPC npc) {
        return base.CheckIfCanDoTask(npc);
    }

    public override void DoTask(NPC npc) {
        base.DoTask(npc);
    }

    public override Task GetAdjacentTasks(NPC npc) {
        return base.GetAdjacentTasks(npc);
    }

}
