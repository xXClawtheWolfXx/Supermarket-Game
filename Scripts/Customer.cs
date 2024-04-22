using Godot;
using System;
using System.Collections.Generic;

public enum Mood { HAPPY, NEUTRAL, ANGRY };

public partial class Customer : CharacterBody3D {

    [Export] Inventory shoppingBasket;

    List<ItemR> ShoppingList = new List<ItemR>();
    Mood mood = Mood.HAPPY;

    void Browse() {

    }

    void Complain() {

    }

    void Praise() {

    }

    void Leave() {

    }

    void Move() {

    }

    void Checkout() {

    }
}
