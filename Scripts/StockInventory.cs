using Godot;

public partial class StockInventory : StaticBody3D, IInteractable {

    [Export] DynamicInventory dynamicInventory;

    public void Interact(Node3D body) {
        if (body is Player)
            if (!Player.Instance.GetHands.IsEmpty())
                dynamicInventory.AddToInventory(Player.Instance.PutDown());
            else
                dynamicInventory.RemoveFromInventory();
    }

}
