using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

using static Godot.GD;

public partial class StockInventory : StaticBody3D, IInteractable {

    [Export] Array<MeshInstance3D> meshes = new Array<MeshInstance3D>();
    [Export] Node3D meshSpawnPos;
    [Export] Node3D itemSpawn;
    [Export] Vector3 posOffset = new Vector3(0.25f, 0, 0.25f);
    [Export] Vector3 endOffset = new Vector3(0.25f, 0.25f, 0.25f);

    List<IGatherable> items = new List<IGatherable>();
    List<CrateR> crates = new List<CrateR>();
    List<Vector3> takenPos = new List<Vector3>();

    int meshIndex = 0;
    Aabb currMeshAABB;

    //if we try too many times
    //since we can't send back null Vector3s for some reason
    Vector3 badVec = new Vector3(-1000, -1000, -1000);
    int randPosAmt = 0;

    bool isFull = false;

    public bool IsEmpty() {
        return items.Count == 0;
    }

    public void AddToInventory(IGatherable gatherable) {
        if (gatherable is CrateR)
            AddCrate((CrateR)gatherable);
    }

    void AddCrate(CrateR crate) {
        int itemAmt = crate.GetAmtToSpawn;
        currMeshAABB = meshes[meshIndex].GetAabb();
        for (; itemAmt > 0; itemAmt--) {

            Node3D item = SpawnItem(crate.GetItemR);
            Vector3 pos = RandomPosition(GetItemAABB(item));
            item.Position = pos;

            if (pos == badVec) {
                meshIndex++;
                if (meshIndex >= meshes.Count) {
                    CleanUp(crate, itemAmt);
                    break;
                }
                pos = RandomPosition(GetItemAABB(item));
                item.Position = pos;
            }
            takenPos.Add(pos);
        }
        if (itemAmt != 0) {
            //crate.
        }

        crates.Add(crate);

        //give back crate if itemAmt is not 0
    }

    void CleanUp(CrateR crate, int itemAmt) {

    }

    Node3D SpawnItem(ItemR item) {
        Node3D newItem = item.GetPackedScene().Instantiate<Node3D>();
        newItem.Position = meshSpawnPos.Position;
        itemSpawn.AddChild(newItem);
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

    // get out

    void GetOut() {

    }



    public void Interact(Node3D body) {
        if (body is Player)
            if (!Player.Instance.GetHands.IsEmpty())
                AddToInventory(Player.Instance.PutDown());
            else
                GetOut();
    }

}
