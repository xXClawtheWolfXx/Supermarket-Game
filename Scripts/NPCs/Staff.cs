using Godot;
using System;
using Godot.Collections;

public partial class Staff : CharacterBody3D {

    [Export] protected NavigationAgent3D agent;
    [Export] float speed = 10;
    [Export] float rotateSpeed = 10;

    [Export] int energyDepleteAmt = 5;
    [Export] Array<RayCast3D> raycasts;
    //need rest area to go to

    bool isWorking = false;

    protected int energy = 100;
    public override void _Ready() {
        GameTime.Instance.OnTimeIncrease += CheckTime;
        GameManager.Instance.OnOpenStore += Work;
    }

    public override void _PhysicsProcess(double delta) {

        //interact with the world
        foreach (RayCast3D raycast in raycasts) {
            if (raycast.IsColliding() && raycast.GetCollider() is IInteractable ini) {
                ini.Interact(this);
                break;
            }
        }

        //move to place

        Vector3 nextLoc = agent.GetNextPathPosition();
        Vector3 offset = nextLoc - GlobalPosition;
        Vector3 newVel = offset.Normalized() * speed;

        Velocity = newVel;
        MoveAndSlide();

        if (GlobalTransform.Origin.IsEqualApprox(GlobalPosition + offset))
            return;

        //rotation
        offset.Y = 0;
        LookAt(GlobalPosition + offset, Vector3.Up);
    }

    protected void Move(Vector3 pos) {
        agent.TargetPosition = pos;
    }


    protected virtual void Work() {
        isWorking = true;
        energy -= energyDepleteAmt;
        if (energy <= 0)
            Rest();

    }

    public void Rest() {
        isWorking = false;
        //go to rest area and leave if energy fully dep
        Move(StaffManager.Instance.Position);
        GD.PrintS("Resting");
    }

    protected void TakeTimeOff() {

    }

    public void CheckTime(Clock gameTime) {
        GD.PrintS("time checkd", gameTime);
        if (gameTime.GetHour > 1 && gameTime.GetHour < 14) {
            Work();
        }
    }


}
