using Godot;
using System;
using System.Collections.Generic;

public partial class RayPickerCamera : Camera3D {
    [Export] RayCast3D raycast;
    [Export] Player player;

    List<Item> items = new List<Item>();
    Item currItem;

    public override void _Ready() {
        raycast.AddExceptionRid(player.GetRid());
    }

    public override void _Process(double delta) {
        Vector2 mousePos = GetViewport().GetMousePosition();
        raycast.TargetPosition = ProjectLocalRayNormal(mousePos) * 100;
        raycast.ForceRaycastUpdate();

        //if raycast colliding, 
        //if currItem dne getcollider, unwhite item.
        //if same, do nothing
        //if no currItem 

        if (raycast.IsColliding() && raycast.GetCollider() is Item item) {

            if (currItem != null && currItem.GetName != item.GetName)
                TurnItemsBackToNormal();
            TurnItemsWhite(item);

        } else {
            if (currItem != null && Player.Instance.GetMouseOverItem != null)
                TurnItemsBackToNormal();
        }

    }

    private void TurnItemsWhite(Item item) {
        currItem = item;
        item.Change(true);
        Player.Instance.GetMouseOverItem = GameManager.Instance.GetItemR(item.GetName);
        GD.PrintS(item);
    }

    private void TurnItemsBackToNormal() {

        currItem.Change(false);
        currItem = null;

    }


}
