using Godot;
using System;
using Godot.Collections;
using System.Reflection;

public partial class Staff : Node3D, ICharacter {

    //[Export] protected NavigationAgent3D agent;
    [Export] protected NPC npcComp;
    // [Export] float speed = 10;
    //[Export] float rotateSpeed = 10;

    [Export] int energyDepleteAmt = 5;
    //[Export] Array<RayCast3D> raycasts;
    [Export] Hands hands;
    [Export] float restTimerTime = 8f;

    protected Timer restTimer;
    private bool isWorking = false;
    protected int energy = 100;

    public override void _Ready() {

        restTimer = new Timer();

        GameTime.Instance.OnTimeIncrease += CheckTime;
        GameManager.Instance.OnOpenStore += Work;


        restTimer.OneShot = true;
        restTimer.WaitTime = restTimerTime;
        restTimer.Timeout += OnRestTimerTimeout;
        AddChild(restTimer);


    }


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
        GD.PrintS(Name, "Resting");
        energy += 5;
        if (restTimer.TimeLeft <= 0)
            restTimer.Start();
    }

    protected void TakeTimeOff() {

    }

    public void CheckTime(Clock gameTime) {
        GD.PrintS(Name, "time checkd", gameTime);
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
