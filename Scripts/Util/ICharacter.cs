using Godot;
using System;

public interface ICharacter {
    public void PickUp(IGatherable item);
    public IGatherable PutDown();

}
