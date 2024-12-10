using System;
using System.Collections.Generic;

[Serializable]
public class Collections
{
    public HashSet<CollectionState> documents;
    public HashSet<CollectionState> photos;
    public HashSet<CollectionState> music;
    public HashSet<CollectionState> specialItems;

    public Collections()
    {
        documents = new();
        photos = new();
        music = new();
        specialItems = new();
    }
}
[Serializable]
public class CollectionState
{
    public CollectionState(string ID, int lockedStates)
    {
        this.ID = ID;
        this.lockedStates = lockedStates;
    }
    public string ID;
    public int lockedStates;
    public override bool Equals(object obj)
    {
        return obj is CollectionState other && ID == other.ID;
    }
    public override int GetHashCode()
    {
        return ID.GetHashCode();
    }
}