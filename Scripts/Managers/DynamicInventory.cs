using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

using static Godot.GD;

/// <summary>
/// An inventory that spawns IGatherables procedurally into the world
/// </summary>
public partial class DynamicInventory : Node3D {

    [Export] Array<MeshInstance3D> meshes = new Array<MeshInstance3D>();
    [Export] Node3D meshSpawnPos;
    [Export] Node3D itemParent;

    //offsets to keep spawned objects away from the edges of the mesh
    [Export] Vector3 posOffset = new Vector3(0.25f, 0, 0.25f);
    [Export] Vector3 endOffset = new Vector3(0.25f, 0.25f, 0.25f);

    List<CrateR> crates = new List<CrateR>();
    List<Vector3> takenPos = new List<Vector3>();

    //spawned objects
    List<List<Item>> itemObjects = new List<List<Item>>();

    public bool GetIsInventoryFull { get { return isFull; } }

    int meshIndex = 0;
    Aabb currMeshAABB;

    //if we try too many times, send back badVec
    Vector3 badVec = new Vector3(-1000, -1000, -1000);
    int randPosAmt = 0;

    bool isFull = false;

    /// <summary>
    /// Checks if the inventory is empty
    /// </summary>
    /// <returns> true if the inventory is empty</returns>
    public bool IsEmpty() {
        return crates.Count <= 0;
    }
    /// <summary>
    /// Checks if a specified itemR exists in the inventory
    /// </summary>
    /// <param name="item"> the item to be checked</param>
    /// <returns></returns>
    public bool HasItemR(ItemR item) {
        foreach (CrateR crateR in crates) {
            if (crateR.GetItemR == item)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Will populate and add IGatherable to inventory if it is a CrateR
    /// </summary>
    /// <param name="gatherable"> the IGatherable to be added to the inventory</param>
    public void AddToInventory(IGatherable gatherable) {
        if (gatherable is CrateR)
            ShowItems((CrateR)gatherable);
    }

    /// <summary>
    /// Shows the items in the crate
    /// If there is no more space, the crate will be given back to the player. If some,but not all of the items are spawned, a modified crate with the remaining stock amount will be picked up by the player.
    /// </summary>
    /// <param name="crate"> the crate to be added</param>
    void ShowItems(CrateR crate) {
        //if we've exhausted all meshes, give the crate back to the player
        if (meshIndex >= meshes.Count) {
            Print("Cannot stock more items");
            Player.Instance.PickUp(crate);
            return;
        }

        List<Item> itemObj = new List<Item>();

        int itemAmt = crate.GetAmtToSpawn;
        currMeshAABB = meshes[meshIndex].GetAabb();

        //Spawns each item if it can in the world
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


    /// <summary>
    /// Returns the old crate with the items it did stock
    /// and crates a new crate with the items it did NOT stock and gives it to the player
    /// </summary>
    /// <param name="crate"> the current crate being unpacked</param>
    /// <param name="itemAmt"> the amount of items not yet stocked</param>
    /// <returns> the old crate with the items it did stock </returns>
    CrateR CleanUp(CrateR crate, int itemAmt) {
        int crateAmt = crate.GetAmtToSpawn;

        CrateR newCrate = new CrateR(crate, itemAmt);
        Player.Instance.PickUp(newCrate);

        crate.UpdateAmt(crateAmt - itemAmt);
        return crate;
    }

    /// <summary>
    /// Spawns an item into the world 
    /// </summary>
    /// <param name="item"> the item to be spawned</param>
    /// <returns> the item spawned as a Node3D</returns>
    Node3D SpawnItem(ItemR item) {
        Node3D newItem = item.GetPackedScene().Instantiate<Node3D>();
        newItem.Position = meshSpawnPos.Position;
        itemParent.AddChild(newItem);
        return newItem;
    }

    /// <summary>
    /// Gets an Node3D's AABB 
    /// </summary>
    /// <param name="item"></param>
    /// <returns> the specified Node3D's AABB</returns>
    Aabb GetItemAABB(Node3D item) {
        foreach (Node child in item.GetChildren(true))
            if (child is MeshInstance3D)
                return ((MeshInstance3D)child).GetAabb();
        return new Aabb();
    }

    /// <summary>
    /// Randomly generates a position that is not taken or outside a mesh's bounds or is touching another item's mesh
    /// </summary>
    /// <param name="objAABB"> the object's AABB</param>
    /// <returns> a Vector3 position</returns>
    Vector3 RandomPosition(Aabb objAABB) {
        //starts at the topleft corner of the mesh with the offset from the beginning
        Vector3 startPos = currMeshAABB.Position - objAABB.Position + posOffset;
        //ends at the bottom right corner of the mesh with the offset from the end
        Vector3 endPos = currMeshAABB.End - objAABB.End - endOffset;

        //The Y sits just above the meshAABB
        Vector3 randPos = new Vector3(
            (float)RandRange(startPos.X, endPos.X),
            currMeshAABB.End.Y + objAABB.End.Y + meshes[meshIndex].Position.Y,
            (float)RandRange(startPos.Z, endPos.Z)
        );

        // if this position is taken, get a new one
        //if it's been done 100 times, return "null"
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

    /// <summary>
    /// Checks if the mesh is too close to another item 
    /// </summary>
    /// <param name="pos"> the position to be checked</param>
    /// <param name="objBounds"> the bounds of the object to be checked</param>
    /// <returns> if the position is taken</returns>
    bool CheckIfPosTaken(Vector3 pos, Aabb objBounds) {
        foreach (Vector3 tp in takenPos) {
            float dist = tp.DistanceTo(pos);
            if (dist < objBounds.End.Z / 2 + .4f || dist < objBounds.End.X / 2 + .4f)
                return true;
        }
        return false;
    }

    //////////////////////////////////// remove from inventory //////////////////////////

    /// <summary>
    /// Removes the items under the player's mouse from the inventory, and gives the first crate of that item to the player.
    /// </summary>
    public void RemoveFromInventory() {
        if (Player.Instance.GetMouseOverItem != null) {
            for (int i = 0; i < itemObjects.Count; i++) {
                //pick up the crate corresponding to that item
                if (itemObjects[i][0].GetName == Player.Instance.GetMouseOverItem.GetName) {
                    DestoryObjects(itemObjects[i]);
                    Player.Instance.PickUp(crates[i]);

                    crates.RemoveAt(i);
                    itemObjects.RemoveAt(i);
                    break;
                }
            }
            isFull = false;
        }
        meshIndex = 0;
        Player.Instance.GetMouseOverItem = null;
    }
    /// <summary>
    /// Removes a specified amount of a specified itemR from the inventory
    /// </summary>
    /// <param name="item">the item to be removed</param>
    /// <param name="amt">the amount of that item to be removed</param>
    public void RemoveFromInventory(ItemR item, int amt) {
        //find the item
        for (int i = 0; i < itemObjects.Count; i++) {
            //as long as that item exists
            if (itemObjects[i].Count != 0 && itemObjects[i][0].GetName == item.GetName) {
                DestoryObjects(itemObjects[i], amt);
                if (itemObjects.Count == 0) {
                    crates.RemoveAt(i);
                    itemObjects.RemoveAt(i);
                }
                break;
            }
        }
        isFull = false;
        meshIndex = 0;
    }

    public CrateR RemoveFromInventoryNPC(ItemR item) {
        CrateR crate = null;
        //find the item
        for (int i = 0; i < itemObjects.Count; i++) {
            //as long as that item exists
            if (itemObjects[i].Count != 0 && itemObjects[i][0].GetName == item.GetName) {
                DestoryObjects(itemObjects[i]);
                if (itemObjects.Count == 0) {
                    crate = crates[i];
                    crates.RemoveAt(i);
                    itemObjects.RemoveAt(i);
                }
                break;
            }
        }
        isFull = false;
        meshIndex = 0;
        return crate;
    }

    /// <summary>
    /// Removes the entirety of the specified list of items from the world
    /// </summary>
    /// <param name="itemObjs"> the Items to be destroyed</param>
    void DestoryObjects(List<Item> itemObjs) {
        foreach (Item i in itemObjs) {
            takenPos.Remove(i.Position);
            i.QueueFree();
        }
    }

    /// <summary>
    /// Removes a certain amount of items from the world
    /// </summary>
    /// <param name="itemObjs"> the items to be removed</param>
    /// <param name="amt"> the amount to be removed</param>
    void DestoryObjects(List<Item> itemObjs, int amt) {
        for (int i = 0; i < amt; i++) {//int i = itemObjs.Count - 1; i < amt; i--) {
            takenPos.Remove(itemObjs[i].Position);
            itemObjs[i].QueueFree();
            itemObjs.RemoveAt(i);
        }
    }

}
