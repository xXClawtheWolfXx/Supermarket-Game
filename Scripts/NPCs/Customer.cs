using Godot;
using System.Collections.Generic;
using Godot.Collections;
using System.Data.Common;

public enum Mood { HAPPY, NEUTRAL, ANGRY, ENRAGED };

/// <summary>
/// Customers browse the store for the items in their cart, fill them, then leave the store
/// </summary>
public partial class Customer : CharacterBody3D {

    [Export] Timer cashierTimer;
    [Export] Inventory shoppingBasket;
    [Export] Array<RayCast3D> raycasts;

    [Export] NavigationAgent3D agent;
    [Export] float speed = 10;
    [Export] float rotateSpeed = 10;

    public Inventory GetShoppingBasket { get => shoppingBasket; }
    public ItemR GetItemLookingFor { get => first; }

    List<ItemR> shoppingList = new List<ItemR>();
    Mood mood = Mood.HAPPY;
    int shoppingIndex = 0;

    bool isCheckingOut = false;

    ItemR first;

    public List<ItemR> ShoppingList { set => shoppingList = value; }
    public bool IsCheckingOut { get => isCheckingOut; }

    //every physics frame, get closer to the target position and check if interacting
    public override void _PhysicsProcess(double delta) {

        //check if Customer is interacting
        foreach (RayCast3D rayCast3D in raycasts) {
            if (rayCast3D.IsColliding() && rayCast3D.GetCollider() is IInteractable) {
                ((IInteractable)rayCast3D.GetCollider()).Interact(this);
                break;
            }
        }


        //if at destination, no need to calculate potition
        if (agent.IsNavigationFinished()) {
            if (agent.TargetPosition == NPCSpawner.Instance.Position)
                NPCSpawner.Instance.DestroyCustomer(this);
            return;
        }

        ///////movememt

        Vector3 nextLoc = agent.GetNextPathPosition();
        Vector3 offset = nextLoc - GlobalPosition;
        Vector3 newVel = offset.Normalized() * speed;

        agent.Velocity = newVel;

        MoveAndSlide();

        ///rotation 
        offset.Y = 0;
        if (GlobalTransform.Origin.IsEqualApprox(GlobalPosition + offset))
            return;
        LookAt(GlobalPosition + offset, Vector3.Up);
    }

    /// Fills the shoppingBasket with a specified item
    public void FillShoppingBasket(ItemR item) {
        if (item == null) {
            Complain("there's no more items");
            while (shoppingList.Contains(first)) {
                shoppingList.Remove(first);
            }
        } else {
            shoppingBasket.AddToInventory(item);
            shoppingList.Remove(item);
        }
        Browse();
    }

    /// Checks if there are still more of a specified item in the shopping list
    void CheckIfMoreInList(ItemR item) {
        if (shoppingIndex >= shoppingList.Count) { Leave(); return; }
        if (shoppingList[shoppingIndex].GetName == item.GetName) {
            shoppingIndex++;
            CheckIfMoreInList(item);
        }
    }

    /// Returns how many items of the specified item are in the shopping list
    public int HowManyItems(ItemR item) {
        int count = 0;
        foreach (ItemR i in shoppingList)
            if (item == i)
                count++;
        return count;
    }

    /// Finds all the items on the shopping list and collects them 
    public void Browse() {
        if (shoppingList.Count == 0) {
            Checkout();
            return;
        }

        first = shoppingList[0];
        //GD.Print("first: " + first.GetName);

        Array<Node> shelves = GetTree().GetNodesInGroup("Shelf");
        foreach (Node shelfNode in shelves) {
            Shelf shelf = shelfNode.GetNode<Shelf>(".");
            if (!shelf.HasItemR())
                continue;
            if (shelf.HasItemR() && shelf.GetItemR?.GetName != first.GetName)
                continue;
            //we found a shelf
            Move(shelf.GetCustomerSpawnPos.GlobalPosition);
            return;
        }
        shoppingList.RemoveAt(0);
        //if can't find anything
        Complain("nothing I want is here");
        Browse();
    }



    /// Decreases mood and prints a message 
    void Complain(string msg) {
        mood++;
        GD.Print(Name + " is " + mood.ToString() + " " + msg);
        if (mood >= Mood.ANGRY)
            Leave();
    }

    //when happy and found a couple of items
    //maybe shopping index % 5 or 3 
    void Praise() {

    }

    public void Leave() {
        Move(NPCSpawner.Instance.Position);
    }

    /// Updates targetPosition to specified position
    public void Move(Vector3 pos) {
        agent.TargetPosition = pos;
    }

    /// Finds a cashier [with conditions] and moves to them
    /// If there's nothing in their shopping basket, they will leave
    void Checkout() {
        isCheckingOut = true;
        if (shoppingBasket.IsEmpty()) {
            Complain("There's nothing for me here!");
            Leave();
            return;
        }
        Array<Node> cashRegisters = GetTree().GetNodesInGroup("CashRegister");
        //eventually check which cashier has the least people on it and go there
        CashRegister cashRegister = cashRegisters[0].GetNode<CashRegister>(".");
        Move(cashRegister.GetSpawnPos.GlobalPosition);
        cashRegister.AddCustomer(this);

    }

    public void StartCashierTimer() {
        cashierTimer.Start();
    }

    public void OnCashierTimerTimeout() {
        Complain("The cashier is taking too long");
    }

    public void OnNavigationAgent3DVelocityComputed(Vector3 safeVel) {
        Velocity = safeVel;//Velocity.MoveToward(safeVel, .9f);
        MoveAndSlide();
    }
}
