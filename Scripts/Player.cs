using Godot;
using System;

/// <summary>
/// Will move every physics Frame, obtain money, and hold items
/// </summary>
public partial class Player : CharacterBody3D {

    private static Player instance;
    public static Player Instance { get { return instance; } }

    [ExportCategory("Movement")]
    [Export] float Speed = 5.0f;
    [Export] float JumpVelocity = 4.5f;
    [Export] float sensitivity = 0.5f;
    [Export] Node3D pivot;

    [ExportCategory("Jimin")]
    [Export] int money = 100;
    [Export] Hands hands;

    ItemR mouseOverItem;
    IInteractable currInteractable;

    public int GetMoney { get { return money; } }

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

    //movement every physics frame
    public override void _PhysicsProcess(double delta) {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            velocity.Y = JumpVelocity;

        //handle quitting
        if (Input.IsActionJustPressed("quit"))
            GetTree().Quit();

        if (Input.IsActionJustPressed("alt"))
            UnstickCursor();
        //put down an IGatherable
        if (Input.IsActionJustPressed("X"))
            PutDown();

        // Get the input direction and handle the movement/deceleration.
        Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        //if movement
        if (direction != Vector3.Zero) {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        } else {//otw slow down
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

    /// <summary>
    /// Unsticks the Cursor or sticks it
    /// </summary>
    public void UnstickCursor() {
        isCursorShowing = !isCursorShowing;
        if (isCursorShowing)
            Input.MouseMode = Input.MouseModeEnum.Visible;
        else
            Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    /// <summary>
    /// adds a specified amount of money to the player's "wallet"
    /// </summary>
    /// <param name="amt"> the amount of money to be added</param>
    public void SetMoney(int amt) {
        money += amt;
        UIManager.Instance.UpdateMoneyLabel(money);
    }

    /// <summary>
    /// Adds an item to the inventory and spawns it in the world
    /// </summary>
    /// <param name="item">the IGatherable to add</param>
    public void PickUp(IGatherable item) {
        hands.PickUp(item);
    }

    /// <summary>
    /// Removes an item from inventory and despawns it inthe world
    /// </summary>
    /// <returns> the IGatherable removed</returns>
    public IGatherable PutDown() {
        return hands.PutDown();
    }

}
