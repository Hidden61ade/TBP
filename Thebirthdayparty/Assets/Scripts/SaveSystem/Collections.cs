using System;
using System.Collections.Generic;

[Serializable]
public class Collections
{
    public HashSet<string> documents;
    public HashSet<string> photos;
    public HashSet<string> music;
    public HashSet<string> specialItems;

    public Collections()
    {
        documents = new HashSet<string>();
        photos = new HashSet<string>();
        music = new HashSet<string>();
        specialItems = new HashSet<string>();
    }
}