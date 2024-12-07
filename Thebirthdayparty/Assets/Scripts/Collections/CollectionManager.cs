// // 在需要解锁收藏品的地方
// public void UnlockCollectionItem(string itemID)
// {
//     CollectionManager.Instance.UnlockCollection(itemID);
// }

// // 在需要保存游戏进度的地方
// private void SaveGame()
// {
//     // 其他保存逻辑...
//     CollectionManager.Instance.SaveUnlockStatus();
// }

using UnityEngine;
using System.Collections.Generic;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance { get; private set; }

    [SerializeField] private CollectionData[] allCollections;
    private Dictionary<string, CollectionData> collectionDict = new Dictionary<string, CollectionData>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeCollections();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeCollections()
    {
        foreach (var collection in allCollections)
        {
            if (!collectionDict.ContainsKey(collection.itemID))
            {
                collectionDict.Add(collection.itemID, collection);
            }
        }
        LoadUnlockStatus();
    }

    public void UnlockCollection(string itemID)
    {
        if (collectionDict.TryGetValue(itemID, out CollectionData collection))
        {
            collection.isUnlocked = true;
            SaveUnlockStatus();
        }
    }

    public bool IsCollectionUnlocked(string itemID)
    {
        return collectionDict.TryGetValue(itemID, out CollectionData collection) && collection.isUnlocked;
    }

    public CollectionData GetCollectionData(string itemID)
    {
        return collectionDict.TryGetValue(itemID, out CollectionData collection) ? collection : null;
    }

    public CollectionData[] GetAllCollections()
    {
        return allCollections;
    }

    private void SaveUnlockStatus()
    {
        foreach (var collection in collectionDict.Values)
        {
            PlayerPrefs.SetInt($"Collection_{collection.itemID}", collection.isUnlocked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    private void LoadUnlockStatus()
    {
        foreach (var collection in collectionDict.Values)
        {
            collection.isUnlocked = PlayerPrefs.GetInt($"Collection_{collection.itemID}", 0) == 1;
        }
    }
}