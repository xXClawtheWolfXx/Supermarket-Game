using Godot;
using System.Collections.Generic;
using Godot.Collections;

public enum Mood { HAPPY, NEUTRAL, ANGRY };

public partial class Customer : CharacterBody3D {


    [Export] Timer cashierTimer;
    [Export] Inventory shoppingBasket;
    [Export] Array<RayCast3D> raycasts;

    [Export] NavigationAgent3D agent;
    [Export] float speed = 10;
    [Export] float rotateSpeed = 10;

    public Inventory GetShoppingBasket { get { return shoppingBasket; } }
    public ItemR GetItemLookingFor { get { return first; } }

    List<ItemR> shoppingList = new List<ItemR>();
    Mood mood = Mood.HAPPY;
    int shoppingIndex = 0;


    ItemR first;

    public List<ItemR> ShoppingList { set { shoppingList = value; } }

    public override void _PhysicsProcess(double delta) {

        foreach (RayCast3D rayCast3D in raycasts) {
            if (rayCast3D.IsColliding() && rayCast3D.GetCollider() is IInteractable) {
                ((IInteractable)rayCast3D.GetCollider()).Interact(this);
                break;
            }
        }

        if (agent.IsNavigationFinished()) {
            if (agent.TargetPosition == NPCSpawner.Instance.Position)
                NPCSpawner.Instance.DestroyCustomer(this);
            return;
        }
        Vector3 nextLoc = agent.GetNextPathPosition();
        Vector3 offset = nextLoc - GlobalPosition;
        Vector3 newVel = offset.Normalized() * speed;

        Velocity = newVel;
        MoveAndSlide();

        offset.Y = 0;
        LookAt(GlobalPosition + offset, Vector3.Up);
    }

    public void FillShoppingBasket(ItemR item) {
        if (item == null) {
            Complain("there's no more items");
            while (shoppingList.Contains(first)) {
                shoppingList.Remove(first);
            }
        } else {
            shoppingBasket.AddToInventory(item);
            shoppingList.Remove(item);

            GD.Print(Name + " filled Shoppingcart with: " + item.GetName);
        }
        Browse();
    }

    void CheckIfMoreInList(ItemR item) {
        if (shoppingIndex >= shoppingList.Count) { Leave(); return; }
        if (shoppingList[shoppingIndex].GetName == item.GetName) {
            shoppingIndex++;
            CheckIfMoreInList(item);
        }
    }

    public int HowManyItems(ItemR item) {
        int count = 0;
        foreach (ItemR i in shoppingList)
            if (item == i)
                count++;
        return count;
    }

    //look for stock that matches the first of shopping list
    public void Browse() {
        if (shoppingList.Count == 0) {
            Checkout();
            return;
        }

        first = shoppingList[0];
        //only remove if get it
        GD.Print("first: " + first.GetName);
        Array<Node> shelves = GetTree().GetNodesInGroup("Shelf");
        foreach (Node shelfNode in shelves) {
            //go to first
            Shelf shelf = shelfNode.GetNode<Shelf>(".");
            if (shelf.GetItemR.GetName != first.GetName)
                continue;
            //we found a shelf
            Move(shelf.GetCustomerSpawnPos.GlobalPosition);
            return;
        }
        Complain("nothing I want is here");
        Browse();
    }



    //when mood decreases
    void Complain(string msg) {
        mood++;
        GD.Print(Name + " is " + mood.ToString() + " " + msg);
        if (mood == Mood.ANGRY)
            Leave();
    }

    //when happy and found a couple of items
    //maybe shopping index % 5 or 3 
    void Praise() {

    }


    //when mood is angry
    public void Leave() {
        Move(NPCSpawner.Instance.Position);
    }

    void Move(Vector3 pos) {
        //need to move
        GD.Print("Moving");
        agent.TargetPosition = pos;
    }

    //when at the end of shopping index
    void Checkout() {
        GD.Print("Checking out");
        if (shoppingBasket.IsEmpty()) {
            Complain("There's nothing for me here!");
            Leave();
        }
        Array<Node> cashiers = GetTree().GetNodesInGroup("Cashier");
        //eventually check which cashier has the least people on it and go there
        Cashier cashier = cashiers[0].GetNode<Cashier>(".");
        Move(cashier.GetSpawnPos.GlobalPosition);

    }

    public void StartCashierTimer() {
        cashierTimer.Start();
    }

    public void OnCashierTimerTimeout() {
        Complain("The cashier is taking too long");
    }

}
