using Godot;
using System;

public partial class BrowseTask : ShoppingTask {

    [Export] private ItemR item;

    //Set by the shelf this is attached to
    public void SetItem(ItemR itemIn) {
        item = itemIn;
    }

    public override bool CheckIfCanDoTask(NPC npc) {
        if (npc.IsBasketFull()) {
            return false;
        }
        return base.CheckIfCanDoTask(npc);
    }

    public override void DoTask(NPC npc) {
        if (npc.GetTaskStep == 0) {
            int chanceOfGettingItem = GD.RandRange(0, 1);
            if (chanceOfGettingItem == 1) {
                npc.PutInBasket(item);
            }
        }

        base.DoTask(npc);
    }

    public override void FinishTask(NPC npc) {
        base.FinishTask(npc);
        if (npc.IsBasketFull()) {
            npc.SetCurrTask(store.GetCheckoutTask);
        }
    }
}

