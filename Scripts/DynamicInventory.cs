using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

using static Godot.GD;

public partial class DynamicInventory : Node3D {

    [Export] Array<MeshInstance3D> meshes = new Array<MeshInstance3D>();
    [Export] Node3D meshSpawnPos;
    [Export] Node3D itemParent;
    [Export] Vector3 posOffset = new Vector3(0.25f, 0, 0.25f);
    [Export] Vector3 endOffset = new Vector3(0.25f, 0.25f, 0.25f);

    List<CrateR> crates = new List<CrateR>();
    List<Vector3> takenPos = new List<Vector3>();
    List<List<Item>> itemObjects = new List<List<Item>>();

    public bool GetIsInventoryFull { get { return isFull; } }


    int meshIndex = 0;
    Aabb currMeshAABB;

    //if we try too many times
    //since we can't send back null Vector3s for some reason
    Vector3 badVec = new Vector3(-1000, -1000, -1000);
    int randPosAmt = 0;

    bool isFull = false;

    public bool IsEmpty() {
        return crates.Count <= 0;
    }

    public void AddToInventory(IGatherable gatherable) {
        if (gatherable is CrateR)
            ShowItems((CrateR)gatherable);
    }

    void ShowItems(CrateR crate) {
        if (meshIndex >= meshes.Count) {
            Print("Cannot stock more items");
            Player.Instance.PickUp(crate);
            return;
        }

        //only get one crate even if all are the same obj
        List<Item> itemObj = new List<Item>();

        int itemAmt = crate.GetAmtToSpawn;
        currMeshAABB = meshes[meshIndex].GetAabb();
        for (; itemAmt > 0; itemAmt--) {
            Node3D item = SpawnItem(crate.GetItemR);
            Vector3 pos = RandomPosition(GetItemAABB(item));
            item.Position = pos;

            //if the position is null, go to the next mesh and pick a new position, and or give back what remains
            if (pos == badVec) {
                meshIndex++;
                if (meshIndex >= meshes.Count) {
                    crate = CleanUp(crate, itemAmt);
                    isFull = true;
                    break;
                }
                pos = RandomPosition(GetItemAABB(item));
                item.Position = pos;
            }
            takenPos.Add(pos);
            itemObj.Add(item.GetNode<Item>("."));
        }

        crates.Add(crate);
        itemObjects.Add(itemObj);
    }

    //returns the old crate with the items it did stock
    //and crates a new crate with the items it did NOT stock and gives it to the player
    CrateR CleanUp(CrateR crate, int itemAmt) {
        int crateAmt = crate.GetAmtToSpawn;

        CrateR newCrate = new CrateR(crate, itemAmt);
        Player.Instance.PickUp(newCrate);

        crate.UpdateAmt(crateAmt - itemAmt);
        return crate;
    }

    /* List<Item> DoesCrateExist(CrateR crate) {
         //check item names
         for (int i = 0; i < itemObjects.Count; i++)
             if (itemObjects[i][0].GetName == crate.GetItemR.GetName)
                 return itemObjects[i];

         List<Item> itemObj = new List<Item>();
         itemObjects.Add(itemObj);
         return itemObj;
     }
     */

    Node3D SpawnItem(ItemR item) {
        Node3D newItem = item.GetPackedScene().Instantiate<Node3D>();
        newItem.Position = meshSpawnPos.Position;
        itemParent.AddChild(newItem);
        return newItem;
    }

    Aabb GetItemAABB(Node3D item) {
        foreach (Node child in item.GetChildren(true))
            if (child is MeshInstance3D)
                return ((MeshInstance3D)child).GetAabb();
        return new Aabb();
    }

    Vector3 RandomPosition(Aabb objAABB) {
        Vector3 startPos = currMeshAABB.Position - objAABB.Position + posOffset;
        Vector3 endPos = currMeshAABB.End - objAABB.End - endOffset;

        Vector3 randPos = new Vector3(
            (float)RandRange(startPos.X, endPos.X),
            currMeshAABB.End.Y + objAABB.End.Y + meshes[meshIndex].Position.Y,
            (float)RandRange(startPos.Z, endPos.Z)
        );

        if (CheckIfPosTaken(randPos, objAABB)) {
            if (randPosAmt >= 100)
                return badVec;
            randPosAmt++;
            return RandomPosition(objAABB);
        }
        isFull = false;
        randPosAmt = 0;
        return randPos;
    }

    bool CheckIfPosTaken(Vector3 pos, Aabb objBounds) {
        foreach (Vector3 tp in takenPos) {
            float dist = tp.DistanceTo(pos);
            if (dist < objBounds.End.Z / 2 + .4f || dist < objBounds.End.X / 2 + .4f)
                return true;
        }
        return false;
    }

    /////// remove from inventory //////////

    public void RemoveFromInventory() {
        if (Player.Instance.GetMouseOverItem != null) {
            //pick up the crate corresponding to that item
            for (int i = 0; i < itemObjects.Count; i++) {
                if (itemObjects[i][0].GetName == Player.Instance.GetMouseOverItem.GetName) {
                    DestoryObjects(itemObjects[i]);
                    Player.Instance.PickUp(crates[i]);

                    crates.RemoveAt(i);
                    itemObjects.RemoveAt(i);
                    break;
                }
            }
        }
        meshIndex = 0;
        Player.Instance.GetMouseOverItem = null;
    }

    public void RemoveFromInventory(ItemR item, int amt) {
        //find the item
        for (int i = 0; i < itemObjects.Count; i++) {
            if (itemObjects[i].Count != 0 && itemObjects[i][0].GetName == item.GetName) {
                DestoryObjects(itemObjects[i], amt);
                if (itemObjects.Count == 0) {
                    crates.RemoveAt(i);
                    itemObjects.RemoveAt(i);
                }
                break;
            }
        }
        meshIndex = 0;
    }
    void DestoryObjects(List<Item> itemObjs) {
        foreach (Item i in itemObjs) {
            takenPos.Remove(i.Position);
            i.QueueFree();
        }
    }

    void DestoryObjects(List<Item> itemObjs, int amt) {
        for (int i = 0; i < amt; i++) {//int i = itemObjs.Count - 1; i < amt; i--) {
            takenPos.Remove(itemObjs[i].Position);
            itemObjs[i].QueueFree();
            itemObjs.RemoveAt(i);
        }
    }




}
