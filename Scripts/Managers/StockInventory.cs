using Godot;

/// Holds the stock that the player bought but cannot stock Shelves with
public partial class StockInventory : StaticBody3D, IInteractable {

    [Export] DynamicInventory dynamicInventory;

    public void Interact(Node3D body) {
        if (body is Player player) {
            if (!Player.Instance.GetHands.IsEmpty())
                dynamicInventory.AddToInventory(Player.Instance.PutDown(), player);
            else
                dynamicInventory.RemoveFromInventory();
        } else if (body is Stocker stocker) {
            if (stocker.IsEmpty()) {
                //pick up stock
                CrateR crate = dynamicInventory.RemoveFromInventory(stocker.GetWorkingItem);
                stocker.PickUp(crate);
                stocker.StockShelf();
            } else {
                dynamicInventory.AddToInventory((CrateR)stocker.PutDown(), stocker);
                stocker.Rest();
            }
        }
    }

    public bool HasItemR(ItemR item) {
        return dynamicInventory.HasItemR(item);
    }

}
