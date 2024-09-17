using Godot;
using System;

public enum Need { EAT, HYGIENE, REST, FUN, ALL, NONE } //add social type

[GlobalClass]
public partial class NPCNeed : Node {

    public Need need;
    public int amount;

    public NPCNeed(Need need, int amount) {
        this.need = need;
        this.amount = amount;
    }

    public int IncreaseAmount(int amt) {
        amount = Mathf.Clamp(amt, 0, 100);
        return amount; //in case we need it
    }
}
