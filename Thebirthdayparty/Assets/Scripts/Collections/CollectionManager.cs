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
using System.Linq;
using System.Xml.Schema;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance { get; private set; }
    private readonly Dictionary<string, CollectionData> collectionDict = new();
    [SerializeField] private List<string> LoadedIDs = new();
    public Dictionary<CollectionData.CollectionType, string> CollectionTypeStringDict = new(){
        {CollectionData.CollectionType.documents,"documents"},
        {CollectionData.CollectionType.photos,"photos"},
        {CollectionData.CollectionType.music,"music"},
        {CollectionData.CollectionType.specialItems,"specialItems"}
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Initialize();
        UnlockCollection("A01");
    }
    void Initialize()
    {
        var t_AllDataResource = GetAllCollectionDatas();
        foreach (var item in t_AllDataResource)
        {
            collectionDict.Add(item.itemID, item);
            LoadedIDs.Add(item.itemID);
        }
    }
    public void UnlockCollection(string itemID)
    {
        var temp = collectionDict[itemID];
        GameSaveManager.Instance.UnlockCollection(itemID, CollectionTypeStringDict[temp.collectionType]);
    }

    public bool IsCollectionUnlocked(string itemID)
    {
        return GetCollectionLockState(itemID) >= 1;
    }
    public int GetCollectionLockState(string itemID)
    {
        var temp = collectionDict[itemID];
        var ctrl = GameSaveManager.Instance.GetCollectionState(itemID, CollectionTypeStringDict[temp.collectionType]);
        if (ctrl is null)
        {
            return 0;
        }
        else
        {
            return ctrl.lockedStates;
        }
    }
    public void AddToCollection(string itemID)
    {
        if(collectionDict.TryGetValue(itemID,out CollectionData data)){
            GameSaveManager.Instance.AddToCollection(data.name,CollectionTypeStringDict[data.collectionType]);
        }else{
            Debug.LogError("No such collection: "+ itemID);
        }
    }
    public CollectionData GetCollectionData(string itemID)
    {
        if (!collectionDict.ContainsKey(itemID))
        {
            return Resources.Load("Collections/" + itemID) as CollectionData;
        }
        collectionDict.TryGetValue(itemID, out CollectionData result);
        return result;
    }

    public CollectionData[] GetAllCollections()
    {
        return collectionDict.Values.ToArray();
    }
    public CollectionData[] GetAllCollectionDatas()
    {
        return Resources.LoadAll<CollectionData>("Collections");
    }

    private void SaveUnlockStatus()
    {
        GameSaveManager.Instance.SaveGame();
    }
}