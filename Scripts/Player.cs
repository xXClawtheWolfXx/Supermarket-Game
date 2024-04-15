using Godot;
using System;

public partial class Player : CharacterBody3D {

    private static Player instance;
    public static Player Instance { get { return instance; } }

    [ExportCategory("Movement")]
    [Export] float Speed = 5.0f;
    [Export] float JumpVelocity = 4.5f;
    [Export] float sensitivity = 0.5f;

    [Export] Node3D pivot;

    [ExportCategory("Jimin")]
    [Export] int money;
    [Export] Inventory hands;



    public int Money { get { return money; } set { money += value; } }

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready() {
        instance = this;
        Input.MouseMode = Input.MouseModeEnum.Captured;

    }

    //movement
    public override void _PhysicsProcess(double delta) {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            velocity.Y = JumpVelocity;

        if (Input.IsActionJustPressed("quit"))
            GetTree().Quit();

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero) {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        } else {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    //mouseMovement
    public override void _Input(InputEvent @event) {
        if (@event is InputEventMouseMotion mouseMotion) {
            RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * sensitivity));
            pivot.RotateX(Mathf.DegToRad(-mouseMotion.Relative.Y * sensitivity));
            pivot.Rotation = new Vector3(Mathf.Clamp(pivot.Rotation.X, Mathf.DegToRad(-90), Mathf.DegToRad(45)), pivot.Rotation.Y, pivot.Rotation.Z);
        }
    }

}
