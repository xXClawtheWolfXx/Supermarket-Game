using Godot;
using System;

public enum FriendshipLevel { }

public partial class NPCRelationship : Node {
    public NPC npc;
    public FriendshipLevel friendshipLevel;
    public int friendshipAmount;

    public void IncreaseFriendshipLevel(int famount) {

    }
}
