using Godot;
using System;

public partial class ShoppingTask : Task {

    [Export] protected Store store;

    //will be called by Store during runtime
    public void SetStore(Store inStore) {
        store = inStore;
    }

}
