using Godot;
using System;
using System.Collections.Generic;

public partial class Stock : StaticBody3D, IInteractable {
    Dictionary<ItemR, int> stockTable = new Dictionary<ItemR, int>();
    int totalStock = 0;

    void StoreStock(CrateR crate) {
        foreach (ItemR item in stockTable.Keys)
            if (item == crate.GetItemR) {
                stockTable[item] += crate.GetAmtToSpawn; //will need to clamp this
                totalStock += crate.GetAmtToSpawn;
            }
        //also need to store stock physically
    }

    void ShowStock() {
        //offset + bounds.y / 2.0 ()
        //put on MUltiMesh3d
    }



    void GetStock() {
        //get rid of stock by pointing at it
        //highlightingit, 
        //on f the stock is put into a crate into the player's hands
        //and deleted from stocktable
        //need to put stock and amt in player hands
    }

    public void Interact(Node3D body) {

    }
}
