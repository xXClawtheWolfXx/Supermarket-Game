using Godot;
using System;
using Godot.Collections;

using static Godot.GD;


/// <summary>
/// No longer used--- will be deleted
/// </summary>
public partial class Stock : StaticBody3D, IInteractable {

    //stock
    Array<CrateR> crates = new Array<CrateR>();
    int totalWeight = 0;
    int totalStock = 0;

    [Export] MeshInstance3D mesh;


    //stock graphics
    [Export] Array<MeshInstance3D> objects = new Array<MeshInstance3D>();
    Array<Vector3> takenPos = new Array<Vector3>();

    Vector3 posOffset = new Vector3(0.25f, 0, 0.25f);
    Vector3 endOffset = new Vector3(0.25f, 0.25f, 0.25f);
    Aabb meshAabb;

    float timer = 0;
    int randPosStockAmt = 0;
    Vector3 badVec = new Vector3(-1000, -1000, -1000);
    bool allHaveGone = true;

    public override void _Ready() {
        meshAabb = mesh.GetAabb();
        randPosStockAmt = 0;
    }

    public override void _Process(double delta) {
        if (timer <= 0 && allHaveGone) {
            allHaveGone = false;
            timer = 2;
            for (int i = 0; i < objects.Count; i++) {
                Vector3 pos = RandomPosition(objects[i].GetAabb(), i);
                if (pos == badVec) {
                    Print("No more");
                    break;
                }
                takenPos.Add(pos);
                objects[i].Position = pos;
            }
            allHaveGone = true;
            takenPos.Clear();
        }
        timer -= (float)delta;
    }

    public bool CheckIfPosTaken(Vector3 pos, Aabb objBounds, int index) {

        foreach (Vector3 tp in takenPos) {
            float dist = tp.DistanceTo(pos);
            if (dist < objBounds.End.Z / 2 + .4f || dist < objBounds.End.X / 2 + .4f)
                return true;
        }
        return false;
    }

    public Vector3 RandomPosition(Aabb objAabb, int i) {

        Vector3 startPos = meshAabb.Position - objAabb.Position + posOffset;
        Vector3 endPos = meshAabb.End - objAabb.End - endOffset;

        Vector3 randPos = new Vector3(
            (float)RandRange(startPos.X, endPos.X),
            meshAabb.End.Y + objAabb.End.Y,
            (float)RandRange(startPos.Z, endPos.Z)
        );

        if (CheckIfPosTaken(randPos, objAabb, i)) {
            if (randPosStockAmt >= 100)
                return badVec;
            randPosStockAmt++;
            return RandomPosition(objAabb, i);
        }
        randPosStockAmt = 0;
        return randPos;
    }

    void StoreStock(CrateR crate) {
        crates.Add(crate);
        AddStock(crate);
        PrintStock();
    }

    void ShowStock() {
    }


    void GetStock() {
        /*
        //get rid of stock by pointing at it
        //highlightingit, 
        //on f the stock is put into a crate into the player's hands
        //and deleted from stocktable
        //need to put stock and amt in player hands
*/
        //give player first item
        if (totalStock != 0) {
            CrateR crate = crates[0];
            crates.RemoveAt(0);
            Player.Instance.PickUp(crate);
            AddStock(crate, true);
            PrintStock();
        }
    }

    void AddStock(CrateR crate, bool sub = false) {
        int s = sub == true ? -1 : 1;
        totalStock += crate.GetAmtToSpawn * s;
        totalWeight += crate.GetAmtToSpawn * crate.GetItemR.Weight * s;
    }

    void PrintStock() {
        Print("---------Stock--------");
        if (crates.Count == 0)
            Print("Stock Empty");
        else {
            foreach (CrateR crateR in crates)
                Print(crateR.ToString());
        }
        Print("total stock: " + totalStock);
    }

    public void Interact(Node3D body) {
        //if player holding nothing, give stock
        if (Player.Instance.GetHands.IsEmpty()) {
            GetStock();
        }
        //otw, store stock
        else {
            //check if player holding crate, if so, store, otw error msg
            IGatherable item = Player.Instance.PutDown();
            if (item is CrateR)
                StoreStock((CrateR)item);
            else {
                Player.Instance.PickUp(item);
                PrintErr("Cannot store " + item.ToString());
            }
        }
    }
}
