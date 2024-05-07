using Godot;
using System;
using Godot.Collections;

public partial class Staff : CharacterBody3D {

    [Export] int energyDepleteAmt = 5;
    [Export] Array<RayCast3D> raycasts;
    //need rest area to go to

    protected int energy = 100;
    public override void _Ready() {
        GameTime.Instance.OnTimeIncrease += CheckTime;
    }

    public override void _Process(double delta) {

        //interact with the world
        foreach (RayCast3D raycast in raycasts) {
            if (raycast.IsColliding() && raycast.GetCollider() is IInteractable) {
                ((IInteractable)raycast.GetCollider()).Interact(this);
                break;
            }
        }
    }



    protected virtual void Work() {
        energy -= energyDepleteAmt;
        if (energy <= 0)
            Rest();
    }

    protected void Rest() {
        //go to rest area and leave 
    }

    protected void TakeTimeOff() {

    }

    void CheckTime(Clock gameTime) {

    }

}
