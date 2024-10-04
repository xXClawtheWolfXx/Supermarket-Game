using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

[GlobalClass]

public partial class Store : Place {

    [ExportGroup("Store")]
    [Export] private Array<Job> jobs = new Array<Job>();
    [Export] private Array<ShoppingTask> allStoreTasks = new Array<ShoppingTask>();
    [Export] private Task checkOutTask;

    [ExportGroup("Cashier")]
    [Export] Node3D custSpawnPos;
    [Export] Node3D itemSpawnPos;
    [Export] Label3D itemPriceLabel;
    [Export] Timer itemSpawnTimer;
    [Export] float custSpawnRadius = 3f;


    private List<NPC> cashierLine = new List<NPC>();
    private Vector3 originalCustSpawnPosition;

    public Task GetCheckoutTask { get => checkOutTask; }

    public override void _Ready() {
        foreach (ShoppingTask task in allStoreTasks)
            task.SetStore(this);
        originalCustSpawnPosition = custSpawnPos.GlobalPosition;
    }

    public void AddCustomerToLine(NPC npc) {
        if (cashierLine.Contains(npc)) return;

        //increase spawnPos
        custSpawnPos.Position += new Vector3(0, 0, -custSpawnRadius);
        
    }
}
