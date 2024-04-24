using Godot;
using System.Collections.Generic;
using Godot.Collections;

public enum Mood { HAPPY, NEUTRAL, ANGRY };

public partial class Customer : CharacterBody3D {

    [Export] Inventory shoppingBasket;
    [Export] NavigationAgent3D agent;
    [Export] float speed = 10;

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
            shoppingIndex++;
        } else {
            shoppingBasket.AddToInventory(item);
            shoppingList.Remove(first);
            GD.Print("Filled Shoppingcart with: " + item.ToString());
        }
        Browse();
    }

    //look for stock that matches the first of shopping list
    public void Browse() {
        if (shoppingIndex >= shoppingList.Count) {
            Checkout();
            return;
        }

        first = shoppingList[shoppingIndex];
        //only remove if get it

        Array<Node> shelves = GetTree().GetNodesInGroup("Shelf");
        foreach (Node shelfNode in shelves) {
            //go to first
            Shelf shelf = shelfNode.GetNode<Shelf>(".");
            GD.Print("shelf: " + shelf.GetItemR.ToString() + " cust: " + first.ToString());
            if (shelf.GetItemR.GetName != first.GetName)
                continue;
            //we found a shelf
            Move(shelf.Position);
            return;

        }
        shoppingIndex++;
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
    void Leave() {

    }

    void Move(Vector3 pos) {
        //need to move
        GD.Print("Moving");
        agent.TargetPosition = pos;
    }

    //when at the end of shopping index
    void Checkout() {

    }
}
