using Godot;
using System;
using Godot.Collections;
using System.Diagnostics.SymbolStore;

public partial class Stocker : Staff {
    [Export] Node3D shelfContainer;
    Array<StockInventory> stocks = new Array<StockInventory>();
    Array<Shelf> shelves = new Array<Shelf>();

    StockInventory currentStock;
    Shelf currentShelf;
    ItemR workingItemR;
    bool isAtCurrShelf;

    public ItemR GetWorkingItem { get { return workingItemR; } }

    public override void _Ready() {
        base._Ready();

        //find all the stocks
        Array<Node> nodes = shelfContainer.GetChildren();
        foreach (Node node in nodes)
            if (node is StockInventory stock)
                stocks.Add(stock);
            else if (node is Shelf shelf)
                shelves.Add(shelf);
    }

    public override void _PhysicsProcess(double delta) {
        if (agent.IsNavigationFinished())
            return;
        base._PhysicsProcess(delta);
    }

    public void StockShelf() {
        Move(currentShelf.Position);
        currentShelf = null;
        isAtCurrShelf = true;
        //if we still have stock left, put it back
    }

    public void DealWithMoreStock() {
        if (!IsEmpty())
            Move(currentStock.Position);
        else
            Rest();
        currentStock = null;
    }


    protected override void Work() {

        currentShelf = LowestShelf();
        if (currentShelf == null) {
            GD.PrintS(Name, "no shelves to stock");
            Rest();
            return;
        }

        GD.PrintS("currShelf:", currentShelf.Name);
        workingItemR = currentShelf?.GetItemR;

        if (workingItemR == null) {
            GD.PrintS(Name, "no item in the shelves to stock");
            Rest();
            return;
        }
        //get stock from stock
        GD.PrintS("currItem:", workingItemR.GetName);

        currentStock = FindCorrectStock(workingItemR);
        if (currentStock != null) {
            Move(currentStock.Position);
            base.Work();
        } else {
            GD.PrintS(Name, "no stock to stock");
        }
    }

    Shelf LowestShelf() {
        int bestAmt = int.MaxValue;
        Shelf bestShelf = null;

        foreach (Shelf shelf in shelves) {
            if (shelf.GetItemR != null && shelf.GetItemAmount < bestAmt) {
                bestAmt = shelf.GetItemAmount;
                bestShelf = shelf;
            }
        }

        if (bestShelf == null || bestShelf.IsFull())
            return null;

        return bestShelf;
    }

    StockInventory FindCorrectStock(ItemR itemR) {
        foreach (StockInventory stock in stocks) {
            if (stock.HasItemR(itemR)) {
                return stock;
            }
        }
        return null;
    }
}
