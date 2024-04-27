using Godot;
using System.Collections.Generic;
using Godot.Collections;

public enum Mood { HAPPY, NEUTRAL, ANGRY };

public partial class Customer : CharacterBody3D {

    [Export] Inventory shoppingBasket;
    [Export] NavigationAgent3D agent;
    [Export] float speed = 10;



    public Inventory GetShoppingBasket { get { return shoppingBasket; } }
    public ItemR GetItemLookingFor { get { return first; } }

    List<ItemR> shoppingList = new List<ItemR>();
    Mood mood = Mood.HAPPY;
    int shoppingIndex = 0;


    ItemR first;

    public List<ItemR> ShoppingList {
        set {
            shoppingList = value;
        }
    }

    public override void _PhysicsProcess(double delta) {
        Vector3 currLoc = GlobalPosition;
        Vector3 nextLoc = agent.GetNextPathPosition();
        Vector3 newVel = (nextLoc - currLoc).Normalized() * speed;

        Velocity = newVel;
        MoveAndSlide();
    }

    public void FillShoppingBasket(ItemR item) {
        if (item == null) {
            Complain("there's no more items");
            GD.Print("sh contan: " + shoppingList.Contains(first));
            while (shoppingList.Contains(first)) {
                shoppingList.Remove(first);
                GD.Print("remove " + first.GetName);
            }
            //shoppingIndex++;

            //CheckIfMoreInList(item);
        } else {
            shoppingBasket.AddToInventory(item);
            shoppingList.Remove(item);

            GD.Print(this.Name + " filled Shoppingcart with: " + item.GetName);
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
        GD.Print("sl Count: " + shoppingList.Count);
        if (shoppingList.Count == 0) {
            GD.Print("Finished shopping");
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
            Move(shelf.GetSpawnPos.GlobalPosition);
            return;

        }

        //shoppingIndex++;
        Complain("nothing I want is here");
        Browse();
    }



    //when mood decreases
    void Complain(string msg) {
        mood++;
        GD.Print(Name + " is " + mood.ToString() + " " + msg);
    }

    //when happy and found a couple of items
    //maybe shopping index % 5 or 3 
    void Praise() {

    }


    //when mood is angry
    public void Leave() {
        Move(NPCSpawner.Instance.Position);
        NPCSpawner.Instance.DestroyCustomer(this);
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

}
