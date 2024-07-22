using Godot;
using System;
using Godot.Collections;

public partial class Staff : Node3D, ICharacter {

    //[Export] protected NavigationAgent3D agent;
    [Export] protected NPC npcComp;
    // [Export] float speed = 10;
    //[Export] float rotateSpeed = 10;

    [Export] int energyDepleteAmt = 5;
    //[Export] Array<RayCast3D> raycasts;
    [Export] Hands hands;
    [Export] float restTimerTime = 8f;

    protected Timer restTimer = new();
    private bool isWorking = false;
    protected int energy = 100;

    public override void _Ready() {
        GameTime.Instance.OnTimeIncrease += CheckTime;
        GameManager.Instance.OnOpenStore += Work;

        restTimer.OneShot = true;
        restTimer.WaitTime = restTimerTime;
        restTimer.Timeout += OnRestTimerTimeout;
        AddChild(restTimer);
    }

    public override void _PhysicsProcess(double delta) {
        /*
        //interact with the world
        foreach (RayCast3D raycast in raycasts) {
            if (raycast.IsColliding() && raycast.GetCollider() is IInteractable ini) {
                ini.Interact(this);
                break;
            }
        }

        if (agent.IsNavigationFinished())
            return;

        //move to place

        Vector3 nextLoc = agent.GetNextPathPosition();
        Vector3 offset = nextLoc - GlobalPosition;
        Vector3 newVel = offset.Normalized() * speed;

        agent.Velocity = newVel;

        MoveAndSlide();

        if (GlobalTransform.Origin.IsEqualApprox(GlobalPosition + offset))
            return;

        //rotation
        offset.Y = 0;
        LookAt(GlobalPosition + offset, Vector3.Up);
        */
    }
    /*
    protected void Move(Vector3 pos) {
        agent.TargetPosition = pos;
    }
*/

    protected virtual void Work() {
        if (!isWorking) return;
        isWorking = true;
        energy -= energyDepleteAmt;
        if (energy <= 0)
            Rest();
    }

    public void OnRestTimerTimeout() {
        energy = 100;
        Work();
    }

    public void Rest() {
        isWorking = false;
        //go to rest area and leave if energy fully dep

        npcComp.Move(StaffManager.Instance.Position);
        GD.PrintS("Resting");
        energy += 5;
        if (restTimer.TimeLeft <= 0)
            restTimer.Start();
    }

    protected void TakeTimeOff() {

    }

    public void CheckTime(Clock gameTime) {
        GD.PrintS("time checkd", gameTime);
        if (gameTime.GetHour > 1 && gameTime.GetHour < 14) {
            Work();
        }
    }
    public void PickUp(IGatherable item) {
        hands.PickUp(item);
    }

    public IGatherable PutDown() {
        return hands.PutDown();
    }

    public bool IsEmpty() {
        return hands.IsEmpty();
    }
    /*
        public void OnNavigationAgent3DVelocityComputed(Vector3 safeVel) {
            Velocity = safeVel;
            MoveAndSlide();
        }
        */
}
