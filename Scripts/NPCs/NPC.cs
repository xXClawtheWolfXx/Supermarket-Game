using Godot;
using System;
using Godot.Collections;

public partial class NPC : CharacterBody3D {

    [Export] private Array<RayCast3D> raycasts;
    [Export] protected NavigationAgent3D agent;
    [Export] private float speed = 10;
    [Export] private float rotateSpeed = 10;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        //check if customer is interacting
        foreach (RayCast3D raycast in raycasts) {
            if (raycast.IsColliding() && raycast.GetCollider() is IInteractable interactable) {
                interactable.Interact(this);
                break;
            }
        }

        //if at destination, no need to calculate potition
        if (agent.IsNavigationFinished()) {
            if (agent.TargetPosition == NPCSpawner.Instance.Position)
                //GD.Print("Finished---------------------");
                NPCSpawner.Instance.DestroyCustomer(this);
            return;
        }

        //move character
        Vector3 nextLoc = agent.GetNextPathPosition();
        Vector3 offset = nextLoc - GlobalPosition;
        Vector3 newVel = offset.Normalized() * speed;

        agent.Velocity = newVel;
        MoveAndSlide();

        //rotation
        offset.Y = 0;
        if (GlobalTransform.Origin.IsEqualApprox(GlobalPosition + offset))
            return;
        LookAt(GlobalPosition + offset, Vector3.Up);
    }

    /// Updates targetPosition to specified position
    public void Move(Vector3 pos) {
        agent.TargetPosition = pos;
    }

    public void OnNavigationAgent3DVelocityComputed(Vector3 safeVel) {
        Velocity = safeVel;
        MoveAndSlide();
    }

}
