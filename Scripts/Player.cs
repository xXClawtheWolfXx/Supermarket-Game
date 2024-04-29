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
    [Export] Hands hands;

    ItemR mouseOverItem;
    IInteractable currInteractable;

    public int Money { get { return money; } set { money += value; } }
    public Hands GetHands { get { return hands; } }

    public ItemR GetMouseOverItem { get { return mouseOverItem; } set { mouseOverItem = value; } }
    public IInteractable GetCurrInteractable { get { return currInteractable; } set { currInteractable = value; } }

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    bool isCursorShowing = false;

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

        if (Input.IsActionJustPressed("alt"))
            UnstickCursor();
        if (Input.IsActionJustPressed("X"))
            PutDown();

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
        if (@event is InputEventMouseMotion mouseMotion && !isCursorShowing) {
            RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * sensitivity));
            pivot.RotateX(Mathf.DegToRad(-mouseMotion.Relative.Y * sensitivity));
            pivot.Rotation = new Vector3(Mathf.Clamp(pivot.Rotation.X, Mathf.DegToRad(-90), Mathf.DegToRad(45)), pivot.Rotation.Y, pivot.Rotation.Z);
        }
    }

    public void UnstickCursor() {
        isCursorShowing = !isCursorShowing;
        if (isCursorShowing)
            Input.MouseMode = Input.MouseModeEnum.Visible;
        else
            Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public void PickUp(IGatherable item) {
        if (hands.PickUp(item)) {
            hands.ShowItem();
        }
    }

    public IGatherable PutDown() {
        return hands.PutDown();
    }

}
