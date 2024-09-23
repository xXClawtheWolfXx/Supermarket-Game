using Godot;
using System;
using System.Xml.Serialization;

/// Will move every physics Frame, obtain money, and hold items
public partial class Player : CharacterBody3D, ICharacter {

    private static Player instance;
    public static Player Instance { get { return instance; } }

    [ExportGroup("Movement")]
    [Export] private float normalSpeed = 600;
    [Export] private float runSpeed = 1200;
    [Export] private float rotationSpeed = 8;
    [Export] private float jumpHeight = 3;
    [Export] private float apexDuration = 0.5f;
    [Export] private float fallGravity = 45;
    [Export] private Node3D meshRoot;

    [ExportGroup("Camera")]
    [Export] private Node3D twistPivot;
    [Export] private SpringArm3D pitchPivot;
    [Export] private float mouseSensitivity = 0.5f;
    [Export] private float zoomSensitivity = 1;
    [Export] private float minZoom = 1;
    [Export] private float maxZoom = 11;


    [ExportCategory("Everything Else")]
    [Export] int money = 100;
    [Export] Hands hands;

    private ItemR mouseOverItem;
    private IInteractable currInteractable;
    private bool isCursorShowing = false;


    private float speed;
    private float jumpGravity;
    private Vector3 vel;
    private float zoom;
    private float twistInput;
    private float pitchInput;

    public int GetMoney { get { return money; } }
    public Hands GetHands { get { return hands; } }
    public ItemR GetMouseOverItem { get { return mouseOverItem; } set { mouseOverItem = value; } }
    public IInteractable GetCurrInteractable { get { return currInteractable; } set { currInteractable = value; } }


    public override void _Ready() {
        instance = this;
        Input.MouseMode = Input.MouseModeEnum.Captured;

        zoom = pitchPivot.SpringLength;
        speed = normalSpeed;
        jumpGravity = fallGravity;
    }

    //movement every physics frame
    public override void _PhysicsProcess(double delta) {
        vel = Velocity;

        Misc();

        //gravity
        if (!IsOnFloor()) {
            if (vel.Y >= 0)
                vel.Y -= jumpGravity * (float)delta;
            else
                vel.Y -= fallGravity * (float)delta;
        }

        //jump
        if (Input.IsActionPressed("jump") && IsOnFloor()) {
            vel.Y = 2 * jumpHeight / apexDuration;
            jumpGravity = vel.Y / apexDuration;
        }

        //zoom
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        pitchPivot.SpringLength = Mathf.MoveToward(pitchPivot.SpringLength, zoom, 10 * (float)delta);

        //run
        if (Input.IsActionPressed("run")) {
            speed = runSpeed;
        } else {
            speed = normalSpeed;
        }

        //movement
        Vector2 input = Input.GetVector("left", "right", "up", "down");
        Vector3 moveDir = (twistPivot.Basis * new Vector3(input.X, 0, input.Y)).Normalized();

        if (moveDir != Vector3.Zero) {
            vel.X = moveDir.X * speed * (float)delta;
            vel.Z = moveDir.Z * speed * (float)delta;

            //rotation
            float targetAngle = Mathf.Atan2(moveDir.X, moveDir.Z) - Rotation.Y;
            meshRoot.Rotation = new Vector3(
                meshRoot.Rotation.X,
                Mathf.LerpAngle(meshRoot.Rotation.Y, targetAngle, rotationSpeed * (float)delta),
                meshRoot.Rotation.Z
            );
        } else {
            vel.X = Mathf.MoveToward(vel.X, 0, speed);
            vel.Z = Mathf.MoveToward(vel.Z, 0, speed);
        }

        Velocity = vel;
        MoveAndSlide();

        twistInput = pitchInput = 0;

    }

    //mouseMovement and player Rotation
    public override void _Input(InputEvent @event) {
        if (@event is InputEventMouseMotion mouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured) {
            twistInput = -mouseMotion.Relative.X * mouseSensitivity;
            pitchInput = -mouseMotion.Relative.Y * mouseSensitivity;

            twistPivot.RotateY(Mathf.DegToRad(twistInput));
            pitchPivot.RotateX(Mathf.DegToRad(pitchInput));
            pitchPivot.Rotation = new Vector3(
                Mathf.Clamp(pitchPivot.Rotation.X, Mathf.DegToRad(-90), Mathf.DegToRad(90)),
                pitchPivot.Rotation.Y,
                pitchPivot.Rotation.Z
            );
        }

        if (@event.IsActionPressed("zoomIn"))
            zoom -= zoomSensitivity;
        else if (@event.IsActionPressed("zoomOut"))
            zoom += zoomSensitivity;
    }

    //quit logic, unstick cursor logic, and get rid of crate logic
    private void Misc() {
        //handle quitting
        if (Input.IsActionJustPressed("quit"))
            GetTree().Quit();

        if (Input.IsActionJustPressed("alt"))
            UnstickCursor();
        //put down an IGatherable
        if (Input.IsActionJustPressed("X"))
            PutDown();
    }

    /// Unsticks the Cursor or sticks it
    public void UnstickCursor() {
        isCursorShowing = !isCursorShowing;
        if (isCursorShowing)
            Input.MouseMode = Input.MouseModeEnum.Visible;
        else
            Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    /// adds a specified amount of money to the player's "wallet"
    public void SetMoney(int amt) {
        money += amt;
        UIManager.Instance.UpdateMoneyLabel(money);
    }

    /// Adds an item to the inventory and spawns it in the world
    public void PickUp(IGatherable item) {
        hands.PickUp(item);
    }

    /// Removes an item from inventory and despawns it inthe world
    public IGatherable PutDown() {
        return hands.PutDown();
    }

}
