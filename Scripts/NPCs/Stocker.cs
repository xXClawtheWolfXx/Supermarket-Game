using Godot;
using System;
using Godot.Collections;
using System.Diagnostics.SymbolStore;

public partial class Stocker : Staff {
    [Export] Node3D shelfContainer;
    [Export] Hands hands;

    Array<StockInventory> stocks = new Array<StockInventory>();
    Array<Shelf> shelves = new Array<Shelf>();

    StockInventory currentStock;
    Shelf currentShelf;
    ItemR workingItemR;

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
        if (agent.IsNavigationFinished()) {
            if (currentStock != null && agent.TargetPosition == currentStock.Position)
                StockStock();
            else if (currentShelf != null && agent.TargetPosition == currentShelf.Position)
                StockShelf();
            return;
        }
        base._PhysicsProcess(delta);

    }

    private void StockShelf() {
        currentShelf.StockItem((CrateR)hands.PutDown());
        currentShelf = null;
        //if we still have stock left, put it back
        if (!hands.IsEmpty())
            Move(currentStock.Position);
    }

    private void StockStock() {
        if (hands.IsEmpty()) {
            //pick up stock
            CrateR crate = currentStock.RemoveFromInventory(workingItemR);
            hands.PickUp(crate);
            hands.ShowItem();
            Move(currentShelf.Position);
        } else
            currentStock.AddToInventory((CrateR)hands.PutDown());
    }

    protected override void Work() {

        currentShelf = LowestShelf();
        GD.PrintS("currShelf:", currentShelf.Name);
        workingItemR = currentShelf.GetItemR;
        //get stock from stock
        GD.PrintS("currItem:", workingItemR.GetName);

        currentStock = FindCorrectStock(workingItemR);
        if (currentStock != null) {
            Move(currentStock.Position);

            base.Work();
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
