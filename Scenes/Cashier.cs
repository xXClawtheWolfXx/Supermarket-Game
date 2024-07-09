using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using Godot;
using Godot.Collections;

public partial class Cashier : Staff {

    [Export] private Node3D cashRegisterContainer;

    private List<CashRegister> cashRegisters = new List<CashRegister>();

    public override void _Ready() {

        Array<Node> nodes = cashRegisterContainer.GetChildren();
        foreach (Node node in nodes) {
            if (node is CashRegister cr) {
                GD.Print(cr.Name);
                cashRegisters.Add(cr);
                cr.OnCustomerAdded += TendToCustomer;
                cr.OnNoCustomers += Rest;
            }
        }
        base._Ready();
    }

    public override void _PhysicsProcess(double delta) {
        if (agent.IsNavigationFinished())
            return;
        base._PhysicsProcess(delta);
    }

    private void TendToCustomer(Vector3 pos) {
        Move(pos);
        Work();
    }

    protected override void Work() {
        base.Work();
    }
}
